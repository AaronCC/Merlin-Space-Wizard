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
    class Enemy : GameObject
    {
        private Vector2 velocity;
        public int waitToFire;
        public int experience;
        // State of keyInput
        bool canFire = false;
        //private Vector2 keyState;
        // future Position
        private Vector2 tempPos;
        public Vector2 centerVector;
        // Player's speed
        const int speed = 1;
        public Vector2 targetVector;
        public Vector2 targetPoint;
        public Vector2 direction;
        // Paramaterized Constructor
        bool reverseSprite;
        private int fireRate = 1500;
        private Random rand;
        private int timeSinceLastAttack;
        public int health;
        public bool isDead = false;
        private List<GameObject> collisions;
        public Enemy(Vector2 levelIndex, bool canCollide)
            : base(levelIndex, canCollide)
        {
            experience = 1;
            // Set attributes
            govName = "Enemy";
            health = 5;
            textures = AssetManager.GetGovTextureAssets(govName);
            
            //sounds = AssetManager.GetGovSoundAssets(govName);
            textureAssetIndex = 0;
            // Get the currentSprite from the current textureAsset
            currentSprite = textures[textureAssetIndex].sprite;
            // set width and height
            width = currentSprite.Width / textures[textureAssetIndex].cols;
            height = currentSprite.Height / textures[textureAssetIndex].rows;
            // set current row/col
            row = (int)frameIndex.Y;
            column = (int)frameIndex.X;
            hitBox = new Rectangle((int)position.X, (int)position.Y, width, height);
            rand = new Random();
            waitToFire = rand.Next(0, 2000);
            rand = new Random((int)(levelIndex.X * levelIndex.Y * this.waitToFire));
            waitToFire = rand.Next(0, 2000);
        }
        public override void Update(GameTime gameTime)
        {
            if(!(hitBox.Intersects(Camera.camHitBox)))
            {
                return;
            }
            if (isDead)
            {
                RemoveObjectQueue.EnQueue(name);
                ObjectManager.currentPlayer.currentExperience += experience;
                return;
            }
            millisecondsPerFrame = textures[textureAssetIndex].millisecondsPerFrame;
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            timeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            waitToFire -= gameTime.ElapsedGameTime.Milliseconds;
            if (waitToFire <= 0)
                canFire = true;
            if (timeSinceLastAttack > fireRate && canFire)
            {
                Attack();
            }
            double targetVectorMagnitude;
            targetPoint = ObjectManager.currentPlayer.attackStack.Pop();
            targetVector.X = ((targetPoint.X - Camera.screenOffset.X) -
                (((position.X - Camera.screenOffset.X))));
            targetVector.Y = ((targetPoint.Y - Camera.screenOffset.Y) -
               (((position.Y - Camera.screenOffset.Y))));
            targetVectorMagnitude = (Math.Sqrt(((Math.Pow(targetVector.X, 2)) + (Math.Pow(targetVector.Y, 2)))));
            
            if (targetVectorMagnitude < speed)
            {
                position.X = targetPoint.X;
                position.Y = targetPoint.Y;
            }
            direction.X = (float)(targetVector.X * (1 / targetVectorMagnitude));
            direction.Y = (float)(targetVector.Y * (1 / targetVectorMagnitude));
            oldPos = position;
            ObjectManager.currentColMap.Remove(oldPos, this);
            if (position.X != targetPoint.X && position.Y != targetPoint.Y)
                position += direction * speed;
            hitBox = new Rectangle((int)position.X, (int)position.Y, width, height);
            ObjectManager.currentColMap.Insert(position, this);
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            millisecondsPerFrame = textures[textureAssetIndex].millisecondsPerFrame;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                frameIndex.X++;
                if (frameIndex.X == textures[textureAssetIndex].cols)
                    frameIndex.X = 0;
                timeSinceLastFrame = 0;
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 healthPos;
            healthPos.Y = (position.Y - Camera.screenOffset.Y) + height;
            healthPos.X = (position.X - Camera.screenOffset.X) - 17;
            spriteBatch.Draw(HUD.orbTextures[1], new Vector2(healthPos.X - 5, healthPos.Y - 2), Color.White);
            spriteBatch.DrawString(Game1.testFont, health.ToString(), healthPos, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            // Width = the spritesheet's width / the number of columns in the spritesheet
            width = currentSprite.Width / textures[textureAssetIndex].cols;
            // Height = the spritesheet's height / the number of rows in the spritesheet
            height = currentSprite.Height / textures[textureAssetIndex].rows;
            // row = (int)float*frameindex.Y
            row = (int)frameIndex.Y;
            // column = (int)float*frameindex.X
            column = (int)frameIndex.X;
            // Create the destiationRectangle in the spritesheet
            destinationRectangle = new Rectangle(width * column, height * row, width, height);
            spriteBatch.Draw(Game1.shadow, new Vector2((position.X - Camera.screenOffset.X) + 3,
                (position.Y - Camera.screenOffset.Y) + (height - 10)), new Rectangle(0, 0, 30, 15), Color.White * 0.6f, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            // Draw the player
            if (!reverseSprite)
                spriteBatch.Draw(currentSprite, position - Camera.screenOffset, destinationRectangle, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            else if (reverseSprite)
                spriteBatch.Draw(currentSprite, position - Camera.screenOffset, destinationRectangle, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);

        }
        public void TakeDamage(int dmg)
        {
            health -= dmg;
            if (health <= 0)
                isDead = true;
        }
        private void Attack()
        {
            GameObjectQueue.EnQueue(new EnemyProjectile(position, true, this,
                ObjectManager.currentPlayer.position.X,
                ObjectManager.currentPlayer.position.Y, 4));
            timeSinceLastAttack = 0;
        }
    }
}
