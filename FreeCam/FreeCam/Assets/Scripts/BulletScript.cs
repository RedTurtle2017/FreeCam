using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour 
{
	public Vector3 speed;
	public Rigidbody rb;


	void Start ()
	{
		rb = GetComponent<Rigidbody> ();

		rb.velocity = transform.forward * (speed.z * Time.deltaTime);
	}

	void Update () 
	{
		
	}
}
