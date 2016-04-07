using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
namespace GameName9
{
    static class ObjectManager
    {

        // Dictionary of gameMenus ** With new menus will come a list of active menus **
        public static Dictionary<string, GameMenu> menuDictionary = new Dictionary<string, GameMenu>();
        // Dictionary of gameObjects
        public static Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();
        // Location of each GameObject in the level array [x,y]
        public static Dictionary<Vector2, GameObject> gameObjectLocations = new Dictionary<Vector2, GameObject>();
        // List of onscreen objects
        static Dictionary<string, GameObject> onScreen = new Dictionary<string, GameObject>();
        // Dictionary of levels
        public static bool gameOver = false;
        public static Dictionary<Rectangle, List<GameObject>> collisionGrid = new Dictionary<Rectangle, List<GameObject>>();
        public static string currentMenuName;
        public static GameMenu currentMenu;
        static Dictionary<int, Level> levels = new Dictionary<int, Level>();
        public static CollisionMap currentColMap;
        public static int levelIndex;
        public static Player currentPlayer;
        public static int enemyCount = 0;
        public static Scene currentScene;
        public static bool newLevel = false;
        public static void InitializeObjectmManager(Dictionary<int, Level> _levels)
        {
            // Set attributes
            levels = _levels;
            currentMenuName = "StartMenu";
            //currentMenuName = "LevelUpMenu";
            levelIndex = 0;
            currentScene = new Scene(levelIndex);
        }
        public static void AddObjectToDictionary(GameObject obj, string objKey)
        {
            string c = "_";
            // Index of duplicate objects
            int duplicateIndex = 2;
            while (true)
            {
                // If !duplicate
                if (objectDictionary.ContainsKey(objKey) == false)
                {
                    // Add the object to objectDictionary
                    objectDictionary.Add(objKey, obj);
                    if (obj.GetType() == typeof(Player))
                    {
                        currentPlayer = (Player)obj;
                    }
                    if (obj.GetType() == typeof(Enemy))
                    {
                        enemyCount++;
                    }
                    // set; objname
                    obj.name = objKey;
                    currentColMap.Insert(obj.position, obj);
                    return;
                }
                else // if duplicate == true
                {
                    // If it's not the first duplicate
                    if (objKey.Contains(c))
                    {
                        // Remove the duplicateIndex
                        objKey = objKey.Substring(objKey.IndexOf('_') + 1);
                    }
                    // Add a duplicateIndex to the objKey
                    objKey = duplicateIndex.ToString() + "_" + objKey;
                    // Incriment the duplicateIndex
                    duplicateIndex++;
                }
            }

        }
        public static void AddMenuToDictionary(GameMenu menu, string menuKey)
        {
            string c = "_";
            // Index of duplicate objects
            int duplicateIndex = 2;
            while (true)
            {
                // If !duplicate
                if (objectDictionary.ContainsKey(menuKey) == false)
                {
                    // Add the object to objectDictionary
                    menuDictionary.Add(menuKey, menu);
                    // set; menuName
                    menu.name = menuKey;
                    return;
                }
                else
                {
                    // If not the first duplicate
                    if (menuKey.Contains(c))
                    {
                        // Remove duplicateIndex
                        menuKey = menuKey.Substring(menuKey.IndexOf('_') + 1);
                    }
                    // Add a duplicateIndex
                    menuKey = duplicateIndex.ToString() + "_" + menuKey;
                    // incriment the duplicateIndex
                    duplicateIndex++;
                }
            }

        }
        /// <summary>
        /// Removes an object from onScreen
        /// </summary>
        /// <param name="objKey"></param>
        public static void RemoveObjectFromDictionary(string key)
        {
            if (objectDictionary[key].GetType() == typeof(Enemy))
                enemyCount--;
            currentColMap.Remove(objectDictionary[key].position, objectDictionary[key]);
            objectDictionary.Remove(key);
        }

