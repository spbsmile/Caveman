namespace Caveman.Network
{
    public static class ServerParams
    {
        public const string UserName = "name";
        public const string ActionType = "action";
        public const string UserId = "id";
        public const string X = "x";
        public const string Y = "y";
        public const string WeaponType = "weapon_type";
        public const string BonusType = "bonus_type";
        public const string TimeLeft = "time_left";

        public const string Player = "player";

        public const string RespawnAction = "respawn";
        public const string StoneAddedAction = "stone_added";
        public const string BonusAddedAction = "bonus_added";

        public const string StoneRemovedAction = "stone_removed";
        public const string TimeAction = "time";
        public const string LoginAction = "login";
        public const string LogoutAction = "logout";
        public const string PingAction = "ping";
        public const string MoveAction = "move";
        public const string PickWeaponAction = "pick_weapon";
        public const string PickBonusAction = "pick_bonus";
        public const string DeadAction = "dead";
        public const string ResultAction = "result";

        //client side action only
        public const string UseWeaponAction = "use_weapon";
        public const string PlayerDeadAction = "player_dead";
    }
}