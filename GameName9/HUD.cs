using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
namespace GameName9
{
    static class HUD
    {
        static List<Texture2D> HUDTextures = new List<Texture2D>();
        public static List<Texture2D> orbTextures = new List<Texture2D>();
        
        public static LevelUpButton levelUpButton;
        static int currentAbility;
        public static void Initialize()
        {
            HUDTextures = AssetManager.GetGovTextures("HUD");
            orbTextures = AssetManager.GetGovTextures("Orb");
            levelUpButton = new LevelUpButton(HUDTextures[3], "LevelUpButton");
            currentAbility = 1;
        }
        public static void Update(GameTime gameTime)
        {
            if(Game1.KBstate.IsKeyDown(Keys.D1) && Game1.oldKBstate.IsKeyUp(Keys.D1) && ObjectManager.currentPlayer.knownAbilities >= 1)
            {
                currentAbility = 1;
                ObjectManager.currentPlayer.projectileType = typeof(MagicMissile);
            }
            if (Game1.KBstate.IsKeyDown(Keys.D2) && Game1.oldKBstate.IsKeyUp(Keys.D2) && ObjectManager.currentPlayer.knownAbilities >= 2)
            {
                currentAbility = 2;
                ObjectManager.currentPlayer.projectileType = typeof(FireBall);
            }
            if (Game1.KBstate.IsKeyDown(Keys.D3) && Game1.oldKBstate.IsKeyUp(Keys.D3) && ObjectManager.currentPlayer.knownAbilities >= 3)
            {
                currentAbility = 3;
                ObjectManager.currentPlayer.projectileType = typeof(ManaBall);
            }
        }
        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int bufferX, bufferY;
            Vector2 healthPos, manaPos;
            healthPos.Y = (ObjectManager.currentPlayer.position.Y - Camera.screenOffset.Y) + ObjectManager.currentPlayer.height;
            healthPos.X = (ObjectManager.currentPlayer.position.X - Camera.screenOffset.X) - 17;
            manaPos.Y = (ObjectManager.currentPlayer.position.Y - Camera.screenOffset.Y) + ObjectManager.currentPlayer.height;
            manaPos.X = (ObjectManager.currentPlayer.position.X - Camera.screenOffset.X) + ObjectManager.currentPlayer.width;
            //spriteBatch.DrawString(Game1.testFont, "Health", new Vector2(10, 800), Color.Red, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(orbTextures[1], new Vector2(healthPos.X - 5, healthPos.Y - 2), Color.White);
            spriteBatch.Draw(orbTextures[0], new Vector2(manaPos.X - 5, manaPos.Y - 2), Color.White);
            spriteBatch.DrawString(Game1.testFont, ObjectManager.currentPlayer.health.ToString(), healthPos, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.testFont, ObjectManager.currentPlayer.mana.ToString(), manaPos, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Game1.testFont, ObjectManager.currentPlayer.currentExperience.ToString() + " / " + ObjectManager.currentPlayer.experinceToNextLevel.ToString(),
                new Vector2(10, 10), Color.Orchid);
            for (int x = 0; x < HUDTextures.Count(); x++)
            {
                bufferX = (x * 100) + 50 - (HUDTextures[x].Width/2);
                bufferY = (Camera.SCREEN_HEIGHT - 50) - (HUDTextures[x].Height / 2);
                if(x < ObjectManager.currentPlayer.knownAbilities)
                {
                    
                    if (currentAbility == x + 1)
                    {
                        spriteBatch.Draw(orbTextures[2], new Vector2((x * 100), Camera.SCREEN_HEIGHT - 100), Color.White);
                        spriteBatch.Draw(HUDTextures[x], new Vector2(bufferX, bufferY),
                            new Rectangle(0, 0, HUDTextures[x].Width, HUDTextures[x].Height), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(Game1.testFont, (x + 1).ToString(), new Vector2(10 + (x * 100), Camera.SCREEN_HEIGHT - 100), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                        //spriteBatch.Draw(orbTextures[0], new Vector2(65 + (x * 100), 905), Color.White);
                        //spriteBatch.DrawString(Game1.testFont, ((x + 1) * 5).ToString(), new Vector2(73 + (x * 100), 906), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(orbTextures[2], new Vector2((x * 100), Camera.SCREEN_HEIGHT - 100), Color.White * .5f);
                        spriteBatch.Draw(HUDTextures[x], new Vector2(bufferX , bufferY),
                            new Rectangle(0, 0, HUDTextures[x].Width, HUDTextures[x].Height), Color.White * 0.5f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(Game1.testFont, (x + 1).ToString(), new Vector2(10 + (x * 100), Camera.SCREEN_HEIGHT - 100), Color.Black * 0.5f, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                        //spriteBatch.Draw(orbTextures[0], new Vector2(65 + (x * 100), 905), Color.White * 0.5f);
                        //spriteBatch.DrawString(Game1.testFont, ((x + 1) * 5).ToString(), new Vector2(73 + (x * 100), 906), Color.Black * 0.5f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    }

                }
            }
        }
    }
}
