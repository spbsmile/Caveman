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
            //serverConnection.SendLogin(SystemInfo.deviceName, SystemInfo.deviceUniqueIdentifier);
            serverConnection.SendRespawn(SystemInfo.deviceUniqueIdentifier, poolPlayers[SystemInfo.deviceUniqueIdentifier].transform.position);
        }

        public void Update()
        {
            serverConnection.Update();
        }

        public void WeaponAddedReceived(string key, Vector2 point)
        {
            if (!poolStones.ContainsKey(key))
            {
                poolStones.New(key).transform.position = point;     
            }
            else
            {
                Debug.LogWarning(key + " An element with the same key already exists in the dictionary.");
            }
        }

        public void BonusAddedReceived(string key, Vector2 point)
        {
            //print(string.Format("BonusAddedReceived {0}", key));
        }

        public void PlayerDeadResceived(string playerId)
        {
            poolPlayers[playerId].Die();
            print(string.Format("PlayerDeadResceived {0}", playerId));
        }

        public void Time(float time)
        {
            print(time);
        }

        public void Player(string player)
        {
            print(player + "player");            
        }

        public void WeaponRemovedReceived(string key)
        {
            poolStones.Store(key);
            //print(string.Format("WeaponRemovedReceived {0}", key));
        }

        public void MoveReceived(string playerId, Vector2 point)
        {
            if (poolPlayers.ContainsKey(playerId))
            {
                //how ai . serverclient move
                 poolPlayers[playerId].SetMove(point);
                //poolPlayers[playerId].transform.position = point;    
            }
            else
            {
                Debug.LogWarning("Player null, but move received invoke");
            }
            
        }

        public void LoginReceived(string playerId, string playerName, Vector2 position)
        {
            CreatePlayer(new Player(playerName), playerId, false, true, prefabServerPlayer);
            poolPlayers[playerId].transform.position = position;
            serverConnection.SendRespawn(SystemInfo.deviceUniqueIdentifier, poolPlayers[SystemInfo.deviceUniqueIdentifier].transform.position);            
            //print(string.Format("LoginReceived {0} by playerId", playerId));
        }

        public void LogoutReceived(string playerId)
        {
            var gameObject = poolPlayers[playerId].gameObject;
            poolPlayers.Remove(playerId);
            Destroy(gameObject);
        }

        public void PickWeaponReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupWeapon(poolStones[key]);
            //print(string.Format("PickWeaponReceived {0} by playerId {1}", key, playerId));
        }

        public void PickBonusReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupBonus(poolBonusesSpeed[key]);
            //print(string.Format("PickBonusReceived {0} by playerId {1}", key, playerId));
        }

        public void UseWeaponReceived(string playerId, Vector2 aim)
        {
            poolPlayers[playerId].Throw(aim);
            //Debug.Log(string.Format("UseWeaponReceived aim {0} by playerId {1}", aim, playerId));
        }

        public void RespawnReceived(string playerId, Vector2 point)
        {
            if (!poolPlayers.ContainsKey(playerId))
            {
                CreatePlayer(new Player("No Name"), playerId, false, true, prefabServerPlayer);
                poolPlayers[playerId].transform.position = point;
            }
            else
            {
                StartCoroutine(poolPlayers[playerId].Respawn(point));
                Debug.Log(string.Format("RespawnReceived {0} by playerId {1}", point, playerId));    
            }
        }

        public void OnDestroy()
        {
            serverConnection.StopSession();
        }
    }
}
