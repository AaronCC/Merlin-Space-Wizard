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
    static class Background
    {
        public static Vector2 position = new Vector2(0, 0);
        public static Dictionary<int, Texture2D> bkgTextures = new Dictionary<int, Texture2D>();
        public static Rectangle destRectangle;
        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bkgTextures[ObjectManager.levelIndex], position - Camera.screenOffset, Color.White);
        }
    }
}