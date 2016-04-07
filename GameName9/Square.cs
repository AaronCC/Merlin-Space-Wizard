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
    class Square : Block
    {
        public Square(Vector2 levelIndex, bool _canCollide)
            : base(levelIndex, _canCollide)
        {
            // Set attributes
            govName = "Block";
            // Get textures from AssetManager
            textures = AssetManager.GetGovTextureAssets(govName);
            // Initialize the index
            textureAssetIndex = 0;
            // get currentSprite from the current textureAsset
            currentSprite = textures[textureAssetIndex].sprite;
            // Initialize the texture's width
            width = currentSprite.Width / textures[textureAssetIndex].cols;
            // Initialize the texture's height
            height = currentSprite.Height / textures[textureAssetIndex].rows;
            // Initialize row/col
            row = (int)frameIndex.Y;
            column = (int)frameIndex.X;
            // Set hitBox
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
        }
        public override void Update(GameTime gameTime)
        {
            // Get the currentSprite from the current textureAsset
            currentSprite = textures[textureAssetIndex].sprite;
            // update hitBox
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            // get Collisions
            collidingWith = ObjectManager.CheckCollisions(this);
            // change the textureIndex if it is colliding
            ObjectManager.currentColMap.Remove(oldPos, this);
            ObjectManager.currentColMap.Insert(position, this);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the square

        }
    }
}