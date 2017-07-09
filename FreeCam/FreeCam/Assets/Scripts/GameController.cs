using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
	[Header ("Players")]
	public int NumberOfPlayers = 1;
	public GameObject PlayerOne;
	public PlayerController playerControllerScript;
	
	[Header ("Time")]
	[Range (0, 100)]
	public float TargetTimeScale = 1;
	public float TimeScaleSmoothing = 1;

	void Start () 
	{
		Time.timeScale = 1;
		TargetTimeScale = 1;
	}

	void Update () 
	{
		SetTimeScale ();

		if (Input.GetKeyDown (KeyCode.R)) 
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	void SetTimeScale ()
	{
		Time.timeScale = Mathf.Lerp (Time.timeScale, TargetTimeScale, TimeScaleSmoothing * Time.unscaledDeltaTime);
	}
}
