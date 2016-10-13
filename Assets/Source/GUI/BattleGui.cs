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
        public CNAbstractController joystick;
        public BonusesPanel bonusesPanel;
        public MainGameTimer mainGameTimer;
        public ResultRound resultRound;
        public RespawnWindow respawnWindow;
        public Text weapons;
        public Text killed;

        private Random rand;

        public void Awake()
        {
            rand = new Random();
            mainGameTimer.RoundEnded += () =>
            {
                resultRound.gameObject.SetActive(true);
                respawnWindow.gameObject.SetActive(false);
            };
        }

        public void Initialization(bool isMultiplayer)
        {
            resultRound.Initialization(isMultiplayer);
            mainGameTimer.Initialization(isMultiplayer);
        }

        public void SubscribeOnEvents(PlayerModelHuman model)
        {
            var playerCore = model.PlayerCore;
            playerCore.WeaponCountChange += count => weapons.text = count.ToString();
            playerCore.KillCountChange += count => killed.text = count.ToString();
            playerCore.BonusActivate += bonusesPanel.BonusActivated;
            playerCore.IsAliveChange += isAlive =>
            {
                if (isAlive)
                {
                    respawnWindow.gameObject.SetActive(false);
                }
                else
                {
                    respawnWindow.Activate(playerCore.Config.RespawnDuration);
                }
            };

            joystick.ControllerMovedEvent += model.MovePlayer;
            joystick.FingerLiftedEvent += controller => model.HandlerOnStopMove();
            respawnWindow.buttonRespawn.onClick.AddListener(delegate
            {
                // TODO: set count gold respawn received from server
                //if (!playerModelBase.SpendGold(0))
                //   return;
	            model.PlayerCore.IsAlive = true;
                model.StopAllCoroutines();
                model.RespawnInstantly(RandomPosition);
            });
        }

        private Vector2 RandomPosition
        {
            get { return new Vector2(rand.Next(Settings.WidthMap), rand.Next(Settings.HeightMap)); }
        }
    }
}