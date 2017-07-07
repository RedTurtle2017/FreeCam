using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleByLoudness : MonoBehaviour 
{
	public AudioSourceLoudnessTester loudnessScript;
	public float AddScale;
	public float AmpScale;

	void Start () 
	{
		loudnessScript = GameObject.Find ("Drums").GetComponent<AudioSourceLoudnessTester> ();
	}

	void Update () 
	{
		transform.localScale = new Vector3 
			(
				AmpScale * loudnessScript.clipLoudness + AddScale,
				AmpScale * loudnessScript.clipLoudness + AddScale,
				AmpScale * loudnessScript.clipLoudness + AddScale
			);
	}
}
