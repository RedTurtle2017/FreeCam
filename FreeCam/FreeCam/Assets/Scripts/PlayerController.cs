using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InControl;

public class PlayerController : MonoBehaviour 
{
	public GameController gameControllerScript;

	[Header ("Pause")]
	public bool isPaused;

	[Header ("Movement")]
	public bool UseKeyboardControls = true;
	public Rigidbody rb;
	public Rigidbody childRb;
	public float MaxVelocity = 100;
	public Vector3 Force;
	public Transform CameraPivot;
	public Vector2 Sensitivity;

	[Header ("Engines")]
	public float MaxEngineEmissionRate;
	public ParticleSystem MainEngine;
	public ParticleSystem EngineTopRearA;
	public ParticleSystem EngineTopRearB;
	public ParticleSystem EngineTopFrontA;
	public ParticleSystem EngineTopFrontB;
	public ParticleSystem EngineBottomRearA;
	public ParticleSystem EngineBottomRearB;
	public ParticleSystem EngineBottomFrontA;
	public ParticleSystem EngineBottomFrontB;
	public ParticleSystem EngineRightSideRearA;
	public ParticleSystem EngineRightSideRearB;
	public ParticleSystem EngineRightSideFrontA;
	public ParticleSystem EngineRightSideFrontB;
	public ParticleSystem EngineLeftSideRearA;
	public ParticleSystem EngineLeftSideRearB;
	public ParticleSystem EngineLeftSideFrontA;
	public ParticleSystem EngineLeftSideFrontB;

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

	void Update ()
	{
		CheckPauseState ();

		CheckParticleEngines ();
	}

	void FixedUpdate () 
	{
		if (isPaused == false) 
		{
			if (playerActions.Shoot.Value > 0.1f) 
			{
				Shoot ();
			}

			MovePlayer ();
		}

		ClampVelocity ();
	}

	void LateUpdate ()
	{
		if (isPaused == false)
		{
			transform.Rotate (Vector3.up * playerActions.Look.Value.x * Sensitivity.x);
			transform.Rotate (Vector3.left * playerActions.Look.Value.y * Sensitivity.y);
		}
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
		playerActions.MoveUp.AddDefaultBinding (InputControlType.Action1);

		playerActions.MoveDown.AddDefaultBinding (Key.LeftControl);
		playerActions.MoveDown.AddDefaultBinding (InputControlType.Action4);

		playerActions.RollLeft.AddDefaultBinding (Key.Q);
		playerActions.RollLeft.AddDefaultBinding (InputControlType.LeftBumper);

		playerActions.RollRight.AddDefaultBinding (Key.E);
		playerActions.RollRight.AddDefaultBinding (InputControlType.RightBumper);

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

		playerActions.Pause.AddDefaultBinding (Key.Escape);
		playerActions.Pause.AddDefaultBinding (InputControlType.Command);
	}

	void MovePlayer ()
	{
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

	void CheckPauseState ()
	{
		if (playerActions.Pause.WasPressed) 
		{
			isPaused = !isPaused;

			if (isPaused == true) 
			{
				gameControllerScript.TargetTimeScale = 0;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}

			if (isPaused == false) 
			{
				gameControllerScript.TargetTimeScale = 1;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}

	void CheckParticleEngines ()
	{
		var EngineTopRearAEmission = EngineTopRearA.emission;
		var EngineTopRearARateOverTime = EngineTopRearAEmission.rateOverTime;

		EngineTopRearARateOverTime.constant = 
			MaxEngineEmissionRate *
		(playerActions.LookDown.Value + playerActions.MoveDown.Value);
		EngineTopRearAEmission.rateOverTime = EngineTopRearARateOverTime;

		var EngineTopRearBEmission = EngineTopRearB.emission;
		var EngineTopRearBRateOverTime = EngineTopRearBEmission.rateOverTime;

		EngineTopRearBRateOverTime.constant = 
			MaxEngineEmissionRate *
			(playerActions.LookDown.Value + playerActions.MoveDown.Value);
		EngineTopRearBEmission.rateOverTime = EngineTopRearBRateOverTime;
	}
}
