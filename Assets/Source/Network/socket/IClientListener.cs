namespace Caveman.Network
{
    public interface IClientListener
    {
        void LoginMessage();
        void TickMessage();
        void PickWeapon();
        void UseWeapon();
        void PickBonus();
        void PlayerGold();
        void Respawn();
        void PlayerDead();
        void AddedKillStat();
        void Move();
        void Logout();
    }
}
