using System;
using Caveman.Bonuses;
using Caveman.Network;
using Caveman.Players;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using RPC = Caveman.Network.RPC;
using Random = System.Random;

namespace Caveman
{
    public class EnterPointMultiplayer : MonoBehaviour, IServerListener
    {
        public Transform prefabStoneFlagmentInc;
        public Transform prefabStone;
        public Transform prefabBonusSpeed;
        public Transform iconBonus;

        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerBonusesSpeed;
        
        private RPC rpc;

        private ObjectPool poolStones;
        private ObjectPool poolStonesSplash;
        private ObjectPool poolBonusesSpeed;
        private Random r;

        public void Start()
        {
            r = new Random();
            poolStonesSplash = CreatePool(Settings.PoolCountSplashStones, containerSplashStones, prefabStoneFlagmentInc, null);
            poolStones = CreatePool(Settings.PoolCountStones, containerStones, prefabStone, InitStoneModel);
            poolBonusesSpeed = CreatePool(Settings.PoolCountBonusesSpeed, containerBonusesSpeed, prefabBonusSpeed, InitBonusesModel);

            rpc = new RPC();
            rpc.ServerListener = this;
            print("device id : " + SystemInfo.deviceUniqueIdentifier);
            //TODO: replace deviceName with user name from main screen text field
            rpc.StartSession(SystemInfo.deviceUniqueIdentifier, SystemInfo.deviceName);
        }

        public void Update()
        {
            rpc.Update();
        }
        
        //
        public void StoneAddedReceived(Vector2 point)
        {
            poolStones.New().transform.position = point;
        }
        //
        public void StoneRemovedReceived(Vector2 point)
        {
            print(string.Format("StoneRemovedReceived {0}", point));
            //poolStones.Store(-);
        }
        //
        public void MoveReceived(string player, Vector2 point)
        {
            print(string.Format("MoveReceived {0} by player {1}", point, player));
        }

        public void LoginReceived(string player)
        {
            throw new System.NotImplementedException();
        }
        //
        public void PickWeaponReceived(string player, Vector2 point)
        {
            print(string.Format("PickWeaponReceived {0} by player {1}", point, player));
        }
        //
        public void PickBonusReceived(string player, Vector2 point)
        {
            print(string.Format("PickBonusReceived {0} by player {1}", point, player));
        }

        public void UseWeaponReceived(string player, Vector2 point)
        {
            throw new System.NotImplementedException();
        }

        private ObjectPool CreatePool(int initialBufferSize, Transform container, Transform prefab, Action<GameObject, ObjectPool> init)
        {
            var pool = container.GetComponent<ObjectPool>();
            pool.CreatePool(prefab, initialBufferSize);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);
                if (init != null)
                {
                    init(item.gameObject, pool);
                }
                item.SetParent(container);
                pool.Store(item.transform);
            }
            return pool;
        }

        private void InitStoneModel(GameObject item, ObjectPool pool)
        {
            var model = item.GetComponent<StoneModel>();
            model.SetPool(pool);
            model.SetPoolSplash(poolStonesSplash);
        }

        private void InitBonusesModel(GameObject item, ObjectPool pool)
        {
            item.GetComponent<BonusBase>().Init(pool, r, iconBonus);
            //item.GetComponent<BonusBase>().ChangedBonus += transform1 => 
        }
    }
}
