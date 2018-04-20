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
                player.velocity.Y += PLAYER_GRAVITY;
            }

            {
                var tempPos = player.position;

                player.position += player.velocity;

                foreach (AABB box in tileBounds) {
                    if (player.Intersects(box)) {

                        if (player.velocity.X > 0) {

                        }

                    }
                }
            }
            
        }

        public void DrawPlayer() {
            spriteBatch.Draw(texPlayer, player.position, Color.White);
        }

    }
}
