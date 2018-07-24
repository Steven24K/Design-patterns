using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIapp
{
    abstract class GuiFactory
    {
        //Represents an abstract GuiFactory with an abstract method wich returns a GuiManager
        public abstract GuiManager Instantiate(string option, Action exit);
    }

    class ConcreteGuiFactory : GuiFactory
    {
        //A conrete GuiFactory, this is where the Guimanager is created and instantiated
        public override GuiManager Instantiate(string option, Action exit)
        {
            GuiManager guiManager = new GuiManager(exit);
            switch (option)
            {
                default:
                  return guiManager;
            }
        }
    }

    abstract class GuiElementFactory
    {
        /// <summary>
        /// Creates a new GuiElement, a button or a label, or a textbox from its ID
        /// id=1 returns a new regular button
        /// id=2 returns a new regular label
        /// id=3 returns a new piano button
        /// id=4 returns a new text box
        /// id=5 returns a new decorated button
        /// id=6 returns a new decorated label
        /// All GuiElements come inside an IOption(Some or None), wich you need to visit.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="top_left_corner"></param>
        /// <param name="color"></param>
        /// <param name="action"></param>
        /// <returns>IOption<IGuiElement></returns>
        public abstract IOption<IGuiElement> Create(int id, string text, Position top_left_corner,Colour color, Action action);
    }

    class ConcreteGuiElementFactory : GuiElementFactory
    {
        public override IOption<IGuiElement> Create(int id, string text, Position top_left_corner,Colour color, Action action)
        {
            switch (id)
            {
                case 1:
                    return new Some<IGuiElement>(new Button(text, top_left_corner,color,200,80,action));
                case 2:
                    return new Some<IGuiElement>(new Label(text,color,top_left_corner));
                case 3:
                    return new Some<IGuiElement>(new Button(text,top_left_corner,Colour.White,50,300,action));
                case 4:
                    return new Some<IGuiElement>(new TextBox(text,top_left_corner,color,600,50));
                case 5:
                    return new Some<IGuiElement>(new ButtonDecorator(new Button(text,top_left_corner,color,200,80,action),Colour.Aqua,Colour.Indigo));
                case 6:
                    return new Some<IGuiElement>(new LabelDecorator(new Label(text,Colour.Black,top_left_corner),35));
                default:
                    return new None<IGuiElement>();
            }
        }
    }
}
