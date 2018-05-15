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
        public string text;
        public ButtonAction action;

        public delegate void ButtonAction();

        public Button(Rectangle solidRec, Rectangle borderRec, Color solid, Color border, string text, ButtonAction action)
        {
            solidRect = solidRec;
            borderRect = borderRec;
            solidColor = solid;
            borderColor = border;
            this.text = text;
            this.action = action;
        }
    }
}
