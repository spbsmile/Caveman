using System;
using System.Collections.Generic;
using System.Linq;
using Caveman.CustomAnimation;
using Caveman.Level;
using Caveman.Network;
using Caveman.Pools;
using JetBrains.Annotations;
using Random = System.Random;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerManager
    {
        private readonly IServerNotify serverNotify;        
	    private readonly List<PlayerModelBase> models;
	    private readonly PlayerPool pool;
	    private readonly Random rand;
        private readonly MapCore mapCore;
        private readonly ObjectPool<ImageBase> imagesDeath;
        private readonly LevelMode levelMode;

        public PlayerManager(IServerNotify serverNotify, Random rand, PlayerPool pool, MapCore mapCore, ObjectPool<ImageBase> imagesDeath
        , LevelMode levelMode)
        : this(rand, pool, mapCore, imagesDeath, levelMode)
        {
            this.serverNotify = serverNotify;              
        }

        public PlayerManager(Random rand, PlayerPool pool,
            MapCore mapCore,
            ObjectPool<ImageBase> imagesDeath, LevelMode levelMode)
        {                  
            this.pool = pool;
            this.rand = rand;
	        this.mapCore = mapCore;
            this.imagesDeath = imagesDeath;
            this.levelMode = levelMode;
            models = new List<PlayerModelBase>();
            models.AddRange(pool.GetCurrentPlayerModels());   
            pool.AddedPlayer += model => models.Add(model);
            pool.RemovePlayer += model => models.Remove(model);                      
        }    

        public void CreateBotModel(PlayerCore playerCore, Transform prefab, Transform containerStones)
        {
            var model = CreateModel(playerCore, prefab);
            var modelAi = (PlayerModelBot) model;
            modelAi.Initialization(rand, mapCore.MaxDistance, containerStones);
        }

        public void CreateServerModel(PlayerCore playerCore, Transform prefab)
        {
            CreateModel(playerCore, prefab);
        }

        public void CreateHeroModel(PlayerCore playerCore, Transform prefab, Action<PlayerModelHero, Func<Vector2>> subscribeGuiOnEvents)
        {
            var model = CreateModel(playerCore, prefab);
            var playerModel = (PlayerModelHero)model;
            playerModel.InitializationByMap(mapCore.Width, mapCore.Height);
            subscribeGuiOnEvents(playerModel, mapCore.GetRandomPosition);            
            if (serverNotify != null) model.GetComponent<SpriteRenderer>().material.color = Color.red;
        }

        private PlayerCore GetById(string playerId)
        {
            return models.FirstOrDefault(pl => pl.PlayerCore.Id == playerId)?.PlayerCore;
        }

        [CanBeNull]
        private PlayerModelBase FindClosestPlayer(PlayerModelBase playerModelBase)
	    {
	        var minDistance = mapCore.MaxDistance;
            var positionPlayer = playerModelBase.transform.position;
		    PlayerModelBase result = null;
		    for (var i = 0; i < models.Count; i++)
		    {
			    if (!models[i].gameObject.activeSelf || models[i] == playerModelBase ||
			        !models[i].spriteRenderer.isVisible || models[i].PlayerCore.Invulnerability) continue;
			    var childDistance = Vector2.SqrMagnitude(models[i].transform.position - positionPlayer);
			    if (minDistance > childDistance)
			    {
				    result = models[i];
				    minDistance = childDistance;
			    }
		    }
		    return result;
	    }

        private PlayerModelBase CreateModel(PlayerCore playerCore, Component prefab)
        {
            var model = prefab.GetComponent<PlayerModelBase>();
            //todo FindClosestPlayer  only for offline mode
            model.Initialization(
                playerCore,
                serverNotify,
                FindClosestPlayer,
                pool, mapCore.GetRandomPosition,
                imagesDeath.New().transform,
                levelMode,
                GetById);
            pool.Add(playerCore.Id, model);
            model.RespawnInstantly(mapCore.RandomPosition);
            model.name = playerCore.Name;
            model.transform.GetChild(0).GetComponent<TextMesh>().text = playerCore.Name;
            return model;
        }
    }
}