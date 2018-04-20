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
            texTileNoGrapple;

        int? level;
        Tile[,] tilemap;

        StreamReader file;
        
        public Tile[,] LoadLevel(string id) {
            file = new StreamReader(Content.RootDirectory + @"\Levels\" + id + @".dat");
            Tile[,] result = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];
            

            for (var j = 0; j < LEVEL_HEIGHT; j++) {
                var line = file.ReadLine();
                
                for (var i = 0; i < LEVEL_WIDTH; i++) {
                    var tile = (Tile)Char.GetNumericValue(line[i]);

                    switch (tile) {
                        case Tile.Solid:
                        case Tile.NoGrapple:
                            
                            break;
                        case Tile.Spawnpoint:
                            if (player == null)
                                player = new Mobile(i * 16, j * 16, 0, 0, 16, 16);
                            else {
                                player.position.X = i * 16;
                                player.position.Y = j * 16;
                            }
                            playerState = PlayerState.OnGround;
                            break;
                    }

                    result[i, j] = tile;
                }
            }

            file.Close();
            return result;
        }


        public void DrawTiles() {
            for (var i = 0; i < LEVEL_WIDTH; i++) {
                var position = new Vector2(i * TILE_WIDTH, 0);
                for (var j = 0; j < LEVEL_HEIGHT; j++) {
                    position.Y = j * TILE_HEIGHT;
                    Texture2D texture = null;

                    switch (tilemap[i, j]) {
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
