using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour 
{
	public Transform Reference;	
	public PlayerController playerControllerScript;
	public float sens = 10;

	void Update () 
	{
		Reference.transform.Rotate 
		(
			0,
			Time.deltaTime * sens,
			0
		);
	}
}
