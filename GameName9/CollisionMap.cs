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
    class CollisionMap
    {
        List<GameObject>[,] colMap = new List<GameObject>[Game1.LEVEL_COLUMNS, Game1.LEVEL_ROWS];
        
        Rectangle nodeHitBox;
        public CollisionMap()
        {
            for (int x = 0; x < Game1.LEVEL_COLUMNS; x++)
            {
                for (int y = 0; y < Game1.LEVEL_ROWS; y++)
                {
                    colMap[x, y] = new List<GameObject>();
                }
            }
        }
        public void Insert(Vector2 index, GameObject obj)
        {
            int xIndex, yIndex;
            xIndex = (int)(index.X - (index.X % 50)) / 50;
            yIndex = (int)(index.Y - (index.Y % 50)) / 50;
            nodeHitBox = new Rectangle(xIndex * 50, yIndex * 50, 50, 50);
            if (xIndex < Game1.LEVEL_COLUMNS && yIndex < Game1.LEVEL_ROWS && xIndex > 0 && yIndex > 0)
            {
                if (nodeHitBox.Intersects(obj.hitBox))
                {
                    Insert(new Vector2(index.X + 50, index.Y), obj);
                    colMap[xIndex, yIndex].Add(obj);
                    Insert(new Vector2(index.X, index.Y + 50), obj);
                }
            }
        }

        public void Remove(Vector2 index, GameObject obj)
        {
            int xIndex, yIndex;
            xIndex = (int)(index.X - (index.X % 50)) / 50;
            yIndex = (int)(index.Y - (index.Y % 50)) / 50;
            if (xIndex < Game1.LEVEL_COLUMNS && yIndex < Game1.LEVEL_ROWS && xIndex > 0 && yIndex > 0)
            {
                if (colMap[xIndex, yIndex].Contains(obj))
                {
                    Remove(new Vector2(index.X + 50, index.Y), obj);
                    colMap[xIndex, yIndex].Remove(obj);
                    Remove(new Vector2(index.X, index.Y + 50), obj);
                }
            }

        }

        public List<List<GameObject>> Querry(Vector2 index, GameObject obj)
        {
            int xIndex, yIndex;
            List<List<GameObject>> objCollections = new List<List<GameObject>>();
            xIndex = (int)(index.X - (index.X % 50)) / 50;
            yIndex = (int)(index.Y - (index.Y % 50)) / 50;
            if (xIndex < Game1.LEVEL_COLUMNS && yIndex < Game1.LEVEL_ROWS && xIndex > 0 && yIndex > 0)
            {
                if (colMap[xIndex, yIndex].Contains(obj))
                {
                    Remove(new Vector2(index.X + 50, index.Y), obj);
                    objCollections.Add(colMap[xIndex, yIndex]);
                    Remove(new Vector2(index.X, index.Y + 50), obj);
                }
            }
            return objCollections;
        }
    }
}
