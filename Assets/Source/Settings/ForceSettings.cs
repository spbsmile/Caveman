using System;
using Assets.Source.Settings;
using Caveman;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ForceSettings : MonoBehaviour 
{
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
        Settings.TimeRespawnWeapon = Convert.ToInt32(InputTimeRespawnWeapon.text);
    }
    public void SetBotsCount()
    {
        Settings.BotsCount = Convert.ToInt32(InputBotsCount.text);
    }
    public void SetTimeThrow()
    {
        Settings.TimeThrowStone = Convert.ToInt32(InputTimeThrow.text);
    }
    public void SetInitialCountWeapons()
    {
        Settings.WeaponsCount = Convert.ToInt32(InputInitialCountWeapons.text);
    }
    public void SetSpeedPlayer()
    {
        Settings.SpeedPlayer = Convert.ToInt32(InputSpeedPlayer.text);
    }
    public void SetSpeedWeapon()
    {
        Settings.SpeedWeapon = Convert.ToInt32(InputSpeedWeapon.text);
    }
    public void Set()
    {

    }
	
}
