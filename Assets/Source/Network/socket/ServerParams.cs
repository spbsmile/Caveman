namespace Caveman.Network
{
    public static class ServerParams
    {
        public const string USER_NAME = "name";
        public const string ACTION_TYPE = "action";
        public const string USER_ID = "id";
        public const string X = "x";
        public const string Y = "y";
        public const string WEAPON_TYPE = "weapon_type";
        public const string BONUS_TYPE = "bonus_type";

        public const string PLAYER = "player";

        public const string RESPAWN_ACTION = "respawn";
        public const string STONE_ADDED_ACTION = "stone_added";
        public const string BONUS_ADDED_ACTION = "bonus_added";


        public const string STONE_REMOVED_ACTION = "stone_removed";
        public const string TIME_ACTION = "time";
        public const string LOGIN_ACTION = "login";
        public const string LOGOUT_ACTION = "logout";
        public const string PING_ACTION = "ping";
        public const string MOVE_ACTION = "move";
        public const string PICK_WEAPON_ACTION = "pick_weapon";
        public const string PICK_BONUS_ACTION = "pick_bonus";
        public const string DEAD_ACTION = "dead";
        public const string RESULT_ACTION = "result";

        //client side action only
        public const string USE_WEAPON_ACTION = "use_weapon";
        public const string PLAYER_DEAD_ACTION = "player_dead";
    }
}