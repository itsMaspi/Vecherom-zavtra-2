using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerVZ networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject mpOptionsPanel = null;

    public void HostLobby()
	{
        networkManager.StartHost();

        mpOptionsPanel.SetActive(false);
	}
}
