using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InControl;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Utility;

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
	public Slider HealthSliderSmoothA;
	public Slider HealthSliderSmoothB;
	public ParticleSystem HitParticles;
	public AudioSource LowHealthSound;

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
	public float VelocityMagnitude;
	public Vector3 Force;
	public Transform CameraPivot;
	public Vector2 Sensitivity;
	public float RollSpeed = 2;
	public float LookSmoothing;
	private float LookSmoothVel;
	public Transform PlayerRotation;
	public Rigidbody PlayerRotationRb;
	public Image SpeedOmeterImage;
	public Image SpeedOmeterImageSmooth;
	private float SpeedOmeterVel;
	public TextMeshProUGUI SpeedText;
	public CameraShake CamShakeScript;

	[Header ("Engines")]
	public float MaxEngineEmissionRate;
	public ParticleSystem MainEngine;
	public AudioSource MainEngineAudio;
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
	public Transform ShotSpawnL;
	public Transform ShotSpawnM;
	public Transform ShotSpawnR;
	public bool usePrimaryBarrel;
	public bool Overheated;

	public int WeaponId;
	public int MaxWeapons = 8;

	public GameObject[] Weapon;

	public bool canUseWeapon1;
	public bool canUseWeapon2;
	public bool canUseWeapon3;
	public bool canUseWeapon4;
	public bool canUseWeapon5;
	public bool canUseWeapon6;
	public bool canUseWeapon7;
	public bool canUseWeapon8;

	public GameObject WeaponIcon1;
	public GameObject WeaponIcon2;
	public GameObject WeaponIcon3;
	public GameObject WeaponIcon4;
	public GameObject WeaponIcon5;
	public GameObject WeaponIcon6;
	public GameObject WeaponIcon7;
	public GameObject WeaponIcon8;

	public RawImage WeaponWheelIcon1;
	public RawImage WeaponWheelIcon2;
	public RawImage WeaponWheelIcon3;
	public RawImage WeaponWheelIcon4;
	public RawImage WeaponWheelIcon5;
	public RawImage WeaponWheelIcon6;
	public RawImage WeaponWheelIcon7;
	public RawImage WeaponWheelIcon8;

	public Animator WeaponWheel;

	public float CurrentFireRate = 0.125f;

	public float FireRateWeapon1 = 0.125f;
	public float FireRateWeapon2;
	public float FireRateWeapon3;
	public float FireRateWeapon4;
	public float FireRateWeapon5;
	public float FireRateWeapon6;
	public float FireRateWeapon7;
	public float FireRateWeapon8;

	private float nextFire;
	public GameObject CrosshairObject;

	public AudioSource HitSound;

	[Header ("Ammo")]
	public Image CoolDownImage;
	public float CooldownTime;
	public float[] WeaponCooldownTime;
	public float[] AddCooldownTime;
	public AudioSource CooldownWarning;
	public float CooldownThreshold = 0.8f;
	public ParticleSystem CooldownWarningParticles;

	[Header ("Bounds")]
	public bool isOutsideBounds;
	public Collider Bounds;
	public float BoundsDamage = 9;
	public AudioSource BoundsDamageSound;

	public Animator BoundsHurtUI;

	[Header ("Death")]
	public bool Died;
	public GameObject MeshObject;
	public ParticleSystem PlayerExplosion;
	public SmoothFollowOrig CameraPivotFollowScript;
	public SmoothDampAngle CameraPivotSmoothDampAngleScript;
	public AudioMixer Mixer;
	public float TargetHighFreq;
	public float HighPassFreqSmoothTime;
	public float DeadTimeScale = 0.1f;
	public float DeadSustain;
	public float MaxDeadTime = 0.5f;
	public Transform DeadPlayerFollow;
	public AudioSource PlayerExplosionSound;
	public Animator TauntText;
	public TextMeshProUGUI TauntTextString;
	public string[] Taunts;
	public GameObject RespawnUI;
	public GameObject MainUI;

	public Animator[] LifeAnims;
	public float DeathTimerDuration = 10.0f;
	public float DeathTimerRemaining;

	[Header ("Respawn")]
	public GameObject[] SpawnPoints;
	public Collider PlayerCollider;
	public ParticleSystem RespawnParticles;
	public TextMeshProUGUI TimedRespawn;

	public PlayerActions playerActions;

	[Header ("Visuals")]
	public PostProcessingProfile MainPostProcess;
	public PostProcessingProfile OverPostProcess;

	[Header ("Audio")]
	public AudioSource SoundtrackBase;
	public AudioSource SoundtrackLayerOne;
	public AudioSource SoundtrackLayerTwo;
	public AudioSource SoundtrackLayerThree;

	[Header ("Misc")]
	public bool isMenuPlayer;

	[Header ("AI")]
	public bool isAI;
	public bool LocatedPlayer;
	public float PlayersCheckingFrequency;
	public float PlayersCheckingRange;
	public float MinSpeed;
	public float LookSpeed = 1;
	public GameObject PlayerOne;

	void Start () 
	{
		if (isAI == false)
		{
			CreateBindings ();
	
			if (isMenuPlayer == false)
			{
				SetStartCursorState ();
				SetStartHealth ();
				SetStartLives ();
				CameraPivotFollowScript.target = gameObject.transform;
			}
		}

		if (isAI == true) 
		{
			InvokeRepeating ("FindPlayers", 3, PlayersCheckingFrequency);
		}
	}

	void Update ()
	{
		if (isAI == false)
		{
			if (isMenuPlayer == false) 
			{
				CheckPauseState ();
				CheckHealthAmount ();
				CheckParticleEngines ();
				CheckWeaponId ();
				CheckSpeed ();
				CheckAudio ();
				CheckCooldown ();
				CheckVisuals ();
			}
		}

		if (isAI == true) 
		{
			if (LocatedPlayer == true) 
			{
				HuntDownLocatedPlayer ();
			}

			if (LocatedPlayer == false) 
			{
				
			}
		}
	}

	void FixedUpdate () 
	{
		if (isAI == false)
		{
			if (isMenuPlayer == false)
			{
				if (isPaused == false)
				{
					if (playerActions.Shoot.Value > 0.1f) 
					{
						if (CurrentHealth > 0)
						{
							Shoot ();
						}

						if (DeathTimerRemaining > 0) 
						{
							if (RespawnUI.activeSelf == true) 
							{
								RespawnPlayerNow ();
							}
						}
					}

					if (Overheated == false) 
					{
					
					}

					if (Overheated == true)
					{
						CooldownTime -= AddCooldownTime [WeaponId - 1] * Time.deltaTime;

						if (CoolDownImage.fillAmount < 0.75f) 
						{
							Overheated = false;
						}
					}

					MovePlayer ();
				}

				ClampVelocity ();
			}
		}
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

		playerActions.NextWeapon.AddDefaultBinding (Mouse.PositiveScrollWheel);
		playerActions.NextWeapon.AddDefaultBinding (InputControlType.Action2);

		playerActions.PreviousWeapon.AddDefaultBinding (Mouse.NegativeScrollWheel);
		playerActions.PreviousWeapon.AddDefaultBinding (InputControlType.Action3);

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
		HealthSliderA.value = 0;
		HealthSliderB.value = 0;
		HealthSliderSmoothA.value = 0;
		HealthSliderSmoothB.value = 0;
	}

	void MovePlayer ()
	{
		if (CurrentHealth > 0 && TargetHealth > 0)
		{
			// Moving
			if (rb.velocity.magnitude < (0.7567f * MaxVelocity))
			{
				rb.AddRelativeForce 
			(
					Mathf.Clamp (playerActions.Move.Value.x, -1, 1) * Force.x, 
					Mathf.Clamp (playerActions.Elevate.Value, -1, 1) * Force.y, 
					Mathf.Clamp (playerActions.Move.Value.y, -1, 1) * Force.z, ForceMode.Force
				);
			}

			// Rolling
			PlayerRotationRb.AddRelativeTorque
			(
				0, 
				0, 
				playerActions.Roll.Value * RollSpeed * -4, ForceMode.Force
			);

			// Looking
			if (UseKeyboardControls == false) 
			{
				Vector3 RotateVertical = Vector3.up * playerActions.Look.Value.x * Sensitivity.x * 1.5f;
				Vector3 RotateHorizontal = Vector3.left * playerActions.Look.Value.y * Sensitivity.y * 1.5f;

				PlayerRotation.transform.Rotate (RotateVertical);
				PlayerRotation.transform.Rotate (RotateHorizontal);	
			}

			if (UseKeyboardControls == true) 
			{
				Vector3 RotateVertical = Vector3.up * playerActions.Look.Value.x * Sensitivity.x * 4;
				Vector3 RotateHorizontal = Vector3.left * playerActions.Look.Value.y * Sensitivity.y * 4;

				PlayerRotation.transform.Rotate (RotateVertical);
				PlayerRotation.transform.Rotate (RotateHorizontal);			
			}
		}

		if (CurrentHealth <= 0) 
		{
			if (Died == true && RespawnUI.activeSelf == true) 
			{
				rb.angularDrag = 0;
				rb.drag = 0;

				CameraPivotFollowScript.target = DeadPlayerFollow.transform;
				CameraPivotFollowScript.SMOOTH_TIME = 0.7f;
			}
		}
	}

	void ClampVelocity ()
	{		
		rb.velocity = Vector3.ClampMagnitude (rb.velocity, MaxVelocity);
		VelocityMagnitude = rb.velocity.magnitude;
	}
		
	void Shoot ()
	{
		if (Overheated == false && Died == false) 
		{
			if (Time.time > nextFire)
			{
				if (WeaponId == 1) 
				{
					Shot = Weapon [0];
					CurrentFireRate = FireRateWeapon1;
					usePrimaryBarrel = !usePrimaryBarrel;

					if (usePrimaryBarrel)
					{
						Instantiate (Shot, ShotSpawnL.position, ShotSpawnL.rotation);
					}

					if (!usePrimaryBarrel)
					{
						Instantiate (Shot, ShotSpawnR.position, ShotSpawnR.rotation);
					}
				}

				if (WeaponId == 2) 
				{
					Shot = Weapon [1];
					CurrentFireRate = FireRateWeapon2;
					Instantiate (Shot, ShotSpawnM.position, ShotSpawnM.rotation);
				}

				if (CoolDownImage.fillAmount < 0.99f) 
				{
					CooldownTime += 0.5f * AddCooldownTime [WeaponId - 1];
					nextFire = Time.time + CurrentFireRate;
				}

				if (CoolDownImage.fillAmount > 0.99f) 
				{
					nextFire = Time.time + CurrentFireRate * 2;
					Overheated = true;
				}
			}
		}

		if (Overheated == true) 
		{

		}
	}

	void CheckCooldown ()
	{
		if (isPaused == false && CooldownTime > 0) 
		{
			CoolDownImage.fillAmount = CooldownTime;
			CooldownTime -= Time.deltaTime * WeaponCooldownTime[WeaponId - 1];
			CoolDownImage.color = new Color (1, 0, 0, Mathf.Clamp(CooldownTime, 0, 0.7f));
			CoolDownImage.rectTransform.sizeDelta = new Vector2 (900 * CooldownTime, 900 * CooldownTime);
		}

		if (CooldownTime > CooldownThreshold) 
		{
			if (CooldownWarning.isPlaying == false && isPaused == false) 
			{
				CooldownWarning.Play ();
			}

			if (CooldownWarningParticles.isPlaying == false && isPaused == false) 
			{
				CooldownWarningParticles.Play ();
			}
		}

		if (CooldownTime <= CooldownThreshold) 
		{
			if (CooldownWarning.isPlaying == true)
			{
				CooldownWarning.Stop ();
			}

			if (CooldownWarningParticles.isPlaying == true) 
			{
				CooldownWarningParticles.Stop (false, ParticleSystemStopBehavior.StopEmitting);
			}
		}
	}

	void CheckSpeed ()
	{
		if (Died == false) 
		{
			SpeedOmeterImage.fillAmount = (rb.velocity.magnitude * 0.75f) / MaxVelocity;
			SpeedOmeterImageSmooth.fillAmount = Mathf.SmoothDamp (SpeedOmeterImageSmooth.fillAmount, SpeedOmeterImage.fillAmount, ref SpeedOmeterVel, 16 * Time.deltaTime);
			SpeedText.text = "" + Mathf.Round (rb.velocity.magnitude * 2);
		}
	}

	void CheckAudio ()
	{
		float HighPassCuttoffFreqValue;

		bool HighPassResult = Mixer.GetFloat ("HighPassCutoffFreq", out HighPassCuttoffFreqValue);

		if (HighPassResult) 
		{
			Mixer.SetFloat ("HighPassCutoffFreq", Mathf.Lerp (HighPassCuttoffFreqValue, TargetHighFreq, HighPassFreqSmoothTime * Time.deltaTime));
		}

		if (Died == false) 
		{
			if (CurrentHealth <= 0)
			{
				Invoke ("ExplodePlayer", Random.Range (0.5f, 1));
				Died = true;
			}
		}

		if (Died == true)
		{
			TargetHighFreq = 2500;
		}

		MainEngineAudio.volume = (rb.velocity.magnitude * 0.25f) / MaxVelocity;
	}

	void CheckPauseState ()
	{
		if (playerActions.Pause.WasPressed) 
		{
			isPaused = !isPaused;

			if (isPaused == true) 
			{
				gameControllerScript.TargetTimeScale = 0;

			}

			if (isPaused == false) 
			{
				gameControllerScript.TargetTimeScale = 1;
			}
		}

		if (Time.timeScale < 0.01f) 
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			SoundtrackBase.volume = 0;
			SoundtrackLayerOne.volume = 0;
			SoundtrackLayerTwo.volume = 0;
			SoundtrackLayerThree.volume = 0;
		}

		if (Time.timeScale > 0.01f && SoundtrackBase.volume != 0.5f) 
		{
			SoundtrackBase.volume = 0.2f;
			SoundtrackLayerOne.volume = 0.2f;
			SoundtrackLayerTwo.volume = 0.2f;
			SoundtrackLayerThree.volume = 0.2f;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void CheckVisuals ()
	{
		var MotionBlurSettings = MainPostProcess.motionBlur.settings;
		MotionBlurSettings.shutterAngle = Mathf.Clamp (4 * rb.velocity.magnitude - 300, 0, 300);

		MainPostProcess.motionBlur.settings = MotionBlurSettings;

		var ChromaticAbberationSettings = MainPostProcess.chromaticAberration.settings;
		ChromaticAbberationSettings.intensity = Mathf.Clamp ((2 * rb.velocity.magnitude) / (MaxVelocity + 500f), 0, 0.8f);

		MainPostProcess.chromaticAberration.settings = ChromaticAbberationSettings;

		Camera.main.fieldOfView = Mathf.Clamp (0.2666666f * transform.InverseTransformDirection (rb.velocity).z + 50, 50, 90);
	}

	void CheckHealthAmount ()
	{
		CurrentHealth = Mathf.SmoothDamp (CurrentHealth, TargetHealth, ref HealthVel, HealthSmoothTime * Time.deltaTime);

		// Clamps everything.
		HealthSliderA.value = Mathf.Clamp (TargetHealth, 0, StartingHealth);
		HealthSliderB.value = Mathf.Clamp (TargetHealth, 0, StartingHealth);
		HealthSliderSmoothA.value = Mathf.Clamp (CurrentHealth, 0, StartingHealth);
		HealthSliderSmoothB.value = Mathf.Clamp (CurrentHealth, 0, StartingHealth);
		CurrentHealth = Mathf.Clamp (CurrentHealth, 0, 100);
		TargetHealth = Mathf.Clamp (TargetHealth, 0, 100);

		if (TargetHealth < 25 && TargetHealth > 0) 
		{
			if (BoundsHurtUI.GetCurrentAnimatorStateInfo (0).IsName ("RedVignettePulse") == false) 
			{
				BoundsHurtUI.Play ("RedVignettePulse");
			}

			if (LowHealthSound.isPlaying == false) 
			{
				LowHealthSound.Play ();
				LowHealthSound.volume = 0.3f;
			}
		}

		if (TargetHealth >= 25) 
		{
			if (BoundsHurtUI.GetCurrentAnimatorStateInfo (0).IsName ("RedVignettePulse") == true) 
			{
				BoundsHurtUI.StopPlayback ();
			}

			if (LowHealthSound.isPlaying == true) 
			{
				LowHealthSound.Stop ();
			}
		}

		if (RespawnUI.activeSelf == true) 
		{
			if (DeathTimerRemaining > 0) 
			{
				DeathTimerRemaining -= Time.deltaTime;
				TimedRespawn.text = "Respawning in " + Mathf.RoundToInt (DeathTimerRemaining) + "...";
			}

			if (DeathTimerRemaining <= 0) 
			{
				RespawnPlayerNow ();
			}
		}
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

	void HurtHealth ()
	{
		TargetHealth -= BoundsDamage;
		// Do UI and sound cool stuff;
		TargetHighFreq = 3600;
		BoundsHurtUI.Play ("RedVignettePulseOnce");
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Bounds") 
		{
			isOutsideBounds = false;
			CancelInvoke ("HurtHealth");
			TargetHighFreq = 0;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Bounds") 
		{
			isOutsideBounds = true;
			InvokeRepeating ("HurtHealth", 0.5f, 1);
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.collider.tag == "Bullet") 
		{
			if (TargetHealth > 0) 
			{
				TargetHealth -= col.collider.GetComponent<BulletScript>().damage;

				Instantiate (HitParticles, col.contacts [0].point, Quaternion.identity);
				HitSound.Play ();
				HitSound.pitch = Random.Range (0.75f, 1.25f);
				CamShakeScript.shakeTimeRemaining = CamShakeScript.shakeDuration;

				TargetHighFreq = 3600;
				Invoke ("RestoreHighPassFreq", 0.5f);
			}

			if (TargetHealth <= 0) 
			{
				rb.AddRelativeTorque ( new Vector3 (
					Random.Range (-50, 50),
					Random.Range (-10, 10),
					Random.Range (-250, 250)
				), 
					ForceMode.VelocityChange);

				StartCoroutine (DeadTimeScaleSet ());

				if (HitSound.isPlaying == false)
				{
					HitSound.Play ();
				}

				if (isAI == true) 
				{
					Destroy (gameObject);
				}
			}
		}

		if (col.collider.tag == "Obstacle") 
		{
			if (TargetHealth > 0) 
			{
				//TargetHealth -= transform.InverseTransformDirection (rb.velocity).z * ObstacleDamage;
				TargetHealth -= rb.velocity.magnitude * ObstacleDamage;
				//rb.velocity = Vector3.zero;

				Instantiate (HitParticles, col.contacts [0].point, Quaternion.identity);
				HitSound.Play ();
				HitSound.pitch = rb.velocity.magnitude / MaxVelocity;
				CamShakeScript.shakeTimeRemaining = CamShakeScript.shakeDuration;

				TargetHighFreq = 3600;
				Invoke ("RestoreHighPassFreq", 0.5f);
			}

			if (TargetHealth <= 0) 
			{
				rb.AddRelativeTorque ( new Vector3 (
					Random.Range (-50, 50),
					Random.Range (-10, 10),
					Random.Range (-250, 250)
					), 
					ForceMode.VelocityChange);

				StartCoroutine (DeadTimeScaleSet ());

				if (HitSound.isPlaying == false)
				{
					HitSound.Play ();
				}
			}
		}
	}

	void RestoreHighPassFreq ()
	{
		TargetHighFreq = 0;
	}

	IEnumerator DeadTimeScaleSet ()
	{
		PlayerCollider.enabled = false;
		gameControllerScript.TargetTimeScale = DeadTimeScale;
		LowHealthSound.pitch = 1.5f;
		LowHealthSound.volume = 0.3f;
		yield return new WaitForSecondsRealtime (MaxDeadTime);
		gameControllerScript.TargetTimeScale = 1;
		LowHealthSound.volume = 0;
		LowHealthSound.Stop ();
	}

	void SetStartLives ()
	{
		LivesLeft = StartingLives;
	}

	void ExplodePlayer ()
	{
		CrosshairObject.SetActive (false);
		Died = true;
		MeshObject.SetActive (false);
		GetComponent<ConstantForce> ().relativeForce = Vector3.zero;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		PlayerExplosion.Play ();
		PlayerExplosionSound.Stop ();
		PlayerExplosionSound.Play ();
		SpeedOmeterImage.fillAmount = 0;
		SpeedText.text = "" + 0;
		LowHealthSound.pitch = 1f;

		if (LivesLeft != 0) 
		{
			StartCoroutine (RespawnPlayer ());
		}
	}

	IEnumerator RespawnPlayer ()
	{
		rb.velocity = Vector3.zero;
		TauntTextString.text = Taunts [Random.Range (0, Taunts.Length)];
		//LifeAnims [LivesLeft - 1].Play ("LifeExit");
		MainUI.SetActive (false);
		TauntText.Play ("TauntText");
		yield return new WaitForSecondsRealtime (1);
		CurrentHealth = 1;
		//LivesLeft -= 1;
		yield return new WaitForSecondsRealtime (3f);
		RespawnUI.SetActive (true);
		DeathTimerRemaining = DeathTimerDuration;
	}

	public void RespawnPlayerNow ()
	{
		TargetHighFreq = 0;
		rb.angularDrag = 2.0f;
		rb.drag = 0.96f;
		gameObject.transform.position = SpawnPoints [Random.Range (0, SpawnPoints.Length)].transform.position;
		gameObject.transform.rotation = SpawnPoints [Random.Range (0, SpawnPoints.Length)].transform.rotation;
		CameraPivotFollowScript.target = gameObject.transform;
		TargetHealth = StartingHealth;
		RespawnUI.SetActive (false);
		MainUI.SetActive (true);
		CameraPivotFollowScript.SMOOTH_TIME = 0.02f;
		MeshObject.SetActive (true);
		RespawnParticles.Play ();
		StartCoroutine (EnableShooting ());
	}

	IEnumerator EnableShooting ()
	{
		TargetHighFreq = 0;
		yield return new WaitForSeconds (3);
		PlayerCollider.enabled = true;
		Died = false;
		RespawnParticles.Stop (true, ParticleSystemStopBehavior.StopEmitting);
		TargetHighFreq = 0;
		CrosshairObject.SetActive (true);
	}

	void CheckWeaponId ()
	{
		if (isPaused == false)
		{
			if (playerActions.NextWeapon.WasPressed) 
			{
				WeaponId += 1;
				WeaponWheel.Play ("WeaponWheelOn", -1, 0);
				Shot = Weapon [WeaponId - 1];

				if (canUseWeapon1 == false) {
					WeaponIcon1.SetActive (false);
				}

				if (canUseWeapon2 == false) {
					WeaponIcon2.SetActive (false);
				}

				if (canUseWeapon3 == false) {
					WeaponIcon3.SetActive (false);
				}

				if (canUseWeapon4 == false) {
					WeaponIcon4.SetActive (false);
				}

				if (canUseWeapon5 == false) {
					WeaponIcon5.SetActive (false);
				}

				if (canUseWeapon6 == false) {
					WeaponIcon6.SetActive (false);
				}

				if (canUseWeapon7 == false) {
					WeaponIcon7.SetActive (false);
				}

				if (canUseWeapon8 == false) {
					WeaponIcon8.SetActive (false);
				}
				
				if (WeaponId > MaxWeapons) {
					WeaponId = 1;
				}

				if (canUseWeapon1 == true) {
					WeaponIcon1.SetActive (true);
				}

				if (canUseWeapon2 == true) {
					WeaponIcon2.SetActive (true);
				}

				if (canUseWeapon3 == true) {
					WeaponIcon3.SetActive (true);
				}

				if (canUseWeapon4 == true) {
					WeaponIcon4.SetActive (true);
				}

				if (canUseWeapon5 == true) {
					WeaponIcon5.SetActive (true);
				}

				if (canUseWeapon6 == true) {
					WeaponIcon6.SetActive (true);
				}

				if (canUseWeapon7 == true) {
					WeaponIcon7.SetActive (true);
				}

				if (canUseWeapon8 == true) {
					WeaponIcon8.SetActive (true);
				}

				switch (WeaponId) {
				case 1:
					WeaponWheelIcon1.color = Color.white;
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon1;
					break;

				case 2:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = Color.white;
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon2;
					break;

				case 3:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = Color.white;
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon3;
					break;

				case 4:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = Color.white;
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon4;
					break;

				case 5:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = Color.white;
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon5;
					break;

				case 6:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = Color.white;
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon6;
					break;

				case 7:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = Color.white;
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon7;
					break;

				case 8:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = Color.white;
					CurrentFireRate = FireRateWeapon8;
					break;
				}
			}

			if (playerActions.PreviousWeapon.WasPressed) 
			{
				if (WeaponId > 0) 
				{
					WeaponId -= 1;
					WeaponWheel.Play ("WeaponWheelOn", -1, 0);
				}

				if (WeaponId <= 0)
				{
					WeaponId = MaxWeapons;
				}

				Shot = Weapon [WeaponId - 1];

				if (canUseWeapon1 == false) {
					WeaponIcon1.SetActive (false);
				}

				if (canUseWeapon2 == false) {
					WeaponIcon2.SetActive (false);
				}

				if (canUseWeapon3 == false) {
					WeaponIcon3.SetActive (false);
				}

				if (canUseWeapon4 == false) {
					WeaponIcon4.SetActive (false);
				}

				if (canUseWeapon5 == false) {
					WeaponIcon5.SetActive (false);
				}

				if (canUseWeapon6 == false) {
					WeaponIcon6.SetActive (false);
				}

				if (canUseWeapon7 == false) {
					WeaponIcon7.SetActive (false);
				}

				if (canUseWeapon8 == false) {
					WeaponIcon8.SetActive (false);
				}
				
				if (canUseWeapon1 == true) {
					WeaponIcon1.SetActive (true);
				}

				if (canUseWeapon2 == true) {
					WeaponIcon2.SetActive (true);
				}

				if (canUseWeapon3 == true) {
					WeaponIcon3.SetActive (true);
				}

				if (canUseWeapon4 == true) {
					WeaponIcon4.SetActive (true);
				}

				if (canUseWeapon5 == true) {
					WeaponIcon5.SetActive (true);
				}

				if (canUseWeapon6 == true) {
					WeaponIcon6.SetActive (true);
				}

				if (canUseWeapon7 == true) {
					WeaponIcon7.SetActive (true);
				}

				if (canUseWeapon8 == true) {
					WeaponIcon8.SetActive (true);
				}
				
				switch (WeaponId) {
				case 1:
					WeaponWheelIcon1.color = Color.white;
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon1;
					break;

				case 2:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = Color.white;
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon2;
					break;

				case 3:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = Color.white;
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon3;
					break;

				case 4:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = Color.white;
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon4;
					break;

				case 5:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = Color.white;
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon5;
					break;

				case 6:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = Color.white;
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon6;
					break;

				case 7:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = Color.white;
					WeaponWheelIcon8.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					CurrentFireRate = FireRateWeapon7;
					break;

				case 8:
					WeaponWheelIcon1.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon2.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon3.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon4.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon5.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon6.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon7.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
					WeaponWheelIcon8.color = Color.white;
					CurrentFireRate = FireRateWeapon8;
					break;
				}
			}
		}
	}

	// For AI only
	void FindPlayers ()
	{
		if (Vector3.Distance (PlayerOne.transform.position, transform.position) < PlayersCheckingRange)
		{
			MinSpeed = 30;
			LocatedPlayer = true;
		} else 
		{
			MinSpeed = 15;
			LocatedPlayer = false;
		}
	}

	void HuntDownLocatedPlayer ()
	{
		if (isPaused == false) 
		{
			var targetRotation = Quaternion.LookRotation (PlayerOne.transform.position - transform.position);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, 5);
			GetComponent<AutoMoveAndRotate> ().moveUnitsPerSecond.value = new Vector3 (0, 0, MinSpeed);
			Shoot ();
		}
	}
}
