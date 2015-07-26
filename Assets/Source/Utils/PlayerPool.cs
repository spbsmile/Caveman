using System;
using System.Collections.Generic;
using Caveman.Players;
using UnityEngine;

namespace Caveman.Utils
{
    public class  PlayerPool : MonoBehaviour
    {
        public Action<PlayerModelBase> AddedPlayer;
        public Action<PlayerModelBase> RemovePlayer;

        private Dictionary<string, PlayerModelBase> pool= new Dictionary<string, PlayerModelBase>();

        public void Add(string Id, PlayerModelBase item)
        {
            item.Id = Id;
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
    }
}
