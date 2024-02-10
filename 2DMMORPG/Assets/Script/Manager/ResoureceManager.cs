using UnityEngine;

namespace Script.Manager
{
    //에셋번들로 변경 예정
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