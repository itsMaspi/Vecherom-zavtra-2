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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Hit: {collision.name}");
            collision.GetComponent<Player>().TakeDamage(10);
        }
    }

    [ServerCallback]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
