using UnityEngine;

namespace Script.Utils
{
    public class MSingleton<T> : MonoBehaviour where T : MSingleton<T>
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError(
                            $"[Singleton] More than two single tones");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        var singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = $"{typeof(T).Name} (Singleton)";
                        DontDestroyOnLoad(singleton);

                    }
                    else
                    {
                        Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                    }

                    return _instance;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}