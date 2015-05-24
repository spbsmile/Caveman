using UnityEngine;

namespace Caveman.Utils
{
    public class Permanent<T> : MonoBehaviour where T : Permanent<T>
    {
        public static T instance;

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T) this;
                DontDestroyOnLoad(this);
            }
            else DestroyImmediate(gameObject);
        }
    }
}
