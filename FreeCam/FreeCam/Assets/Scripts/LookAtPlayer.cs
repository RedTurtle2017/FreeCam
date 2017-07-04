using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour 
{
	public Transform LookDirection;
	public bool flip;

	void Update ()
	{
		if (flip == false) 
		{
			transform.LookAt (LookDirection);	
		}

		if (flip == true) 
		{
			transform.LookAt (2 * transform.position - LookDirection.position);	
		}
	}
}
