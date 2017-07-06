using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour 
{
	public Vector3 speed;
	public Rigidbody rb;
	public Rigidbody playerRb;
	public float BumperForce;

	void Start ()
	{
		playerRb = GameObject.FindGameObjectWithTag ("PlayerRb").GetComponent<Rigidbody> ();
		rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.up * speed.z;
	}

	void FixedUpdate ()
	{
		//rb.velocity = transform.up * speed.z;
	}

	void OnCollisionEnter (Collision col)
	{
		rb.AddForce (col.contacts [0].normal * BumperForce, ForceMode.Impulse);
	}
}
