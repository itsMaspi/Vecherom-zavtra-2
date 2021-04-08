using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
	private void Reset()
	{
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

	public abstract void Interact();

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//Debug.Log("Interactable object enter triggered");
			collision.GetComponent<PlayerController>().OpenInteractableIcon();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//Debug.Log("Interactable object exit triggered");
			collision.GetComponent<PlayerController>().CloseInteractableIcon();
		}
	}
}
