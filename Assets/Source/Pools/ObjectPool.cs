using System.Collections.Generic;
using UnityEngine;

namespace Caveman.Pools
{
    public abstract class ASupportPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        public abstract void InitializationPool(ObjectPool<T> item);
        public string Id;
    }

    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        private Stack<T> stack; 
        private T prefab;

        //for multiplayer <id, item>
        private Dictionary<string, T> poolServer;

        public void Initialization(T prefab, int initialBufferSize, bool isMultiplayer)
        {
            if (isMultiplayer) poolServer = new Dictionary<string, T>();

            stack = new Stack<T>(initialBufferSize);
            this.prefab = prefab;
            GetActivedCount = initialBufferSize;
        }

        public T New()
        {
            var item = GetItem() ?? CreateItem();
            item.gameObject.SetActive(true);
            GetActivedCount++;
            return item;
        }

        private T CreateItem()
        {
            var item = Instantiate(prefab);
            if (item.GetComponent<ASupportPool<T>>())
            {
                item.transform.SetParent(transform);
                item.GetComponent<ASupportPool<T>>().InitializationPool(this);
            }
            return item;
        }

        private T GetItem()
        {
            return stack.Count > 0 ? stack.Pop() : null;
        }

        public void  Store(T obj)
        {
            GetActivedCount--;
            obj.gameObject.SetActive(false);
            obj.gameObject.transform.position = new Vector3(100, 100, 100);
            stack.Push(obj);            
        }

        /// <summary>
        /// for multiplayer mode.
        /// if item with id(key) no contain on map
        /// add to map
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T New(string key)
        {
            var item = GetItem() ?? CreateItem();
            while (!string.IsNullOrEmpty(item.GetComponent<ASupportPool<T>>().Id))
            {
                item = GetItem() ?? CreateItem();
            }
            item.GetComponent<ASupportPool<T>>().Id = key;
            item.name = item.name + key;
            poolServer.Add(key, item);
            item.gameObject.SetActive(true);
            return item;
        }

        /// <summary>
        /// for multiplayer mode.
        /// </summary>
        /// <param name="key"></param>
        public void Store(string key)
        {
            T value;
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

        public bool ContainsKey(string key)
        {
            return poolServer.ContainsKey(key);
        }

        public T this[string key]
        {
            get
            {
                return poolServer[key];
            }
        }

        public int GetActivedCount { private set; get; }
    }
}
