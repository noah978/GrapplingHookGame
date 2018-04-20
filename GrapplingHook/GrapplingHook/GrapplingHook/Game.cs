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
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        //Logic
        GameState state;
        

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D
            texPixel;

        //Input
        KeyboardState keyboard;
        GamePadState gamepad;
        
        public Game()
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
            state = GameState.Level;
            level = null;

            //Graphics
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texPixel = new Texture2D(GraphicsDevice, 1, 1);
            texPixel.SetData(new[] { Color.White });

            tilemap = LoadLevel("test");

            IsMouseVisible = true;
            
        }
        
        protected override void LoadContent()
        {
            //Let's try to only put file loading and actual Content.Load calls here
            texTileSolid = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "Solid");
            texTileNoGrapple = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "NoGrapple");
            texPlayer = Content.Load<Texture2D>(@"Textures\" + "Player");
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
                    if (gamepad.Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                        this.Exit();
                    
                    break;
                case GameState.Options:
                    
                    break;
                case GameState.Level:
                    UpdatePlayer();
                    break;
                case GameState.Pause:
                    
                    break;
                case GameState.Cutscene:
                    
                    break;
                case GameState.Credits:
                    
                    break;
            }
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var matrix = Matrix.CreateScale(2);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, matrix);

            switch (state)
            {
                case GameState.Title:
                    DrawTiles();
                    //DrawCharacters();
                    break;
                case GameState.Options:
                    //DrawSoundOptions();
                    break;
                case GameState.Level:
                    DrawTiles();
                    //DrawEnemies();
                    //DrawApples();
                    //DrawParticles();
                    DrawPlayer();
                    break;
                case GameState.Pause:
                    DrawTiles();
                    //DrawEnemies();
                    //DrawApples();
                    //DrawParticles();
                    //DrawPlayer();
                    //DrawSoundOptions();
                    break;
                case GameState.Cutscene:
                    DrawTiles();
                    //DrawParticles();
                    //DrawCharacters();
                    //DrawLetterbox();
                    break;
                case GameState.Credits:
                    DrawTiles();
                    //DrawParticles();
                    //DrawCharacters();
                    //DrawLetterbox();
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }   

        



    }
}
