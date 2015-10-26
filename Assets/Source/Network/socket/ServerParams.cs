namespace Caveman.Network
{
    public static class ServerParams
    {
        public const string UserName = "name";
        public const string ActionType = "action";
        public const string UserId = "id";
        public const string Killer = "killer";
        public const string X = "x";
        public const string Y = "y";

        public const string GameTimeAction = "time";
        public const string GameResultAction = "result";
        public const string GameTimeLeft = "time_left";

        public const string PlayerMoveAction = "move";
        public const string PlayerRespawnAction = "respawn";

        public const string WeaponPickAction = "pick_weapon";
        public const string StoneAddedAction = "stone_added";
        public const string StoneRemovedAction = "stone_removed";

        public const string BonusPickAction = "pick_bonus";
        public const string BonusAddedAction = "bonus_added";
        public const string BonusRemovedAction = "bonus_removed";
        public const string DeadAction = "dead";
       
        public const string Data = "data";
        public const string Kills = "kills";
        public const string Deaths = "deaths";

        public const string LoginAction = "login";
        public const string LogoutAction = "logout";
        public const string PingAction = "ping";

        //client side action only
        public const string UseWeaponAction = "use_weapon";
        public const string PlayerDeadAction = "player_dead";
    }
}