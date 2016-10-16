using System.Collections.Generic;
using Caveman.Level;
using Caveman.Network;
using Caveman.Pools;
using Caveman.UI;
using Random = System.Random;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayersManager
    {
        private readonly IServerNotify serverNotify;
        private readonly SmoothCamera smoothCamera;
	    private readonly List<PlayerModelBase> models;
	    private readonly PlayerPool pool;
	    private readonly Random rand;
        private readonly MapCore mapCore;

	    public PlayersManager(IServerNotify serverNotify, SmoothCamera smoothCamera, Random rand, PlayerPool pool, MapCore mapCore)
        {
            this.serverNotify = serverNotify;
            this.smoothCamera = smoothCamera;
            this.pool = pool;
            this.rand = rand;
	        this.mapCore = mapCore;
	        
            models = new List<PlayerModelBase>();
            models.AddRange(pool.GetCurrentPlayerModels());
            // only for multiplayer
            pool.AddedPlayer += model => models.Add(model);
            pool.RemovePlayer += model => models.Remove(model);
        }

        public void StartUseWeapon()
        {
            var players = pool.GetCurrentPlayerModels();
            foreach (var player in players)
            {
                var playerClient = (PlayerModelClient) player;
                playerClient.StartUseWeapon();
            }
        }
     
        private PlayerModelBase CreateModel(PlayerCore playerCore, Transform prefab)
        {
            var model = prefab.GetComponent<PlayerModelBase>();
            model.Initialization(playerCore, serverNotify, FindClosestPlayer, pool, mapCore.GetRandomPosition);
            pool.Add(playerCore.Id, model);

            model.WeaponPoolChange += PoolsManager.instance.ChangeWeaponPool;
            // todo deleted this row, ectracte in method
            model.RespawnInstantly(mapCore.RandomPosition);
            model.name = playerCore.Name;
            model.transform.GetChild(0).GetComponent<TextMesh>().text = playerCore.Name;
            return model;
        }

        public void CreateClientAiModel(PlayerCore playerCore, Transform prefab)
        {
            var model = CreateModel(playerCore, prefab);
            var modelAi = (PlayerModelAi) model;
            modelAi.Initialization(rand, mapCore.MaxDistance);
        }

        public void CreateServerModel(PlayerCore playerCore, Transform prefab)
        {
            CreateModel(playerCore, prefab);
        }

        public void CreatePlayerModel(PlayerCore playerCore, Transform prefab, BattleGui battleGui)
        {
            var model = CreateModel(playerCore, prefab);
            var playerModel = (PlayerModelHuman)model;
            playerModel.InitializationByMap(mapCore.Width, mapCore.Height);
            battleGui.SubscribeOnEvents(playerModel, mapCore.GetRandomPosition);
            smoothCamera.target = prefab.transform;
            smoothCamera.SetPlayer(prefab.GetComponent<PlayerModelBase>());
            if (serverNotify != null) model.GetComponent<SpriteRenderer>().material.color = Color.red;
        }

        private PlayerModelBase FindClosestPlayer(PlayerModelBase playerModelBase)
	    {
	        var minDistance = mapCore.MaxDistance;
            var positionPlayer = playerModelBase.transform.position;
		    PlayerModelBase result = null;
		    for (var i = 0; i < models.Count; i++)
		    {
			    if (!models[i].gameObject.activeSelf || models[i] == playerModelBase ||
			        !models[i].spriteRenderer.isVisible || models[i].invulnerability) continue;
			    var childDistance = Vector2.SqrMagnitude(models[i].transform.position - positionPlayer);
			    if (minDistance > childDistance)
			    {
				    result = models[i];
				    minDistance = childDistance;
			    }
		    }
		    return result;
	    }
    }
}