using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour
{
	[Header ("Shrink")]
	public float ShrinkRate;

	public int ShrinkSizeIndex;
	public float[] ShrinkSizes;

	public Vector2 ShrinkDelay = new Vector2 (10, 30);
	public float ShrinkTimeRemaining;
	public float StartShrinkTime;

	public bool isShrinking;
	public Vector3 NewSize;

	public float SmoothScaleTime = 100;

	[Header ("Visuals")]
	public Renderer rend;
	public Color NormalColour;
	public Color ShrinkingColour;

	void Start ()
	{
		ShrinkSizeIndex = 0;
		ShrinkTimeRemaining = StartShrinkTime;
	
		rend = GetComponent<MeshRenderer> ();
		rend.material.EnableKeyword ("_TintColor");
		rend.material.SetColor ("_TintColor", NormalColour);
	}

	void Update ()
	{
		transform.localScale = new Vector3 
			(
				Mathf.Lerp (transform.localScale.x, NewSize.x, SmoothScaleTime * Time.deltaTime),
				Mathf.Lerp (transform.localScale.y, NewSize.y, SmoothScaleTime * Time.deltaTime),
				Mathf.Lerp (transform.localScale.z, NewSize.z, SmoothScaleTime * Time.deltaTime)
			);

		if (ShrinkTimeRemaining < 0 && ShrinkSizeIndex < ShrinkSizes.Length - 1) 
		{
			Shrink ();
		}

		if (ShrinkTimeRemaining > 0) 
		{
			ShrinkTimeRemaining -= Time.deltaTime;
		}

		if (transform.localScale.x < NewSize.x + 1) 
		{
			rend.material.SetColor ("_TintColor", NormalColour);
		}
	}

	void Shrink ()
	{
		rend.material.SetColor ("_TintColor", ShrinkingColour);
		ShrinkSizeIndex += 1;
		ShrinkTimeRemaining = Random.Range (ShrinkDelay.x, ShrinkDelay.y);

		NewSize = new Vector3 
		(
			ShrinkSizes [ShrinkSizeIndex], 
			ShrinkSizes [ShrinkSizeIndex], 
			ShrinkSizes [ShrinkSizeIndex]
		);
	}
}
