#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using System.IO;
#endregion

namespace GameName9
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static SpriteFont testFont;
        // Number of levels in the game
        public const int NUM_LEVELS = 3;
        // Number of rows in each level 
        public const int LEVEL_ROWS = 20;
        // Number of columns in each level
        public const int LEVEL_COLUMNS = 20;
        public static Texture2D permaBackground;
        public static Texture2D shadow;
        public static bool fullScreen, windowed;
        /// <summary>
        /// The current Keyboard state
        /// </summary>
        public static KeyboardState KBstate { get; set; }
        /// <summary>
        /// The old Keyboard state
        /// </summary>
        public static KeyboardState oldKBstate { get; set; }

        /// <summary>
        /// 0: Menu
        /// 1: Game
        /// </summary>
        public static int gameState { get; set; } // Will use a stack for gamestate in the future 
        Dictionary<int, Texture2D> _bkgTextures = new Dictionary<int, Texture2D>();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // MouseState
        public static MouseState mState;
        public static MouseState oldmState;
        // Dictionary of menuNames and their button's textures
        Dictionary<string, List<Texture2D>> menuButtons = new Dictionary<string, List<Texture2D>>();
        // Dictionary of menuNames and their buttonNames
        Dictionary<string, List<string>> menuGovDict = new Dictionary<string, List<string>>();
        // Dictionary of Key<govName> and Value<List of that gov's textures> 
        Dictionary<string, List<Texture2D>> textures = new Dictionary<string, List<Texture2D>>();
        // Dictionarhy of Key<govName> and Value<List of names for that gov's textures>
        Dictionary<string, List<string>> textureGovDict = new Dictionary<string, List<string>>();
        // Dictionary of Key<int index> and Value<2D char[,] array of characters in that level index>
        Dictionary<int, Level> _levels = new Dictionary<int, Level>();
        // Temp 2D char[,] array to contain parsed leveldata
        char[,] tempLevelArray = new char[10, 10];
        // Temporary level object
        Level tempLevel;
        // Dictionary of govNames and a list of their sounds
        Dictionary<string, List<SoundEffect>> sounds = new Dictionary<string, List<SoundEffect>>();
        // A dictionary of govNames and a list of their textureNames
        Dictionary<string, List<string>> soundGovDict = new Dictionary<string, List<string>>();
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            gameState = 0;
            
            // TODO: Add your initialization logic here
            //graphics.IsFullScreen = true;
            // Set screenwidth & height
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.IsFullScreen = false;
            // Apply the changes
            graphics.ApplyChanges();
            // Initialize KB states
            KBstate = new KeyboardState();
            oldKBstate = new KeyboardState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // index 
            int i = 0;
            string govName, buttonName;
            Texture2D tempTexture;
            SoundEffect tempSound;
            testFont = Content.Load<SpriteFont>("MyFont");
            //SoundEffect tempSound;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Initialize streamreader
            System.IO.StreamReader textureFile = new System.IO.StreamReader("../../../Content/Textures/Textures.txt");
            // Call the parseAssets function, passing it the stream
            textureGovDict = Parser.ParseAssets(textureFile);
            // Foreach govName and list of textureNames in the pair
            foreach (KeyValuePair<string, List<string>> pair in textureGovDict)
            {
                // Add a new item to the textures dict
                textures.Add(pair.Key, new List<Texture2D>());
                foreach (string textureName in pair.Value)
                {
                    // Add each texture to that item
                    tempTexture = Content.Load<Texture2D>(textureName);
                    textures[pair.Key].Add(tempTexture);
                }
            }
            // Initialize new stream for the Menus.txt file
            System.IO.StreamReader menuFile = new System.IO.StreamReader("../../../Content/Textures/Menus.txt");
            // Parse the file
            menuGovDict = Parser.ParseMenus(menuFile);
            // Foreach menuName and list of buttonNames in menuGovDict
            foreach (KeyValuePair<string, List<string>> pair in menuGovDict)
            {
                govName = pair.Key;
                // Create new element in menuButtons
                menuButtons.Add(govName, new List<Texture2D>());
                // Add each texture to the new menuButtons element
                foreach (string line in pair.Value)
                {
                    buttonName = line.Split(',')[0];
                    tempTexture = Content.Load<Texture2D>(buttonName + ".png");
                    menuButtons[govName].Add(tempTexture);
                }
            }
            System.IO.StreamReader soundFile = new System.IO.StreamReader("../../../Content/Sounds/Sounds.txt");
            soundGovDict = Parser.ParseAssets(soundFile);
            foreach (KeyValuePair<string, List<string>> pair in soundGovDict)
            {
                // Add a new item to the textures dict
                sounds.Add(pair.Key, new List<SoundEffect>());
                foreach (string soundName in pair.Value)
                {
                    // Add each texture to that item
                    tempSound = Content.Load<SoundEffect>(soundName);
                    sounds[pair.Key].Add(tempSound);
                }
            }
            
            AssetManager.InitializeAssets(textures, sounds, menuButtons, menuGovDict);
            for (i = 0; i < NUM_LEVELS; i++)
            {
                // For each level, initialize a new stream
                System.IO.StreamReader levelFile = new System.IO.StreamReader("../../../Content/Levels/Level_" + i + ".txt");
                // parse the level file
                tempLevelArray = Parser.ParseLevels(levelFile, LEVEL_ROWS, LEVEL_COLUMNS);
                // create a new level object
                tempLevel = new Level(tempLevelArray, i);
                // Add it to the dictionary
                _levels.Add(i, tempLevel);
                tempTexture = Content.Load<Texture2D>("Background_" + i + ".png");
                _bkgTextures.Add(i, tempTexture);
                Background.bkgTextures = _bkgTextures;
            }
            // Initialize manager classes
            ObjectManager.InitializeObjectmManager(_levels);
            HUD.Initialize();
            shadow = Content.Load<Texture2D>("Shadow.png");
            permaBackground = Content.Load<Texture2D>("PermaBackground.png");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferHeight = 1000;
                graphics.PreferredBackBufferWidth = 1000;
            }
            if(fullScreen)
            {
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.IsFullScreen = true;
                fullScreen = false;
            }
            else if(windowed)
            {
                graphics.IsFullScreen = false;
                windowed = false;
            }

            graphics.ApplyChanges();
            // Get keyboard and mouse states
            mState = Mouse.GetState();
            KBstate = Keyboard.GetState();
            // Switch (gameState) to update
            switch (gameState)
            {
                case 0:
                    // Update the menus
                    ObjectManager.UpdateMenu(gameTime);
                    break;
                case 1:
                    // Update the gameObjects
                    ObjectManager.UpdateGame(gameTime);
                    break;
                default:
                    break;
            }
            // TODO: Add your update logic here
            oldKBstate = KBstate;
            oldmState = mState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.MidnightBlue);
            // Switch (gameState) to draw
            if (ObjectManager.currentMenuName == "GameOverMenu")
                ObjectManager.DrawGame(gameTime, spriteBatch);
            switch (gameState)
            {
                case 0:
                    // Draw the menus
                    ObjectManager.DrawMenu(gameTime, spriteBatch);
                    break;
                case 1:
                    // Draw the gameObjects
                    ObjectManager.DrawGame(gameTime, spriteBatch);
                    break;
                default:
                    break;
            }

            // TODO: Add your drawing code here

            spriteBatch.End();

            base.Draw(gameTime);
        }
        
    }
}
