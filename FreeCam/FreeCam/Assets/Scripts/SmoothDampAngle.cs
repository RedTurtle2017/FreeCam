using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDampAngle : MonoBehaviour 
{
	public Transform Reference;
	public Vector3 Angle;
	//public Quaternion AngleQ;
	public Vector3 smoothTime;
	public method MatchMethod;
	public enum method
	{
		EulerAngles,
		Slerp
	}
		
	void LateUpdate () 
	{
		switch (MatchMethod) 
		{
		case method.EulerAngles:
			OriginalMethod ();
			break;
		case method.Slerp:
			NewMethod ();
			break;
		}
	}

	void NewMethod ()
	{
		transform.rotation = Quaternion.Slerp
			(
			transform.rotation, Reference.transform.rotation, smoothTime.x * Time.deltaTime
			);
	}

	void OriginalMethod ()
	{
		Angle = new Vector3 
			(
				Mathf.LerpAngle (transform.eulerAngles.x, Reference.eulerAngles.x, smoothTime.x * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.y, Reference.eulerAngles.y, smoothTime.y * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.z, Reference.eulerAngles.z, smoothTime.z * Time.deltaTime)
			);

		transform.eulerAngles = Angle;
	}
}
