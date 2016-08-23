using Caveman.Players;
using Caveman.Setting;
using Caveman.UI.Battle;
using Caveman.UI.Common;
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

        public bool IsMultiplayerMode { get; private set; }

        private Random r;

        public void Awake()
        {
            r = new Random();
            instance = this;
            mainGameTimer.RoundEnded += () =>
            {
                resultRound.gameObject.SetActive(true);
                waitForResp.gameObject.SetActive(false);
            };

            gameObject.SetActive(false);
            movementJoystick.gameObject.SetActive(false);           
        }

        public void SubscribeOnEvents(PlayerCore playerCore)
        {
            playerCore.WeaponCountChange += count => weapons.text = count.ToString();
            playerCore.KillCountChange += count => killed.text = count.ToString();
	        playerCore.BonusActivate += bonusesPanel.BonusActivated;
        }

        public void SubscribeOnEvents(PlayerModelBase playerModelBase)
        {
	        playerModelBase.PlayerCore.IsAliveChange += isAlive => ReportIsAliveChange(playerModelBase.PlayerCore.Config.RespawnDuration, isAlive);
            waitForResp.buttonRespawn.onClick.AddListener(delegate
            {
                // TODO: set count gold respawn received from server
                //if (!playerModelBase.SpendGold(0))
                //   return;
	            playerModelBase.PlayerCore.IsAlive = true;
                playerModelBase.StopAllCoroutines();
                playerModelBase.RespawnInstantly(RandomPosition);
            });
        }


	    private void ReportIsAliveChange(int respawnDuration, bool isAlive)
	    {
		    if (isAlive)
		    {
			    waitForResp.gameObject.SetActive(false);
		    }
		    else
		    {
			    waitForResp.Activate(respawnDuration);
		    }
	    }

        private Vector2 RandomPosition
        {
            get { return new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap)); }
        }
    }
}