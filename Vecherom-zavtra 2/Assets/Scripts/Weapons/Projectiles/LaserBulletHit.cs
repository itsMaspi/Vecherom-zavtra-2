using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBulletHit : NetworkBehaviour
{
    // Start is called before the first frame update
    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), 1f);    
    }


    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
