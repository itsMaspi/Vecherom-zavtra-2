using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
	public float Range { get; set; }
	public int Damage { get; set; }
	public float Speed { get; set; }
	public Vector3 Force { get; set; }

	// Start is called before the first frame update
	void Start()
	{
		GetComponent<Rigidbody2D>().AddForce(Force * Speed); // el fill no es mou, transform.right sempre es positiu
		//Debug.Log(Force * Speed);
    }
}
