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
    class Portal : GameObject
    {

        public Portal(Vector2 levelIndex, bool canCollide)
            : base(levelIndex, canCollide)
        {
            // Set attributes
            govName = "Portal";
            frameIndex.X = 0;
            frameIndex.Y = 0;
            position = new Vector2(460, 460);
            textures = AssetManager.GetGovTextureAssets(govName);
            textureAssetIndex = ObjectManager.levelIndex;
            // Get the currentSprite from the current textureAsset
            currentSprite = textures[textureAssetIndex].sprite;
            // set width and height
            width = (currentSprite.Width / 4) / textures[textureAssetIndex].cols;
            height = (currentSprite.Height / 4) / textures[textureAssetIndex].rows;
            // set current row/col
            row = (int)frameIndex.Y;
            column = (int)frameIndex.X;
            hitBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            width = (currentSprite.Width / 4) / textures[textureAssetIndex].cols;
            // Height = the spritesheet's height / the number of rows in the spritesheet
            height = (currentSprite.Height / 4) / textures[textureAssetIndex].rows;
            // row = (int)float*frameindex.Y
            row = (int)frameIndex.Y;
            // column = (int)float*frameindex.X
            column = (int)frameIndex.X;
            // Create the destiationRectangle in the spritesheet
            destinationRectangle = new Rectangle(currentSprite.Width * column, currentSprite.Height * row, currentSprite.Width, currentSprite.Height);
            spriteBatch.Draw(currentSprite, position - Camera.screenOffset, destinationRectangle, Color.White, 0, Vector2.Zero, .25f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            millisecondsPerFrame = textures[textureAssetIndex].millisecondsPerFrame;
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                frameIndex.X++;
                if (frameIndex.X == textures[textureAssetIndex].cols)
                    frameIndex.X = 0;
                timeSinceLastFrame = 0;
            }
        }
    }
}
