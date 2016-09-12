using UnityEngine;

namespace Caveman.Network
{
    public interface IServerNotify
    {
        void PickWeaponSend(string weaponId, int type);
        void ActivateWeaponSend(Vector2 pointClient, int type);
        void PickBonusSend(string bonusId, int type);

        void RespawnSend(Vector2 pointClient);
        void PlayerDeadSend();
        void AddedKillStatSend(string killerId);
        void MoveSend(Vector2 pointClient);
        void LogoutSend();
    }
}
