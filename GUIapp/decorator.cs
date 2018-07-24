using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIapp
{
    abstract class GuiDecorator : IGuiElement
    {
        //Abstract GuiElement Decorator, to add some behavioral aspects to gui elements dynamicly
        protected IGuiElement DecoratedElement;

        public GuiDecorator(IGuiElement element)
        {
            this.DecoratedElement = element;
        }

        public abstract void Draw(IDrawVisitor visitor);
        public abstract void Update(IUpdateVisitor visitor, float dt);
    }


    class LabelDecorator : GuiDecorator
    {
        //To decorate a label
        InputManager inputManager;
        Label label;
        int Max;
        public LabelDecorator(Label label, int max_length) : base(label)
        {
            this.inputManager = new MonoGameInputManager();
            this.label = label;
            this.Max = max_length;
        }

        public override void Draw(IDrawVisitor visitor)
        {
            base.DecoratedElement.Draw(visitor);
        }

        public override void Update(IUpdateVisitor visitor, float dt)
        {
            //A decorated label is like an input field
            base.DecoratedElement.Update(visitor,dt);
            inputManager.Typing().Visit(() => Do.Nothing(), (keys) =>
            {
                switch (keys[0].ToString())
                {
                    case "Back":
                        if (label.Content.Length != 0) label.Content = label.Content.Remove(label.Content.Length - 1, 1);
                        break;
                    case "Space":
                        if (label.Content.Length <= Max) label.Content += " ";
                        break;
                    case "Enter":
                        if (label.Content.Length <= Max) label.Content += "\n";
                        break;
                    case "Delete":
                        label.Content = "";
                        break;
                    default:
                        if (label.Content.Length <= Max) label.Content += keys[0];
                        break;
                }
            });
        }
    }

    class ButtonDecorator : GuiDecorator
    {
        //Decorates a button
        Button DecoratedButton;
        InputManager inputManager;
        Colour ClickColor, OldColor, HoverColor;
        public ButtonDecorator(Button button, Colour click_color, Colour hover_color) : base(button)
        {
            this.DecoratedButton = button;
            this.inputManager = new MonoGameInputManager();
            this.ClickColor = click_color;
            this.HoverColor = hover_color;
            this.OldColor = button.Color;//You need to store the oldColor if want the button to flash back when you stop hovering
        }

        public override void Draw(IDrawVisitor visitor)
        {
            base.DecoratedElement.Draw(visitor);
        }

        public override void Update(IUpdateVisitor visitor, float dt)
        {
            base.DecoratedElement.Update(visitor ,dt);
            //A decorated button will flash into an other color when you hover the mouse over it
            if(DecoratedButton.is_intersecting(new Position(inputManager.Hover().X, inputManager.Hover().Y)))
            {
                DecoratedButton.Color = HoverColor;
                inputManager.Click().Visit(() => Do.Nothing(), (pos2) => DecoratedButton.Color = ClickColor);
            }
            else
            {
                DecoratedButton.Color = OldColor;
            }
        }
    }
}
