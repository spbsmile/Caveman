using System;
using Caveman.Level;
using Caveman.Network;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.UI;
using Random = System.Random;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayersManager 
    {
        private readonly IClientListener serverNotify;        
        private readonly SmoothCamera smoothCamera;
        private readonly Random r;

        public bool[] name; 

        public PlayersManager(IClientListener clientListener, SmoothCamera smoothCamera, System.Random random)
        {
            r = random;
            serverNotify = clientListener;
            this.smoothCamera = smoothCamera;
        }

        public void CreatePlayerModel(PlayerCore playerCore, bool isAiPlayer, bool isServerPlayer, Transform prefabModel)
        {
            var prefab = prefabModel;
            var playerModel = prefab.GetComponent<PlayerModelBase>();
            if (!isServerPlayer && !isAiPlayer)
            {
                BattleGui.instance.SubscribeOnEvents(playerModel);
                smoothCamera.target = prefab.transform;
                smoothCamera.SetPlayer(prefab.GetComponent<PlayerModelBase>());
                if (serverNotify != null) playerModel.GetComponent<SpriteRenderer>().material.color = Color.red;
            }
            playerModel.Init(playerCore, r, serverNotify);
            PlayerPool.instance.Add(playerCore.Id, playerModel);

            playerModel.ChangedWeaponsPool += PoolsManager.instance.SwitchPoolWeapons;
            playerModel.Birth(new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1)));
            
            // copy past
            playerModel.name = playerCore.Name;
            playerModel.transform.GetChild(0).GetComponent<TextMesh>().text = playerCore.Name;
        }

        public void CreateAllPlayersModels()
        {
            
        }

        public void Respawn()
        {

        }

        public void PickupWeapon()
        {

        }

        public void PickupBonus()
        {

        }

        public void ThrowWeapon()
        {

        }
    }
}