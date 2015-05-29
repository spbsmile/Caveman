using System;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.Setting
{
    public class ForceSettings : MonoBehaviour
    {
        public InputField roundTime;
        public InputField timeRespawnWeapon;
        public InputField botsCount;
        public InputField speedWeapon;
        public InputField speedPlayer;
        public InputField timeThrow;
        public InputField initialCountWeapons;
        public InputField countMaxWeapons;

        public void Start()
        {
            roundTime.text = Settings.RoundTime.ToString();
            timeRespawnWeapon.text = Settings.TimeRespawnWeapon.ToString();
            botsCount.text = Settings.BotsCount.ToString();
            speedPlayer.text = Settings.SpeedPlayer.ToString();
            speedWeapon.text = Settings.SpeedWeapon.ToString();
            timeThrow.text = Settings.TimeThrowStone.ToString();
            initialCountWeapons.text = Settings.WeaponsCount.ToString();
            countMaxWeapons.text = Settings.MaxCountWeapons.ToString();
        }

        public void SetRoundTime(string value)
        {
            Settings.RoundTime = Convert.ToInt32(roundTime.text);
        }

        public void SetTimeRespawnWeapon()
        {
            Settings.TimeRespawnWeapon = Convert.ToInt32(timeRespawnWeapon.text);
        }

        public void SetBotsCount()
        {
            Settings.BotsCount = Convert.ToInt32(botsCount.text);
        }

        public void SetTimeThrow()
        {
            Settings.TimeThrowStone = Convert.ToInt32(timeThrow.text);
        }

        public void SetInitialCountWeapons()
        {
            Settings.WeaponsCount = Convert.ToInt32(initialCountWeapons.text);
        }

        public void SetSpeedPlayer()
        {
            Settings.SpeedPlayer = Convert.ToInt32(speedPlayer.text);
        }

        public void SetSpeedWeapon()
        {
            Settings.SpeedWeapon = Convert.ToInt32(speedWeapon.text);
        }

        public void Set()
        {

        }
    }
}