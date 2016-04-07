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
    class TargetStack
    {
        List<Vector2> targetPoints = new List<Vector2>();


        public void Push(Vector2 target)
        {
            targetPoints.Add(target);
        }
        public Vector2 Pop()
        {
            Vector2 target = new Vector2();
            if (IsEmpty())
                return target;

            target = targetPoints[targetPoints.Count - 1];
            targetPoints.Remove(targetPoints[targetPoints.Count - 1]);

            return target;
        }
        public bool IsEmpty()
        {
            if (targetPoints.Count == 0)
                return true;
            return false;
        }
        public int Count()
        {
            return targetPoints.Count();
        }
       

    }
}
