using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName9
{
    static class GameObjectQueue
    {
        static List<GameObject> objectQueue = new List<GameObject>();
        public static void EnQueue(GameObject obj)
        {
            objectQueue.Add(obj);
        }
        public static GameObject DeQueue()
        {
            GameObject obj;
            if (IsEmpty())
                return null;
            obj = objectQueue[0];
            objectQueue.Remove(objectQueue[0]);
            return obj;
        }
        public static GameObject Peek()
        {
            return objectQueue[0];
        }
        public static bool IsEmpty()
        {
            if (objectQueue.Count > 0)
                return false;
            return true;

        }
        public static int Count()
        {
            return objectQueue.Count;
        }
    }
}
