using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStandardShaderEmissionColor : MonoBehaviour 
{
	[ColorUsageAttribute (true, true, 0, 100.0f, 0, 100.0f)]
	public Color[] Colors;

	Renderer rend;

	void Start () 
	{
		rend = GetComponent<Renderer> ();
		rend.material.EnableKeyword ("_EMISSION");
		rend.material.SetColor ("_EmissionColor", Colors [Random.Range (0, Colors.Length)]);
	}
}
