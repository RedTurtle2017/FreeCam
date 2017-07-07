using UnityEngine;
using System.Collections;

public class PitchByOtherAudioSourcePitch : MonoBehaviour 
{
	private AudioSource MainAudioSource;
	private AudioSource thisAudio;

	void Start () 
	{
		MainAudioSource = GameObject.FindGameObjectWithTag ("BGM").GetComponent<AudioSource>();
		thisAudio = GetComponent<AudioSource> ();
	}

	void Update () 
	{
		thisAudio.pitch = MainAudioSource.pitch;
	}
}