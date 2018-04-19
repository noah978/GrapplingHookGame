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

        const int VIEWPORT_WIDTH = 320;
        const int VIEWPORT_HEIGHT = 240;

        const int LEVEL_WIDTH = 20;
        const int LEVEL_HEIGHT = 15;

        const int TILE_WIDTH = 16;
        const int TILE_HEIGHT = 16;

        //Logic
        GameState state;
        int? level;
        int[,] tilemap;

        //Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D
            texPixel,
            texTileSolid,
            texTileNoGrapple;

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

            tilemap = new int[20, 15];
            tilemap[3, 14] = 1;
            tilemap[4, 14] = 1;
            tilemap[5, 14] = 1;
            tilemap[6, 14] = 1;
            tilemap[7, 14] = 1;
            tilemap[8, 14] = 2;
            tilemap[9, 14] = 2;
            tilemap[10, 14] = 2;
        }
        
        protected override void LoadContent()
        {
            //Let's try to only put file loading and actual Content.Load calls here
            texTileSolid = Content.Load<Texture2D>(@"Textures\Tiles\" + "Solid");
            texTileNoGrapple = Content.Load<Texture2D>(@"Textures\Tiles\" + "NoGrapple");
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
                    //DrawPlayer();
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

        public void DrawTiles() {
            for (var i = 0; i < LEVEL_WIDTH; i++) {
                var position = new Vector2(i * TILE_WIDTH, 0);
                for (var j = 0; j < LEVEL_HEIGHT; j++) {
                    position.Y = j * TILE_HEIGHT;
                    Texture2D texture = null;

                    switch ((Tile)tilemap[i, j]) {
                        case Tile.Solid:
                            texture = texTileSolid;
                            break;
                        case Tile.NoGrapple:
                            texture = texTileNoGrapple;
                            break;
                    }
                    if (texture != null)
                        spriteBatch.Draw(texture, position, Color.White);
                }
            }
        }



    }
}
