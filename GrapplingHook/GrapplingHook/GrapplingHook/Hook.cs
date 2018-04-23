using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GrapplingHook.Logic;

namespace GrapplingHook
{
    partial class Game
    {
        Texture2D texHook;
        Mobile hook;
        HookState hookState;
        double ropeLength;

        public void initializeHook()
        {
            hook = new Mobile(-1, -1, 0, 0, 4, 4);
            hookState = HookState.Inactive;
            ropeLength = -1;
        }

        public void UpdateHook()
        {
            //Deactivate jook of it's too far from the player
            if (hookState == HookState.Thrown)
            {
                if(Distance(player.Center.X, player.Center.Y, hook.Center.X, hook.Center.Y) > HOOK_MAX_LENGTH)
                {
                    hook.position = new Vector2(-1, -1);
                    hookState = HookState.Inactive;
                }
            }
            //Deactivate jook of the player jumps or right-click is pressed
            if (hookState == HookState.Hooked)
            {
                if ((keyboard.IsKeyDown(Keys.Up) && keyboardOld.IsKeyUp(Keys.Up)) || 
                    (mouse.RightButton == ButtonState.Pressed && mouseOld.RightButton == ButtonState.Released))
                {
                    hook.position = new Vector2(-1, -1);
                    hookState = HookState.Inactive;
                }
            }

            //Throw hook if left-click is pressed
            if (mouse.LeftButton == ButtonState.Pressed && mouseOld.LeftButton == ButtonState.Released)
            {
                hook.position = player.Center - new Vector2(hook.bounds.X / 2, hook.bounds.Y / 2);
                Vector2 distanceVector = new Vector2((mouse.X / 2) - player.Center.X, (mouse.Y / 2) - player.Center.Y);
                distanceVector.Normalize();
                hook.velocity = distanceVector * HOOK_SPEED;
                hookState = HookState.Thrown;
            }

            //Attach hook to solid tile
            if (hookState == HookState.Thrown)
            {
                foreach (Hitbox tile in TilesSolid)
                {
                    if (hook.WillIntersect(tile))
                    {
                        hook.position += hook.velocity;
                        hookState = HookState.Hooked;
                        ropeLength = Distance(hook.Center.X, hook.Center.Y, player.Center.X, player.Center.Y);
                        break;
                    }
                }
            }

            //Move hook if it's being thrown
            if(hookState == HookState.Thrown)
            {
                hook.position += hook.velocity;
            }
        }

        public void DrawHook()
        {
            if(hookState != HookState.Inactive)
            {
                DrawLine(player.Center, hook.Center, new Color(32, 32, 32));
                spriteBatch.Draw(texHook, hook.position, Color.White);
            }
        }

        //Probably needs to bemove to a different file
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
