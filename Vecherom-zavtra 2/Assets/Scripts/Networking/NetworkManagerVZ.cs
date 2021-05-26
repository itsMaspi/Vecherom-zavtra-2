using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class NetworkManagerVZ : NetworkManager
{
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerVZ roomPlayerPrefab = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);

		OnClientConnected?.Invoke();
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);

		OnClientDisconnected?.Invoke();
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		if (numPlayers >= maxConnections)
		{
			conn.Disconnect();
			return;
		}

		if (SceneManager.GetActiveScene().name != menuScene)
		{
			conn.Disconnect();
			return;
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		if (SceneManager.GetActiveScene().name == menuScene)
		{
			NetworkRoomPlayerVZ roomPlayerInstance = Instantiate(roomPlayerPrefab);

			NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
		}
	}
}
