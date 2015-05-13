namespace Caveman
{
    public class Settings
    {
        public const int MaxCountPlayers = 10;

        public const int BoundaryRandom = 10;
        public const float BoundaryEndMap = 10; 

        public static int RoundTime = 3;
        public static int TimeRespawnWeapon = 5;

        public static int BotsCount = 4;
        public static int WeaponsCount = 10;
        
        public static float SpeedWeapon = 4f;
        public static float SpeedPlayer = 2.5f;

        public static int TimeThrowStone = 3;
        public const int TimeRespawnPlayer = 1;

        public const int RotateStoneParameter = 10; 

        public const string AnimRunF = "run_f";
        public const string AnimRunB = "run_b";
        public const string AnimThrowF = "throw_f";
        public const string AnimThrowB = "throw_b";
        public const string AnimStayB = "stay_b";
        public const string AnimStayF = "stay_f";
        public const string AnimPickup = "pickup";
    }
}
