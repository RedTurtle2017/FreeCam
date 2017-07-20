using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour
{
	public float ShrinkRate;

	public int ShrinkSizeIndex;
	public float[] ShrinkSizes;

	public Vector2 ShrinkDelay = new Vector2 (10, 30);

	public bool isShrinking;
	public Vector3 NewSize;

	public float SmoothScaleTime = 100;

	void Start ()
	{
		ShrinkSizeIndex = 0;

		StartCoroutine (Shrink ());
	}

	void Update ()
	{
		if (isShrinking == true) 
		{
			if (transform.localScale.magnitude > NewSize.magnitude) 
			{
				transform.localScale = new Vector3 
				(
					Mathf.Lerp (transform.localScale.x, NewSize.x, SmoothScaleTime * Time.deltaTime),
					Mathf.Lerp (transform.localScale.y, NewSize.y, SmoothScaleTime * Time.deltaTime),
					Mathf.Lerp (transform.localScale.z, NewSize.z, SmoothScaleTime * Time.deltaTime)
				);
			}
		}

		if (transform.localScale.magnitude < NewSize.magnitude + 1) 
		{
			StartCoroutine (Shrink ());
			isShrinking = false;
		}
	}

	IEnumerator Shrink ()
	{
		ShrinkSizeIndex += 1;
		float RandomShrinkDelayTime = Random.Range (ShrinkDelay.x, ShrinkDelay.y);
		yield return new WaitForSeconds (RandomShrinkDelayTime);

		NewSize = new Vector3 
			(
				ShrinkSizes [ShrinkSizeIndex], 
				ShrinkSizes [ShrinkSizeIndex], 
				ShrinkSizes [ShrinkSizeIndex]
			);

		isShrinking = true;
		StopCoroutine (Shrink ());
	}
}
