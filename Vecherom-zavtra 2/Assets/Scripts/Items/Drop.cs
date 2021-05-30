using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : Interactable
{
    public string itemSlug;
    public float angle = 0f;
    public float increaseAngle = 1f;
    public float sinMultiplier = 0.4f;

    private float initialY;

	void Start()
	{
        initialY = transform.position.y + 1.5f;
	}
	void Update()
	{
        angle += increaseAngle;
        transform.position = new Vector3(transform.position.x, initialY + Mathf.Sin(Utils.ConvertToRadians(angle)) * sinMultiplier, transform.position.z);
    }

	public override void Interact(GameObject gameObject)
    {
        gameObject.GetComponent<InventoryController>().GiveItem(itemSlug);
        CmdDestroySelf();
    }

    [Command(requiresAuthority = false)]
    public void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
