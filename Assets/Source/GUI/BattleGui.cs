using Caveman.Players;
using Caveman.Setting;
using Caveman.UI.Battle;
using Caveman.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Caveman.UI
{
    public class BattleGui : MonoBehaviour
    {
        public CNAbstractController movementJoystick;
        public BonusesPanel bonusesPanel;
        public MainGameTimer mainGameTimer;
        public ResultRound resultRound;
        public WaitForResp waitForResp;
        public Text weapons;
        public Text killed;

        public static BattleGui instance;

        private System.Random r;

        public void Awake()
        {
            r = new Random();
            instance = this;
            mainGameTimer.RoundEnded += () =>
            {
                resultRound.gameObject.SetActive(true);
                waitForResp.gameObject.SetActive(false);
            };
        }

        public void SubscribeOnEvents(Player player)
        {
            player.WeaponsCountChanged += WeaponsCountChanged;
            player.KillsCountChanged += KillsCountChanged;
            player.BonusActivated += bonusesPanel.BonusActivated;
        }

        public void SubscribeOnEvents(PlayerModelBase playerModelBase)
        {
            playerModelBase.Death += vector2 => waitForResp.Activate(playerModelBase.specification.TimeRespawn);
            playerModelBase.RespawnGUIDisabled += () => waitForResp.gameObject.SetActive(false);
            waitForResp.buttonRespawn.onClick.AddListener(delegate
            {
                playerModelBase.StopAllCoroutines();
                playerModelBase.Birth(RandomPosition);
                waitForResp.gameObject.SetActive(false);
            });
        }

        private void WeaponsCountChanged(int count)
        {
            weapons.text = count.ToString();
        }

        private void KillsCountChanged(int count)
        {
            killed.text = count.ToString();
        }
       
        private Vector2 RandomPosition
        {
            get { return new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap)); }
        }
    }
}