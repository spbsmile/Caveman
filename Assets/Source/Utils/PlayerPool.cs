using System.Collections.Generic;
using Caveman.Players;
using UnityEngine;

namespace Caveman.Utils
{
    public class PlayerPool : MonoBehaviour
    {
        private Dictionary<string, PlayerModelBase> pool;
        private PlayerModelBase prefab;

        public void Init(PlayerModelBase prefab)
        {
            pool = new Dictionary<string, PlayerModelBase>();
            this.prefab = prefab;
        }

        public void Add(string Id, PlayerModelBase item)
        {
            item.Id = Id;
            pool.Add(Id, item);
        }

        public void Store(PlayerModelBase player)
        {
            player.gameObject.SetActive(false);
        }

        public PlayerModelBase New(string id)
        {
            PlayerModelBase item;
            if (pool.TryGetValue(id, out item))
            {
                return item;
            }
            item = Instantiate(prefab);
            item.Id = id;
            item.SetPool(this);
            pool.Add(id, item);
            return item;
        }

        public PlayerModelBase this[string key]
        {
            get
            {
                return pool[key];
            }
        }
    }
}
