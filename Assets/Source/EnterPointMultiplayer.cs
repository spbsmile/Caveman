﻿using System;
using Caveman.Bonuses;
using Caveman.Network;
using Caveman.Players;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman
{
    public class EnterPointMultiplayer : MonoBehaviour, IServerListener
    {
        public Transform prefabStoneFlagmentInc;
        public Transform prefabStone;
        public Transform prefabBonusSpeed;

        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerBonusesSpeed;

        private ServerConnection serverConnection;

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

            serverConnection = new ServerConnection();
            serverConnection.ServerListener = this;
            print("device id : " + SystemInfo.deviceUniqueIdentifier);
            //TODO: replace deviceName with user name from main screen text field
            serverConnection.StartSession(SystemInfo.deviceUniqueIdentifier, SystemInfo.deviceName);
            serverConnection.SendRespawn(Vector2.zero);
        }

        public void Update()
        {
            serverConnection.Update();
        }

        public void WeaponAddedReceived(Vector2 point)
        {
            Debug.Log("stone added : " + point);
            poolStones.New().transform.position = point;
        }

        public void WeaponRemovedReceived(Vector2 point)
        {
            print(string.Format("WeaponRemovedReceived {0}", point));
            //poolStones.Store(-);
        }

        public void MoveReceived(string playerId, Vector2 point)
        {
            print(string.Format("MoveReceived {0} by playerId {1}", point, playerId));
        }

        public void LoginReceived(string playerId)
        {
            print(string.Format("LoginReceived {0} by playerId {1}", playerId));
        }

        public void PickWeaponReceived(string playerId, Vector2 point)
        {
            print(string.Format("PickWeaponReceived {0} by playerId {1}", point, playerId));
        }

        public void PickBonusReceived(string playerId, Vector2 point)
        {
            print(string.Format("PickBonusReceived {0} by playerId {1}", point, playerId));
        }

        public void UseWeaponReceived(string playerId, Vector2 point)
        {
            Debug.Log(string.Format("UseWeaponReceived {0} by playerId {1}", point, playerId));
        }

        public void RespawnReceived(string playerId, Vector2 point)
        {
            Debug.Log(string.Format("RespawnReceived {0} by playerId {1}", point, playerId));
        }

        public void BonusAddedReceived(Vector2 point)
        {
            Debug.Log(string.Format("RespawnReceived {0}", point));
        }

        public void PlayerDeadResceived(string playerId, Vector2 point)
        {

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
            item.GetComponent<BonusBase>().Init(pool, r, 1);
            //item.GetComponent<BonusBase>().PickupBonus += transform1 => 
        }

        public void StopSession()
        {
            serverConnection.StopSession();
        }
    }
}
