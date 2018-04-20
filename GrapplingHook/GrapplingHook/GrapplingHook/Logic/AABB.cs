using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GrapplingHook.Logic {
    class AABB {
        public Vector2 position;
        public Vector2 bounds;

        public AABB(float xpos, float ypos, float xbnd, float ybnd) {
            position = new Vector2(xpos, ypos);
            bounds = new Vector2(xbnd, ybnd);
        }

        public bool Intersects(AABB other) {
            return (position.X + bounds.X > other.position.X && position.X < other.position.X + other.bounds.X)
                && (position.Y + bounds.Y > other.position.Y && position.Y < other.position.Y + other.bounds.Y);
        }
    }
}
