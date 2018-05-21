using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GrapplingHook
{
    public partial class Game
    {
        List<Button> PauseButtons;

        public void CreatePauseScreen()
        {
            int numButtons = 2, spacing = (VIEWPORT_WIDTH - 50) / numButtons;
            PauseButtons = new List<Button>();
            Rectangle solidRect = new Rectangle(90 / numButtons, VIEWPORT_HEIGHT - 75, 200 / numButtons, 40);
            PauseButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Resume", delegate () { state = Logic.GameState.Level; }));

            solidRect = new Rectangle(solidRect.X + spacing, solidRect.Y, solidRect.Width, solidRect.Height);
            PauseButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Options", delegate () { state = Logic.GameState.Options; }));
        }

        public void UpdatePauseScreen()
        {
            prevState = Logic.GameState.Pause;

            foreach (Button b in PauseButtons)
            {
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

        public void DrawPauseScreen()
        {
            spriteBatch.Draw(texPixel, new Rectangle(0, 0, VIEWPORT_WIDTH, VIEWPORT_HEIGHT), new Color(128, 128, 128, 190));

            foreach (Button b in PauseButtons)
            {
                spriteBatch.Draw(texButton, b.borderRect, b.borderColor);
                spriteBatch.Draw(texButton, b.solidRect, b.solidColor);
                var size = font.MeasureString(b.text);
                spriteBatch.DrawString(font, b.text, new Vector2(b.solidRect.X + b.solidRect.Width / 2 - size.X / 2, b.solidRect.Y + b.solidRect.Height / 2 - size.Y / 2), Color.Black);
            }
        }
    }
}
