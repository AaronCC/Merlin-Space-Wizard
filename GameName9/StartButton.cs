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
    class StartButton : Button
    {
        public StartButton(Texture2D s, string n)
            : base(s, n)
        {
        }
        public override void ClickEvent()
        {

            SpriteFont font = Game1.testFont;
            // start the game
            ObjectManager.Changelevel(ObjectManager.levelIndex);
            Game1.gameState = 1;
        }
    }
}
