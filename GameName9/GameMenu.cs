using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
namespace GameName9
{
    class GameMenu
    {
        // Dictionary of menu's buttons and their location
        protected Dictionary<Button, Vector2> buttons = new Dictionary<Button, Vector2>();
        // number of buttons in the menu
        protected int numButtons;
        // menu's sprite
        protected Texture2D sprite;
        // mouse hitBox
        protected Rectangle mouseBox;
        public int y = 0;
        public bool isPlaying = false;
        // name of the menu
        public SoundEffect menuTheme;
        public SoundEffectInstance themeInstance;
        public List<SoundAsset> sounds;
        public string name { get; set; }
        // paramaterized constructor
        public GameMenu(Texture2D s, string n, Dictionary<Button, Vector2> btn)
        {
            if (sounds == null)
                sounds = AssetManager.GetGovSoundAssets("Menu");
            menuTheme = sounds[0].sound;
            themeInstance = menuTheme.CreateInstance();
            themeInstance.IsLooped = true;
            // set attributes
            buttons = btn;
            numButtons = buttons.Count;
            sprite = s;
            name = n;
        }
        public virtual void Update(GameTime gameTime)
        {
            if (!isPlaying)
            {
                themeInstance.Play();
                isPlaying = true;
            }
            // calculate the mouseBox rectangle
            mouseBox = new Rectangle((int)Game1.mState.X, (int)Game1.mState.Y, 1, 1);
            // foreach button
            foreach (KeyValuePair<Button, Vector2> pair in buttons)
            {
                // update the button's clickBox
                pair.Key.SetClickBox(pair.Value);
                // If the clickBox intersects the mouseBox and the left button has been pressed
                if (mouseBox.Intersects(pair.Key.clickBox) && Game1.mState.LeftButton == ButtonState.Pressed && Game1.oldmState.LeftButton == ButtonState.Released)
                {
                    // Fire the button's ClickEvent
                    pair.Key.ClickEvent();
                    themeInstance.Stop();
                    isPlaying = false;
                }
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the menu
            //spriteBatch.Draw(sprite, new Vector2((Camera.SCREEN_WIDTH / 2) - (sprite.Width / 2), 0), Color.White);
            if (!ObjectManager.gameOver)
                spriteBatch.Draw(sprite, new Vector2(0, 0), Color.White);
            else
            {
                if (y > -4000)
                    y -= 3;
                spriteBatch.Draw(sprite, new Vector2(0, y), Color.White);
            }
            // Draw each of the menu's buttons
            foreach (KeyValuePair<Button, Vector2> pair in buttons)
            {
                //pair.Key.SetClickBox(new Vector2(pair.Value.X + ((Camera.SCREEN_WIDTH / 2) - (sprite.Width / 2)), pair.Value.Y));
                //float x = pair.Value.X;
                //x += ((Camera.SCREEN_WIDTH / 2) - (sprite.Width / 2));
                spriteBatch.Draw(pair.Key.sprite, pair.Value, Color.White);
            }
        }
    }
}
