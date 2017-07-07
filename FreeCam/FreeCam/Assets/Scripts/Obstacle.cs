using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour 
{
	//public AudioSource Soundtrack;
	public AudioSourceLoudnessTester AudioLoudness;
	public float ScaleMultiplier;
	public float AddScale = 30;
	public float ScaleVel;
	public float ScaleSmoothing = 1;

	void Start () 
	{
		AddScale = Random.Range (30, 60);
		ScaleMultiplier = Random.Range (60, 120);
		ScaleSmoothing = Random.Range (4, 7);
	}

	void Update () 
	{
		transform.localScale = new Vector3
			(
				Mathf.SmoothDamp (transform.localScale.x, Mathf.Clamp(ScaleMultiplier * -AudioLoudness.clipLoudness + AddScale, 10, 80), ref ScaleVel, ScaleSmoothing * Time.deltaTime),
				Mathf.SmoothDamp (transform.localScale.y, Mathf.Clamp(ScaleMultiplier * -AudioLoudness.clipLoudness + AddScale, 10, 80), ref ScaleVel, ScaleSmoothing * Time.deltaTime),
				Mathf.SmoothDamp (transform.localScale.z, Mathf.Clamp(ScaleMultiplier * -AudioLoudness.clipLoudness + AddScale, 10, 80), ref ScaleVel, ScaleSmoothing * Time.deltaTime)
			);
	}
}
