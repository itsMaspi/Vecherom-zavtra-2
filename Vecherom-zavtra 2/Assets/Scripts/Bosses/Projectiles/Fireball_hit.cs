using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_hit : NetworkBehaviour
{
    public override void OnStartServer()
    {
        float t = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Invoke(nameof(DestroySelf), t);
    }


    [ServerCallback]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
