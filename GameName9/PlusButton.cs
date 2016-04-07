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
    class PlusButton : Button
    {
        public int counter;
        public string atName { get; set; }
        public PlusButton(Texture2D s, string n)
            : base(s, n)
        {
            counter = 0;
        }
        public override void ClickEvent()
        {
            switch (atName)
            {
                case "Mana":
                    ObjectManager.currentPlayer.maxMana+=5;
                    ObjectManager.currentPlayer.skillPoints--;
                    break;
                case "ManaRegen":
                    ObjectManager.currentPlayer.manaRegen++;
                    ObjectManager.currentPlayer.skillPoints--;
                    break;
                case "FireRate":
                    if (ObjectManager.currentPlayer.fireRate - 50 >= 0)
                        ObjectManager.currentPlayer.fireRate -= 50;
                    ObjectManager.currentPlayer.skillPoints--;
                    break;
                case "Health":
                    ObjectManager.currentPlayer.maxHealth++;
                    ObjectManager.currentPlayer.health++;
                    ObjectManager.currentPlayer.skillPoints--;
                    break;
            }
        }
    }
}
