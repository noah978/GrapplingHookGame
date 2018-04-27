using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrapplingHook.Logic;

namespace GrapplingHook {
    partial class Game {
        Texture2D texBird;
        Texture2D texEye;

        List<Mobile> Flyer;
        

        public void UpdateFlyers() {
            for (int i = 0; i < Flyer.Count; i++) {
                var flyer = Flyer[i];

                var tileX = (int)((flyer.position.X - flyer.position.X % 16) / 16);
                var tileY = (int)((flyer.position.Y - flyer.position.Y % 16) / 16);

                if (flyer.velocity.X < 0) {
                    if (tileX == 0) {
                        flyer.velocity.X *= -1;
                    } else if (tilemap[tileX, tileY] == Tile.Wall || tilemap[tileX, tileY] == Tile.NoGrapple) {
                        flyer.velocity.X *= -1;
                    }
                } else if (flyer.velocity.X > 0) {
                    if (tileX == LEVEL_WIDTH - 1) {
                        flyer.velocity.X *= -1;
                    } else if (tilemap[tileX + 1, tileY] == Tile.Wall || tilemap[tileX + 1, tileY] == Tile.NoGrapple) {
                        flyer.velocity.X *= -1;
                    }
                }


                flyer.position += flyer.velocity;


            }
        }

        public void DrawFlyers() {
            for (int i = 0; i < Flyer.Count; i++) {
                var flyer = Flyer[i];
                spriteBatch.Draw((levelType == LevelType.Canyon ? texBird : texEye), flyer.position, Color.White);
            }
        }

    }
}
