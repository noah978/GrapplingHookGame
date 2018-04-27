using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrapplingHook.Logic;

namespace GrapplingHook {
    partial class Game {
        Texture2D texMole;
        Texture2D texSpider;

        List<Mobile> Grounders;
        

        public void UpdateGrounders() {
            for (int i = 0; i < Grounders.Count; i++) {
                var grounder = Grounders[i];

                var tileX = (int)((grounder.position.X - grounder.position.X % 16) / 16);
                var tileY = (int)((grounder.position.Y - grounder.position.Y % 16) / 16);

                if (grounder.velocity.X < 0) {
                    if (tileX == 0) {
                        grounder.velocity.X *= -1;
                    } else if (tilemap[tileX, tileY] == Tile.Wall || tilemap[tileX, tileY] == Tile.NoGrapple) {
                        grounder.velocity.X *= -1;
                    } else if (tilemap[tileX, tileY + 1] != Tile.Wall && tilemap[tileX, tileY + 1] != Tile.NoGrapple && tilemap[tileX, tileY + 1] != Tile.OneWayPlatform) {
                        grounder.velocity.X *= -1;
                    }
                } else if (grounder.velocity.X > 0) {
                    if (tileX == LEVEL_WIDTH - 1) {
                        grounder.velocity.X *= -1;
                    } else if (tilemap[tileX + 1, tileY] == Tile.Wall || tilemap[tileX + 1, tileY] == Tile.NoGrapple) {
                        grounder.velocity.X *= -1;
                    }
                    else if (tilemap[tileX + 1, tileY + 1] != Tile.Wall && tilemap[tileX + 1, tileY + 1] != Tile.NoGrapple && tilemap[tileX + 1, tileY + 1] != Tile.OneWayPlatform) {
                        grounder.velocity.X *= -1;
                    }
                }
                

                grounder.position += grounder.velocity;


            }
        }

        public void DrawGrounders() {
            for (int i = 0; i < Grounders.Count; i++) {
                var grounder = Grounders[i];
                spriteBatch.Draw((levelType == LevelType.Canyon ? texMole : texSpider), grounder.position, Color.White);
            }
        }

    }
}
