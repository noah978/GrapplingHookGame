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
        Texture2D
            texTitleScreen,
            texButton;

        SpriteFont titleFont;

        List<Button> TitleButtons;
        
        public void CreateTitleScreen()
        {
            int numButtons = 2, spacing = (VIEWPORT_WIDTH - 50) / numButtons;
            TitleButtons = new List<Button>();
            Rectangle solidRect = new Rectangle(90 / numButtons, VIEWPORT_HEIGHT - 75, 200 / numButtons, 40);
            TitleButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "New Game"));

            solidRect = new Rectangle(solidRect.X + spacing, solidRect.Y, solidRect.Width, solidRect.Height);
            TitleButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "Options"));

            //solidRect = new Rectangle(solidRect.X + spacing, solidRect.Y, solidRect.Width, solidRect.Height);
            //TitleButtons.Add(new Button(solidRect, getBorderRect(solidRect), Color.LightGray, Color.Black, "IDK"));
        }
        private Rectangle getBorderRect(Rectangle r)
        {
            return new Rectangle(r.X - 1, r.Y - 1, r.Width + 3, r.Height + 2);
        }
        public void UpdateTitleScreen()
        {
            foreach (Button b in TitleButtons)
            {
                if (b.solidRect.Contains(mouse.X / 2, mouse.Y / 2) && mouse.LeftButton == ButtonState.Pressed)
                {
                    switch (b.action)
                    {
                        case "New Game":
                            state = Logic.GameState.Level;
                            break;
                        case "Options":
                            state = Logic.GameState.Options;
                            break;
                        default:
                            throw new InvalidOperationException("Button action does not exist.");
                    }
                }
                else if (b.solidRect.Contains(mouse.X / 2, mouse.Y / 2))
                    b.borderColor = Color.Gold;
                else
                    b.borderColor = Color.Black;

            }
        }
        public void DrawTitleScreen()
        {
            GraphicsDevice.Clear(Color.AliceBlue);

            string titleText = "Grappling Hook Game";
            Vector2 size = titleFont.MeasureString(titleText);
            spriteBatch.DrawString(titleFont, titleText, new Vector2(VIEWPORT_WIDTH / 2 - size.X / 2, 25), Color.Gold);
            foreach (Button b in TitleButtons) {
                spriteBatch.Draw(texButton, b.borderRect, b.borderColor);
                spriteBatch.Draw(texButton, b.solidRect, b.solidColor);
                size = font.MeasureString(b.action);
                spriteBatch.DrawString(font, b.action, new Vector2(b.solidRect.X + b.solidRect.Width/2 - size.X/2, b.solidRect.Y + b.solidRect.Height/2 - size.Y/2), Color.Black);
            }
        }
    }
}
