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
    class Button
    {
        // width and height of the button
        public int width, height;
        // button's texture
        public Texture2D sprite;
        // button's name
        public string name;
        // where the button can be clicked
        public Rectangle clickBox;
        // paramaterized constructor
        public Button(Texture2D s, string n)
        {
            // set attributes
            sprite = s;
            width = sprite.Width;
            height = sprite.Height;
            name = n;
        }
        public void SetClickBox(Vector2 position)
        {
            clickBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }
        public virtual void ClickEvent()
        {
            // overriden
        }
    }
}
