using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour 
{
	public AudioSourceLoudnessTester AudioLoudness;

	public Vector2 ScaleMultRange;
	public Vector2 AddScaleRange;
	public Vector2 ScaleSmoothRange;

	private float ScaleMultiplier;
	private float AddScale = 30;
	private float ScaleVel;
	private float ScaleSmoothing = 1;

	void Start () 
	{
		AddScale = Random.Range (AddScaleRange.x, AddScaleRange.y);
		ScaleMultiplier = Random.Range (ScaleMultRange.x, ScaleMultRange.y);
		ScaleSmoothing = Random.Range (ScaleSmoothRange.x, ScaleSmoothRange.y);
	}

	void Update () 
	{
		if (Time.timeScale > 0) {
			transform.localScale = new Vector3 (
				Mathf.SmoothDamp (transform.localScale.x, Mathf.Clamp (ScaleMultiplier * -AudioLoudness.clipLoudness + AddScale, 10, 80), ref ScaleVel, ScaleSmoothing * Time.deltaTime),
				Mathf.SmoothDamp (transform.localScale.y, Mathf.Clamp (ScaleMultiplier * -AudioLoudness.clipLoudness + AddScale, 10, 80), ref ScaleVel, ScaleSmoothing * Time.deltaTime),
				Mathf.SmoothDamp (transform.localScale.z, Mathf.Clamp (ScaleMultiplier * -AudioLoudness.clipLoudness + AddScale, 10, 80), ref ScaleVel, ScaleSmoothing * Time.deltaTime)
			);
		}
	}
}
