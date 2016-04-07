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
    class EnemyProjectile : Projectile
    {
        public EnemyProjectile(Vector2 _position, bool canCollide, GameObject gov, float xPos, float yPos, int spd)
            : base(_position, canCollide, gov, xPos, yPos, spd)
        {
            speed = spd;
            govObject = gov;
            govName = "Projectile";
            textures = AssetManager.GetGovTextureAssets(govName);
            position = _position;
            position.X += gov.width / 2;
            position.Y += gov.height / 2;
            textureAssetIndex = 1;
            currentSprite = textures[textureAssetIndex].sprite;

            targetVector.X = ((xPos - Camera.screenOffset.X) - 
                (((position.X - Camera.screenOffset.X)) + currentSprite.Width/2)) + 
                (ObjectManager.currentPlayer.width/2);

            targetVector.Y = ((yPos - Camera.screenOffset.Y) - 
                (((position.Y - Camera.screenOffset.Y)) + currentSprite.Width / 2)) + 
                (ObjectManager.currentPlayer.height/2);

            double mouseVectorMagnitude = (Math.Sqrt(((Math.Pow(targetVector.X, 2)) + (Math.Pow(targetVector.Y, 2)))));
            direction.X = (float)(targetVector.X * (1 / mouseVectorMagnitude));
            direction.Y = (float)(targetVector.Y * (1 / mouseVectorMagnitude));
        }
        public override void Update(GameTime gameTime)
        {
            currentSprite = textures[textureAssetIndex].sprite;
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            velocity = speed * direction;
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            oldPos = position;
            ObjectManager.currentColMap.Remove(oldPos, this);
            position += velocity;
            ObjectManager.currentColMap.Insert(position, this);
            collisions = ObjectManager.CheckCollisions(this, govObject);
            if(collisions.Count > 0)
            {
                // Call hit events here
                foreach(GameObject obj in collisions)
                {
                    if (obj.GetType() == typeof(Player))
                    {
                        Player pl = (Player)obj;
                        pl.TakeDamage(1);
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
