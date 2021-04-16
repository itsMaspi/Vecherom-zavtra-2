using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public GameObject interactionIcon;
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
    }

    void Start()
    {
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

    public void OpenInteractableIcon()
	{
        interactionIcon.SetActive(true);
	}

    public void CloseInteractableIcon()
    {
        interactionIcon.SetActive(false);
    }

    public void CheckInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
			if (dialogueSystem.GetComponent<DialogueManager>().dialoguePanel.activeSelf)
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
                        rc.transform.GetComponent<Interactable>().Interact();
                        return;
                    }
                }
            }
        }
    }
}
