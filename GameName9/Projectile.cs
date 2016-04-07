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
    class Projectile : GameObject
    {
        public GameObject govObject;
        public int speed;
        public Vector2 direction;

        public Vector2 colGridIndex;
        public Vector2 velocity;
        public Vector2 targetVector;
        public List<GameObject> collisions;
        public Projectile(Vector2 _position, bool canCollide, GameObject gov, float xPos, float yPos, int spd)
            : base(_position, canCollide)
        {

        }
        public override void Update(GameTime gameTime)
        {
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
