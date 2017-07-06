using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour 
{
	public Vector3 speed;
	public Rigidbody rb;
	public Rigidbody playerRb;
	public float BumperForce;
	public Collider BulletCol;
	public float BulletColDelay = 0.1f;

	void Start ()
	{
		Invoke ("EnableBulletCol", BulletColDelay);
		playerRb = GameObject.FindGameObjectWithTag ("PlayerRb").GetComponent<Rigidbody> ();
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate ()
	{
		rb.velocity = transform.up * speed.z * Time.deltaTime;
	}

	void OnCollisionEnter (Collision col)
	{
		rb.AddForce (col.contacts [0].normal * BumperForce, ForceMode.Impulse);
	}

	void EnableBulletCol ()
	{
		BulletCol.enabled = true;
	}
}
