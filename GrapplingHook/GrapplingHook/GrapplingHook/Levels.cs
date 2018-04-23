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
        const string TileLetters = "-PGWNOSRDLUMB";

        Texture2D
            texTileGoal,
            texTileWall,
            texTileOneWayPlatform,
            texTileNoGrapple,
            texTileSpike,
            texTileWind;

        string[] levelNames;
        int level;
        Tile[,] tilemap;

        StreamReader file;

        List<Hitbox>
            TilesSolid,
            TilesSpike,
            TilesOneWayPlatform,
            TilesRightWind,
            TilesUpWind,
            TilesLeftWind,
            TilesDownWind;

        Hitbox goal;
        
        public void ChangeLevel(int id) {
            level = id;
            tilemap = LoadTilemap(levelNames[id]);
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

        public void ResetLevel() {
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
                        case Tile.RightWind:
                            TilesRightWind.Add(new Hitbox(i * 16, j * 16, 16, 16));
                            break;
                        case Tile.DownWind:
                            TilesDownWind.Add(new Hitbox(i * 16, j * 16, 16, 16));
                            break;
                        case Tile.LeftWind:
                            TilesLeftWind.Add(new Hitbox(i * 16, j * 16, 16, 16));
                            break;
                        case Tile.UpWind:
                            TilesUpWind.Add(new Hitbox(i * 16, j * 16, 16, 16));
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
                            texture = texTileWall;
                            break;
                        case Tile.NoGrapple:
                            texture = texTileNoGrapple;
                            break;
                        case Tile.OneWayPlatform:
                            texture = texTileOneWayPlatform;
                            break;
                        case Tile.Spike:
                            texture = texTileSpike;
                            break;
                        case Tile.RightWind:
                            texture = texTileWind;
                            break;
                        case Tile.DownWind:
                            origin.Y = 16;
                            texture = texTileWind;
                            rotation = MathHelper.PiOver2;
                            break;
                        case Tile.LeftWind:
                            origin.X = 16;
                            origin.Y = 16;
                            texture = texTileWind;
                            rotation = MathHelper.Pi;
                            break;
                        case Tile.UpWind:
                            origin.X = 16;
                            texture = texTileWind;
                            rotation = MathHelper.Pi + MathHelper.PiOver2;
                            break;
                    }
                    if (texture != null)
                        spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        
    }
}
