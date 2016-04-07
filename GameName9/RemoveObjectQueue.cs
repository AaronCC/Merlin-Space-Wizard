using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName9
{
    static class RemoveObjectQueue
    {
        static List<string> objectQueue = new List<string>();
        public static void EnQueue(string key)
        {
            objectQueue.Add(key);
        }
        public static string DeQueue()
        {
            string key;
            if (IsEmpty())
                return null;
            key = objectQueue[0];
            objectQueue.Remove(objectQueue[0]);
            return key;
        }
        public static string Peek()
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
