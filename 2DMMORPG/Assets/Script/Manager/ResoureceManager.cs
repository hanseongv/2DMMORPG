using UnityEngine;

namespace Script
{
    public class ResoureceManager
    {
        internal T[] LoadAllResources<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }
         internal T LoadResources<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}