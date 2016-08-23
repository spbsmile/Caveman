using System.Collections.Generic;
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
        private readonly IServerNotify serverNotify;
        private readonly SmoothCamera smoothCamera;
	    private List<PlayerModelBase> playersModel;
	    private PlayerPool playerPool;
	    private readonly Random r;

	    public PlayersManager(IServerNotify serverNotify, SmoothCamera smoothCamera, Random random)
        {
            this.serverNotify = serverNotify;
            this.smoothCamera = smoothCamera;
	        r = random;
	        playerPool = PlayerPool.instance;
        }

	    public void CreateAllPlayerModel()
	    {
		    playersModel = new List<PlayerModelBase>();
		    playersModel.AddRange(playerPool.GetCurrentPlayers());
		    // todo may be deleted it
		    playerPool.AddedPlayer += @base => playersModel.Add(@base);
		    playerPool.RemovePlayer += @base => playersModel.Remove(@base);
	    }

	    public void CreatePlayerModel(PlayerCore playerCore, bool isAiPlayer, bool isServerPlayer, Transform prefab)
        {
            var model = prefab.GetComponent<PlayerModelBase>();
            if (!isServerPlayer && !isAiPlayer)
            {
                BattleGui.instance.SubscribeOnEvents(model);
                smoothCamera.target = prefab.transform;
                smoothCamera.SetPlayer(prefab.GetComponent<PlayerModelBase>());
                if (serverNotify != null) model.GetComponent<SpriteRenderer>().material.color = Color.red;
            }
            model.Init(playerCore, r, serverNotify, this, playerPool);
            PlayerPool.instance.Add(playerCore.Id, model);

            model.ChangedWeaponsPool += PoolsManager.instance.SwitchPoolWeapons;
	        // todo deleted this row, ectracte in method
            model.RespawnInstantly(new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1)));
            model.name = playerCore.Name;
            model.transform.GetChild(0).GetComponent<TextMesh>().text = playerCore.Name;
        }

	    public PlayerModelBase FindClosestPlayer(PlayerModelBase playerModelBase)
	    {
		    var minDistance = (float) Settings.HeightMap * Settings.WidthMap;
		    var positionPlayer = playerModelBase.transform.position;
		    PlayerModelBase result = null;
		    for (var i = 0; i < playersModel.Count; i++)
		    {
			    if (!playersModel[i].gameObject.activeSelf || playersModel[i] == playerModelBase ||
			        !playersModel[i].spriteRenderer.isVisible || playersModel[i].invulnerability) continue;
			    var childDistance = Vector2.SqrMagnitude(playersModel[i].transform.position - positionPlayer);
			    if (minDistance > childDistance)
			    {
				    result = playersModel[i];
				    minDistance = childDistance;
			    }
		    }
		    return result;
	    }
    }
}