using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFramerate : MonoBehaviour 
{
	public int framerate = 60;

	void Awake () 
	{
		Application.targetFrameRate = framerate;
	}
}
