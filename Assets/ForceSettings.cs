using System;
using Caveman;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ForceSettings : MonoBehaviour 
{
    public const int MaxCountPlayers = 10;

    public const int BoundaryRandom = 10;
    public const float BoundaryEndMap = 10;

    public const int RoundTime = 3;
    public const int TimeRespawnWeapon = 5;

    public const int BotsCount = 4;
    public const int WeaponsCount = 10;

    public const float SpeedWeapon = 4f;
    public const float SpeedPlayer = 2.5f;

    public const int TimeThrowStone = 3;
    public const int TimeRespawnPlayer = 1;

    public const int RotateStoneParameter = 10;

    public InputField InputRoundTime;
    public InputField InputTimeRespawnWeapon;
    public InputField InputBotsCount;
    public InputField InputSpeedWeapon;
    public InputField InputSpeedPlayer;
    public InputField InputTimeThrow;
    public InputField InputInitialCountWeapons;

    public void Start()
    {
        InputRoundTime.text = Settings.RoundTime.ToString();
        InputTimeRespawnWeapon.text = Settings.TimeRespawnWeapon.ToString();
        InputBotsCount.text = Settings.BotsCount.ToString();
        InputSpeedPlayer.text = Settings.SpeedPlayer.ToString();
        InputSpeedWeapon.text = Settings.SpeedWeapon.ToString();
        InputTimeThrow.text = Settings.TimeThrowStone.ToString();
        InputInitialCountWeapons.text = Settings.WeaponsCount.ToString();

    }

    public void SetRoundTime(string value)
    {
        Settings.RoundTime = Convert.ToInt32(InputRoundTime.text);
    }
    public void SetTimeRespawnWeapon()
    {

    }
    public void SetBotsCount()
    {

    }
    public void SetTimeThrow()
    {

    }
    public void SetInitialCountWeapons()
    {

    }
    public void SetSpeedPlayer()
    {

    }
    public void SetSpeedWeapon()
    {

    }
    public void Set()
    {

    }
	
}
