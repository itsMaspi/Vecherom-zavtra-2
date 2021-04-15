using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
	public float Range { get; set; }
	public int Damage { get; set; }
	public float Speed { get; set; }

	// Start is called before the first frame update
	void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * Speed); // el fill no es mou, transform.right sempre es positiu
		Debug.Log(Speed);
    }
}
