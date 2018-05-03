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
        float deathTimer;

        List<Hitbox> collisionsSolid;

        public void InitializePlayer(float x, float y) {
            player = new Mobile(x + 2, y + 2, 0, 0, 12, 12);
            playerState = PlayerState.OnGround;
            collisionsSolid = new List<Hitbox>();
            initializeHook();
        }
        
        public void PlayerDie() {
            playerState = PlayerState.Dead;
            deathTimer = PLAYER_DEATH_TIMER;
        }

        public void UpdatePlayer() {
            if (playerState != PlayerState.Dead) {
                player.velocity.X = MathHelper.Clamp(
                    player.velocity.X + PLAYER_ACCELERATION * ((keyboard.IsKeyDown(Keys.D) ? 1 : 0) - (keyboard.IsKeyDown(Keys.A) ? 1 : 0)),
                    -PLAYER_MAX_SPEED_X,
                    PLAYER_MAX_SPEED_X);

                //adds wind adjustment
                switch (windDir)
                {
                    case (Direction.Right):
                        player.velocity.X += WIND_STRENGTH;
                        break;
                    case (Direction.Left):
                        player.velocity.X -= WIND_STRENGTH;
                        break;
                }

                if (playerState == PlayerState.OnGround) {
                    //Friction
                    if (player.velocity.X < -PLAYER_FRICTION_GROUND || player.velocity.X > PLAYER_FRICTION_GROUND) {
                        player.velocity.X = player.velocity.X - PLAYER_FRICTION_GROUND * Math.Sign(player.velocity.X);
                    }
                    else {
                        player.velocity.X = 0;
                    }
                    //Jumping
                    if (keyboard.IsKeyDown(Keys.Space) && keyboardOld.IsKeyUp(Keys.Space)) {
                        player.velocity.Y = PLAYER_JUMP;
                        playerState = PlayerState.InAir;
                    }
                }
                else if (playerState == PlayerState.InAir) {
                    //Friction
                    if (player.velocity.X < -PLAYER_FRICTION_AIR || player.velocity.X > PLAYER_FRICTION_AIR) {
                        player.velocity.X = player.velocity.X - PLAYER_FRICTION_AIR * Math.Sign(player.velocity.X);
                    }
                    else {
                        player.velocity.X = 0;
                    }
                }
                
                player.velocity.Y += GRAVITY;
                
                //Handle rope tension
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
                }
            
                for (int i = 0; i < TilesSpike.Count; i++) {
                    var spike = TilesSpike[i];
                    if (player.Intersects(spike)) {
                        PlayerDie();
                        return;
                    }
                }

                for (int i = Apples.Count - 1; i >= 0; i--) {
                    var apple = Apples[i];
                    if (player.WillIntersect(apple)) {
                        Apples.RemoveAt(i);
                        appleCount++;
                        return;
                    }
                }

                for (int i = 0; i < Grounders.Count; i++) {
                    var mole = Grounders[i];
                    if (player.Intersects(mole)) {
                        PlayerDie();
                        return;
                    }
                }

                for (int i = 0; i < Flyer.Count; i++) {
                    var bird = Flyer[i];
                    if (player.Intersects(bird)) {
                        PlayerDie();
                        return;
                    }
                }
                
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

                    if (player.Intersects(goal)) {
                        level = (level + 1) % levelNames.Length;
                        ChangeLevel(level);
                    }
                }

                player.position += player.velocity;
            } else {
                deathTimer -= 1f;
                if (deathTimer <= 0) {
                    ResetLevel();
                }
            }

            
        }

        public void DrawPlayer() {

            if (playerState != PlayerState.Dead) {
                spriteBatch.Draw(texPlayer, player.position, Color.White);
            } else {
                spriteBatch.Draw(texPlayer, player.position, Color.Gray);
            }

        }

    }
}
