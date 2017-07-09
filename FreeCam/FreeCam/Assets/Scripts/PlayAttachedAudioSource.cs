using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAttachedAudioSource : MonoBehaviour
{
	public void PlayAudioSource ()
	{
		GetComponent<AudioSource> ().Play ();
	}
}
