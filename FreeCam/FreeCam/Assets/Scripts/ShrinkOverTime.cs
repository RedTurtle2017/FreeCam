using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour
{
	public float ShrinkRate;

	public void Update ()
	{
		gameObject.transform.localScale = new Vector3 
			(
				transform.localScale.x - Time.deltaTime * ShrinkRate,
				transform.localScale.y - Time.deltaTime * ShrinkRate,
				transform.localScale.z - Time.deltaTime * ShrinkRate
			);
	}
}
