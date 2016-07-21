using Caveman.Network;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.UI;
using UnityEngine;

namespace Caveman.Players
{
  public class PlayersManager : MonoBehaviour
  {
    private IClientListener serverNotify;
    private bool isMultiplayer;

    public void Init(IClientListener serverNotify)
    {
      this.serverNotify = serverNotify;
      if (serverNotify != null) isMultiplayer = true;
    }

    protected void CreatePlayer(Player player, bool isAiPlayer, bool isServerPlayer, Transform prefabModel)
    {
      var prefab = Instantiate(prefabModel);
      var playerModel = prefab.GetComponent<PlayerModelBase>();
      if (!isServerPlayer && !isAiPlayer)
      {
        BattleGui.instance.SubscribeOnEvents(playerModel);
        smoothCamera.target = prefab.transform;
        smoothCamera.SetPlayer(prefab.GetComponent<PlayerModelBase>());
        if (serverNotify != null) playerModel.GetComponent<SpriteRenderer>().material.color = Color.red;
      }
      playerModel.Init(player, r, serverNotify);
      PlayerPool.instance.Add(player.Id, playerModel);

      playerModel.ChangedWeaponsPool += PoolsManager.instance.SwitchPoolWeapons;
      playerModel.Birth(new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1)));

      playerModel.name = player.Name;
      playerModel.transform.GetChild(0).GetComponent<TextMesh>().text = name;
      Player = player;
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