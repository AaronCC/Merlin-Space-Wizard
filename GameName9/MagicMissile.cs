﻿using System;
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
    class MagicMissile : Projectile
    {
        public int manaCost;
        public MagicMissile(Vector2 _position, bool canCollide, GameObject gov, float xPos, float yPos, int spd)
            : base(_position, canCollide, gov, xPos, yPos, spd)
        {
            manaCost = 5;
            speed = spd;
            govObject = gov;
            govName = "Projectile";
            textures = AssetManager.GetGovTextureAssets(govName);
            position = _position;
            textureAssetIndex = 0;
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
            currentSprite = textures[textureAssetIndex].sprite;
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            velocity = speed * direction;
            oldPos = position;
            ObjectManager.currentColMap.Remove(oldPos, this);
            position += velocity;
            ObjectManager.currentColMap.Insert(position, this);
            hitBox = new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
            collisions = ObjectManager.CheckCollisions(this, govObject);
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
