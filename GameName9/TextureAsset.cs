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
    /// <summary>
    /// Contains information about a sprite
    /// </summary>
    class TextureAsset
    {
        /// <summary>
        /// Milliseconds inbetween frameIndex incriments
        /// </summary>
        public int millisecondsPerFrame { get; set; }
        /// <summary>
        /// Columns in the sprite
        /// </summary>
        public int cols { get; set; }
        /// <summary>
        /// Rows in the sprite
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// Name of the sprite
        /// </summary>
        public string textureName { get; set; }
        /// <summary>
        /// The sprite
        /// </summary>
        public Texture2D sprite { get; set; }
        public TextureAsset(int r, int c, int mps, Texture2D s, string n)
        {
            millisecondsPerFrame = mps;
            cols = c;
            rows = r;
            textureName = n;
            sprite = s;
        }
    }
}
