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
    class Scene
    {
        public List<string> lines = new List<string>();
        public List<string> speakers = new List<string>();
        public Dictionary<string, Texture2D> speakerTextures = new Dictionary<string, Texture2D>();
        SpriteFont sceneFont;
        public int lineIndex = 0, speakerIndex = 0;
        public bool endScene = false;
        public int wordsPerLine = 30;
        public Texture2D sceneOverlay;
        string line;
        string[] splitLine;
        public Scene(int index)
        {

            Game1.windowed = true;
            switch(ObjectManager.levelIndex)
            {
                case 1:
                    ObjectManager.currentPlayer.knownAbilities = 2;
                    break;
                case 2:
                    ObjectManager.currentPlayer.knownAbilities = 3;
                    break;
                default:
                    break;
            }
            sceneOverlay = AssetManager.GetGovTextures("Scene")[0];
            sceneFont = Game1.testFont;
            System.IO.StreamReader sceneFile = new System.IO.StreamReader("../../../Content/Scenes/Scene_" + index + ".txt");
            line = sceneFile.ReadLine();
            splitLine = line.Split(',');
            foreach (string speaker in splitLine)
            {
                speakers.Add(speaker);
                speakerTextures.Add(speaker, AssetManager.GetGovTextures(speaker)[0]);
            }
            while ((line = sceneFile.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
        public void Update(GameTime gameTime)
        {
            if (Game1.KBstate.IsKeyDown(Keys.Enter) && Game1.oldKBstate.IsKeyUp(Keys.Enter))
            {
                lineIndex++;
                if (lineIndex == lines.Count())
                {
                    endScene = true;
                    Game1.fullScreen = true;
                    Game1.gameState = 0;
                    ObjectManager.currentMenuName = "ReadyGoMenu";
                    return;
                }
                    speakerIndex++;
                if (speakerIndex == speakers.Count())
                    speakerIndex = 0;
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            List<string> words = new List<string>();
            spriteBatch.Draw(sceneOverlay, new Vector2(0, 0), Color.White);
            for (int x = 0; x < speakers.Count; x++)
            {
                if (x == speakerIndex)
                    spriteBatch.Draw(speakerTextures[speakers[x]], new Vector2(100 + (x * 550), 50), Color.White);
                else
                    spriteBatch.Draw(speakerTextures[speakers[x]], new Vector2(100 + (x * 550), 50), Color.White * 0.5f);
            }
            splitLine = lines[lineIndex].Split(' ');
            foreach(string word in splitLine)
            {
                words.Add(word);
            }
            spriteBatch.DrawString(sceneFont, speakers[speakerIndex] + ": ", new Vector2(50, 765), Color.LightBlue, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            int i = 0, line = 0, charDistance = 20, lineLength = 830, xPos = 0;
            foreach (string word in words)
            {
                foreach(char c in word)
                {
                    if (c == '-')
                    {
                        line++;
                        i = 0;
                        xPos = 0;
                    }
                    else
                    {
                        spriteBatch.DrawString(sceneFont, c.ToString(), new Vector2(50 + (xPos), 800 + (line * 30)), Color.LightBlue, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                        if (c.ToString().ToUpper() == c.ToString())
                            charDistance = 10;
                        if (c == 'M' || c == 'W')
                            charDistance = 25;
                        else if (c == 'I')
                            charDistance = 10;
                        else
                        {
                            switch (c.ToString())
                            {
                                case "w":
                                    charDistance = 20;
                                    break;
                             
                                case "m":
                                    charDistance = 20;
                                    break;
                                case "i":
                                    charDistance = 7;
                                    break;
                                case "l":
                                    charDistance = 5;
                                    break;
                                case "t":
                                    charDistance = 10;
                                    break;
                                case "'":
                                    charDistance = 5;
                                    break;
                                case "f":
                                    charDistance = 8;
                                    break;
                                case "r":
                                    charDistance = 8;
                                    break;
                                default:
                                    charDistance = 15;
                                    break;
                            }
                        }
                        xPos += charDistance;
                    }
                }
                xPos += 15;
                if ((xPos >= lineLength))
                {
                    line++;
                    i = 0;
                    xPos = 0;
                }
            }
            if(lineIndex != lines.Count()-1)
                spriteBatch.DrawString(sceneFont, "Press <Enter> to Continue", new Vector2(630, 665), Color.LightBlue, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            else
                spriteBatch.DrawString(sceneFont, "Press <Enter> to Start the Level", new Vector2(560, 665), Color.LightBlue, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
        }
    }
}
