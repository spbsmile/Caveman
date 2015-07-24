using System;
using System.Collections.Generic;
using Caveman.Players;
using UnityEngine;

namespace Caveman.Utils
{
    public class  PlayerPool : MonoBehaviour
    {
        public Action<PlayerModelBase> AddedPlayer;

        private Dictionary<string, PlayerModelBase> pool= new Dictionary<string, PlayerModelBase>();
        private PlayerModelBase prefab;
        
        //todo в зависимости от разных игроков разные одежки ,/ deleted
        public void SetPrefab(Transform prefab)
        {
            this.prefab = prefab.GetComponent<PlayerModelBase>();
        }

        public void Add(string Id, PlayerModelBase item)
        {
            item.Id = Id;
            if (AddedPlayer != null)
            {
                AddedPlayer(item);
            }
            pool.Add(Id, item);
        }

        public IEnumerable<PlayerModelBase> GetCurrentPlayers()
        {
            return pool.Values;
        }

        public void Store(PlayerModelBase player)
        {
            player.enabled = false;
            player.transform.position = new Vector3(100,100,100);
        }

        public PlayerModelBase New(string id)
        {
            PlayerModelBase item;
            if (pool.TryGetValue(id, out item))
            {
                item.enabled = true;
                return item;
            }
            item = Instantiate(prefab);
            item.SetPool(this);
            Add(id, item);
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
