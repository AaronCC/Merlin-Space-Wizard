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
    /// <summary>
    /// *** Coming soon! ***
    /// Controls screen position and onScreen gameObjects on the current level
    /// </summary>
    
    static class Camera
    {
        public static Rectangle camHitBox = new Rectangle(0, 0, 0, 0);
        public static Vector2 screenOffset;
        public static int SCREEN_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static int SCREEN_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public const int LEVEL_WIDTH = Game1.LEVEL_COLUMNS * 50;
        public const int LEVEL_HEIGHT = Game1.LEVEL_ROWS * 50;
        public static Player player;
        public static void InitializeCamera(Player _player)
        {
            player = _player;
            screenOffset.X = 0;
            screenOffset.Y = 0;
            camHitBox = new Rectangle((int)(player.position.X - (SCREEN_WIDTH / 2)), (int)(player.position.Y - (SCREEN_HEIGHT / 2)), SCREEN_WIDTH, SCREEN_HEIGHT);
            screenOffset.X = camHitBox.X;
            screenOffset.Y = camHitBox.Y;
            if (screenOffset.X < 0)
                screenOffset.X = 0;
            if (screenOffset.Y < 0)
                screenOffset.Y = 0;
            CenterCamera();
        }
        public static void Update()
        {
            camHitBox = new Rectangle((int)(player.position.X - (SCREEN_WIDTH / 2)), (int)(player.position.Y - (SCREEN_HEIGHT / 2)), SCREEN_WIDTH, SCREEN_HEIGHT);
            //ObjectManager.UpdateScreen(camHitBox);
            
        }
        //public static bool IsCentered()
        //{
        //    if (screenOffset.X == (player.position.X - (((Camera.SCREEN_WIDTH / 2) - (player.height / 2)))) && screenOffset.Y == (player.position.Y - ((Camera.SCREEN_HEIGHT / 2) - (player.width / 2))))
        //        return true;
        //    return false;
        //}
        public static void CenterCamera()
        {
            screenOffset -= (player.centerVector -(player.position - Camera.screenOffset));
        }
    }
}
