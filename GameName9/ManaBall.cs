using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace GameName9
{
    class ManaBall : Projectile
    {
        public bool canDuplicate;
        public int manaCost;
        public int duplicateTime;
        public int duplicateNum = 5;
        public ManaBall(Vector2 _position, bool canCollide, GameObject gov, float xPos, float yPos, int spd, bool cd)
            : base(_position, canCollide, gov, xPos, yPos, spd)
        {
            canDuplicate = cd;
            manaCost = 15;
            speed = spd;
            govObject = gov;
            govName = "Projectile";
            textures = AssetManager.GetGovTextureAssets(govName);
            position = _position;
            textureAssetIndex = 3;
            currentSprite = textures[textureAssetIndex].sprite;
            if (ObjectManager.currentPlayer.reverseSprite == false)
                position.X += gov.width / 2 - ((currentSprite.Width / 2) - 15);
            else
                position.X += gov.width / 2 - ((currentSprite.Width / 2) + 15);
            position.Y += gov.height / 2 - (currentSprite.Height / 2);
            targetVector.X = xPos - (((position.X - Camera.screenOffset.X)) + currentSprite.Width / 2);
            targetVector.Y = yPos - (((position.Y - Camera.screenOffset.Y)) + currentSprite.Width / 2);
            double targetVectorMagnitude = (Math.Sqrt(((Math.Pow(targetVector.X, 2)) + (Math.Pow(targetVector.Y, 2)))));
            direction.X = (float)(targetVector.X * (1 / targetVectorMagnitude));
            direction.Y = (float)(targetVector.Y * (1 / targetVectorMagnitude));
        }
        public override void Update(GameTime gameTime)
        {
            if (canDuplicate)
            {
                duplicateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (duplicateTime % 200 < 20 && duplicateNum > 0)
                {
                    duplicateNum--;
                    GameObjectQueue.EnQueue(new ManaBall(ObjectManager.currentPlayer.position, true, ObjectManager.currentPlayer, Game1.mState.X, Game1.mState.Y, 8, false));
                    //GameObjectQueue.EnQueue(this);
                    duplicateTime = 0;
                }
            }
            currentSprite = textures[textureAssetIndex].sprite;
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            velocity = speed * direction;
            oldPos = position;
            ObjectManager.currentColMap.Remove(oldPos, this);
            position += velocity;
            ObjectManager.currentColMap.Insert(position, this);
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            collisions = ObjectManager.CheckCollisions(this, govObject);
            GameObject tempObj;
            if (collisions.Count > 0)
            {
                // Call hit events here
                foreach (GameObject obj in collisions)
                {
                    if (obj.objectType == typeof(Enemy))
                    {
                        Enemy en = (Enemy)obj;
                        en.TakeDamage(3);
                    }
                }
                RemoveObjectQueue.EnQueue(name);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentSprite, position - Camera.screenOffset, Color.White);
        }
        public void collisionEvent(List<GameObject> collidingWith)
        {

        }
    }
}
