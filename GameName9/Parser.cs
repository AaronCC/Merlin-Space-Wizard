#region Using Statements
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
#endregion
namespace GameName9
{
    static class Parser
    {
        /// <summary>
        /// Parses Menus.txt
        /// </summary>
        /// System.IO.StreamReader(Menus.txt);
        /// <param name="stream"></param>
        /// Dictionary of menuNames and their button's dataString
        /// <returns></returns>
        static public Dictionary<string, List<string>> ParseMenus(System.IO.StreamReader stream)
        {
            // Dictionary of menuNames and thier button's names
            Dictionary<string, List<string>> menus = new Dictionary<string, List<string>>();
            // line of the file and governing body's name
            string line, govName;
            // line.Split(',')
            string[] splitLine;
            // number of buttons
            int numButtons;
            // While there are still lines in the file
            while ((line = stream.ReadLine()) != null)
            {
                splitLine = line.Split(',');
                govName = splitLine[0];
                numButtons = Convert.ToInt32(splitLine[1]);
                menus.Add(govName, new List<string>());
                // For each button
                for (int x = 0; x < numButtons; x++)
                {
                    // Read the line
                    line = stream.ReadLine();
                    // Add the dataString to menus
                    menus[govName].Add(line);
                }
            }
            // Close the stream
            stream.Close();
            return menus;
        }
        /// <summary>
        /// Creates the game's menuObjects
        /// </summary>
        /// <param name="menuButtons"></param>
        /// <param name="menuGovDict"></param>
        static public void CreateMenus(Dictionary<string, List<Texture2D>> menus, Dictionary<string, List<string>> menuGovDict)
        {
            // Index i
            // names for menu and button
            string menuName, buttonName;
            // line.Split(',') of buttondata
            string[] buttonString;
            // Location of the button on the menu
            Vector2 buttonLocation;
            // Temp button obj
            Button tempButton;
            // List of menu textures
            List<Texture2D> menuTextures;
            // Menu's texture
            Texture2D menuSprite;
            // Temp Menu obj
            GameMenu tempMenu;
            // Dictionary of buttons and their locations
            Dictionary<Button, Vector2> buttons;
            foreach (KeyValuePair<string, List<Texture2D>> pair in menus)
            {
                // set menuName
                menuName = pair.Key;
                // Get it's textures from the AssetManager
                menuTextures = AssetManager.GetGovTextures(menuName);
                buttons = new Dictionary<Button, Vector2>();
                // Foreach texture
                foreach (Texture2D mSprite in menuTextures)
                {
                    int i = 0;
                    // current menuSprite
                    menuSprite = mSprite;
                    foreach (Texture2D sprite in pair.Value)
                    {
                        // Split the buttonDataString and set vars
                        buttonString = menuGovDict[menuName][i].Split(',');
                        buttonName = buttonString[0];
                        buttonLocation.X = Convert.ToInt32(buttonString[1]);
                        buttonLocation.Y = Convert.ToInt32(buttonString[2]);
                        // Switch (buttonName) to button objects
                        switch (buttonName)
                        {
                            case "StartButton":
                                // Create a startButton and add it to list of buttons
                                tempButton = new StartButton(sprite, buttonName);
                                buttons.Add(tempButton, buttonLocation);
                                break;
                            case "ControlsButton":
                                tempButton = new ControlsButton(sprite, buttonName);
                                buttons.Add(tempButton, buttonLocation);
                                break;
                            case "MainMenuButton":
                                tempButton = new MainMenuButton(sprite, buttonName);
                                buttons.Add(tempButton, buttonLocation);
                                break;
                            case "PlusButton":
                                tempButton = new PlusButton(sprite, buttonName);
                                buttons.Add(tempButton, buttonLocation);
                                break;
                            default:
                                tempButton = new Button(sprite, buttonName);
                                buttons.Add(tempButton, buttonLocation);
                                break;
                        }
                        i++;
                    }
                    // Create a temporary menu obj
                    switch (menuName)
                    {
                        case "LevelUpMenu":
                            tempMenu = new LevelUpMenu(menuSprite, menuName, buttons);
                            break;
                        case "ReadyGoMenu":
                            tempMenu = new ReadyGoMenu(menuSprite, menuName, buttons);
                            break;
                        default:
                            tempMenu = new GameMenu(menuSprite, menuName, buttons);
                            break;
                    }
                    // add it to the menu dictionary
                    ObjectManager.AddMenuToDictionary(tempMenu, menuName);
                }
            }
        }
        /// <summary>
        /// Parse level_index.txt
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="rNum"></param>
        /// <param name="cNum"></param>
        /// <returns></returns>
        static public char[,] ParseLevels(System.IO.StreamReader stream, int rNum, int cNum)
        {
            // 2D char array representing the level file
            char[,] level = new char[rNum, cNum];
            // A line of the file
            string line;
            // columns and rows
            int c = 0, r = 0;
            // While there are still lines in the file
            while ((line = stream.ReadLine()) != null)
            {
                // parse the lines into chars
                for (c = 0; c < line.Length; c++)
                {
                    level[r, c] = line[c];
                }
                r++;
            }
            // close the stream
            stream.Close();
            // return the level[,]
            return level;
        }
        /// <summary>
        /// Parses sound and texture assets
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        static public Dictionary<string, List<string>> ParseAssets(System.IO.StreamReader stream)
        {
            // Dictionary of objName and list of dataStrings
            Dictionary<string, List<string>> _assets = new Dictionary<string, List<string>>();
            // Line of file, asset's gov, asset's name
            string line, assetGov, assetName;
            // line.Split()
            string[] splitLine;
            // While there are still lines to read
            while ((line = stream.ReadLine()) != null)
            {
                // Split the line and set asset's vars
                splitLine = line.Split('_');
                assetGov = splitLine[0];
                assetName = splitLine[1];
                // If the gov was already added
                if (_assets.ContainsKey(assetGov))
                {
                    // Add to the gov's list
                    _assets[assetGov].Add(assetName);
                }
                else
                {
                    // Create a new element
                    _assets.Add(assetGov, new List<string>());
                    // Add to that element's list
                    _assets[assetGov].Add(assetName);
                }
            }
            // Clost the stream
            stream.Close();
            // Return the assets
            return _assets;
        }
        /// <summary>
        /// Creates texture assets
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Dictionary<string, List<TextureAsset>> ParseTextureData(System.IO.StreamReader stream)
        {
            // Dictionary from ParseAssets
            Dictionary<string, List<string>> textureData = new Dictionary<string, List<string>>();
            // Dictionary of govNames and their textureAssets
            Dictionary<string, List<TextureAsset>> _textureAssets = new Dictionary<string, List<TextureAsset>>();
            // temporary textureAsset
            TextureAsset tempAsset;
            // List of tempTextures
            List<Texture2D> _sprites;
            // List of tempAssets
            List<TextureAsset> tempAssets = new List<TextureAsset>();
            // line of file, name of textures, texture's gov
            string line, textureName, _textureGov;
            // line.Split()
            string[] splitLine;
            // vars for the textureAsset obj
            int _rows = 0, _cols = 0, _millisecondsPerFrame = 0;
            // newGovName is being read
            bool newGov = true;
            // While there are lines to read
            while ((line = stream.ReadLine()) != null)
            {
                // If it's a new gov
                if (newGov)
                {
                    // No longer a new gov
                    newGov = false;
                    _textureGov = line;
                    // Get the list of texture's for _textureGov from AssetManager
                    _sprites = AssetManager.GetGovTextures(_textureGov);
                    // initialize tempAssets
                    tempAssets = new List<TextureAsset>();
                    // foreach texture in _sprites
                    foreach (Texture2D sprite in _sprites)
                    {
                        // read a new line
                        line = stream.ReadLine();
                        // Split the line and set vars
                        splitLine = line.Split(',');
                        _rows = Convert.ToInt32(splitLine[0]);
                        _cols = Convert.ToInt32(splitLine[1]);
                        _millisecondsPerFrame = Convert.ToInt32(splitLine[2]);
                        textureName = splitLine[3];
                        // Create a tempAsset
                        tempAsset = new TextureAsset(_rows, _cols, _millisecondsPerFrame, sprite, textureName);
                        // Add it to tempAssets
                        tempAssets.Add(tempAsset);
                    }
                    // newGoverning body
                    newGov = true;
                    // Add tempAssets and the last gov to _textureAssets
                    _textureAssets.Add(_textureGov, tempAssets);
                }
            }
            // Close the stream
            stream.Close();
            // Return the dictionary
            return _textureAssets;
        }
        /// <summary>
        /// Creates soundAssets
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Dictionary<string, List<SoundAsset>> ParseSoundData(System.IO.StreamReader stream) // Coming soon!
        {
            Dictionary<string, List<SoundAsset>> _soundAssets = new Dictionary<string, List<SoundAsset>>();
            // temporary textureAsset
            SoundAsset tempAsset;
            // List of tempTextures
            List<SoundEffect> _sounds;
            // List of tempAssets
            List<SoundAsset> tempAssets = new List<SoundAsset>();
            // line of file, name of textures, texture's gov
            string line, soundName, _soundGov;
            // line.Split()
            string[] splitLine;
            // vars for the textureAsset obj
            double duration = 0;
            bool loop;
            // newGovName is being read
            bool newGov = true;
            // While there are lines to read
            while ((line = stream.ReadLine()) != null)
            {
                // If it's a new gov
                if (newGov)
                {
                    // No longer a new gov
                    newGov = false;
                    _soundGov = line;
                    // Get the list of texture's for _textureGov from AssetManager
                    _sounds = AssetManager.GetGovSounds(_soundGov);
                    // initialize tempAssets
                    tempAssets = new List<SoundAsset>();
                    // foreach texture in _sprites
                    foreach (SoundEffect sound in _sounds)
                    {
                        // read a new line
                        line = stream.ReadLine();
                        // Split the line and set vars
                        splitLine = line.Split(',');
                        duration = Convert.ToDouble(splitLine[0]);
                        loop = Convert.ToBoolean(Convert.ToInt32(splitLine[1]));
                        soundName = splitLine[2];
                        // Create a tempAsset
                        tempAsset = new SoundAsset(duration, loop, sound, soundName);
                        // Add it to tempAssets
                        tempAssets.Add(tempAsset);
                    }
                    // newGoverning body
                    newGov = true;
                    // Add tempAssets and the last gov to _textureAssets
                    _soundAssets.Add(_soundGov, tempAssets);
                }
            }
            // Close the stream
            stream.Close();
            // Return the dictionary
            return _soundAssets;
        }
        /// <summary>
        /// Creates gameObjects from a level object 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="rNum"></param>
        /// <param name="cNum"></param>
        /// SoundEffectInstance oldMusic;
        public static SoundEffectInstance music;
        public static void LoadLevel(Level level)
        {

            int r, c;
            if (music != null)
                music.Stop();
            music = level.song.CreateInstance();
            music.IsLooped = level.songs[level.index].loop;
            music.Play();

            // row and column index
            foreach (KeyValuePair<string, GameObject> pair in ObjectManager.objectDictionary)
            {
                if (!pair.Key.Contains("Player"))
                    RemoveObjectQueue.EnQueue(pair.Key);
            }
            while (!RemoveObjectQueue.IsEmpty())
            {
                ObjectManager.RemoveObjectFromDictionary(RemoveObjectQueue.DeQueue());
            }
            ObjectManager.gameObjectLocations.Clear();
            r = 0;
            c = 0;
            // step through the level array
            for (c = 0; c < Game1.LEVEL_COLUMNS; c++)
            {
                for (r = 0; r < Game1.LEVEL_ROWS; r++)
                {
                    
                    // switch(char) to create gameObjects
                    switch (level.levelArray[r, c])
                    {
                        case 'x':
                            // Create a new Block
                            Block block = new Block(new Vector2(c, r), true);
                            // Add it to the ObjectManager's objectDictionary
                            ObjectManager.AddObjectToDictionary(block, "Block");

                            break;
                        case 'p':
                            Player player = new Player(new Vector2(c, r), true);
                            ObjectManager.AddObjectToDictionary(player, "Player");
                            break;
                        case '-':
                            // Create a new square
                            //Square square = new Square(new Vector2(c, r), false);
                            // Add it to the ObjectManager's objectDictionary
                            //ObjectManager.AddObjectToDictionary(square, "Square");
                            break;
                        case 'e':
                            Enemy enemy = new Enemy(new Vector2(c, r), true);
                            ObjectManager.AddObjectToDictionary(enemy, "Enemy");
                            break;
                        case 'o':
                            Square sq = new Square(new Vector2(c, r), true);
                            ObjectManager.AddObjectToDictionary(sq, "Block");
                            break;
                        default:
                            break;
                    }
                }
            }
            ObjectManager.currentPlayer.mana = ObjectManager.currentPlayer.maxMana;

            ObjectManager.currentPlayer.health = ObjectManager.currentPlayer.maxHealth;
        }
    }
}