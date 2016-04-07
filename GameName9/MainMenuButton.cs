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
    class MainMenuButton : Button
    {
        public MainMenuButton(Texture2D s, string n)
            : base(s, n)
        {
        }
        public override void ClickEvent()
        {
            // start the game
            ObjectManager.currentMenuName = "StartMenu";
        }
    }
}
