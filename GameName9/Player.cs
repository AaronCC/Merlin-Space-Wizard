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
using System.IO;
namespace GameName9
{
    class Player : GameObject
    {

        
        // Player's velocity
        public int manaRegen;
        private Vector2 velocity;
        public int skillPoints;
        //State of keyInput
        private Vector2 keyState;
        // future Position
        private Vector2 tempPos;
        public Vector2 centerVector;
        public int health = 10;
        public Vector2 colGridIndex;
        public int mana = 40;
        public int maxMana = 40;
        public int maxHealth = 10;
        public int currentExperience;
        public int experinceToNextLevel;
        public TargetStack attackStack = new TargetStack();
        Rectangle mouseBox;
        public int knownAbilities;
        private int manaRegenTimer;
        // Has / Hasn't moved this frame
        bool movedX;
        bool movedY;
        // Player's speed
        const int speed = 3;
        // Paramaterized Constructor
        public bool reverseSprite;
        public int fireRate = 100;
        private int timeSinceLastAttack;
        public bool isDead;
        public Type projectileType;
        public Player(Vector2 levelIndex, bool canCollide)
            : base(levelIndex, canCollide)
        {
            colGridIndex = levelIndex;
            manaRegen = 5;
            skillPoints = 0;
            currentExperience = 0;
            experinceToNextLevel = 5;
            knownAbilities = 1;
            // Set attributes
            govName = "Player";
            projectileType = typeof(MagicMissile);
            textures = AssetManager.GetGovTextureAssets(govName);
            sounds = AssetManager.GetGovSoundAssets(govName);
            textureAssetIndex = 0;
            // Get the currentSprite from the current textureAsset
            currentSprite = textures[textureAssetIndex].sprite;
            // set width and height
            width = currentSprite.Width / textures[textureAssetIndex].cols;
            height = currentSprite.Height / textures[textureAssetIndex].rows;
            // set current row/col
            row = (int)frameIndex.Y;
            column = (int)frameIndex.X;
            Camera.screenOffset.X = 0;
            Camera.screenOffset.Y = 0;
           
            centerVector.X = ((Camera.SCREEN_WIDTH / 2) - (height / 2)) + Camera.screenOffset.X;
            centerVector.Y = ((Camera.SCREEN_HEIGHT / 2) - (width / 2)) + Camera.screenOffset.Y;
            hitBox = new Rectangle((int)position.X, (int)position.Y, width, height);

            Camera.InitializeCamera(this);
            Camera.CenterCamera();
        }
        public override void Update(GameTime gameTime)
        {
            
            if (isDead)
            {
                GameOver();
                
                return;
            }
            Camera.CenterCamera();
            if (currentExperience >= experinceToNextLevel)
            {
                    mouseBox = new Rectangle((int)Game1.mState.X, (int)Game1.mState.Y, 1, 1);
                if (Game1.mState.LeftButton == ButtonState.Pressed && Game1.oldmState.LeftButton == ButtonState.Released && mouseBox.Intersects(HUD.levelUpButton.clickBox))
                {
                    HUD.levelUpButton.ClickEvent();
                    return;
                }
            }
            CalcAttackVectors();
            manaRegenTimer += gameTime.ElapsedGameTime.Milliseconds;
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            RegenMana();
            millisecondsPerFrame = textures[textureAssetIndex].millisecondsPerFrame;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                frameIndex.X++;
                if (frameIndex.X == textures[textureAssetIndex].cols)
                    frameIndex.X = 0;
                timeSinceLastFrame = 0;
            }
            // Hasn't moved this frame
            movedX = false;
            movedY = false;
            // Get the current sprite
            currentSprite = textures[textureAssetIndex].sprite;
            SetVelocity();
            CalcFrameIndex();
            
            CheckCollisions();
            // If it hasn't moved yet

