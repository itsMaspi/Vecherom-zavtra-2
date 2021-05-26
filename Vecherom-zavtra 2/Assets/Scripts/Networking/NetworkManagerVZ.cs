using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class NetworkManagerVZ : NetworkManager
{
	[SerializeField] private int minPlayers = 1;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerVZ roomPlayerPrefab = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

	public List<NetworkRoomPlayerVZ> RoomPlayers { get; } = new List<NetworkRoomPlayerVZ>();

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

		if ($"Assets/_Scenes/{SceneManager.GetActiveScene().name}.unity" != menuScene)
		{
			conn.Disconnect();
			return;
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		if ($"Assets/_Scenes/{SceneManager.GetActiveScene().name}.unity" == menuScene)
		{
			bool isLeader = RoomPlayers.Count == 0;

			NetworkRoomPlayerVZ roomPlayerInstance = Instantiate(roomPlayerPrefab);

			roomPlayerInstance.IsLeader = isLeader;

			NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
		}
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		if (conn.identity != null)
		{
			var player = conn.identity.GetComponent<NetworkRoomPlayerVZ>();

			RoomPlayers.Remove(player);

			NotifyPlayersOfReadyState();
		}

		base.OnServerDisconnect(conn);
	}

	public override void OnStopServer()
	{
		RoomPlayers.Clear();
	}

	public void NotifyPlayersOfReadyState()
	{
		foreach (var player in RoomPlayers)
		{
			player.HandleReadyToStart(IsReadyToStart());
		}
	}

	private bool IsReadyToStart()
	{
		if (numPlayers < minPlayers) return false;

		foreach (var player in RoomPlayers)
		{
			if (!player.IsReady) return false;
		}

		return true;
	}
}
