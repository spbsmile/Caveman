using UnityEngine;

namespace Caveman.Network
{
    public interface IClientListener
    {
        void PickWeapon(string weaponId, int type);
        void UseWeapon(Vector2 pointClient, int type);
        void PickBonus(string bonusId, int type);
        void PlayerGold(int gold);
        void Respawn(Vector2 pointClient);
        void PlayerDead();
        void AddedKillStat(string killerId);
        void Move(Vector2 pointClient);
        void Logout();

        void Update();
        void StartSession(string userId, string userName);
        void StopSession();
    }
}
