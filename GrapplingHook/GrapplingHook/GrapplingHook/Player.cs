using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GrapplingHook.Logic;

namespace GrapplingHook {
    partial class Game {
        Texture2D texPlayer;
        Mobile player;
        PlayerState playerState;
        
        public void UpdatePlayer() {
            player.velocity.X = MathHelper.Clamp(
                player.velocity.X + PLAYER_ACCELERATION * ((keyboard.IsKeyDown(Keys.Right) ? 1 : 0) - (keyboard.IsKeyDown(Keys.Left) ? 1 : 0)),
                -3,
                3);
            //Apply friction

            if (playerState == PlayerState.OnGround) {
                if (player.velocity.X < -PLAYER_FRICTION_GROUND || player.velocity.X > PLAYER_FRICTION_GROUND) {
                    player.velocity.X = player.velocity.X - PLAYER_FRICTION_GROUND * Math.Sign(player.velocity.X);
                }
                else {
                    player.velocity.X = 0;
                }
            } else {
                player.velocity.Y += GRAVITY;
            }

            {
                List<Point> tileCoordsToCheck = new List<Point>();

                var moddedX = player.position.X % 16;
                var playerTileX = (int)(moddedX / 16);
                var isAlignedX = moddedX == 0;

                var moddedY = player.position.Y % 16;
                var playerTileY = (int)(moddedY / 16);
                var isAlignedY = moddedY == 0;


                if (isAlignedY)
                {
                    if (player.velocity.Y >= 0) {
                        tileCoordsToCheck.Add(new Point(playerTileX, playerTileY + 1));
                        if (!isAlignedX)
                        {
                            tileCoordsToCheck.Add(new Point(playerTileX + 1, playerTileY + 1));
                        }
                    }
                    if (player.velocity.Y < 0)
                    {

                        tileCoordsToCheck.Add(new Point(playerTileX, playerTileY - 1));
                        if (isAlignedX)
                        {
                            tileCoordsToCheck.Add(new Point(playerTileX + 1, playerTileY - 1));
                        }
                    }
                } else {

                }
                
            }
            
        }

        public void DrawPlayer() {
            spriteBatch.Draw(texPlayer, player.position, Color.White);
        }

    }
}
