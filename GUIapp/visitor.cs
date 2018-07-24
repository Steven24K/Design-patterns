using System;
using System.Threading;


namespace GUIapp
{
    interface IOption<T>
    {
        //Returns either Some or None, in the shape of a lambda, Func or Action
        U Visit<U>(Func<U> onNone, Func<T, U> onSome);
        void Visit(Action onNone, Action<T> onSome);
    }

    interface IUpdateVisitor
    {
        //The Update visitor must contain Update methods for all Gui elements
        void UpdateButton(Button element, float dt);
        void UpdateLabel(Label element, float dt);
        void UpdateGui(GuiManager element, float dt);
    }

    interface IDrawVisitor
    {
        //The Draw visitor must contain Draw methods for Gui elements
        void DrawButton(Button element);
        void DrawLabel(Label element);
        void DrawGui(GuiManager element);
    }

    public class None<T> : IOption<T>
    {
        //Represents a None object, can be used instead of returning null values
        public U Visit<U>(Func<U> onNone, Func<T, U> onSome)
        {
            return onNone(); //Only returns onNone
        }
        public void Visit(Action onNone, Action<T> onSome)
        {
            onNone(); //Only invokes onNone
        }
    }

    public class Some<T> : IOption<T>
    {
        T value;
        //A Some object can contain any single value from any type
        public Some(T value)
        {
            this.value = value;
        }
        public U Visit<U>(Func<U> onNone, Func<T, U> onSome)
        {
            return onSome(value); //Only returns onSome
        }
        public void Visit(Action onNone, Action<T> onSome)
        {
            onSome(value); //Only invokes onSome
        }
    }

    class Do
    {
        // Can be used when a void action is not supposed to do anything
        //Represents an Empty class with no constructor and a method with no body
        public static void Nothing()
        {
            //Just a method wich does do nothing
        }
    }

    class DefaultDrawVisitor : IDrawVisitor
    {
        //Represents a concrete Draw visitor, all attributes and methods come from the Draw manager in the adapter
        IDrawManager DrawManager;
        public DefaultDrawVisitor(IDrawManager draw_manager)
        {
            this.DrawManager = draw_manager;
        }
        public void DrawButton(Button element)
        {
            //Draws a button
            this.DrawManager.DrawRectangle(element.TopLeftCorner,element.Width,element.Height,element.Color);
        }
        public void DrawLabel(Label element)
        {
            //Draws a laber
            this.DrawManager.DrawString(element.Content,element.TopLeftCorner,element.Color);
        }
        public void DrawGui(GuiManager gui_manager)
        {
            //Draws all elements from the list in the guimanager
            Iterator<IGuiElement> elementIterator;
            elementIterator = new LinkedListIterator<IGuiElement>(gui_manager.elements);
            elementIterator.getCurrent().Visit(()=>Do.Nothing(), (button)=>button.Draw(this));
            while (elementIterator.getCurrent().Visit(()=>false,(element)=>true))
            {
                elementIterator.moveNext().Visit(() => Do.Nothing(), button => button.Draw(this));
            }
            
        }

    }

    class DefaultUpdateVisitor : IUpdateVisitor
    {
        //Represents a concrete update visitor, all methods and attributes come from the input manager in the adapter
        private InputManager inputManager;
        public DefaultUpdateVisitor(InputManager input_manager)
        {
            this.inputManager = input_manager;
        }

        public void UpdateButton(Button element, float dt)
        {
            //Updates a button, if the button is clicked then execute action
            inputManager.Click().Visit(()=>Do.Nothing(),(position)=> {
              if(element.is_intersecting(new Position(position.X,position.Y)))
                { element.Action();}
            });

        }
        public void UpdateLabel(Label element, float dt)
        {
            //Regular labels don't have to be updated
        }
        public void UpdateGui(GuiManager gui_manager, float dt)
        {
            //Updates all elements from the list in the gui manager
            Iterator<IGuiElement> elementIterator;
            elementIterator = new LinkedListIterator<IGuiElement>(gui_manager.elements);
            elementIterator.getCurrent().Visit(()=> Do.Nothing(), (button)=>button.Update(this,dt));
            while (elementIterator.hasNext())
            {
                elementIterator.moveNext().Visit(() => Do.Nothing(), button => button.Update(this,dt));
            }

        }
    }
}
