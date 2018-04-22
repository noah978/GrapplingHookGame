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
        Texture2D
            texTileSolid,
            texTileNoGrapple,
            texTileSpike,
            texTileWind;

        string[] levelNames;
        int level;
        Tile[,] tilemap;

        StreamReader file;

        List<Hitbox> TilesSolid;
        List<Hitbox> TilesSpike;
        List<Hitbox> TilesWindRight;
        List<Hitbox> TilesWindUp;
        List<Hitbox> TilesWindLeft;
        List<Hitbox> TilesWindDown;

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
                    result[i, j] = (Tile)Char.GetNumericValue(line[i]);
            }
            file.Close();
            return result;
        }

        public void ResetLevel() {
            for (var j = 0; j < LEVEL_HEIGHT; j++)
                for (var i = 0; i < LEVEL_WIDTH; i++)
                    switch (tilemap[i, j]) {
                        case Tile.Spawnpoint:
                            InitializePlayer(i * 16, j * 16);
                            break;
                        case Tile.Goal:
                            goal = new Mobile(i * 16, j * 16, 0, 0, 16, 16);
                            break;
                        case Tile.Solid:
                            TilesSolid.Add(new Mobile(i * 16, j * 16, 0, 0, 16, 16));
                            break;
                        case Tile.NoGrapple:
                            TilesSolid.Add(new Mobile(i * 16, j * 16, 0, 0, 16, 16));
                            break;
                        case Tile.Spike:
                            TilesSpike.Add(new Mobile(i * 16, j * 16, 0, 0, 16, 16));
                            break;
                        case Tile.WindRight:
                            TilesWindRight.Add(new Mobile(i * 16, j * 16, 0, 0, 16, 16));
                            break;
                        case Tile.WindUp:
                            TilesWindUp.Add(new Mobile(i * 16, j * 16, 0, 0, 16, 16));
                            break;
                        case Tile.WindLeft:
                            TilesWindLeft.Add(new Mobile(i * 16, j * 16, 0, 0, 16, 16));
                            break;
                        case Tile.WindDown:
                            TilesWindDown.Add(new Mobile(i * 16, j * 16, 0, 0, 16, 16));
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
                        case Tile.Solid:
                            texture = texTileSolid;
                            break;
                        case Tile.NoGrapple:
                            texture = texTileNoGrapple;
                            break;
                        case Tile.Spike:
                            texture = texTileSpike;
                            break;
                        case Tile.WindRight:
                            texture = texTileWind;
                            break;
                        case Tile.WindUp:
                            origin.X = 16;
                            texture = texTileWind;
                            rotation = MathHelper.Pi + MathHelper.PiOver2;
                            break;
                        case Tile.WindLeft:
                            origin.X = 16;
                            origin.Y = 16;
                            texture = texTileWind;
                            rotation = MathHelper.Pi;
                            break;
                        case Tile.WindDown:
                            origin.Y = 16;
                            texture = texTileWind;
                            rotation = MathHelper.PiOver2;
                            break;
                    }
                    if (texture != null)
                        spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
        }
        
    }
}
