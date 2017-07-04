using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour 
{
	public Vector3 speed;
	public Rigidbody rb;
	public Rigidbody playerRb;

	void Start ()
	{
		playerRb = GameObject.FindGameObjectWithTag ("PlayerRb").GetComponent<Rigidbody> ();
		rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.up * (speed.z * Time.deltaTime) + playerRb.velocity;
	}

	void Update () 
	{
		
	}
}
