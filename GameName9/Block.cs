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
    class Block : GameObject
    {
        public Block(Vector2 levelIndex, bool _canCollide)
            : base(levelIndex, _canCollide)
        {
            // set attributes
            objectType = typeof(Block);
            govName = "Block";
            // get the textures from the AssetManager
            textures = AssetManager.GetGovTextureAssets(govName);
            // initialize the textureAssetIndex
            textureAssetIndex = 0;
            // get the currentSprite from the current textureAsset
            currentSprite = textures[textureAssetIndex].sprite;
            // initialize the width and height
            width = currentSprite.Width / textures[textureAssetIndex].cols;
            height = currentSprite.Height / textures[textureAssetIndex].rows;
            // initialize the row and col
            row = (int)frameIndex.Y;
            column = (int)frameIndex.X;
            // initialize the hitBox
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            int x, y;
            for (x = (int)(position.X - (position.X % 50)); x < (int)((position.X + width) - ((position.X + width) % 50)); x += 50)
            {
                for (y = (int)(position.Y - (position.Y % 50)); y < (int)((position.Y + height) - ((position.Y + height) % 50)); y += 50)
                {
                    collisionCollections.Add(new Rectangle(x, y, 50, 50));
                }
                y = (int)(position.Y - (position.Y % 50));
            }
        }
        public override void Update(GameTime gameTime)
        {
            // update the hitBox
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            // update the currentSprite
            currentSprite = textures[textureAssetIndex].sprite;

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the block
            spriteBatch.Draw(currentSprite, position - Camera.screenOffset, Color.White);

        }
    }
}
