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
            texTileSpikeTower;

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

            if (level < LEVEL_TYPE_SHIFT)
                levelType = LevelType.Ravine;
            else if (level < LEVEL_BOSS)
                levelType = LevelType.Tower;
            else if (level == LEVEL_BOSS)
                levelType = LevelType.Boss;

            tilemap = LoadTilemap(levelNames[id]);
            ResetLevel();
            windDir = LoadWind(levelNames[id]);
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
            windDir = Direction.None;

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
                            TilesSpike.Add(new Hitbox(i * 16, j * 16, 16, 16));
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
                            texture = (levelType == LevelType.Ravine ? texTileWallRavine : texTileWallTower);
                            break;
                        case Tile.NoGrapple:
                            texture = (levelType == LevelType.Ravine ? texTileNoGrappleRavine : texTileNoGrappleTower);
                            break;
                        case Tile.OneWayPlatform:
                            texture = (levelType == LevelType.Ravine ? texTileOneWayPlatformRavine : texTileOneWayPlatformTower);
                            break;
                        case Tile.Spike:
                            texture = (levelType == LevelType.Ravine ? texTileSpikeRavine : texTileSpikeTower);
                            break;
                    }
                    if (texture != null)
                        spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        }

        public void DrawWind(int r)
        {

        }
    }
}
