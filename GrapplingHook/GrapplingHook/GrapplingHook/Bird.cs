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

        List<Mobile> Birds;
        

        public void UpdateBirds() {
            for (int i = 0; i < Birds.Count; i++) {
                var bird = Birds[i];

                var tileX = (int)((bird.position.X - bird.position.X % 16) / 16);
                var tileY = (int)((bird.position.Y - bird.position.Y % 16) / 16);

                if (bird.velocity.X < 0) {
                    if (tileX == 0) {
                        bird.velocity.X *= -1;
                    } else if (tilemap[tileX, tileY] == Tile.Wall || tilemap[tileX, tileY] == Tile.NoGrapple) {
                        bird.velocity.X *= -1;
                    }
                } else if (bird.velocity.X > 0) {
                    if (tileX == LEVEL_WIDTH - 1) {
                        bird.velocity.X *= -1;
                    } else if (tilemap[tileX + 1, tileY] == Tile.Wall || tilemap[tileX + 1, tileY] == Tile.NoGrapple) {
                        bird.velocity.X *= -1;
                    }
                }


                bird.position += bird.velocity;


            }
        }

        public void DrawBirds() {
            for (int i = 0; i < Birds.Count; i++) {
                var bird = Birds[i];
                spriteBatch.Draw(texBird, bird.position, Color.White);
            }
        }

    }
}
