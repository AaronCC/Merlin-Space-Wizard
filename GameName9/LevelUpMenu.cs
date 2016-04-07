using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
////using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
namespace GameName9
{
    class LevelUpMenu : GameMenu
    {
        public LevelUpMenu(Texture2D s, string n, Dictionary<Button,Vector2> btn) : base(s,n,btn)
        {
            int i = 0;
            foreach(KeyValuePair<Button,Vector2> pair in buttons)
            {
                PlusButton button;
                try
                {
                    button = (PlusButton)pair.Key;
                }
                catch { return; }
                switch(i)
                {
                    case 0:
                        button.atName = "Mana";
                        break;
                    case 1:
                        button.atName = "FireRate";
                        break;
                    case 2:
                        button.atName = "ManaRegen";
                        break;
                    case 3:
                        button.atName = "Health";
                        break;
                    default:
                        break;
                }
                i++;
            }
        }
        public override void Update(GameTime gameTime)
        {
            // calculate the mouseBox rectangle
            mouseBox = new Rectangle((int)Game1.mState.X, (int)Game1.mState.Y, 1, 1);
            // foreach button
            foreach (KeyValuePair<Button, Vector2> pair in buttons)
            {
                // update the button's clickBox
                pair.Key.SetClickBox(pair.Value);
                // If the clickBox intersects the mouseBox and the left button has been pressed
                if(mouseBox.Intersects(pair.Key.clickBox) && Game1.mState.LeftButton == ButtonState.Pressed && Game1.oldmState.LeftButton == ButtonState.Released)
                {
                    // Fire the button's ClickEvent
                    pair.Key.ClickEvent();
                }

            }
            if (ObjectManager.currentPlayer.skillPoints == 0)
            {
                ObjectManager.currentMenuName = "ReadyGoMenu";
                Game1.fullScreen = true;
                return;
            }
        }
        public override void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            // Draw the menu
            spriteBatch.Draw(sprite, new Vector2(0, 0), Color.White);
            // Draw each of the menu's buttons
            foreach(KeyValuePair<Button,Vector2> pair in buttons)
            {
                spriteBatch.Draw(pair.Key.sprite, pair.Value, Color.White);
            }
            spriteBatch.DrawString(Game1.testFont, "Skill Points: " + ObjectManager.currentPlayer.skillPoints.ToString(),
                new Vector2(60, 225), Color.Yellow, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.testFont, "Mana: " + ObjectManager.currentPlayer.maxMana.ToString(),
                new Vector2(170, 325), Color.Yellow, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.testFont, "Fire Rate: " + ObjectManager.currentPlayer.fireRate.ToString() + "milliseconds",
                new Vector2(170, 475), Color.Yellow, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.testFont, "Mana Regeneration: " + ObjectManager.currentPlayer.manaRegen.ToString(),
                new Vector2(170, 625), Color.Yellow, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.testFont, "Health: " + ObjectManager.currentPlayer.maxHealth.ToString(),
                new Vector2(170, 775), Color.Yellow, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
        }
    }
}
