using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
////using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
namespace GameName9
{
    class GameObject
    {
        public Type objectType;
        public List<SoundAsset> sounds;
        // Object's collision rectangle
        public Rectangle hitBox;
        // true if colliding with canCollide object
        public bool isColliding;
        // Object's name
        public string name;
        // List of object's TextureAssets
        protected List<TextureAsset> textures;
        // Object's govName (name of it's governing body)
        protected int timeSinceLastFrame;
        protected int millisecondsPerFrame;

        public List<Rectangle> collisionCollections = new List<Rectangle>();
        public string govName;
        // Position of the object
        public Vector2 position;
        // Index for Object's spritesheet
        protected Vector2 frameIndex;
        // Index for the current textureAsset
        protected int textureAssetIndex;
        // Current Texture2D to draw from
        protected Texture2D currentSprite;
        // Can be collided with
        public bool canCollide;

        protected Vector2 oldPos;
        // Rectangle used to denote location in a spritesheet
        protected Rectangle destinationRectangle;
        /// <summary>
        /// width: width of the object (sprite/cols)
        /// height: height of the object (sprite/rows)
        /// row: current row in spritesheet
        /// column: current column in spritesheet
        /// </summary>
        public int width, height, row, column;
        // List of objects that are colliding with this object
        public List<GameObject> collidingWith;
        // Paramaterized Constructor
        public GameObject(Vector2 levelIndex, bool _canCollide)
        {
            // set initial position and canCollide
            position.X = levelIndex.X * 50;
            position.Y = levelIndex.Y * 50;
            canCollide = _canCollide;
            objectType = this.GetType();
        }
        public virtual void Update(GameTime gameTime)
        {
            // overriden method
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // overriden method
        }
    }
}