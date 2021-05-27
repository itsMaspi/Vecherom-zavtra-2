using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerVZ networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject mpOptionsPanel = null;
    [SerializeField] private GameObject playButton = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

	private void OnEnable()
	{
        NetworkManagerVZ.OnClientConnected += HandleClientConnected;
        NetworkManagerVZ.OnClientDisconnected += HandleClientDisconnected;
	}

    private void OnDisable()
    {
        NetworkManagerVZ.OnClientConnected -= HandleClientConnected;
        NetworkManagerVZ.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
	{
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
	}

    private void HandleClientConnected()
	{
        joinButton.interactable = true;

        gameObject.SetActive(false);
        mpOptionsPanel.SetActive(false);
        playButton.SetActive(false);
    }

    private void HandleClientDisconnected()
	{
        joinButton.interactable = true;
	}
}
