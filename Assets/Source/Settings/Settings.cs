namespace Caveman.Setting
{
    public class Settings
    {
        public static int WidthMap = 24;
        public static int HeightMap = 24;
        public static int RoundTime = 60;

        public static int BotsCount = 3;
        public static float PlayerSpeed = 2.5f;
        public const int PlayerTimeRespawn = 3;
        public const float PlayerTimeInvulnerability = 2;
        public static float StoneSpeed = 5f;
        public const int StoneRotateParameter = 10;
        public static int WeaponTimeRespawn = 20;
        public static int WeaponTimerThrow = 2;
        public static int WeaponsMaxOnPlayer = 10;
        public static int WeaponInitialLying = 10;
        public static int WeaponCountPickup = 1;
        
        //todo rename all weapon
        public static int CountLyingSkulls = 3;

        public static float BonusTimeRespawn = 10;
        //todo created on requirement
        public static int BonusSpeedPoolCount = 6;
        public static int BonusSpeedDuration = 4;
        public static int BonusSpeedMaxCount = 6;

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
