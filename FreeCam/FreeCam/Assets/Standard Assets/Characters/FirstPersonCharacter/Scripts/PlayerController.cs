using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	[Header ("Movement")]
	public bool UseKeyboardControls = true;
	public Rigidbody rb;
	public Rigidbody childRb;
	public float MaxVelocity = 100;
	public Vector3 Force;
	public Transform CameraPivot;
	public Vector2 Sensitivity;

	[Header ("Shooting")]
	public GameObject Shot;
	public Transform ShotSpawn;
	public float FireRate;
	private float nextFire;

	void Start () 
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void FixedUpdate () 
	{
		if (Input.GetMouseButton (0)) 
		{
			Shoot ();
		}

		if (UseKeyboardControls == true) 
		{
			MoveKeyboard ();
		}

		ClampVelocity ();
	}

	void LateUpdate ()
	{
		transform.Rotate (Vector3.up * Input.GetAxis ("Mouse X") * Sensitivity.x);
		transform.Rotate (Vector3.left * Input.GetAxis ("Mouse Y") * Sensitivity.y);
	}

	void MoveKeyboard ()
	{
		if (Input.GetKey (KeyCode.D)) 
		{
			rb.AddRelativeForce (Force.x, 0, 0);
		}

		if (Input.GetKey (KeyCode.A)) 
		{
			rb.AddRelativeForce (-Force.x, 0, 0);
		}

		if (Input.GetKey (KeyCode.W)) 
		{
			//childRb.AddRelativeForce (transform.InverseTransformDirection (0, 0, transform.forward.z * Force.z));
			rb.AddRelativeForce (0, 0, Force.z);
		}

		if (Input.GetKey (KeyCode.S)) 
		{
			rb.AddRelativeForce (0, 0, -Force.z);
		}

		if (Input.GetKey (KeyCode.Space)) 
		{
			rb.AddRelativeForce (0, Force.y, 0);
		}


		if (Input.GetKey (KeyCode.LeftControl)) 
		{
			rb.AddRelativeForce (0, -Force.y, 0);
		}
	}

	void ClampVelocity ()
	{
		rb.velocity = new Vector3 
			(
				Mathf.Clamp (rb.velocity.x, -MaxVelocity, MaxVelocity),
				Mathf.Clamp (rb.velocity.y, -MaxVelocity, MaxVelocity), 
				Mathf.Clamp (rb.velocity.z, -MaxVelocity, MaxVelocity)
			);
	}

	void Shoot ()
	{
		if (Time.time > nextFire) 
		{
			Instantiate (Shot, ShotSpawn.position, ShotSpawn.rotation);
			nextFire = Time.time + FireRate;
		}
	}
}
