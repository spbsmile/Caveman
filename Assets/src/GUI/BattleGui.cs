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
        [SerializeField] private CNAbstractController joystick;
        [SerializeField] private BonusesPanel bonusesPanel;
        [SerializeField] private MainGameTimer mainGameTimer;
        [SerializeField] private ResultRound resultRound;
        [SerializeField] private RespawnWindow respawnWindow;
        [SerializeField] private ChestIcon chestIcon;
        [SerializeField] private Text weaponCount;
        [SerializeField] private Text killedCount;
        [SerializeField] private WeaponIconFSM weaponIcon;

        public ResultRound ResultRound => resultRound;
        public MainGameTimer MainGameTimer => mainGameTimer;

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
            var core = model.Core;
            core.WeaponCountChange += count => weaponCount.text = count.ToString();
            core.WeaponTypeChange += type => weaponIcon.SetImage(type);
            core.KillCountChange += count => killedCount.text = count.ToString();
            core.BonusActivate += bonusesPanel.BonusActivated;
            core.ChestActivate += (openHandler, isEnable) => 
            {
                chestIcon.ChestActivated(openHandler, isEnable);
            };
            core.IsAliveChange += isAlive =>
            {
                if (isAlive)
                    respawnWindow.gameObject.SetActive(false);
                else
                    respawnWindow.Activate(core.Config.RespawnDuration);
            };

            joystick.ControllerMovedEvent += model.MovePlayer;
            joystick.FingerLiftedEvent += controller => model.HandlerOnStopMove();
            respawnWindow.ButtonRespawn.onClick.AddListener(delegate
            {
	            model.Core.IsAlive = true;
                model.StopAllCoroutines();
                model.RespawnInstantly(randomPosition());
            });
        }
    }
}