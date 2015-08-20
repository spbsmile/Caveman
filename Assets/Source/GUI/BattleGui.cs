using Caveman.Players;
using Caveman.UI.Battle;
using Caveman.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

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

        public void Awake()
        {
            instance = this;
            mainGameTimer.RoundEnded += () => resultRound.gameObject.SetActive(true);
        }

        public void SubscribeOnEvents(Player player)
        {
            player.WeaponsCountChanged += WeaponsCountChanged;
            player.KillsCountChanged += KillsCountChanged;
            player.Bonus += bonusesPanel.BonusActivated;
        }

        //public void SubscribeOnEvents(PlayerModelBase playerModelBase)
        //{
        //    playerModelBase.Death += vector2 => resultRound.gameObject.SetActive(true);
        //}

        private void WeaponsCountChanged(int count)
        {
            weapons.text = count.ToString();
        }

        private void KillsCountChanged(int count)
        {
            killed.text = count.ToString();
        }
    }
}