using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(ChangeNickname))]
    public string nickname;
    private string localNickname;

    public TMPro.TextMeshProUGUI nickText;
    public GameObject interactionIcon;
    public GameObject interactionButton;
    public GameObject dialogueSystem;
    public GameObject virtualCamera;

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
        if (!isLocalPlayer) return;
    }

    void OnApplicationQuit()
    {
        Utils.DeleteUserInfo();
    }

    public void OpenInteractableIcon()
	{
        interactionIcon.SetActive(true);
        interactionButton.SetActive(true);
	}

    public void CloseInteractableIcon()
    {
        interactionIcon.SetActive(false);
        interactionButton.SetActive(false);
    }

    public void OnInteract()
    {
        if (!isLocalPlayer) return;
        if (PauseManager.pauseState == PauseState.Paused) return;
        
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

    public void OnPause()
	{
        if (!isLocalPlayer) return;

        PauseManager.instance.Toggle();
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
