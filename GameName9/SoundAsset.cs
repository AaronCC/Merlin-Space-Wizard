using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;

namespace GameName9
{
    class SoundAsset
    {
        // **Coming Soon**
        public string name;
        public double duration;
        public bool loop;
        public SoundEffect sound;

        public SoundAsset(double d, bool l, SoundEffect s, string n)
        {
            duration = d;
            loop = l;
            sound = s;
            name = n;
        }
    }
}
