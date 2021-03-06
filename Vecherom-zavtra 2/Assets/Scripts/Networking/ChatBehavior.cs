using Mirror;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatBehavior : NetworkBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI chatText = null;
    [SerializeField] private TMPro.TMP_InputField inputField = null;
    [SerializeField] private GameObject canvas = null;

    private static event Action<string> OnMessage;

    // Called when the a client is connected to the server
    public override void OnStartAuthority()
    {
        canvas.SetActive(true);

        OnMessage += HandleNewMessage;
    }

	void Update()
	{
        if (!isLocalPlayer) return;
        //Debug.Log(inputField.isFocused);
        GetComponent<PlayerController>().SetChatting(inputField.isFocused);
	}

    public void SetFocus()
	{
        var m_EventSystem = EventSystem.current;
        m_EventSystem.SetSelectedGameObject(null);
        m_EventSystem.SetSelectedGameObject(inputField.gameObject);
	}

	// Called when a client has exited the server
	[ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }

        OnMessage -= HandleNewMessage;
    }

    // When a new message is added, update the Scroll View's Text to include the new message
    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }

    // When a client hits the enter button, send the message in the InputField
    [Client]
    public void Send()
    {
        //if (!Input.GetKeyDown(KeyCode.Return)) { return; }
        if (string.IsNullOrWhiteSpace(inputField.text)) { return; }
        CmdSendMessage(Utils.GetUserNickname(), inputField.text);
        inputField.text = string.Empty;
        SetFocus();
    }

    [Command]
    private void CmdSendMessage(string nickname, string message)
    {
        // Validate message
        RpcHandleMessage($"[{nickname}]: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }

}
