using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomInTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().ZoomIn();
            Destroy(gameObject);
        }
    }
}
