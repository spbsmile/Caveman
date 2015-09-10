namespace Caveman.Setting
{
    public class Settings
    {
        public const int TimeRespawnPlayer = 3;
        public const int RotateStoneParameter = 10;

        public static int WidthMap = 24;
        public static int HeightMap = 24;
        public static int RoundTime = 60;
        public static int TimeRespawnWeapon = 20;
        public static int BotsCount = 3;
        public static int InitialLyingWeapons = 10;
        public static float SpeedStone = 5f;
        public static float SpeedPlayer = 2.5f;
        public static int TimeThrowWeapon = 2;
        public static int MaxCountWeapons = 10;
        public static int CountLyingSkulls = 3;
        public static int PoolCountBonusesSpeed = 6;
        public static float TimeRespawnBonuses = 10;
        public static int InitalCountBonusesSpeed = 5;
        public static int DurationBonusSpeed = 4;

        public static bool multiplayerMode;

        public const int PoolCountStones = 40;
        public const int PoolCountDeathImages = 9;
        public const int PoolCountSkulls = 30;
        public const int PoolCountSplashStones = 30;
        
        public const string AnimRunF = "run_f";
        public const string AnimRunB = "run_b";
        public const string AnimThrowF = "throw_f";
        public const string AnimThrowB = "throw_b";
        public const string AnimStayB = "stay_b";
        public const string AnimStayF = "stay_f";
        public const string AnimPickup = "pickup";
    }
}
