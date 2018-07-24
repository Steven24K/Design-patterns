using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GUIapp
{
    //The game class, initilizes, draws and update the entire game and all game elements.
    public class Game1 : Game
    {
        IGraphicsDeviceManager AdaptedGraphics;
        SpriteBatch spriteBatch;
        public Game1()
        {
            AdaptedGraphics = new MonoGameGraphicsDeviceAdapter(this); //Sets the graphics device manager from monogame, including the content manager
            AdaptedGraphics.setHeihtandWidth(1080,1920); //Sets the height and the width from the monogame window
        }

        //Initilizes the visitors and guimanager
        GuiManager GuiManager;
        IDrawVisitor DrawVisitor;
        IUpdateVisitor UpdateVisitor;
        protected override void Initialize()
        {
            base.Initialize();
            base.IsMouseVisible = true;

            GuiFactory guiFactory = new ConcreteGuiFactory(); 
            GuiManager = guiFactory.Instantiate("None",()=>Exit());//This is from where the Guimanager is created
            UpdateVisitor = new DefaultUpdateVisitor(new MonoGameInputManager()); //The update visitor must contain an inputmanager
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DrawVisitor = new DefaultDrawVisitor(new MonoGameDrawAdapter(spriteBatch,Content)); //The draw visitor must contain a draw adapter
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Updates the entire game
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            GuiManager.Update(UpdateVisitor, dt);
            
            base.Update(gameTime);
        }

        //Draws the whole game
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            spriteBatch.Begin();

            GuiManager.Draw(DrawVisitor);
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
