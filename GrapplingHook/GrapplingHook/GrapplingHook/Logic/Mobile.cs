using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GrapplingHook.Logic {
    class Mobile : Hitbox {
        public Vector2 velocity;

        public float FutureLeft { get { return Left + velocity.X; } }
        public float FutureUp { get { return Up + velocity.Y; } }
        public float FutureRight { get { return Right + velocity.X; } }
        public float FutureDown { get { return Down + velocity.Y; } }

        public Mobile (float xpos, float ypos, float xvel, float yvel, float xbnd, float ybnd) : base(xpos, ypos, xbnd, ybnd) {
            velocity = new Vector2(xvel, yvel);
        }

        public bool WillIntersect(Hitbox other) {
            return (FutureRight > other.Left && FutureLeft < other.Right) && (FutureDown > other.Up && FutureUp < other.Down);
        }
    }
}
