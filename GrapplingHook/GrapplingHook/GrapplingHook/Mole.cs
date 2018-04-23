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

        }

        public void DrawMoles() {
            for (int i = 0; i < Moles.Count; i++) {
                var mole = Moles[i];
                spriteBatch.Draw(texMole, mole.position, Color.White);
            }
        }

    }
}