        /// <summary>
        /// Update each onScreen object
        /// </summary>
        /// <param name="gameTime"></param>
        public static void UpdateGame(GameTime gameTime)
        {
            newLevel = false;
            Camera.Update();
            if (currentScene.endScene == false)
            {
                currentScene.Update(gameTime);
                return;
            }

            while (!GameObjectQueue.IsEmpty())
            {
                AddObjectToDictionary(GameObjectQueue.Peek(), GameObjectQueue.DeQueue().govName);
            }
            while (!RemoveObjectQueue.IsEmpty())
            {
                RemoveObjectFromDictionary(RemoveObjectQueue.DeQueue());
            }
            if (objectDictionary.ContainsKey("Player"))
                currentPlayer.Update(gameTime);
            else
                InitializeObjectmManager(levels);
            foreach (KeyValuePair<string, GameObject> pair in objectDictionary)
            {
                if (!pair.Key.Contains("Portal") && !pair.Key.Contains("Player"))
                    pair.Value.Update(gameTime);
                if (newLevel)
                    break;
            }
            if (enemyCount == 0 && (levelIndex + 1 != levels.Count()))
            {
                if (!objectDictionary.ContainsKey("Portal" + levelIndex))
                    AddObjectToDictionary((new Portal(new Vector2(9, 9), true)), "Portal" + levelIndex);
                objectDictionary["Portal" + levelIndex].Update(gameTime);
            }
            else if(levelIndex +1 == levels.Count() && enemyCount == 0)
            {
                Game1.gameState = 0;
                Game1.windowed = true;
                Parser.music.Stop();
                ObjectManager.currentMenuName = "YouWinMenu";
                gameOver = true;
            }
            HUD.Update(gameTime);

        }
        /// <summary>
        /// Draw each onScreen object
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public static void DrawGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentScene.endScene == false)
                currentScene.Draw(gameTime, spriteBatch);
            else
            {
                spriteBatch.Draw(Game1.permaBackground,
                new Vector2((float)((-1 * (Camera.SCREEN_WIDTH / 2)) - Camera.screenOffset.X), (float)((-1 * (Camera.SCREEN_HEIGHT / 2)) - Camera.screenOffset.Y)),
                Color.White);
                Background.Draw(gameTime, spriteBatch);
                foreach (KeyValuePair<string, GameObject> pair in objectDictionary)
                {
                    if (!pair.Key.Contains("Portal"))
                        pair.Value.Draw(gameTime, spriteBatch);
                    else if (enemyCount == 0)
                    {
                        objectDictionary["Portal" + levelIndex].Draw(gameTime, spriteBatch);
                    }
                }

                HUD.Draw(gameTime, spriteBatch);
            }

        }
        /// <summary>
        /// Update menus
        /// </summary>
        /// <param name="gameTime"></param>
        public static void UpdateMenu(GameTime gameTime)
        {
            currentMenu = menuDictionary[currentMenuName];
            currentMenu.Update(gameTime);
        }
        /// <summary>
        /// Draw menus
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public static void DrawMenu(GameTime gameTime, SpriteBatch spriteBatch)
        {

            currentMenu = menuDictionary[currentMenuName];
            currentMenu.Draw(gameTime, spriteBatch);

        }
        /// <summary>
        /// Load a level from index
        /// </summary>
        /// <param name="index"></param>
        public static void LoadLevelFromIndex(int index)
        {
            newLevel = true;
            currentColMap = new CollisionMap();
            Parser.LoadLevel(levels[index]);
        }
        public static List<GameObject> CheckCollisions(GameObject obj)
        {
            List<List<GameObject>> collisionLists = currentColMap.Querry(obj.position, obj);
            List<GameObject> collisions = new List<GameObject>();
            //foreach (List<GameObject> objList in collisionLists)
            //{
            //    foreach (GameObject cObj in objList)
            //    {
            //        // If the hitBoxes intersect, the obj canCollide, and the object is not itself
            //        if (cObj.hitBox.Intersects(obj.hitBox) && cObj.canCollide == true && cObj.govName != obj.govName)
            //        {
            //            // It is colliding
            //            obj.isColliding = true;
            //            // Add to collisions
            //            collisions.Add(cObj);
            //        }
            //        // If there were no collisions
            //        if (collisions.Count == 0)
            //        {
            //            // It is not colliding
            //            obj.isColliding = false;
            //        }
            //    }
            //}
            foreach (GameObject cObj in objectDictionary.Values)
            {
                // If the hitBoxes intersect, the obj canCollide, and the object is not itself
                if (cObj.hitBox.Intersects(obj.hitBox) && cObj.canCollide == true && cObj.govName != obj.govName)
                {
                    // It is colliding
                    obj.isColliding = true;
                    // Add to collisions
                    collisions.Add(cObj);
                }
                // If there were no collisions
                if (collisions.Count == 0)
                {
                    // It is not colliding
                    obj.isColliding = false;
                }
            }
            //Return the collisions
            return collisions;
        }
        public static List<GameObject> CheckCollisions(GameObject obj, GameObject cantCollide)
        {
            // List of collisions
            List<List<GameObject>> collisionLists = currentColMap.Querry(obj.position, obj);
            List<GameObject> collisions = new List<GameObject>();
            //foreach(List<GameObject> objList in collisionLists)
            //{
            //    foreach (GameObject cObj in objList)
            //    {
            //        // If the hitBoxes intersect, the obj canCollide, and the object is not itself
            //        if (cObj.hitBox.Intersects(obj.hitBox) && cObj.canCollide == true && cObj.govName != obj.govName && cObj.GetType() != cantCollide.GetType())
            //        {
            //            // It is colliding
            //            obj.isColliding = true;
            //            // Add to collisions
            //            collisions.Add(cObj);
            //        }
            //        // If there were no collisions
            //        if (collisions.Count == 0)
            //        {
            //            // It is not colliding
            //            obj.isColliding = false;
            //        }
            //    }
            //}

                foreach (GameObject cObj in objectDictionary.Values)
                {
                    // If the hitBoxes intersect, the obj canCollide, and the object is not itself
                    if (cObj.hitBox.Intersects(obj.hitBox) && cObj.canCollide == true && cObj.govName != obj.govName && cObj.GetType() != cantCollide.GetType())
                    {
                        // It is colliding
                        obj.isColliding = true;
                        // Add to collisions
                        collisions.Add(cObj);
                    }
                    // If there were no collisions
                    if (collisions.Count == 0)
                    {
                        // It is not colliding
                        obj.isColliding = false;
                    }
                }
            
            // Return the collisions
            return collisions;
        }
        public static void UpdateScreen(Rectangle screen)
        {
            onScreen.Clear();
            foreach (KeyValuePair<string, GameObject> pair in objectDictionary)
            {
                if (pair.Value.hitBox.Intersects(screen))
                {
                    onScreen.Add(pair.Key, pair.Value);
                }
            }
        }
        public static void Changelevel(int Index)
        {
            Game1.fullScreen = true;
            levelIndex = Index;
            LoadLevelFromIndex(levelIndex);
            currentScene = new Scene(Index);
        }


    }
}