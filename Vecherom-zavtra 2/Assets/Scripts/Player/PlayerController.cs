using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using System.IO;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(ChangeNickname))]
    public string nickname;
    private string localNickname;

    public TMPro.TextMeshProUGUI nickText;
    public GameObject interactionIcon;
    [HideInInspector] public GameObject dialogueSystem = null;
    public GameObject chatPanel;
    [HideInInspector] public GameObject virtualCamera = null;

    public bool isChatting = true;

    private Vector2 boxSize = new Vector2(1f, 1f);


    public override void OnStartLocalPlayer()
    {
        // Get the player nickname and apply the nickname
        string path = Application.persistentDataPath + "/usr.vz";
        using (BinaryReader r = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            r.ReadInt32();
            localNickname = r.ReadString();
        }
        CmdSetNickname(localNickname);
    }

	void Start()
    {
        if (!isLocalPlayer)
		{
            GetComponent<PlayerInput>().enabled = false;
            transform.Find("HUD Canvas").gameObject.SetActive(false);
		}
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isLocalPlayer) return;
        if (virtualCamera == null)
		{
            virtualCamera = GameObject.Find("CM vcam");
		    if (virtualCamera != null)
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        if (dialogueSystem == null)
        {
            dialogueSystem = GameObject.Find("DialogueSystem");
        }
        

    }

    void OnApplicationQuit()
    {
        Utils.DeleteUserInfo();
    }

    public void OpenInteractableIcon()
	{
        interactionIcon.SetActive(true);
	}

    public void CloseInteractableIcon()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputValue value)
    {
        if (!isLocalPlayer) return;
        if (PauseManager.pauseState == PauseState.Paused) return;
        if (isChatting) return;
        if (value.isPressed)
		{
            if (dialogueSystem != null && dialogueSystem.GetComponent<DialogueManager>().dialoguePanel.activeSelf)
            {
                dialogueSystem.GetComponent<DialogueManager>().ContinueDialog();
                return;
            }

            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);

            if (hits.Length > 0)
            {
                foreach (var rc in hits)
                {
                    if (rc.transform.GetComponent<Interactable>())
                    {
                        rc.transform.GetComponent<Interactable>().Interact(gameObject);
                        return;
                    }
                }
            }
        }
    }

    public void OnPause(InputValue value)
	{
        if (!isLocalPlayer) return;
        if (isChatting) return;
        if (value.isPressed)
		{
            PauseManager.instance.Toggle();
		}
	}

    public void OnToggleChat(InputValue value)
	{
        if (!isLocalPlayer) return;
        if (isChatting) return;
        if (value.isPressed)
        {
            chatPanel.SetActive(!chatPanel.activeSelf);
            GetComponent<ChatBehavior>().SetFocus();
        }
    }

    public void SetChatting(bool isChatting)
	{
        this.isChatting = isChatting;
	}

    void ChangeNickname(string oldNick, string newNick)
	{
        nickText.text = newNick;
    }

    [Command]
    public void CmdSetNickname(string n)
	{
        nickname = n;
	}
}
