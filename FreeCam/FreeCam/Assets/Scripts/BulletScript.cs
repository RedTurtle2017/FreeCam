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

	void Awake ()
	{
		Invoke ("EnableBulletCol", BulletColDelay);
		playerRb = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody> ();
		rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * speed.z * Time.deltaTime;
	}

	void OnCollisionEnter (Collision col)
	{
		rb.transform.rotation = Quaternion.LookRotation (rb.velocity);
	}

	void EnableBulletCol ()
	{
		BulletCol.enabled = true;
	}
}
