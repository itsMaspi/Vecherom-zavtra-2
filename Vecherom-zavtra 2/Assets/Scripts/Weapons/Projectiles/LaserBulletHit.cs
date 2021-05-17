using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletHit : NetworkBehaviour
{
    // Start is called before the first frame update
    public override void OnStartServer()
    {
        float t = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Invoke(nameof(DestroySelf), t);    
    }


    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
