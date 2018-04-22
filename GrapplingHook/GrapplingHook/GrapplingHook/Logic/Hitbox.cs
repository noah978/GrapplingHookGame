using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GrapplingHook.Logic {
    class Hitbox {
        public Vector2 position;
        public Vector2 bounds;

        public float Left { get { return position.X; } }
        public float Up { get { return position.Y; } }
        public float Right { get { return position.X + bounds.X; } }
        public float Down { get { return position.Y + bounds.Y; } }
        

        public Hitbox(float xpos, float ypos, float xbnd, float ybnd) {
            position = new Vector2(xpos, ypos);
            bounds = new Vector2(xbnd, ybnd);
        }

        public bool Intersects(Hitbox other) {
            return (Right > other.Left && Left < other.Right) && (Down > other.Up && Up < other.Down);
        }
    }
}
