using UnityEngine;

namespace Caveman.Network
{
    public interface IClientListener
    {
        void LoginMessage(string userName);
        void TickMessage();
        void PickWeapon(string weaponId);
        void UseWeapon(Vector2 pointClient);
        void PickBonus(string bonusId);
        void PlayerGold(int gold);
        void Respawn(Vector2 pointClient);
        void PlayerDead();
        void AddedKillStat(string killerId);
        void Move(Vector2 pointClient   );
        void Logout();
    }
}
