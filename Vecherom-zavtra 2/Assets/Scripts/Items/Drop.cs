using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : Interactable
{
    public string itemSlug;
    public override void Interact(GameObject gameObject)
    {
        Debug.Log($"Interacted with: {gameObject}");
        gameObject.GetComponent<InventoryController>().GiveItem(itemSlug);
        CmdDestroySelf();
    }

    [Command(requiresAuthority = false)]
    public void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }


}
