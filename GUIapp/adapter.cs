using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GUIapp
{
  interface IDrawManager
    {
        //Manages all drawing functions for the application
        void DrawRectangle(Position top_left_coordinate, int width, int height, Colour color);
        void DrawString(string text, Position top_left_coordinate, Colour color);
    }

    interface InputManager
    {
        //Manages all input actions from the keyboard
        IOption<Point> Click();
        IOption<Keys[]> Typing();
        Point Hover();
    }

    interface IGraphicsDeviceManager
    {
        //Manages the graphicsDevice from monogame
        void setHeihtandWidth(int prefered_height, int prefered_width);
    }

    class MonoGameInputManager : InputManager
    {
        //Concrete implementation of inputmanager
        public Point Hover()
        {
            //Returns the current mouse position
            MouseState mouse = Mouse.GetState();
            return new Point(mouse.Position.X,mouse.Position.Y);
        }

        public IOption<Point> Click()
        {
            //Retuns the current mouse position, only if the left button is pressed
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed) return new Some<Point>(new Point(mouse.Position.X, mouse.Position.Y));
            return new None<Point>();
        }

        public IOption<Keys[]> Typing()
        {
            //Retuns an array of all keys wich is being pressed on the keyboard
            KeyboardState keyboardstate = Keyboard.GetState();
            Keys[] key = keyboardstate.GetPressedKeys();
            if (key.Length > 0 && keyboardstate.IsKeyDown(key[0])) return new Some<Keys[]>(key);
            return new None<Keys[]>();
        }
    }
    
    //Colour library
    public enum Colour { Black,White,Red,Green,Yellow,Blue,Aqua,Ivory,Indigo }
    

    class MonoGameDrawAdapter : IDrawManager
    {
        //Concrete implementation of Drawmanager
        //Contains all attributes from monogame wich you need to draw an item
        SpriteBatch SpriteBatch;
        ContentManager ContentManager;
        Texture2D WhitePixel;
        SpriteFont DefaultFont;

        public MonoGameDrawAdapter(SpriteBatch sprite_batch, ContentManager content_manager)
        {
            this.SpriteBatch = sprite_batch;
            this.ContentManager = content_manager;
            this.WhitePixel = ContentManager.Load<Texture2D>("white_pixel");
            this.DefaultFont = content_manager.Load<SpriteFont>("Arial");
        }

        //To convert the Colour to a monogame Color
        private Color ColorToColourConverter(Colour color)
        {
            switch (color)
            {
                case Colour.Black:
                    return Color.Black;
                case Colour.White:
                    return Color.White;
                case Colour.Green:
                    return Color.Green;
                case Colour.Red:
                    return Color.Red;
                case Colour.Blue:
                    return Color.Blue;
                case Colour.Yellow:
                    return Color.Yellow;
                case Colour.Aqua:
                    return Color.Aqua;
                case Colour.Ivory:
                    return Color.Ivory;
                case Colour.Indigo:
                    return Color.Indigo;
                default:
                    return Color.Transparent;
                    
            }
        }

        public void DrawRectangle(Position top_left_coordinate, int width, int height, Colour color)
        {
            SpriteBatch.Draw(WhitePixel, new Rectangle(top_left_coordinate.X, top_left_coordinate.Y, width, height), ColorToColourConverter(color));
        }

        public void DrawString(string text, Position top_left_coordinate, Colour color)
        {
            SpriteBatch.DrawString(DefaultFont, text, new Vector2(top_left_coordinate.X, top_left_coordinate.Y), ColorToColourConverter(color));
        }
    }

    class MonoGameGraphicsDeviceAdapter : IGraphicsDeviceManager
    {
        //Concrete implementation of Graphicsdevicemanager
        GraphicsDeviceManager Graphics;

        public MonoGameGraphicsDeviceAdapter(Game game)
        {
            this.Graphics = new GraphicsDeviceManager(game);
            game.Content.RootDirectory = "Content";
        }


        public void setHeihtandWidth(int prefered_height, int prefered_width)
        {
            Graphics.PreferredBackBufferHeight = prefered_height;
            Graphics.PreferredBackBufferWidth = prefered_width;
        }
    }
}
