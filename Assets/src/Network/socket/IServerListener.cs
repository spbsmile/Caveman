using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public interface IServerListener
    {
        void PlayerRespawnReceive(string playerId, Vector2 point);
        void PlayerMoveReceive(string playerId, Vector2 point);
        void PlayerDeadReceive(string playerId);

        void WeaponPickReceive(string playerId, string key);
        void WeaponUseReceive(string playerId, Vector2 aim);
        void WeaponAddedReceive(string key, Vector2 point);
        void WeaponRemovedReceive(string key);

        void BonusPickReceive(string playerId, string key);
        void BonusAddedReceive(string key, Vector2 point);
        void BonusRemovedReceive(string key, Vector2 point);

        void GameResultReceive(JToken jToken);
        void GameInfoReceive(JToken jToken);
        void GameTimeReceive(float time);

        void LoginReceive(string playerId, string playerName);
        void LogoutReceive(string playerId);
        
    }
}