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
	    private List<PlayerModelBase> players;
	    private PlayerPool poolPlayers;
	    private readonly Random r;

	    public PlayersManager(IServerNotify serverNotify, SmoothCamera smoothCamera, Random random)
        {
            this.serverNotify = serverNotify;
            this.smoothCamera = smoothCamera;
	        r = random;
        }

	    public void CreateAllPlayerModel()
	    {
		    players = new List<PlayerModelBase>();
		    players.AddRange(poolPlayers.GetCurrentPlayers());
		    // todo may be deleted it
		    poolPlayers.AddedPlayer += @base => players.Add(@base);
		    poolPlayers.RemovePlayer += @base => players.Remove(@base);
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
            model.Init(playerCore, r, serverNotify, this);
            PlayerPool.instance.Add(playerCore.Id, model);

            model.ChangedWeaponsPool += PoolsManager.instance.SwitchPoolWeapons;
	        // todo deleted this row, ectracte in method
            model.RespawnInstantly(new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1)));

            // copy past
            model.name = playerCore.Name;
            model.transform.GetChild(0).GetComponent<TextMesh>().text = playerCore.Name;
        }

	    public PlayerModelBase FindClosestPlayer(PlayerModelBase playerModelBase)
	    {
		    var minDistance = (float) Settings.HeightMap * Settings.WidthMap;
		    var positionPlayer = playerModelBase.transform.position;
		    PlayerModelBase result = null;
		    for (var i = 0; i < players.Count; i++)
		    {
			    if (!players[i].gameObject.activeSelf || players[i] == playerModelBase ||
			        !players[i].spriteRenderer.isVisible || players[i].invulnerability) continue;
			    var childDistance = Vector2.SqrMagnitude(players[i].transform.position - positionPlayer);
			    if (minDistance > childDistance)
			    {
				    result = players[i];
				    minDistance = childDistance;
			    }
		    }
		    return result;
	    }
    }
}