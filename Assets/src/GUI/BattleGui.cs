using System;
using System.Collections.Generic;
using Caveman.Level;
using Caveman.Players;
using Caveman.UI.Battle;
using Caveman.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI
{
    public class BattleGui : MonoBehaviour
    {
        public CNAbstractController joystick;
        public BonusesPanel bonusesPanel;
        public MainGameTimer mainGameTimer;
        public ResultRound resultRound;
        public RespawnWindow respawnWindow;
        public ChestIcon chestIcon;
        public Text weapons;
        public Text killed;

        public void Awake()
        {
            mainGameTimer.RoundEnded += () =>
            {
                resultRound.gameObject.SetActive(true);
                respawnWindow.gameObject.SetActive(false);
            };
        }

        public void Initialization(bool isMultiplayer, int roundTime, bool isObservableMode, Func<IEnumerable<PlayerModelBase>> getCurrentPlayers, LevelMode levelMode)
        {
            // todo update this after improve get time from server
            resultRound.Initialization(isMultiplayer, getCurrentPlayers);
            respawnWindow.Initialization(isMultiplayer, getCurrentPlayers);

            mainGameTimer.Initialization(isMultiplayer, roundTime, levelMode);
            if (isObservableMode)
            {
                joystick.Disable();
            }
        }

        public void SubscribeOnEvents(PlayerModelHero model, Func<Vector2> randomPosition)
        {
            var playerCore = model.PlayerCore;
            playerCore.WeaponCountChange += count => weapons.text = count.ToString();
            playerCore.KillCountChange += count => killed.text = count.ToString();
            playerCore.BonusActivate += bonusesPanel.BonusActivated;
            playerCore.ChestActivate += (openHandler, isEnable) => 
            {
                chestIcon.ChestActivated(openHandler, isEnable);
            };
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
                model.RespawnInstantly(randomPosition());
            });
        }
    }
}