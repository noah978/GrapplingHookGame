using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrapplingHook.Logic;
using Microsoft.Xna.Framework.Audio;

namespace GrapplingHook {
    partial class Game {
        Texture2D texApple;

        List<Hitbox> Apples;

        SoundEffect soundPickupApple;

        public void DrawApples() {
            for (int i = 0; i < Apples.Count; i++) {
                var apple = Apples[i];
                spriteBatch.Draw(texApple, apple.position, Color.White);
            }
        }


    }
}
