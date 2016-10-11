using System;
using System.Collections.Generic;
using Caveman.Players;
using UnityEngine;

namespace Caveman.Pools
{
    public class  PlayerPool : MonoBehaviour
    {
        public static PlayerPool instance;

        public Action<PlayerModelBase> AddedPlayer;
        public Action<PlayerModelBase> RemovePlayer;

        private readonly Dictionary<string, PlayerModelBase> pool= new Dictionary<string, PlayerModelBase>();

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void Add(string Id, PlayerModelBase item)
        {
            item.transform.SetParent(transform);
            if (AddedPlayer != null)
            {
                AddedPlayer(item);
            }
            pool.Add(Id, item);
        }

        public void Remove(string playerId)
        {
            if (RemovePlayer != null)
            {
                RemovePlayer(pool[playerId]);
            }
            pool.Remove(playerId);
        }

        public IEnumerable<PlayerModelBase> GetCurrentPlayerModels()
        {
            return pool.Values;
        }

        public void Store(PlayerModelBase player)
        {
            player.enabled = false;
            player.transform.position = new Vector3(100, 100, 100);
        }

        public PlayerModelBase New(string id)
        {
            PlayerModelBase item;
            if (!pool.TryGetValue(id, out item)) return null;
            item.enabled = true;
            return item;
        }

        public PlayerModelBase this[string key]
        {
            get
            {
                return pool[key];
            }
        }

        public bool ContainsKey(string playerId)
        {
            return pool.ContainsKey(playerId);
        }
    }
}
