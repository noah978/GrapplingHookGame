using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GrapplingHook.Logic;

namespace GrapplingHook {
    partial class Game {
        Texture2D texWorm;
        Texture2D texSpectre;

        List<Hitbox> Sneakers;
        List<int> SneakerTimers;

        public void UpdateSneakers() {
            for (int i = 0; i < Sneakers.Count; i++) {
                var sneaker = Sneakers[i];
                if (SneakerTimers[i] > 0) {
                    SneakerTimers[i] -= 1;
                }
                else if (SneakerTimers[i] == 0) {
                    if (playerState != PlayerState.Dead) {
                        if (player.Intersects(sneaker)) {
                            PlayerDie();
                        }
                    }
                } else {
                    if (sneaker.Intersects(player)) {
                        SneakerTimers[i] = 40;
                    }
                }
            }
        }

        public void DrawSneakers() {
            for (int i = 0; i < Sneakers.Count; i++) {
                var sneaker = Sneakers[i];
                spriteBatch.Draw((levelType == LevelType.Ravine ? texWorm : texSpectre), sneaker.position + (SneakerTimers[i] != 0 ? new Vector2(0, 12) : Vector2.Zero), Color.White);
            }
        }

    }
}
