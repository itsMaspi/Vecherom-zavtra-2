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
    [HideInInspector] public GameObject dialogueSystem;
    public GameObject chatPanel;
    [HideInInspector] public GameObject virtualCamera;

    public bool isChatting = true;

    private Vector2 boxSize = new Vector2(1f, 1f);

    // Start is called before the first frame update

    public override void OnStartLocalPlayer()
    {
        virtualCamera = GameObject.Find("CM vcam");

        if (virtualCamera != null)
        {
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        dialogueSystem = GameObject.Find("DialogueSystem");

        // Get the player nickname and apply the nickname
        string path = Application.persistentDataPath + "/usr.vz";
        using (BinaryReader r = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            r.ReadInt32();
            localNickname = r.ReadString();
        }
        CmdSetNickname(localNickname);
    }

	public override void OnStartServer()
	{
		base.OnStartServer();

        
        //nickText.text = nickname;
        //CmdSetNickname();
    }

	void Start()
    {
        if (!isLocalPlayer)
		{
            GetComponent<PlayerInput>().enabled = false;
            transform.Find("HUD Canvas").gameObject.SetActive(false);
		}
        
        /*
        GameObject[] gObjects =  FindObjectsOfType<GameObject>();
        foreach (var Object in gObjects)
        {
            if (Object.GetComponent<CinemachineVirtualCamera>() != null)
            {
                virtualCamera = Object;
                continue;
            }
        }



        virtualCamera = GameObject.Find("CM vcam");

        if (virtualCamera != null)
        {
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        dialogueSystem = GameObject.Find("DialogueSystem");
        */
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer) return;
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
