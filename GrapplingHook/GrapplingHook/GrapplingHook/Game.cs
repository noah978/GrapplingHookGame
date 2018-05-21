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
        GameState prevState;

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
        List<Rectangle> windRs;
        int windTimer;

        SpriteFont font;

        Random random;

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

            windTimer = 0;
            windRs = new List<Rectangle>();

            //Logic
            state = GameState.Title;
            prevState = GameState.Title;
            level = 0;
            random = new Random();

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
            Sneakers = new List<Hitbox>();
            SneakerTimers = new List<int>();

            IsMouseVisible = true;

            CreateTitleScreen();
            CreateOptionsScreen();
            CreatePauseScreen();
            ChangeLevel(level);
        }
        
        protected override void LoadContent()
        {
            //Let's try to only put file loading and actual Content.Load calls here
            texPlayer = Content.Load<Texture2D>(@"Textures\" + "Player");
            texTileGoal = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + "Goal");
            texTileWallRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Ravine\" + "Wall");
            texTileOneWayPlatformRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Ravine\" + "OneWayPlatform");
            texTileNoGrappleRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Ravine\" + "NoGrapple");
            texTileWallTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Tower\" + "Wall");
            texTileOneWayPlatformTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Tower\" + "OneWayPlatform");
            texTileNoGrappleTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Tower\" + "NoGrapple");
            texTileSpikeRavine = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Ravine\" + "Spike");
            texTileSpikeTower = Content.Load<Texture2D>(@"Textures\" + @"Tiles\" + @"Tower\" + "Spike");
            texHook = Content.Load<Texture2D>(@"Textures\" + "Hook");
            texApple = Content.Load<Texture2D>(@"Textures\" + "Apple");
            font = Content.Load<SpriteFont>(@"Fonts\" + "Font");
            titleFont = Content.Load<SpriteFont>(@"Fonts\" + "Title");
            texMole = Content.Load<Texture2D>(@"Textures\Enemies\Grounder\" + "Mole");
            texBird = Content.Load<Texture2D>(@"Textures\Enemies\Flyer\" + "Bird");
            texSpider = Content.Load<Texture2D>(@"Textures\Enemies\Grounder\" + "Spider");
            texEye = Content.Load<Texture2D>(@"Textures\Enemies\Flyer\" + "Eye");
            texButton = Content.Load<Texture2D>(@"Textures\Interface\" + "Button");
            texWind = Content.Load<Texture2D>(@"Textures\" + @"Particles\" + "Wind");
            texMole = Content.Load<Texture2D>(@"Textures\" + @"Enemies\" + @"Grounder\" + "Mole");
            texBird = Content.Load<Texture2D>(@"Textures\" + @"Enemies\" + @"Flyer\" + "Bird");
            texWorm = Content.Load<Texture2D>(@"Textures\" + @"Enemies\" + @"Sneaker\" + "Worm");
            texSpider = Content.Load<Texture2D>(@"Textures\" + @"Enemies\" + @"Grounder\" + "Spider");
            texEye = Content.Load<Texture2D>(@"Textures\Enemies\Flyer\" + "Eye");
            texSpectre = Content.Load<Texture2D>(@"Textures\" + @"Enemies\" + @"Sneaker\" + "Spectre");
            titleFont = Content.Load<SpriteFont>(@"Fonts\" + "Title");
            texButton = Content.Load<Texture2D>(@"Textures\" +@"Interface\" + "Button");
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
                    if ((gamepad.Buttons.Back == ButtonState.Pressed && gamepadOld.Buttons.Back == ButtonState.Pressed) || (keyboard.IsKeyDown(Keys.Escape) && keyboardOld.IsKeyDown(Keys.Escape)))
                        this.Exit();
                    UpdateTitleScreen();
                    break;
                case GameState.Options:
                    UpdateOptionsScreen();
                    break;
                case GameState.Level:
                    if (keyboard.IsKeyDown(Keys.Escape))
                        state = GameState.Pause;
                    UpdatePlayer();
                    UpdateHook();
                    UpdateEnemies();
                    UpdateWind();
                    if (!IsActive)
                        state = GameState.Pause;
                    break;
                case GameState.Pause:
                    UpdatePauseScreen();
                    break;
                case GameState.Cutscene:
                    
                    break;
                case GameState.Credits:
                    
                    break;
            }
            
        }
        
        protected override void Draw(GameTime gameTime)
        {
            if (levelType == LevelType.Ravine)
                GraphicsDevice.Clear(new Color(153, 204, 255));
            else
                GraphicsDevice.Clear(new Color(0, 89, 179));

            var matrix = Matrix.CreateScale(2);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, matrix);

            switch (state)
            {
                case GameState.Title:
                    DrawTitleScreen();
                    //DrawCharacters();
                    break;
                case GameState.Options:
                    DrawOptions();
                    break;
                case GameState.Level:
                    DrawEnemies();
                    DrawTiles();
                    DrawApples();
                    if (!(windDir == Direction.None) && (windTimer % 15 == 0))
                        AddWindParticle(random);
                    DrawParticles();
                    DrawPlayer();
                    DrawHook();
                    DrawGUI();
                    break;
                case GameState.Pause:
                    DrawEnemies();
                    DrawTiles();
                    DrawApples();
                    if (!(windDir == Direction.None) && (windTimer % 15 == 0))
                        AddWindParticle(random);
                    DrawParticles();
                    DrawPlayer();
                    DrawHook();
                    DrawGUI();

                    DrawPauseScreen();
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

            if (windTimer < 60)
                windTimer++;
            else
                windTimer = 0;

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
            UpdateSneakers();
        }

        public void DrawEnemies() {
            DrawGrounders();
            DrawFlyers();
            DrawSneakers();
        }

        public void DrawParticles()
        {
            switch (state)
            {
                case GameState.Level:
                    DrawWind(windRs);
                    break;

            }
        }

        public void DrawGUI() {
            spriteBatch.Draw(texApple, new Vector2(12, 8), Color.White);
            spriteBatch.DrawString(font, "x " + appleCount, new Vector2(28, 8), Color.White);
        }

    }
}
