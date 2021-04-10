using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject interactionIcon;
    public GameObject dialogueSystem;

    private Vector2 boxSize = new Vector2(1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
