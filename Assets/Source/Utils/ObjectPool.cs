using System;
using System.Collections.Generic;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Utils
{
    public abstract class ASupportPool : MonoBehaviour
    {
        public abstract void SetPool(ObjectPool item);
        public string Id;
    }

    public class ObjectPool : MonoBehaviour
    {
        public Func<ObjectPool> RelatedPool;

        private Stack<Transform> stack;
        private Transform prefab;

        private Dictionary<string, Transform> poolServer;

        public void CreatePool(Transform prefab, int initialBufferSize, bool multiplayer)
        {
            stack = new Stack<Transform>(initialBufferSize);
            this.prefab = prefab;
            if (multiplayer) poolServer = new Dictionary<string, Transform>();
        }

        /// <summary>
        /// when single game mode
        /// </summary>
        /// <returns></returns>
        public Transform New()
        {
            Transform item;
            if (stack.Count > 0)
            {
                item = stack.Pop();
            }
            else
            {
                item = Instantiate(prefab);
                if (item.GetComponent<ASupportPool>())
                {
                    item.GetComponent<ASupportPool>().SetPool(this);
                }
                if (RelatedPool != null && item.GetComponent<StoneModel>())
                {
                    item.GetComponent<StoneModel>().SetPoolSplash(RelatedPool());
                }
            }
            item.gameObject.SetActive(true);
            return item;
        }

        public void Store(Transform obj)
        {
            obj.gameObject.SetActive(false);
            obj.gameObject.transform.position = new Vector3(100, 100, 100);
            stack.Push(obj);            
        }

        /// <summary>
        /// when multiplayer game mode
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Transform New(Vector2 point)
        {
            var key = GenerateKey(point);
            var item = New();
            item.GetComponent<ASupportPool>().Id = key;
            if (!poolServer.ContainsKey(key))
            {
                poolServer.Add(key, item);
            }
            else
            {
                Debug.LogWarning(key + " An element with the same key already exists in the dictionary.");
            }
            return item;
        }

        /// <summary>
        /// when multiplayer game mode
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public void Store(Vector2 point)
        {
            var key = GenerateKey(point);
            Transform value;
            if (poolServer.TryGetValue(key, out value))
            {
                Store(value);
                poolServer.Remove(key);
            }
            else
            {
                Debug.LogWarning(key + " key not found in Dictionary");
            }
        }

        private string GenerateKey(Vector2 point)
        {
            return point.x + ":" + point.y; 
        }
    }
}
