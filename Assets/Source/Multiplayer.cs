using Caveman;
using Caveman.Network;
using Caveman.Setting;
using UnityEngine;

public class Multiplayer : EnterPoint, IServerListener 
{
	public override void Start () 
    {
        serverConnection = new ServerConnection { ServerListener = this };
        serverConnection.StartSession(SystemInfo.deviceUniqueIdentifier, SystemInfo.deviceName);
        //todo valid value!! id!! // in base Start
        serverConnection.SendRespawn(name);
        base.Start();
	}

    public void Update()
    {
        serverConnection.Update();
    }

    public void WeaponAddedReceived(Vector2 point)
    {
        Debug.Log("stone added : " + point);
        //todo var value size map 
        poolStones.New().transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br));
    }

    public void BonusAddedReceived(Vector2 point)
    {
        print(string.Format("WeaponRemovedReceived {0}", point));
    }

    public void PlayerDeadResceived(string playerId, Vector2 point)
    {
        print(string.Format("WeaponRemovedReceived {0}", point));
    }

    public void WeaponRemovedReceived(Vector2 point)
    {
        print(string.Format("WeaponRemovedReceived {0}", point));
    }

    public void MoveReceived(string playerId, Vector2 point)
    {
        print(string.Format("MoveReceived {0} by playerId {1}", point, playerId));
    }

    public void LoginReceived(string playerId)
    {
        print(string.Format("LoginReceived {0} by playerId {1}", playerId));
    }

    public void PickWeaponReceived(string playerId, Vector2 point)
    {
        print(string.Format("PickWeaponReceived {0} by playerId {1}", point, playerId));
    }

    public void PickBonusReceived(string playerId, Vector2 point)
    {
        print(string.Format("PickBonusReceived {0} by playerId {1}", point, playerId));
    }

    public void UseWeaponReceived(string playerId, Vector2 point)
    {
        Debug.Log(string.Format("UseWeaponReceived {0} by playerId {1}", point, playerId));
    }

    public void RespawnReceived(string playerId, Vector2 point)
    {
        Debug.Log(string.Format("RespawnReceived {0} by playerId {1}", point, playerId));
    }
}
