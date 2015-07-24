using Caveman.Players;
using UnityEngine;

namespace Caveman.Network
{
    public class Multiplayer : EnterPoint, IServerListener
    {
        public Transform prefabServerPlayer;

        public const float WidthMapServer = 1350;
        public const float HeigthMapServer = 1350;

        public override void Start()
        {
            serverConnection = new ServerConnection {ServerListener = this};
            serverConnection.StartSession(SystemInfo.deviceUniqueIdentifier, SystemInfo.deviceName);
            base.Start();
            serverConnection.SendRespawn(IdHostPlayer, poolPlayers[IdHostPlayer].transform.position);
            poolPlayers.SetPrefab(prefabServerPlayer);
        }

        public void Update()
        {
            serverConnection.Update();
        }

        public void WeaponAddedReceived(string key, Vector2 point)
        {
            poolStones.New(key).transform.position = point; //Debug.Log("stone added : " + key);
        }

        public void BonusAddedReceived(string key, Vector2 point)
        {
            //print(string.Format("BonusAddedReceived {0}", key));
        }

        public void PlayerDeadResceived(string playerId)
        {
            poolPlayers[playerId].Die(); print(string.Format("PlayerDeadResceived {0}", playerId));
        }

        public void WeaponRemovedReceived(string key)
        {
            //poolStones.Store(key); print(string.Format("WeaponRemovedReceived {0}", key));
        }

        public void MoveReceived(string playerId, Vector2 point)
        {
            print(string.Format("MoveReceived {0} by playerId {1}", point, playerId));
        }

        public void LoginReceived(string playerId)
        {
            CreatePlayer(new Player("server"), playerId, false, true, prefabServerPlayer);
            //print(string.Format("LoginReceived {0} by playerId", playerId));
        }

        public void PickWeaponReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupWeapon(poolStones[key]); //print(string.Format("PickWeaponReceived {0} by playerId {1}", key, playerId));
        }

        public void PickBonusReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupBonus(poolBonusesSpeed[key]); //print(string.Format("PickBonusReceived {0} by playerId {1}", key, playerId));
        }

        public void UseWeaponReceived(string playerId, Vector2 aim)
        {
            poolPlayers[playerId].Throw(aim); //Debug.Log(string.Format("UseWeaponReceived aim {0} by playerId {1}", aim, playerId));
        }

        public void RespawnReceived(string playerId, Vector2 point)
        {
            StartCoroutine(poolPlayers[playerId].Respawn());
            poolPlayers[playerId].transform.position = point;
            //poolPlayers.New(playerId).transform.position = point; Debug.Log(string.Format("RespawnReceived {0} by playerId {1}", point, playerId));
        }

        public void OnDestroy()
        {
            serverConnection.StopSession();
        }
    }
}
