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
        
        //todo в зависимости от разных игроков разные одежки 
        public void SetPrefab(PlayerModelBase prefab)
        {
            this.prefab = prefab;
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

        public Dictionary<string, PlayerModelBase>.ValueCollection GetCurrentPlayers()
        {
            return pool.Values;
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
                item.gameObject.SetActive(true);
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
