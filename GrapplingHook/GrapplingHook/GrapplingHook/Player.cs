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
        Vector2 preTensionVelocity;


        List<Hitbox> collisionsSolid;

        public void InitializePlayer(float x, float y) {
            player = new Mobile(x + 2, y + 2, 0, 0, 12, 12);
            playerState = PlayerState.OnGround;
            collisionsSolid = new List<Hitbox>();
            initializeHook();
        }

        public void UpdatePlayer() {
            player.velocity.X = MathHelper.Clamp(
                player.velocity.X + PLAYER_ACCELERATION * ((keyboard.IsKeyDown(Keys.Right) ? 1 : 0) - (keyboard.IsKeyDown(Keys.Left) ? 1 : 0)),
                -PLAYER_MAX_SPEED_X,
                PLAYER_MAX_SPEED_X);
            
            if (playerState == PlayerState.OnGround) {
                //Friction
                if (player.velocity.X < -PLAYER_FRICTION_GROUND || player.velocity.X > PLAYER_FRICTION_GROUND) {
                    player.velocity.X = player.velocity.X - PLAYER_FRICTION_GROUND * Math.Sign(player.velocity.X);
                }
                else {
                    player.velocity.X = 0;
                }
                //Jumping
                if (keyboard.IsKeyDown(Keys.Up) && keyboardOld.IsKeyUp(Keys.Up)) {
                    player.velocity.Y = PLAYER_JUMP;
                    playerState = PlayerState.InAir;
                }
            } else if (playerState == PlayerState.InAir) {
                //Friction
                if (player.velocity.X < -PLAYER_FRICTION_AIR || player.velocity.X > PLAYER_FRICTION_AIR) {
                    player.velocity.X = player.velocity.X - PLAYER_FRICTION_AIR * Math.Sign(player.velocity.X);
                }
                else {
                    player.velocity.X = 0;
                }
            }

            //Gravity
            player.velocity.Y += GRAVITY;

            //Grappling hook tension
            preTensionVelocity = player.velocity;
            if (hookState == HookState.Hooked)
            {
                Vector2 oldNextPosition = player.Center + player.velocity;
                Vector2 newNextPositionRelative = oldNextPosition - hook.Center;
                if (newNextPositionRelative.Length() >= ropeLength)
                {
                    newNextPositionRelative.Normalize();
                    newNextPositionRelative.X *= (float)ropeLength;
                    newNextPositionRelative.Y *= (float)ropeLength;
                    Vector2 newNextPosition = hook.Center + newNextPositionRelative;
                    float velocityMagnitude = player.velocity.Length();
                    player.velocity = newNextPosition - player.Center;
                    player.velocity.Normalize();
                    player.velocity *= velocityMagnitude;
                }
                //Something's wrong here. It's not behaving correctly when swinging idly
            }

            //Collision with wind tiles
            var windRight = false;
            for (int i = 0; i < TilesRightWind.Count; i++) {
                var tile = TilesRightWind[i];
                if (player.Intersects(tile)) {
                    windRight = true;
                    break;
                }
            }
            var windLeft = false;
            for (int i = 0; i < TilesLeftWind.Count; i++) {
                var tile = TilesLeftWind[i];
                if (player.Intersects(tile)) {
                    windLeft = true;
                    break;
                }
            }
            var windDown = false;
            for (int i = 0; i < TilesDownWind.Count; i++) {
                var tile = TilesDownWind[i];
                if (player.Intersects(tile)) {
                    windDown = true;
                    break;
                }
            }
            var windUp = false;
            for (int i = 0; i < TilesUpWind.Count; i++) {
                var tile = TilesUpWind[i];
                if (player.Intersects(tile)) {
                    windUp = true;
                    break;
                }
            }

            player.velocity.X += WIND_STRENGTH * ((windRight ? 1 : 0) - (windLeft ? 1 : 0));
            player.velocity.Y += WIND_STRENGTH * ((windDown ? 1 : 0) - (windUp ? 1 : 0));

            //Collision with solid tiles
            for (int i = 0; i < TilesSolid.Count; i++) {
                var tile = TilesSolid[i];
                if (player.WillIntersect(tile)) {
                    var oldVelX = player.velocity.X;
                    player.velocity.X = 0;
                    if (player.velocity.Y > 0) {
                        if (player.WillIntersect(tile)) {
                            player.position.Y = tile.Up - player.bounds.Y;
                            player.velocity.Y = 0;
                            playerState = PlayerState.OnGround;
                        }
                    }
                    else if (player.velocity.Y < 0) {
                        if (player.WillIntersect(tile)) {
                            player.position.Y = tile.Down;
                            player.velocity.Y = 0;
                        }
                    }
                    player.velocity.X = oldVelX;
                    var oldVelY = player.velocity.Y;
                    player.velocity.Y = 0;
                    if (player.velocity.X > 0) {
                        if (player.WillIntersect(tile)) {
                            player.position.X = tile.Left - player.bounds.X;
                            player.velocity.X = 0;
                        }
                    }
                    else if (player.velocity.X < 0) {
                        if (player.WillIntersect(tile)) {
                            player.position.X = tile.Right;
                            player.velocity.X = 0;
                        }
                    }
                    player.velocity.Y = oldVelY;
                }
            }

            //Collision with one-way platforms
            for (int i = 0; i < TilesOneWayPlatform.Count; i++) {
                var tile = TilesOneWayPlatform[i];
                if (player.WillIntersect(tile)) {
                    var oldVelX = player.velocity.X;
                    player.velocity.X = 0;
                    if (player.velocity.Y > 0 && player.Down <= tile.Up) {
                        if (player.WillIntersect(tile)) {
                            player.position.Y = tile.Up - player.bounds.Y;
                            player.velocity.Y = 0;
                            playerState = PlayerState.OnGround;
                        }
                    }
                    player.velocity.X = oldVelX;
                }
            }

            if (playerState == PlayerState.OnGround) {
                if (player.velocity.Y != 0) {
                    playerState = PlayerState.InAir;
                }
            }
            
            player.position += player.velocity;
            
        }

        public void DrawPlayer() {
            spriteBatch.Draw(texPlayer, player.position, Color.White);
            DrawLine(player.position, (player.position + player.velocity * 50), Color.White);
            DrawLine(player.position, (player.position + preTensionVelocity * 50), Color.Green);
        }

    }
}
