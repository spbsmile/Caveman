using System;
using System.Collections.Generic;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Utils
{
    public abstract class ISupportPool : MonoBehaviour
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
            var item = GetItem();
            item.gameObject.SetActive(true);
            return item;
        }

        private Transform GetItem()
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            var item = Instantiate(prefab);
            if (item.GetComponent<ISupportPool>())
            {
                item.GetComponent<ISupportPool>().SetPool(this);
            }
            if (RelatedPool != null && item.GetComponent<StoneModel>())
            {
                item.GetComponent<StoneModel>().SetPoolSplash(RelatedPool());
            }
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
            var key = point.x + ":" + point.y;
            var item = GetItem();
            item.GetComponent<ISupportPool>().Id = key;
            poolServer.Add(key, item);
            return item;
        }

        /// <summary>
        /// when multiplayer game mode
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public void Store(Vector2 point)
        {
            var key = point.x + ":" + point.y;
            Transform value;
            if (poolServer.TryGetValue(key, out value))
            {
                
            }
            else
            {
                Debug.LogWarning(key + " not found in Dictionary");
            }
            Store(value);
        }
    }
}
