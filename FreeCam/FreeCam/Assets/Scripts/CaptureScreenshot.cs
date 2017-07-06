using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureScreenshot : MonoBehaviour
{
	public int superSize = 1;
	public int ScreenshotsTaken;

	void Awake ()
	{
		ScreenshotsTaken = PlayerPrefs.GetInt ("ScreenshotsTaken");
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.F11))
		{
			Application.CaptureScreenshot ("Screenshot_" + ScreenshotsTaken + ".png", superSize);
			ScreenshotsTaken += 1;
			PlayerPrefs.SetInt ("ScreenshotsTaken", ScreenshotsTaken);
			Debug.Log ("Saved a screenshot with upscale of " + superSize);
		}

		if (Input.GetKeyDown (KeyCode.F10)) 
		{
			ScreenshotsTaken = 0;
			PlayerPrefs.SetInt ("ScreenshotsTaken", 0);
		}
	}
}
