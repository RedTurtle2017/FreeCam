using UnityEngine;
using System.Collections;

public class VolDisplay : MonoBehaviour {
	
	public float amp;
	public float[] smooth = new float[2];
	
	void Start () {
		// initalising the filter
		for (int i = 0; i < 2; i++) {
			smooth [i] = 0.1f;	
		}
	}
	
	// Update is called once per frame
	void Update () {
		//intensity of light, controlled by the amplitude of the sound
		GetComponent<Light>().intensity = amp;
	}
	
	void OnAudioFilterRead (float[] data, int channels)
	{		
		for (var i = 0; i < data.Length; i = i + channels) {
			// the absolute value of every sample
			float absInput = Mathf.Abs(data[i]);
			// smoothening filter doing its thing
			smooth[0] = ((0.5f * absInput) + (0.99f * smooth[1]));
			// exaggerating the amplitude
			amp = smooth[0]*8f - 0.5f;
			// it is a recursive filter, so it is doing its recursive thing
			smooth[1] = smooth[1];
		}
	}
}