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
        MouseState mouse;
        MouseState mouseOld;

        int appleCount;

        SpriteFont font;

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

            Apples = new List<Hitbox>();

            Grounders = new List<Mobile>();
            Flyer = new List<Mobile>();

            IsMouseVisible = true;

            ChangeLevel(level);
        }
        
        protected override void LoadContent()
        {
            //Let's try to only put file loading and actual Content.Load calls here
            texPlayer = Content.Load<Texture2D>(@"Textures\" + "Player");
            texTileGoal = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "Goal");
            texTileWallRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Ravine\" + "Wall");
            texTileOneWayPlatformRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Ravine\" + "OneWayPlatform");
            texTileNoGrappleRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Ravine\" + "NoGrapple");
            texTileWallTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Tower\" + "Wall");
            texTileOneWayPlatformTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Tower\" + "OneWayPlatform");
            texTileNoGrappleTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Tower\" + "NoGrapple");
            texTileSpikeRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Ravine\" + "Spike");
            texTileSpikeTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\Tower\" + "Spike");
            texHook = Content.Load<Texture2D>(@"Textures\" + "Hook");
            texApple = Content.Load<Texture2D>(@"Textures\" + "Apple");
            font = Content.Load<SpriteFont>(@"Fonts\" + "Font");
            texMole = Content.Load<Texture2D>(@"Textures\Enemies\Grounder\" + "Mole");
            texBird = Content.Load<Texture2D>(@"Textures\Enemies\Flyer\" + "Bird");
            texSpider = Content.Load<Texture2D>(@"Textures\Enemies\Grounder\" + "Spider");
            texEye = Content.Load<Texture2D>(@"Textures\Enemies\Flyer\" + "Eye");
        }
        
        protected override void UnloadContent() {}
        
        protected override void Update(GameTime gameTime)
        {
            keyboardOld = keyboard;
            gamepadOld = gamepad;
            mouseOld = mouse;
            keyboard = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);
            mouse = Mouse.GetState();

            if (keyboard.IsKeyDown(Keys.OemPeriod) && keyboardOld.IsKeyUp(Keys.OemPeriod)) {
                level = (level + 1) % levelNames.Length;
                ChangeLevel(level);
            }
            if (keyboard.IsKeyDown(Keys.OemComma) && keyboardOld.IsKeyUp(Keys.OemComma)) {
                level = (level - 1) % levelNames.Length;
                ChangeLevel(level);
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
                    UpdateHook();
                    UpdateEnemies();
                    break;
                case GameState.Pause:
                    
                    break;
                case GameState.Cutscene:
                    
                    break;
                case GameState.Credits:
                    
                    break;
            }
            
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
                    DrawEnemies();
                    DrawApples();
                    //DrawParticles();
                    DrawPlayer();
                    DrawHook();
                    DrawWind();
                    DrawGUI();
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
            
        }

        public void DrawLine(Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(texPixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void UpdateEnemies() {
            UpdateGrounders();
            UpdateFlyers();
        }

        public void DrawEnemies() {
            DrawGrounders();
            DrawFlyers();
        }

        public void DrawGUI() {
            spriteBatch.Draw(texApple, new Vector2(16, 16), Color.White);
            spriteBatch.DrawString(font, "x " + appleCount, new Vector2(28, 8), Color.White);
        }


    }
}
