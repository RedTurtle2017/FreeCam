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
		// Moving
		rb.AddRelativeForce 
		(
			playerActions.Move.Value.x * Force.x, 
			playerActions.Elevate.Value * Force.y, 
			playerActions.Move.Value.y * Force.z
		);

		// Rolling
		rb.AddRelativeTorque
		(
			0, 
			0, 
			playerActions.Roll.Value * -4 * Sensitivity.x
		);

		// Looking
		if (UseKeyboardControls == false) 
		{
			transform.Rotate (Vector3.up * playerActions.Look.Value.x * Sensitivity.x);
			transform.Rotate (Vector3.left * playerActions.Look.Value.y * Sensitivity.y);
		}

		if (UseKeyboardControls == true) 
		{
			transform.Rotate (Vector3.up * playerActions.Look.Value.x * Sensitivity.x * 3);
			transform.Rotate (Vector3.left * playerActions.Look.Value.y * Sensitivity.y * 3);
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
		// Main engine
		var MainEngineEmission = MainEngine.emission;
		var MainEngineRateOverTime = MainEngineEmission.rateOverTime;

		MainEngineRateOverTime.constant = Mathf.Clamp 
			(
				transform.InverseTransformDirection (rb.velocity).z, 
				0, MaxEngineEmissionRate * 2
			);
		MainEngineEmission.rateOverTime = MainEngineRateOverTime;

		// Top Front
		var EngineTopFrontAEmission = EngineTopFrontA.emission;
		var EngineTopFrontARateOverTime = EngineTopFrontAEmission.rateOverTime;

		EngineTopFrontARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookDown.Value + 
					playerActions.LookRight.Value + 
					playerActions.MoveDown.Value + 
					playerActions.MoveRight.Value
				), 0, MaxEngineEmissionRate);
		EngineTopFrontAEmission.rateOverTime = EngineTopFrontARateOverTime;

		var EngineTopFrontBEmission = EngineTopFrontB.emission;
		var EngineTopFrontBRateOverTime = EngineTopFrontBEmission.rateOverTime;

		EngineTopFrontBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookDown.Value + 
					playerActions.LookLeft.Value + 
					playerActions.MoveDown.Value + 
					playerActions.MoveLeft.Value
				), 0, MaxEngineEmissionRate);
		EngineTopFrontBEmission.rateOverTime = EngineTopFrontBRateOverTime;

		// Top Rear
		var EngineTopRearAEmission = EngineTopRearA.emission;
		var EngineTopRearARateOverTime = EngineTopRearAEmission.rateOverTime;

		EngineTopRearARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookUp.Value + 
					playerActions.LookLeft.Value + 
					playerActions.MoveDown.Value +
					playerActions.MoveRight.Value
				), 0, MaxEngineEmissionRate);
		EngineTopRearAEmission.rateOverTime = EngineTopRearARateOverTime;

		var EngineTopRearBEmission = EngineTopRearB.emission;
		var EngineTopRearBRateOverTime = EngineTopRearBEmission.rateOverTime;

		EngineTopRearBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookUp.Value + 
					playerActions.LookRight.Value + 
					playerActions.MoveDown.Value + 
					playerActions.MoveLeft.Value
				), 0, MaxEngineEmissionRate);
		EngineTopRearBEmission.rateOverTime = EngineTopRearBRateOverTime;

		// Bottom Front
		var EngineBottomFrontAEmission = EngineBottomFrontA.emission;
		var EngineBottomFrontARateOverTime = EngineBottomFrontAEmission.rateOverTime;

		EngineBottomFrontARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookUp.Value + 
					playerActions.LookRight.Value + 
					playerActions.MoveUp.Value + 
					playerActions.MoveRight.Value
				), 0, MaxEngineEmissionRate);
		EngineBottomFrontAEmission.rateOverTime = EngineBottomFrontARateOverTime;

		var EngineBottomFrontBEmission = EngineBottomFrontB.emission;
		var EngineBottomFrontBRateOverTime = EngineBottomFrontBEmission.rateOverTime;

		EngineBottomFrontBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookUp.Value + 
					playerActions.LookLeft.Value + 
					playerActions.MoveUp.Value +
					playerActions.MoveLeft.Value
				), 0, MaxEngineEmissionRate);
		EngineBottomFrontBEmission.rateOverTime = EngineBottomFrontBRateOverTime;

		// Bottom rear
		var EngineBottomRearAEmission = EngineBottomRearA.emission;
		var EngineBottomRearARateOverTime = EngineBottomFrontAEmission.rateOverTime;

		EngineBottomRearARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookDown.Value + 
					playerActions.LookLeft.Value + 
					playerActions.MoveUp.Value + 
					playerActions.MoveRight.Value
				), 0, MaxEngineEmissionRate);
		EngineBottomRearAEmission.rateOverTime = EngineBottomRearARateOverTime;

		var EngineBottomRearBEmission = EngineBottomRearB.emission;
		var EngineBottomRearBRateOverTime = EngineBottomRearBEmission.rateOverTime;

		EngineBottomRearBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(
					playerActions.LookDown.Value + 
					playerActions.LookRight.Value + 
					playerActions.MoveUp.Value + 
					playerActions.MoveLeft.Value
				), 0, MaxEngineEmissionRate);
		EngineBottomRearBEmission.rateOverTime = EngineBottomRearBRateOverTime;

		// Left side front
		var EngineLeftSideFrontAEmission = EngineLeftSideFrontA.emission;
		var EngineLeftSideFrontARateOverTime = EngineLeftSideFrontAEmission.rateOverTime;

		EngineLeftSideFrontARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
		EngineLeftSideFrontAEmission.rateOverTime = EngineLeftSideFrontARateOverTime;

		var EngineLeftSideFrontBEmission = EngineLeftSideFrontB.emission;
		var EngineLeftSideFrontBRateOverTime = EngineLeftSideFrontBEmission.rateOverTime;

		EngineLeftSideFrontBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
		EngineLeftSideFrontBEmission.rateOverTime = EngineLeftSideFrontBRateOverTime;

		// Left side rear
		var EngineLeftSideRearAEmission = EngineLeftSideRearA.emission;
		var EngineLeftSideRearARateOverTime = EngineLeftSideRearAEmission.rateOverTime;

		EngineLeftSideRearARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
		EngineLeftSideRearAEmission.rateOverTime = EngineLeftSideRearARateOverTime;

		var EngineLeftSideRearBEmission = EngineLeftSideRearB.emission;
		var EngineLeftSideRearBRateOverTime = EngineLeftSideRearBEmission.rateOverTime;

		EngineLeftSideRearBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
		EngineLeftSideRearBEmission.rateOverTime = EngineLeftSideRearBRateOverTime;

		// Right side front
		var EngineRightSideFrontAEmission = EngineRightSideFrontA.emission;
		var EngineRightSideFrontARateOverTime = EngineRightSideFrontAEmission.rateOverTime;

		EngineRightSideFrontARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
		EngineRightSideFrontAEmission.rateOverTime = EngineRightSideFrontARateOverTime;

		var EngineRightSideFrontBEmission = EngineRightSideFrontB.emission;
		var EngineRightSideFrontBRateOverTime = EngineRightSideFrontBEmission.rateOverTime;

		EngineRightSideFrontBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
		EngineRightSideFrontBEmission.rateOverTime = EngineRightSideFrontBRateOverTime;

		// Right side rear
		var EngineRightSideRearAEmission = EngineRightSideRearA.emission;
		var EngineRightSideRearARateOverTime = EngineRightSideRearAEmission.rateOverTime;

		EngineRightSideRearARateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
		EngineRightSideRearAEmission.rateOverTime = EngineRightSideRearARateOverTime;

		var EngineRightSideRearBEmission = EngineRightSideRearB.emission;
		var EngineRightSideRearBRateOverTime = EngineRightSideRearBEmission.rateOverTime;

		EngineRightSideRearBRateOverTime.constant = 
			Mathf.Clamp(MaxEngineEmissionRate *
				(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
		EngineRightSideRearBEmission.rateOverTime = EngineRightSideRearBRateOverTime;
	}
}
