using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GrapplingHook {
    partial class Game {
        List<Button> OptionsButtons;
        
        public void CreateOptionsScreen() {
            OptionsButtons = new List<Button>();
            int numButtons = 2, spacing = (VIEWPORT_WIDTH - 50) / numButtons;
            OptionsButtons = new List<Button>();
            Rectangle solidRect = new Rectangle(90 / numButtons, VIEWPORT_HEIGHT - 200, 200 / numButtons, 40);
            OptionsButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Mute SFX", delegate () { /**/ }));

            solidRect = new Rectangle(solidRect.X + spacing, solidRect.Y, solidRect.Width, solidRect.Height);
            OptionsButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Mute Music", delegate () { /**/ }));

            solidRect = new Rectangle(solidRect.X, solidRect.Y + 64, solidRect.Width, solidRect.Height);
            OptionsButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Back", delegate () { state = Logic.GameState.Title; }));

        }
        public void UpdateOptionsScreen() {
            foreach (Button b in OptionsButtons) {
                if (b.solidRect.Contains(mouse.X / 2, mouse.Y / 2) && mouse.LeftButton == ButtonState.Pressed && mouseOld.LeftButton == ButtonState.Released)
                    b.action.Invoke();
                else if (b.solidRect.Contains(mouse.X / 2, mouse.Y / 2))
                    b.borderColor = Color.Gold;
                else
                    b.borderColor = Color.Black;
            }
        }

        public void DrawOptions() {
            foreach (Button b in OptionsButtons) {
                spriteBatch.Draw(texButton, b.borderRect, b.borderColor);
                spriteBatch.Draw(texButton, b.solidRect, b.solidColor);
                var size = font.MeasureString(b.text);
                spriteBatch.DrawString(font, b.text, new Vector2(b.solidRect.X + b.solidRect.Width / 2 - size.X / 2, b.solidRect.Y + b.solidRect.Height / 2 - size.Y / 2), Color.Black);
            }
        }
    }
}
