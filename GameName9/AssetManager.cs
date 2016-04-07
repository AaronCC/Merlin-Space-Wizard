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
    /// Manages game assets
    /// </summary>
    static class AssetManager
    {
        /// <summary>
        /// Dictionary of govNames and a list of their textures
        /// </summary>
        static Dictionary<string, List<Texture2D>> textures;
        /// <summary>
        /// Dictionary of govNames and a list of their textureAssets
        /// </summary>
        static Dictionary<string, List<TextureAsset>> textureAssets = new Dictionary<string, List<TextureAsset>>();
        /// <summary>
        /// Dictionary of govNames and a list of their sounds
        /// </summary>
        static Dictionary<string, List<SoundEffect>> sounds;
        /// <summary>
        /// Dictionary of govNames and a list of their soundAssets
        /// </summary>
        static Dictionary<string, List<SoundAsset>> soundAssets = new Dictionary<string, List<SoundAsset>>();
        /// <summary>
        /// Dictionary of menuNames and its button textures
        /// </summary>
        static Dictionary<string, List<Texture2D>> menuButtons = new Dictionary<string, List<Texture2D>>();
        
        public static void InitializeAssets(Dictionary<string, List<Texture2D>> _textures, Dictionary<string, List<SoundEffect>> _sounds, Dictionary<string,List<Texture2D>> _menuButtons, Dictionary<string,List<string>> menuGovDict)
        {
            // Initialize vars
            menuButtons = _menuButtons;
            textures = _textures;
            sounds = _sounds;
            // Initialize stream: TextureData.txt
            System.IO.StreamReader textureDataFile = new System.IO.StreamReader("../../../Content/Textures/TextureData.txt");
            // Parse texturedata.txt
            textureAssets = Parser.ParseTextureData(textureDataFile);
            // Create menu objects (menuButtons, Dictionary of menuNames and the data for its buttons)
            // Sounds Coming Soon!
            System.IO.StreamReader soundDataFile = new System.IO.StreamReader("../../../Content/Sounds/SoundData.txt");
            soundAssets = Parser.ParseSoundData(soundDataFile);

            Parser.CreateMenus(menuButtons, menuGovDict);
        }
        public static List<TextureAsset> GetGovTextureAssets(string key)
        {
            // Returns the gov's list of texture assets
            return textureAssets[key];
        }
        public static List<SoundAsset> GetGovSoundAssets(string key)
        {
            // Returns the gov's list of sound assets
            return soundAssets[key];
        }
        public static List<SoundEffect> GetGovSounds(string key)
        {
            // Returns the gov's list of sounds
            return sounds[key];
        }
        public static List<Texture2D> GetGovTextures(string key)
        {
            // Returns the gov's list of textures
            return textures[key];
        }
    }

}
