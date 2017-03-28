using System;
using Caveman.DevSetting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Caveman.UI.Menu.Options
{
    // This for developer settings, when game on build, possible changed parameters
    public class ForceSettings : MonoBehaviour
    {
        [SerializeField] private InputField roundTime;

        [SerializeField] private InputField weaponTimeRespawn;
        [SerializeField] private InputField weaponInitialCount;
        [SerializeField] private InputField weaponSpeed;
        [SerializeField] private InputField weaponCooldown;
        [SerializeField] private InputField weaponCountItems;
        [SerializeField] private InputField weaponWeight;

        [SerializeField] private InputField bonusSpeedMaxCount;
        [SerializeField] private InputField bonusSpeedTimeRespawn;
       
        [SerializeField] private InputField botsCount;
        [SerializeField] private InputField playerSpeed;

        [SerializeField] private InputField serverPingTime;
        [SerializeField] private InputField ip_server;
        [SerializeField] private Toggle disableSendMove;

        public void Start()
        {
            //roundTime.text = Settings.RoundTime.ToString();
            //botsCount.text = Settings.BotsCount.ToString();
            //playerSpeed.text = Settings.PlayerSpeed.ToString();
            //weaponSpeed.text = Settings.StoneSpeed.ToString();
            //weaponCooldown.text = Settings.WeaponTimerThrow.ToString();
            //weaponInitialCount.text = Settings.WeaponInitialLying.ToString();
            //weaponTimeRespawn.text = Settings.WeaponTimeRespawn.ToString();
            //weaponCountItems.text = Settings.WeaponCountPickup.ToString();
            //bonusSpeedMaxCount.text = Settings.BonusSpeedMaxCount.ToString();
            //bonusSpeedTimeRespawn.text = Settings.BonusTimeRespawn.ToString();
            //weaponWeight.text = Settings.WeaponsMaxOnPlayer.ToString();
            serverPingTime.text = DevSettings.ServerPingTime.ToString();    

            ip_server.text = PlayerPrefs.HasKey(DevSettings.KeyIpServer) ? PlayerPrefs.GetString(DevSettings.KeyIpServer) : DevSettings.IP_SERVER;

            //roundTime.onEndEdit.AddListener(delegate
            //{
            //    Settings.RoundTime = Convert.ToInt32(roundTime.text);
            //});
            botsCount.onEndEdit.AddListener(delegate
            {
              //  Settings.BotsCount = Convert.ToInt32(botsCount.text);
            });
            //playerSpeed.onEndEdit.AddListener(delegate
            //{
            //    Settings.PlayerSpeed = Convert.ToInt32(playerSpeed.text);
            //});
            //weaponSpeed.onEndEdit.AddListener(delegate
            //{
            //    Settings.StoneSpeed = Convert.ToInt32(weaponSpeed.text);
            //});
            //weaponTimeRespawn.onEndEdit.AddListener(delegate
            //{
            //    Settings.WeaponTimeRespawn = Convert.ToInt32(weaponTimeRespawn.text);
            //});
            //weaponCooldown.onEndEdit.AddListener(delegate
            //{
            //    Settings.WeaponTimerThrow = Convert.ToInt32(weaponCooldown.text);
            //});
            //weaponInitialCount.onEndEdit.AddListener(delegate
            //{
            //    Settings.WeaponInitialLying = Convert.ToInt32(weaponInitialCount.text);
            //});
            //weaponCountItems.onEndEdit.AddListener(delegate
            //{
            //    Settings.WeaponCountPickup = Convert.ToInt32(weaponCountItems.text);
            //});
            //weaponWeight.onEndEdit.AddListener(delegate
            //{
            //    Settings.WeaponsMaxOnPlayer = Convert.ToInt32(weaponWeight.text);
            //});
            bonusSpeedMaxCount.onEndEdit.AddListener(delegate
            {
                //Settings.BonusSpeedMaxCount = Convert.ToInt32(bonusSpeedMaxCount.text);
            });
            bonusSpeedTimeRespawn.onEndEdit.AddListener(delegate
            {
                //Settings.BonusTimeRespawn = Convert.ToInt32(bonusSpeedTimeRespawn.text);
            });
            serverPingTime.onEndEdit.AddListener(delegate
            {
                DevSettings.ServerPingTime = Convert.ToInt32(serverPingTime.text);
            });
            ip_server.onEndEdit.AddListener(delegate
            {
                DevSettings.IP_SERVER = Convert.ToString(ip_server.text);
                PlayerPrefs.SetString(DevSettings.KeyIpServer, Convert.ToString(ip_server.text));
            });

            disableSendMove.onValueChanged.AddListener(delegate
            {
                if (disableSendMove.isOn)
                {
                    DevSettings.DisableSendMove = true;
                }
                else {
                    DevSettings.DisableSendMove = false;
                }
            });
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}