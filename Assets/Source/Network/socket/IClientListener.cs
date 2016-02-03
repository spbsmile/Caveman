using UnityEngine;

namespace Caveman.Network
{
    public interface IClientListener
    {
        void LoginMessage(string userName);
        void TickMessage();
        void PickWeapon(string weaponId, int type);
        void UseWeapon(Vector2 pointClient, int type);
        void PickBonus(string bonusId, int type);
        void PlayerGold(int gold);
        void Respawn(Vector2 pointClient);
        void PlayerDead();
        void AddedKillStat(string killerId);
        void Move(Vector2 pointClient   );
        void Logout();
    }
}
