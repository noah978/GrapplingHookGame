using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using GrapplingHook.Logic;

namespace GrapplingHook
{
    enum GameState
    {
        Title,
        Options,
        Level,
        Pause,
        Cutscene,
        Credits
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Constants
        const int WINDOW_WIDTH = 640;
        const int WINDOW_HEIGHT = 480;

        //Logic
        GameState state;
        int? level;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texPixel;

        //Input
        KeyboardState keyboard;
        GamePadState gamepad;
        
        public Game1()
        {
            //Let's try not to add anything here
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize(); //Calls LoadContent

            //Logic
            state = GameState.Title;
            level = null;

            //Graphics
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texPixel = new Texture2D(GraphicsDevice, 1, 1);
            texPixel.SetData(new[] { Color.White });

        }
        
        protected override void LoadContent()
        {
            //Let's try to only put file loading and actual Content.Load calls here
            
        }
        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            switch (state)
            {
                case GameState.Title:

                    break;
                case GameState.Options:

                    break;
                case GameState.Level:

                    break;
                case GameState.Pause:

                    break;
                case GameState.Cutscene:

                    break;
                case GameState.Credits:

                    break;
            }

            if (gamepad.Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                this.Exit();
            

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();

            switch (state)
            {
                case GameState.Title:

                    break;
                case GameState.Options:

                    break;
                case GameState.Level:

                    break;
                case GameState.Pause:

                    break;
                case GameState.Cutscene:

                    break;
                case GameState.Credits:

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
