using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrapplingHook
{
    public class Button
    {
        public Rectangle solidRect, borderRect;
        public Color solidColor, borderColor;
        public String action;

        public Button(Rectangle solidRec, Rectangle borderRec, Color solid, Color border, String action)
        {
            solidRect = solidRec;
            borderRect = borderRec;
            solidColor = solid;
            borderColor = border;
            this.action = action;
        }
    }
}
