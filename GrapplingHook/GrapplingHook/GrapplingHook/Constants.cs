using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GrapplingHook {
    partial class Game {
        const int WINDOW_WIDTH = 640;
        const int WINDOW_HEIGHT = 480;

        const int VIEWPORT_WIDTH = 320;
        const int VIEWPORT_HEIGHT = 240;

        const int LEVEL_WIDTH = 20;
        const int LEVEL_HEIGHT = 15;

        const int TILE_WIDTH = 16;
        const int TILE_HEIGHT = 16;

        const float PLAYER_ACCELERATION = .3f;
        const float GRAVITY = .2f;
        const float PLAYER_FRICTION_GROUND = .2f;
    }
}
