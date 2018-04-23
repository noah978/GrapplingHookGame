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

        List<Mobile> Moles;
        

        public void UpdateMoles() {
            for (int i = 0; i < Moles.Count; i++) {
                var mole = Moles[i];

                var tileX = (int)((mole.position.X - mole.position.X % 16) / 16);
                var tileY = (int)((mole.position.Y - mole.position.Y % 16) / 16);

                if (mole.velocity.X < 0) {
                    if (tileX == 0) {
                        mole.velocity.X *= -1;
                    } else if (tilemap[tileX, tileY] == Tile.Wall || tilemap[tileX, tileY] == Tile.NoGrapple) {
                        mole.velocity.X *= -1;
                    } else if (tilemap[tileX, tileY + 1] != Tile.Wall && tilemap[tileX, tileY + 1] != Tile.NoGrapple && tilemap[tileX, tileY + 1] != Tile.OneWayPlatform) {
                        mole.velocity.X *= -1;
                    }
                } else if (mole.velocity.X > 0) {
                    if (tileX == LEVEL_WIDTH - 1) {
                        mole.velocity.X *= -1;
                    } else if (tilemap[tileX + 1, tileY] == Tile.Wall || tilemap[tileX + 1, tileY] == Tile.NoGrapple) {
                        mole.velocity.X *= -1;
                    }
                    else if (tilemap[tileX + 1, tileY + 1] != Tile.Wall && tilemap[tileX + 1, tileY + 1] != Tile.NoGrapple && tilemap[tileX + 1, tileY + 1] != Tile.OneWayPlatform) {
                        mole.velocity.X *= -1;
                    }
                }
                

                mole.position += mole.velocity;


            }
        }

        public void DrawMoles() {
            for (int i = 0; i < Moles.Count; i++) {
                var mole = Moles[i];
                spriteBatch.Draw(texMole, mole.position, Color.White);
            }
        }

    }
}
