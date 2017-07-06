using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitScript : MonoBehaviour 
{
	public void StartQuit (float delay)
	{
		Invoke ("QuitGame", delay);
	}

	public void QuitGame ()
	{
		Application.Quit ();

		#if UNITY_EDITOR
		Debug.Log ("Tried to Quit in editor");
		#endif
	}
}
