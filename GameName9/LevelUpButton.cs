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
    class LevelUpButton : Button
    {
        public LevelUpButton(Texture2D s, string n)
            : base(s, n)
        {
            clickBox = new Rectangle(10, 50, sprite.Width, sprite.Height);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Vector2(10, 50), Color.White);
        }
        public override void ClickEvent()
        {
            ObjectManager.currentPlayer.LevelUp();
            Game1.windowed = true;
        }
    }
}
