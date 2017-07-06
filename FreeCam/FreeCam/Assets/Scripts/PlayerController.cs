using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InControl;

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

	public PlayerActions playerActions;

	void Start () 
	{
		CreateBindings ();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void FixedUpdate () 
	{
		if (Input.GetMouseButton (0)) 
		{
			Shoot ();
		}

		MovePlayer ();

		ClampVelocity ();
	}

	void LateUpdate ()
	{
		transform.Rotate (Vector3.up * Input.GetAxis ("Mouse X") * Sensitivity.x);
		transform.Rotate (Vector3.left * Input.GetAxis ("Mouse Y") * Sensitivity.y);
	}

	void CreateBindings ()
	{
		playerActions = new PlayerActions ();

		playerActions.MoveLeft.AddDefaultBinding (Key.A);
		playerActions.MoveLeft.AddDefaultBinding (InputControlType.LeftStickLeft);

		playerActions.MoveRight.AddDefaultBinding (Key.D);
		playerActions.MoveRight.AddDefaultBinding (InputControlType.LeftStickRight);

		playerActions.MoveReverse.AddDefaultBinding (Key.S);
		playerActions.MoveReverse.AddDefaultBinding (InputControlType.LeftStickDown);

		playerActions.MoveForward.AddDefaultBinding (Key.W);
		playerActions.MoveForward.AddDefaultBinding (InputControlType.LeftStickUp);

		playerActions.MoveUp.AddDefaultBinding (Key.Space);
		playerActions.MoveUp.AddDefaultBinding (InputControlType.RightBumper);

		playerActions.MoveDown.AddDefaultBinding (Key.LeftControl);
		playerActions.MoveDown.AddDefaultBinding (InputControlType.LeftBumper);

		playerActions.LookLeft.AddDefaultBinding (Mouse.NegativeX);
		playerActions.LookLeft.AddDefaultBinding (InputControlType.RightStickLeft);

		playerActions.LookRight.AddDefaultBinding (Mouse.PositiveX);
		playerActions.LookRight.AddDefaultBinding (InputControlType.RightStickRight);

		playerActions.LookDown.AddDefaultBinding (Mouse.NegativeY);
		playerActions.LookDown.AddDefaultBinding (InputControlType.RightStickDown);

		playerActions.LookUp.AddDefaultBinding (Mouse.PositiveY);
		playerActions.LookUp.AddDefaultBinding (InputControlType.RightStickUp);

		playerActions.Shoot.AddDefaultBinding (Mouse.LeftButton);
		playerActions.Shoot.AddDefaultBinding (InputControlType.RightTrigger);

		playerActions.Ability.AddDefaultBinding (Mouse.RightButton);
		playerActions.Ability.AddDefaultBinding (InputControlType.LeftTrigger);
	}

	void MovePlayer ()
	{
		/*if (Input.GetKey (KeyCode.D)) 
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
		}*/

		rb.AddRelativeForce 
		(
			playerActions.Move.Value.x * Force.x, 
			playerActions.Elevate.Value * Force.y, 
			playerActions.Move.Value.y * Force.z
		);
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
