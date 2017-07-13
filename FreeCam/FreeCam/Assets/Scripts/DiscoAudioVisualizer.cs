using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DiscoAudioVisualizer : MonoBehaviour 
{
	public AudioSourceLoudnessTester LoudnessScript;
	public PostProcessingProfile postProcessingProfile;
	public float Smoothing;
	private float smoothVel;
	public float addIntenisity;

	void Start () 
	{
		LoudnessScript = GetComponent<AudioSourceLoudnessTester> ();
	}

	void Update () 
	{
		var BloomSettings = postProcessingProfile.bloom.settings;

		BloomSettings.bloom.intensity = Mathf.SmoothDamp(BloomSettings.bloom.intensity, LoudnessScript.clipLoudness + addIntenisity, ref smoothVel, Smoothing * Time.deltaTime);

		postProcessingProfile.bloom.settings = BloomSettings;
	}
}
