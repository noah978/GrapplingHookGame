using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        KeyboardState keyboardOld;
        GamePadState gamepad;
        GamePadState gamepadOld;

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
            level = 0;

            //Graphics
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texPixel = new Texture2D(GraphicsDevice, 1, 1);
            texPixel.SetData(new[] { Color.White });
            
            levelNames = Directory.GetFiles(Content.RootDirectory + @"\Levels\");
            Array.Sort(levelNames);

            TilesSolid = new List<Hitbox>();
            TilesOneWayPlatform = new List<Hitbox>();
            TilesSpike = new List<Hitbox>();
            TilesRightWind = new List<Hitbox>();
            TilesUpWind = new List<Hitbox>();
            TilesLeftWind = new List<Hitbox>();
            TilesDownWind = new List<Hitbox>();

            IsMouseVisible = true;

            ChangeLevel(level);
        }
        
        protected override void LoadContent()
        {
            //Let's try to only put file loading and actual Content.Load calls here
            texPlayer = Content.Load<Texture2D>(@"Textures\" + "Player");
            texTileGoal = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "Goal");
            texTileWall = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "Wall");
            texTileOneWayPlatform = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "OneWayPlatform");
            texTileNoGrapple = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "NoGrapple");
            texTileSpike = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "Spike");
            texTileWind = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "Wind");
        }
        
        protected override void UnloadContent() {}
        
        protected override void Update(GameTime gameTime)
        {
            keyboardOld = keyboard;
            gamepadOld = gamepad;
            keyboard = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            if (keyboard.IsKeyDown(Keys.OemPeriod) && keyboardOld.IsKeyUp(Keys.OemPeriod)) {
                ChangeLevel(++level);
            }

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
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, matrix);

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
