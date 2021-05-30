using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class NetworkGamePlayerVZ : NetworkBehaviour
{
    [SyncVar]
    public string displayName = "Loading...";

    private NetworkManagerVZ room;
    private NetworkManagerVZ Room
	{
		get
		{
            if (room != null) return room;
            return room = NetworkManager.singleton as NetworkManagerVZ;
		}
	}

	public override void OnStartClient()
	{
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
	}

	public override void OnStopClient()
	{
        Room.GamePlayers.Remove(this);
	}

	[Server]
	public void SetDisplayName(string displayName)
	{
		this.displayName = displayName;
	}
}
