using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIapp
{
    interface IUpdateable
    {
        //Updateable interface
        void Update(IUpdateVisitor visitor, float dt);
    }

    interface IDrawable
    {
        //Drawable interface
        void Draw(IDrawVisitor visitor);
    }

    interface IGuiElement : IDrawable, IUpdateable { } //GuiElements are Drawable and updateable
 
    class Position
    {
        //Defines the position for elements
        private int localX;
        private int localY;
        public Position(int x, int y)
        {
            this.localX = x;
            this.localY = y;
        }

        public int X { get { return this.localX; } }
        public int Y { get { return this.localY; } }
    }

    class Label : IGuiElement
    {
        //Reperesents a label
        public string Content;
        public Colour Color;
        public Position TopLeftCorner;
        public Label(string content, Colour color, Position top_left_corner)
        {
            this.Content = content;
            this.Color = color;
            this.TopLeftCorner = top_left_corner;
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.DrawLabel(this);
        }

        public void Update(IUpdateVisitor visitor, float dt)
        {
            visitor.UpdateLabel(this, dt);
        }
    }

    class Button : IGuiElement
    {
        //Represents a button
        public int Width, Height;
        public Action Action;
        public Colour Color;
        public Label Label;
        public Position TopLeftCorner;
        public Button(string text, Position top_left_corner, Colour color, int width, int height, Action action)
        {
            this.Action = action;
            this.Width = width;
            this.Height = height;
            this.Color = color;
            this.TopLeftCorner = top_left_corner;
            this.Label = new Label(text, Colour.Black, new Position(top_left_corner.X + width/2 - text.Length ,top_left_corner.Y + height/2));
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.DrawButton(this);
            visitor.DrawLabel(this.Label);
        }

        public bool is_intersecting(Position point)
        {
            return point.X > TopLeftCorner.X && point.Y > TopLeftCorner.Y &&
                   point.X < TopLeftCorner.X + Width && point.Y < TopLeftCorner.Y + Height;
        }

        public void Update(IUpdateVisitor visitor, float dt)
        {
            visitor.UpdateButton(this, dt);
        }
    }

  
    class TextBox : IGuiElement
    {
        public Button NonClickAbleButton;
        public IGuiElement inputField;
        private GuiElementFactory LabelFactory;
        public TextBox(string message, Position top_left_corner, Colour color, int width, int height)
        {
            this.LabelFactory = new ConcreteGuiElementFactory();
            this.NonClickAbleButton = new Button(message,top_left_corner,color, width, height, ()=>Do.Nothing());
            LabelFactory.Create(6,"",top_left_corner,color,()=>Do.Nothing()).Visit(()=>Do.Nothing(),(element)=>this.inputField = element);
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.DrawButton(this.NonClickAbleButton);
            visitor.DrawLabel(this.NonClickAbleButton.Label);
            this.inputField.Draw(visitor);
        }

        public void Update(IUpdateVisitor visitor, float dt)
        {
            visitor.UpdateButton(this.NonClickAbleButton, dt);
            this.inputField.Update(visitor, dt);
        }
    }

    class GuiManager : IUpdateable, IDrawable
    {
        public ILinkedList<IGuiElement> elements;      
        private GuiElementFactory menuFactory;
        public GuiManager(Action exit)
        {
            this.elements = new Empty<IGuiElement>();
            this.menuFactory = new ConcreteGuiElementFactory();
            StartWindow(exit);
        }

        private void StartWindow(Action exit)
        {
            this.elements = new Empty<IGuiElement>();
            this.menuFactory.Create(5,"Start",new Position(700,300),Colour.Red,()=>InputWindow(exit)).Visit(()=>Do.Nothing(),(element)=>this.elements = this.elements.Add(element));
            this.menuFactory.Create(5, "Input", new Position(700, 400), Colour.White, () => LabelWindow(exit)).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
            this.menuFactory.Create(5, "Exit", new Position(700, 500), Colour.Blue, () => ExitWindow(exit)).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
        }

        private void InputWindow(Action exit)
        {
            this.elements = new Empty<IGuiElement>();
            this.menuFactory.Create(5, "Play Music", new Position(800, 400), Colour.Yellow, () => Music.PlaySong(Music.BuildSong(2))).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
            this.menuFactory.Create(5, "Go Back", new Position(800, 600), Colour.Blue, () => StartWindow(exit)).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
        }

        private void LabelWindow(Action exit)
        {
            this.elements = new Empty<IGuiElement>();
            this.menuFactory.Create(4,"Start typing: ", new Position(500,30),Colour.White,()=>Do.Nothing()).Visit(()=>Do.Nothing(),(element)=> this.elements = this.elements.Add(element));
            this.menuFactory.Create(2, "Press DELETE to refresh the input field", new Position(50, 50), Colour.Black, () => Do.Nothing()).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
            this.menuFactory.Create(5, "Go Back", new Position(1400, 200), Colour.Blue, () => StartWindow(exit)).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));

            this.menuFactory.Create(3,"C",new Position(500,550),Colour.White,()=>Music.PlayNote(new Note(Tone.C,Duration.QUARTER))).Visit(()=>Do.Nothing(),(element)=>this.elements = this.elements.Add(element));
            this.menuFactory.Create(3, "D", new Position(560, 550), Colour.White, () => Music.PlayNote(new Note(Tone.D, Duration.QUARTER))).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
            this.menuFactory.Create(3, "E", new Position(620, 550), Colour.White, () => Music.PlayNote(new Note(Tone.E, Duration.QUARTER))).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
            this.menuFactory.Create(3, "F", new Position(680, 550), Colour.White, () => Music.PlayNote(new Note(Tone.F, Duration.QUARTER))).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element)); 
            this.menuFactory.Create(3, "G", new Position(740, 550), Colour.White, () => Music.PlayNote(new Note(Tone.G, Duration.QUARTER))).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element)); 
            this.menuFactory.Create(3, "A", new Position(800, 550), Colour.White, () => Music.PlayNote(new Note(Tone.A, Duration.QUARTER))).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element)); 
            this.menuFactory.Create(3, "B", new Position(860, 550), Colour.White, () => Music.PlayNote(new Note(Tone.B, Duration.QUARTER))).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element)); 
        }

        private void ExitWindow(Action exit)
        {
            this.elements = new Empty<IGuiElement>();
            this.menuFactory.Create(1, "Yes", new Position(1000, 500), Colour.Red, () => exit()).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
            this.menuFactory.Create(1, "No", new Position(300, 500), Colour.Green, () => StartWindow(exit)).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
            this.menuFactory.Create(2, "Are you sure you want to quit?", new Position(700, 500), Colour.Black, () => Do.Nothing()).Visit(() => Do.Nothing(), (element) => this.elements = this.elements.Add(element));
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.DrawGui(this);
        }

        public void Update(IUpdateVisitor visitor, float dt)
        {
           visitor.UpdateGui(this,dt);
        }
    }
}