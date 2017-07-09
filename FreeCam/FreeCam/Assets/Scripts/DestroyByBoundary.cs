using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour 
{
	public Vector2 xBound;
	public Vector2 yBound;
	public Vector2 zBound;
	
	void Update () {
		
		// If object falls outseide above parameters.
		if (transform.position.x < xBound.x ||
			transform.position.x > xBound.y ||
			transform.position.y < yBound.x ||
			transform.position.y > yBound.y ||
			transform.position.z < zBound.x ||
			transform.position.z > zBound.y) 
		{
			Destroy (gameObject);
		}
	}
}
