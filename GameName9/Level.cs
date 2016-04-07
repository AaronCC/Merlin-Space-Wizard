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
    class Level
    {
        // ** more attributes in the future **
        // Char array of the level
        public char[,] levelArray { get; set; }
        // Paramaterized constructor
        public List<SoundAsset> songs;
        public SoundEffect song;
        public int index;
        public Level(char[,] l, int index)
        {
            // Set attributes
            levelArray = l;
            songs = AssetManager.GetGovSoundAssets("Level");
            song = songs[index].sound;
        }
    }
}
