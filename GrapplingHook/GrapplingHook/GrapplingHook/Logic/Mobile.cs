using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GrapplingHook.Logic {
    class Mobile : AABB {
        public Vector2 velocity;
        
        public Mobile (float xpos, float ypos, float xvel, float yvel, float xbnd, float ybnd) : base(xpos, ypos, xbnd, ybnd) {
            velocity = new Vector2(xvel, yvel);
        }
    }
}
