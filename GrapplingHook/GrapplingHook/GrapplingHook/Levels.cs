using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrapplingHook.Logic;

namespace GrapplingHook {
    public partial class Game {
        const string TileLetters = "-PGWNOSARFB";

        // - = Empty; p = Player; G = NonGrappleable; O = One Way Platform; S = Spike; A = Apple; R = Grounder; F = Flyer; B = Sneaker

        LevelType levelType;

        Texture2D
            texTileGoal,
            texTileWallRavine,
            texTileWallTower,
            texTileOneWayPlatformRavine,
            texTileOneWayPlatformTower,
            texTileNoGrappleRavine,
            texTileNoGrappleTower,
            texTileSpikeRavine,
            texTileSpikeTower,
            texWind;

        string[] levelNames;
        int level;
        Tile[,] tilemap;
        Direction windDir;

        StreamReader file;

        List<Hitbox>
            TilesSolid,
            TilesSpike,
            TilesOneWayPlatform;

        Hitbox goal;
        
        public void ChangeLevel(int id) {
            level = id;
            totalAppleCount += appleCount;
            appleCount = 0;

            if (level < LEVEL_TYPE_SHIFT)
                levelType = LevelType.Ravine;
            else if (level == LEVEL_TYPE_SHIFT)
                levelType = LevelType.Transition;
            else if (level < LEVEL_BOSS)
                levelType = LevelType.Tower;
            else if (level == LEVEL_BOSS)
                levelType = LevelType.Boss;

            tilemap = LoadTilemap(levelNames[id]);
            windDir = LoadWind(levelNames[id]);
            ResetLevel();
        }

        public Tile[,] LoadTilemap(string path) {
            file = new StreamReader(path);
            Tile[,] result = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];
            for (var j = 0; j < LEVEL_HEIGHT; j++) {
                var line = file.ReadLine();
                for (var i = 0; i < LEVEL_WIDTH; i++)
                    result[i, j] = (Tile)TileLetters.IndexOf(line[i]);
            }
            file.Close();
            return result;
        }

        public Direction LoadWind(string path)
        {
            file = new StreamReader(path);
            for (var j = 0; j < LEVEL_HEIGHT; j++)
                file.ReadLine();
            char wind = ((file.ReadLine()).ToCharArray()[0]);
            switch (wind)
            {  
                case 'L': 
                    return Direction.Left;
                case 'R':
                    return Direction.Right;
                default:
                    return Direction.None;
            }
        }

        public void ResetLevel() {
            TilesSolid.Clear();
            TilesOneWayPlatform.Clear();
            TilesSpike.Clear();
            Apples.Clear();
            Grounders.Clear();
            Flyer.Clear();
            windRs.Clear();
            Sneakers.Clear();
            SneakerTimers.Clear();
<<<<<<< HEAD
            //windDir = Direction.None;
=======
            if (windDir != Direction.None)
                PreLoadWind(random);
>>>>>>> master

            for (var j = 0; j < LEVEL_HEIGHT; j++)
                for (var i = 0; i < LEVEL_WIDTH; i++)
                    switch (tilemap[i, j]) {
                        case Tile.Player:
                            InitializePlayer(i * 16, j * 16);
                            break;
                        case Tile.Goal:
                            goal = new Hitbox(i * 16, j * 16, 16, 16);
                            break;
                        case Tile.Wall:
                            TilesSolid.Add(new Hitbox(i * 16, j * 16, 16, 16));
                            break;
                        case Tile.NoGrapple:
                            TilesSolid.Add(new Hitbox(i * 16, j * 16, 16, 16));
                            break;
                        case Tile.OneWayPlatform:
                            TilesOneWayPlatform.Add(new Hitbox(i * 16, j * 16, 16, 16));
                            break;
                        case Tile.Spike:
                            TilesSpike.Add(new Hitbox(i * 16, j * 16 + 8, 16, 8));
                            break;
                        case Tile.Apple:
                            Apples.Add(new Hitbox(i * 16 + 2, j * 16 + 2, 12, 12));
                            break;
                        case Tile.Grounder:
                            Grounders.Add(new Mobile(i * 16, j * 16, GROUNDER_SPEED, 0, 16, 16));
                            break;
                        case Tile.Flyer:
                            Flyer.Add(new Mobile(i * 16, j * 16, FLYER_SPEED, 0, 16, 16));
                            break;
                        case Tile.Sneaker:
                            Sneakers.Add(new Hitbox(i * 16, j * 16, 16, 16));
                            SneakerTimers.Add(-1);
                            break;
                    }
        }

        public void DrawTiles() {
            for (var i = 0; i < LEVEL_WIDTH; i++) {
                var position = new Vector2(i * TILE_WIDTH, 0);
                for (var j = 0; j < LEVEL_HEIGHT; j++) {
                    position.Y = j * TILE_HEIGHT;
                    Texture2D texture = null;

                    Vector2 origin = Vector2.Zero;
                    float rotation = 0;

                    switch (tilemap[i, j]) {
                        case Tile.Goal:
                            texture = texTileGoal;
                            break;
                        case Tile.Wall:
                            texture = (levelType == LevelType.Ravine || (levelType == LevelType.Transition && j >= 9) ? texTileWallRavine : texTileWallTower);
                            break;
                        case Tile.NoGrapple:
                            texture = (levelType == LevelType.Ravine || (levelType == LevelType.Transition && j >= 9) ? texTileNoGrappleRavine : texTileNoGrappleTower);
                            break;
                        case Tile.OneWayPlatform:
                            texture = (levelType == LevelType.Ravine || (levelType == LevelType.Transition && j >= 9) ? texTileOneWayPlatformRavine : texTileOneWayPlatformTower);
                            break;
                        case Tile.Spike:
                            texture = (levelType == LevelType.Ravine || (levelType == LevelType.Transition && j >= 9) ? texTileSpikeRavine : texTileSpikeTower);
                            break;
                    }
                    if (texture != null)
                        spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        public void AddWindParticle(Random rand)
        {
            switch (windDir)
            {
                case Direction.Right:
                    windRs.Add(new Rectangle(-20, rand.Next(10, VIEWPORT_HEIGHT - 10), 20, 2));
                    break;
                case Direction.Left:
                    windRs.Add(new Rectangle(VIEWPORT_WIDTH, rand.Next(10, VIEWPORT_HEIGHT - 10), 20, 2));
                    break;
            }
        }
        public void UpdateWind()
        {
            for (int i = 0; i < windRs.Count; i++)
            {
                if (windRs[i].X < -20 || windRs[i].X > VIEWPORT_WIDTH)
                    windRs.RemoveAt(i);
            }
            for (int i = 0; i < windRs.Count; i++)
            {
                switch (windDir)
                {
                    case Direction.Right:
                        windRs[i] = new Rectangle(windRs[i].X + (int)Math.Round(WIND_STRENGTH * 4), windRs[i].Y, 20, 2);
                        break;
                    case Direction.Left:
                        windRs[i] = new Rectangle(windRs[i].X - (int)Math.Round(WIND_STRENGTH * 4), windRs[i].Y, 20, 2);
                        break;
                }
            }
        }

        public void DrawWind(List<Rectangle> rs)
        {
            foreach (Rectangle r in windRs)
            {
                spriteBatch.Draw(texWind, r, Color.WhiteSmoke);
            }
        }
        public void PreLoadWind(Random rand)
        {
            for (int i = 0; i < 15; i++)
            {
                AddWindParticle(rand);
                for (int j = 0; j < 10; j++)
                    UpdateWind();
            }
        }
    }
}
