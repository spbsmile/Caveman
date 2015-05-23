using System.Collections.Generic;
using UnityEngine;

namespace Caveman.Utils
{
    public class ObjectPool : MonoBehaviour
    {
        private Stack<Transform> stack;
        private Transform prefab;

        public void CreatePool(Transform prefab, int initialBufferSize)
        {
            stack = new Stack<Transform>(initialBufferSize);
            this.prefab = prefab;
        }

        public Transform New()
        {
            if (stack.Count > 0)
            {
                var t = stack.Pop();
                t.gameObject.SetActive(true);
                return t;
            }
            else
            {
                var t = Instantiate(prefab);
                t.gameObject.SetActive(true);
                return t;
            }
        }

        public void Store(Transform obj)
        {
            obj.gameObject.SetActive(false);
            obj.gameObject.transform.position = new Vector3(100, 100, 100);
            stack.Push(obj);            
        }
    }
}
