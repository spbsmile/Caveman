using Caveman.Players;
using Caveman.UI;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Network
{
    public class Multiplayer : EnterPoint, IServerListener
    {
        public Transform prefabServerPlayer;

        public const float WidthMapServer = 1350;
        public const float HeigthMapServer = 1350;

        public void Awake()
        {
            Setting.Settings.multiplayerMode = true;
        }

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

        }

        public void PlayerDeadReceived(string playerId)
        {
            poolPlayers[playerId].Die();
        }

        public void ResultReceived(string result)
        {
            var resultRound = BattleGui.instance.resultRound;
            resultRound.gameObject.SetActive(true);
            //todo
            resultRound.Write("sfsdf", resultRound.deaths, 1);
        }

        public void TimeReceived(float time)
        {
            StartCoroutine(BattleGui.instance.mainGameTimer.UpdateTime((int)time));
        }

        public void Player(string player)
        {
            print(player + "player");            
        }

        public void WeaponRemovedReceived(string key)
        {
            poolStones.Store(key);
        }

        public void MoveReceived(string playerId, Vector2 point)
        {
           
            if (poolPlayers.ContainsKey(playerId))
            {
                var playerServer = poolPlayers[playerId];
                print(Vector2.SqrMagnitude((Vector2)playerServer.transform.position - point));
                if (Vector2.SqrMagnitude((Vector2)playerServer.transform.position - point) < UnityExtensions.ThresholdPosition)
                {
                    playerServer.StopMove();
                }
                else
                {
                    playerServer.SetMove(point);
                }
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
            serverConnection.SendRespawn(SystemInfo.deviceUniqueIdentifier,
                poolPlayers[SystemInfo.deviceUniqueIdentifier].transform.position);
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
        }

        public void PickBonusReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupBonus(poolBonusesSpeed[key]);
        }

        public void UseWeaponReceived(string playerId, Vector2 aim)
        {
            poolPlayers[playerId].Throw(aim);
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