            if (!movedX)
            {
                // Move
                position.X += velocity.X;
                
            }
            if (!movedY)
            {
                // Move
                position.Y += velocity.Y;
            }
            ObjectManager.currentColMap.Insert(position, this);
            Camera.CenterCamera();
            // Update the hitbox based on new position
            hitBox = new Rectangle((int)position.X, (int)position.Y, width, height);
            timeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            if (Game1.mState.LeftButton == ButtonState.Pressed && timeSinceLastAttack > fireRate)
            {
                Attack();
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentExperience >= experinceToNextLevel)
            {
                HUD.levelUpButton.Draw(spriteBatch);
            }
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
                (position.Y - Camera.screenOffset.Y) + (height - 8)), new Rectangle(0, 0, 30, 15), Color.White * 0.6f, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            // Draw the player
            if (!reverseSprite)
                spriteBatch.Draw(currentSprite, position - Camera.screenOffset, destinationRectangle, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            else if (reverseSprite)
                spriteBatch.Draw(currentSprite, position - Camera.screenOffset, destinationRectangle, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
            //spriteBatch.DrawString(Game1.testFont, "X: " + Camera.screenOffset.X.ToString() + ", Y: " + Camera.screenOffset.Y.ToString(), new Vector2(0, 0), Color.White);
        }
        private void Attack()
        {
            if (projectileType == typeof(MagicMissile))
            {
                MagicMissile proj = new MagicMissile(position, true, this, Game1.mState.X, Game1.mState.Y, 6);
                if (mana - proj.manaCost >= 0)
                {
                    sounds[0].sound.Play();
                    mana -= proj.manaCost;
                    GameObjectQueue.EnQueue(proj);
                }
            }
            if (projectileType == typeof(FireBall))
            {
                FireBall proj = new FireBall(position, true, this, Game1.mState.X, Game1.mState.Y, 3);
                if (mana - proj.manaCost >= 0)
                {
                    sounds[0].sound.Play();
                    mana -= proj.manaCost;
                    GameObjectQueue.EnQueue(proj);
                }
            }
            if (projectileType == typeof(ManaBall))
            {
                ManaBall proj = new ManaBall(position, true, this, Game1.mState.X, Game1.mState.Y, 8, true);
                if (mana - proj.manaCost >= 0)
                {
                    sounds[0].sound.Play();
                    
                    mana -= proj.manaCost;
                    GameObjectQueue.EnQueue(proj);
                }
            }

            timeSinceLastAttack = 0;
        }
        private void CalcFrameIndex()
        {

            if (velocity.X > 0 && velocity.Y == 0)
            {

                if (frameIndex.Y < 3)
                    frameIndex.Y = 1;
                else if (frameIndex.Y >= 3)
                    frameIndex.Y = 4;
                reverseSprite = false;
            }
            else if (velocity.X < 0 && velocity.Y == 0)
            {
                if (frameIndex.Y < 3)
                    frameIndex.Y = 1;
                else if (frameIndex.Y >= 3)
                    frameIndex.Y = 4;
                reverseSprite = true;
            }
            else if (velocity.X == 0 && velocity.Y > 0)
            {
                frameIndex.Y = 2;
            }
            else if (velocity.X == 0 && velocity.Y < 0)
            {
                frameIndex.Y = 5;
            }
            else if (velocity.X > 0 && velocity.Y > 0)
            {
                frameIndex.Y = 1;
                reverseSprite = false;
            }
            else if (velocity.X < 0 && velocity.Y > 0)
            {
                frameIndex.Y = 1;
                reverseSprite = true;
            }
            else if (velocity.X > 0 && velocity.Y < 0)
            {
                frameIndex.Y = 4;
                reverseSprite = false;
            }
            else if (velocity.X < 0 && velocity.Y < 0)
            {
                frameIndex.Y = 4;
                reverseSprite = true;
            }
            else if (velocity.X == 0 && velocity.Y == 0)
            {
                if (frameIndex.Y < 3)
                    frameIndex.Y = 0;
                if (frameIndex.Y >= 3)
                    frameIndex.Y = 3;
            }
        }
        private void SetVelocity()
        {
            if ((Game1.KBstate.IsKeyUp(Keys.A) && Game1.KBstate.IsKeyUp(Keys.D)))
            {
                keyState.X = -1;
            }
            if (keyState.X == 0 && Game1.KBstate.IsKeyDown(Keys.D) && Game1.oldKBstate.IsKeyUp(Keys.D))
            {
                keyState.X = 1;
            }
            if (keyState.X == 1 && Game1.KBstate.IsKeyDown(Keys.A) && Game1.oldKBstate.IsKeyUp(Keys.A))
            {
                keyState.X = 0;
            }
            if (Game1.KBstate.IsKeyDown(Keys.A) && (keyState.X == -1 || (Game1.oldKBstate.IsKeyUp(Keys.D) && Game1.KBstate.IsKeyUp(Keys.D))))
            {
                keyState.X = 0;
            }
            if (Game1.KBstate.IsKeyDown(Keys.D) && (keyState.X == -1 || (Game1.oldKBstate.IsKeyUp(Keys.A) && Game1.KBstate.IsKeyUp(Keys.A))))
            {
                keyState.X = 1;
            }
            // Set velocity and frameindex based on keystate
            // *** This will set the frameIndex.Y and incriment frameIndex.X based on millisecondsPerFrame in future updates ***
            if (keyState.X == 1)
            {
                velocity.X = speed;
            }
            if (keyState.X == 0)
            {
                velocity.X = -speed;
            }
            if (keyState.X == -1)
            {
                velocity.X = 0;
                velocity.Y = 0;
            }
            if (Game1.KBstate.IsKeyDown(Keys.S))
            {
                keyState.Y = 2;
                velocity.Y = speed;
            }
            else if (Game1.KBstate.IsKeyDown(Keys.W))
            {
                keyState.Y = 2;
                velocity.Y = -speed;
            }
            else
            {
                velocity.Y = 0;
                keyState.Y = -1;
            }

        }
        private void CheckCollisions()
        {
            oldPos = position;
            ObjectManager.currentColMap.Remove(oldPos, this);
            // initialize tempPos
            tempPos = position;
            // set it = to what pos will be
            tempPos.X += velocity.X;
            // set hitBox
            hitBox = new Rectangle((int)tempPos.X, (int)tempPos.Y, width, height);
            ObjectManager.currentColMap.Insert(tempPos, this);
            collidingWith = ObjectManager.CheckCollisions(this);
            foreach (GameObject obj in collidingWith)
            {
                if (obj.name.Contains("Portal"))
                {
                    ObjectManager.Changelevel(ObjectManager.levelIndex + 1);
                    break;
                }
            }
            // Foreach collision
            foreach (GameObject obj in collidingWith)
            {
                // If it's colliding with a block
                if (obj.name.Contains("Block"))
                {
                    if (keyState.X != -1)
                        movedX = true;
                    ObjectManager.currentColMap.Remove(tempPos, this);
                    tempPos.X -= velocity.X;
                }
            }
            tempPos.Y += velocity.Y;
            ObjectManager.currentColMap.Insert(tempPos, this);
            // set hitBox
            hitBox = new Rectangle((int)tempPos.X, (int)tempPos.Y, width, height);
            collidingWith = ObjectManager.CheckCollisions(this);
            // Foreach collision
            foreach (GameObject obj in collidingWith)
            {
                // If it's colliding with a block
                if (obj.name.Contains("Block"))
                {
                    if (keyState.Y != -1)
                        movedY = true;
                    ObjectManager.currentColMap.Remove(tempPos, this);
                    tempPos.Y -= velocity.Y;
                }
            }
        }
        public void TakeDamage(int dmg)
        {
            health -= dmg;
            sounds[1].sound.Play();
            if (health <= 0)
            {
                isDead = true;
            }
        }
        public void LevelUp()
        {
            skillPoints += 5;
            currentExperience -= experinceToNextLevel;
            experinceToNextLevel += 10;
            Game1.gameState = 0;
            ObjectManager.currentMenuName = "LevelUpMenu";
        }
        public void GameOver()
        {
            Camera.CenterCamera();
            Game1.windowed = true;
            ObjectManager.levelIndex = 0;
            Parser.music.Stop();
            Game1.gameState = 0;
            ObjectManager.currentMenuName = "GameOverMenu";
            RemoveObjectQueue.EnQueue(name);
        }
        public void CalcAttackVectors()
        {
            double interval = Math.PI / 4;
            double cos;
            double sin;
            double angle;
            double distance = 100;
            double enemyGroupCount = 0;
            double stage = 1;
            double stageCapacity = 8;
            Vector2 tempAttackVector;

            while (attackStack.Count() <= ObjectManager.enemyCount)
            {
                angle = interval * (attackStack.Count() % (stageCapacity));
                cos = Math.Cos(angle);
                sin = Math.Sin(angle);
                tempAttackVector = new Vector2((float)(position.X + (cos * distance)), (float)(position.Y + (sin * distance)));
                attackStack.Push(tempAttackVector);
                enemyGroupCount++;
                if ((enemyGroupCount % (stageCapacity)) == 0)
                {
                    stage++;
                    distance += 100;
                    interval -= interval / 2;
                    enemyGroupCount = 0;
                    stageCapacity *= 2;
                }
            }
        }
        public void RegenMana()
        {
            if (manaRegenTimer >= 1000)
            {
                if (mana < maxMana)
                    mana += manaRegen;
                manaRegenTimer = 0;
                if (mana > maxMana)
                    mana = maxMana;
            }
        }
    }

}
