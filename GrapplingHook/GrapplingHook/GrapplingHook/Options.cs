﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GrapplingHook {
    partial class Game {
        List<Button> OptionsButtons;
        Boolean isSoundEffectsOn, isMusicOn;
        
        public void CreateOptionsScreen() {
            OptionsButtons = new List<Button>();
            int numButtons = 2, spacing = (VIEWPORT_WIDTH - 50) / numButtons;
            OptionsButtons = new List<Button>();
            Rectangle solidRect = new Rectangle(90 / numButtons, VIEWPORT_HEIGHT - 200, 200 / numButtons, 40);
            OptionsButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, (isSoundEffectsOn ? "Mute" : "Enable") + " SFX", delegate () { isSoundEffectsOn = !isSoundEffectsOn; CreateOptionsScreen(); }));

            solidRect = new Rectangle(solidRect.X + spacing, solidRect.Y, solidRect.Width, solidRect.Height);
            OptionsButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, (isMusicOn ? "Mute" : "Enable") + " Music", delegate () { isMusicOn = !isMusicOn; CreateOptionsScreen(); }));

            solidRect = new Rectangle(solidRect.X, solidRect.Y + 64, solidRect.Width, solidRect.Height);
            OptionsButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Back", delegate () { state = prevState; }));

            solidRect = new Rectangle(solidRect.X - spacing, solidRect.Y, solidRect.Width, solidRect.Height);
            OptionsButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Main Menu", delegate () { state = Logic.GameState.Title; }));

        }
        public void UpdateOptionsScreen() {
            foreach (Button b in OptionsButtons) {
                if (b.solidRect.Contains(mouse.X / 2, mouse.Y / 2) && mouse.LeftButton == ButtonState.Pressed && mouseOld.LeftButton == ButtonState.Released)
                {
                    soundSelect.Play();
                    b.action.Invoke();
                }
                else if (b.solidRect.Contains(mouse.X / 2, mouse.Y / 2))
                    b.borderColor = Color.Gold;
                else
                    b.borderColor = Color.Black;
            }
        }

        public void DrawOptions() {
            GraphicsDevice.Clear(Color.AliceBlue);
            foreach (Button b in OptionsButtons) {
                spriteBatch.Draw(texButton, b.borderRect, b.borderColor);
                spriteBatch.Draw(texButton, b.solidRect, b.solidColor);
                var size = font.MeasureString(b.text);
                spriteBatch.DrawString(font, b.text, new Vector2(b.solidRect.X + b.solidRect.Width / 2 - size.X / 2, b.solidRect.Y + b.solidRect.Height / 2 - size.Y / 2), Color.Black);
            }
        }
    }
}
