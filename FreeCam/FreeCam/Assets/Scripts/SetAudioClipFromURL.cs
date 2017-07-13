using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio;

public class SetAudioClipFromURL : MonoBehaviour
{
	public string musicUrl;

	void Start ()
	{
		StartCoroutine (Stream ());
	}

	IEnumerator Stream ()
	{
		WWW www = new WWW (musicUrl);
		while(!www.isDone){
			yield return 0;
		}
		GetComponent<AudioSource> ().clip = NAudioPlayer.FromMp3Data(www.bytes);
	}
}
