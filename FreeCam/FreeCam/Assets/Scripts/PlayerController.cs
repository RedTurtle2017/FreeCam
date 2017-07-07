using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InControl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	public GameController gameControllerScript;
	public int DeviceID = 1;

	[Header ("Health")]
	public float CurrentHealth = 100;
	public float TargetHealth = 100;
	public float StartingHealth = 100;
	public float HealthVel;
	public float HealthSmoothTime;
	public Slider HealthSliderA;
	public Slider HealthSliderB;
	public TextMeshProUGUI HealthText;

	[Header ("Lives")]
	public int LivesLeft = 3;
	public int StartingLives = 3;

	public float ObstacleDamage = 2;

	[Header ("Pause")]
	public bool isPaused;

	[Header ("Movement")]
	public bool UseKeyboardControls = true;
	public Rigidbody rb;
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

	[Header ("Death")]
	public bool Died;
	public GameObject MeshObject;
	public ParticleSystem PlayerExplosion;
	public SmoothFollowOrig CameraPivotFollowScript;
	public SmoothDampAngle CameraPivotSmoothDampAngleScript;

	[Header ("Respawn")]
	public Transform[] SpawnPoints;

	public PlayerActions playerActions;

	void Start () 
	{
		CreateBindings ();
		SetStartCursorState ();
		SetStartHealth ();
		SetStartLives ();
		CameraPivotFollowScript.offset = Vector3.zero;
	}

	void Update ()
	{
		CheckPauseState ();
		CheckHealthAmount ();
		CheckParticleEngines ();

		if (Died == false && CurrentHealth <= 0) 
		{
			Invoke ("ExplodePlayer", Random.Range (2, 3));
			Died = true;
		}

		HealthText.text = "" + Mathf.Clamp(Mathf.Round (CurrentHealth), 0, 100);
	}

	void FixedUpdate () 
	{
		if (isPaused == false) 
		{
			if (playerActions.Shoot.Value > 0.1f && CurrentHealth > 0) 
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

	void SetStartCursorState ()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void SetStartHealth ()
	{
		CurrentHealth = 1;
		TargetHealth = StartingHealth;
	}

	void MovePlayer ()
	{
		if (CurrentHealth > 0)
		{
			// Moving
			rb.AddRelativeForce 
			(
				playerActions.Move.Value.x * Force.x, 
				playerActions.Elevate.Value * Force.y, 
				playerActions.Move.Value.y * Force.z, ForceMode.Force
			);

			// Rolling
			rb.AddRelativeTorque
			(
				0, 
				0, 
				playerActions.Roll.Value * -4 * Sensitivity.x, ForceMode.Acceleration
			);

			// Looking
			if (UseKeyboardControls == false) 
			{
				transform.Rotate (Vector3.up * playerActions.Look.Value.x * Sensitivity.x);
				transform.Rotate (Vector3.left * playerActions.Look.Value.y * Sensitivity.y);
			}

			if (UseKeyboardControls == true) 
			{
				transform.Rotate (Vector3.up * playerActions.Look.Value.x * Sensitivity.x * 4);
				transform.Rotate (Vector3.left * playerActions.Look.Value.y * Sensitivity.y * 4);
			}
		}

		if (CurrentHealth <= 0) 
		{
			rb.angularDrag = 0;
			rb.drag = 0;
			CameraPivotFollowScript.offset = new Vector3 (0, 0, -15);
			CameraPivotFollowScript.transform.LookAt (gameObject.transform);
			CameraPivotSmoothDampAngleScript.enabled = false;
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

	void CheckHealthAmount ()
	{
		CurrentHealth = Mathf.SmoothDamp (CurrentHealth, TargetHealth, ref HealthVel, HealthSmoothTime * Time.deltaTime);

		HealthSliderA.value = Mathf.Clamp (CurrentHealth, 0, StartingHealth);
		HealthSliderB.value = Mathf.Clamp (CurrentHealth, 0, StartingHealth);
	}

	void CheckParticleEngines ()
	{
		if (CurrentHealth > 0) 
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
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
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
			EngineLeftSideFrontAEmission.rateOverTime = EngineLeftSideFrontARateOverTime;

			var EngineLeftSideFrontBEmission = EngineLeftSideFrontB.emission;
			var EngineLeftSideFrontBRateOverTime = EngineLeftSideFrontBEmission.rateOverTime;

			EngineLeftSideFrontBRateOverTime.constant = 
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
			EngineLeftSideFrontBEmission.rateOverTime = EngineLeftSideFrontBRateOverTime;

			// Left side rear
			var EngineLeftSideRearAEmission = EngineLeftSideRearA.emission;
			var EngineLeftSideRearARateOverTime = EngineLeftSideRearAEmission.rateOverTime;

			EngineLeftSideRearARateOverTime.constant = 
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
			EngineLeftSideRearAEmission.rateOverTime = EngineLeftSideRearARateOverTime;

			var EngineLeftSideRearBEmission = EngineLeftSideRearB.emission;
			var EngineLeftSideRearBRateOverTime = EngineLeftSideRearBEmission.rateOverTime;

			EngineLeftSideRearBRateOverTime.constant = 
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
			EngineLeftSideRearBEmission.rateOverTime = EngineLeftSideRearBRateOverTime;

			// Right side front
			var EngineRightSideFrontAEmission = EngineRightSideFrontA.emission;
			var EngineRightSideFrontARateOverTime = EngineRightSideFrontAEmission.rateOverTime;

			EngineRightSideFrontARateOverTime.constant = 
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
			EngineRightSideFrontAEmission.rateOverTime = EngineRightSideFrontARateOverTime;

			var EngineRightSideFrontBEmission = EngineRightSideFrontB.emission;
			var EngineRightSideFrontBRateOverTime = EngineRightSideFrontBEmission.rateOverTime;

			EngineRightSideFrontBRateOverTime.constant = 
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
			EngineRightSideFrontBEmission.rateOverTime = EngineRightSideFrontBRateOverTime;

			// Right side rear
			var EngineRightSideRearAEmission = EngineRightSideRearA.emission;
			var EngineRightSideRearARateOverTime = EngineRightSideRearAEmission.rateOverTime;

			EngineRightSideRearARateOverTime.constant = 
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollLeft.Value), 0, MaxEngineEmissionRate);
			EngineRightSideRearAEmission.rateOverTime = EngineRightSideRearARateOverTime;

			var EngineRightSideRearBEmission = EngineRightSideRearB.emission;
			var EngineRightSideRearBRateOverTime = EngineRightSideRearBEmission.rateOverTime;

			EngineRightSideRearBRateOverTime.constant = 
			Mathf.Clamp (MaxEngineEmissionRate *
			(playerActions.RollRight.Value), 0, MaxEngineEmissionRate);
			EngineRightSideRearBEmission.rateOverTime = EngineRightSideRearBRateOverTime;
		}

		if (CurrentHealth <= 0) 
		{
			
		}
	}

	void OnCollisionEnter (Collision col)
	{
		//TargetHealth -= transform.InverseTransformDirection (rb.velocity).magnitude * ObstacleDamage;

		if (col.collider.tag == "Obstacle") 
		{
			if (TargetHealth > 0) 
			{
				TargetHealth -= transform.InverseTransformDirection (rb.velocity).magnitude * ObstacleDamage;
			}

			if (TargetHealth <= 0) 
			{
				rb.AddRelativeTorque (
					new Vector3 (
						Random.Range (-50, 50),
						Random.Range (-10, 10),
						Random.Range (-250, 250)
						), 
					ForceMode.VelocityChange);
			}
		}
	}

	void SetStartLives ()
	{
		LivesLeft = StartingLives;
	}

	void ExplodePlayer ()
	{
		MeshObject.SetActive (false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		PlayerExplosion.Play ();

		if (LivesLeft > 0) 
		{
			StartCoroutine (RespawnPlayer ());
		}
	}

	IEnumerator RespawnPlayer ()
	{
		yield return new WaitForSeconds (3);
		CurrentHealth = 1;
		LivesLeft -= 1;
		rb.angularDrag = 2.0f;
		rb.drag = 0.5f;
		gameObject.transform.position = SpawnPoints [Random.Range (0, SpawnPoints.Length)].position;
		gameObject.transform.rotation = SpawnPoints [Random.Range (0, SpawnPoints.Length)].rotation;
		CameraPivotFollowScript.offset = new Vector3 (0, 0, 0);
		TargetHealth = StartingHealth;
		CameraPivotSmoothDampAngleScript.enabled = true;
		yield return new WaitForSeconds (0.5f);
		MeshObject.SetActive (true);
		Died = false;
	}
}
