using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace GameName9
{
    class ReadyGoMenu : GameMenu
    {
        int frameTimer = 0;
        int column = 0;
        public ReadyGoMenu(Texture2D s, string n, Dictionary<Button,Vector2> btn):base(s,n,btn)
        {

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
            
        }
        public override void Draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            frameTimer += gameTime.ElapsedGameTime.Milliseconds;
            if(frameTimer >= 1000)
            {
                column = 1;
            }
            // Draw the menu
            Rectangle destRect = new Rectangle(1000 * column, 0, sprite.Width/2, sprite.Height);
            
            spriteBatch.Draw(Game1.permaBackground, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(sprite, new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width /2 ) - (sprite.Width / 4), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (sprite.Height / 2)), destRect, Color.White);
            //spriteBatch.Draw(sprite, new Vector2(0,0), destRect, Color.White);
            // Draw each of the menu's buttons

            if (column == 1 && frameTimer >= 2000)
            {
                column = 0;
                Game1.gameState = 1;
                frameTimer = 0;
            }
        }
    }
}
