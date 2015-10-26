using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public interface IServerListener
    {
        void PlayerRespawnReceived(string playerId, Vector2 point);
        void PlayerMoveReceived(string playerId, Vector2 point);
        void PlayerDeadReceived(string playerId);

        void WeaponPickReceived(string playerId, string key);
        void WeaponUseReceived(string playerId, Vector2 aim);
        void WeaponAddedReceived(string key, Vector2 point);
        //void WeaponRemovedReceived(string key);

        void BonusPickReceived(string playerId, string key);
        void BonusAddedReceived(string key, Vector2 point);
        void BonusRemovedReceived(string key, Vector2 point);

        void GameResultReceived(JToken data);
        void GameTimeReceived(float time);

        void LoginReceived(string playerId, string playerName);
        void LogoutReceived(string playerId);
    }
}