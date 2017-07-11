using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shakeDuration = 0f;
	public float shakeTimeRemaining;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	public bool useNewMethod;
	private float ShakeSmoothVel;
	public float Smoothing;
	
	Vector3 originalPos;
	
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (shakeTimeRemaining > 0)
		{
			if (useNewMethod == false)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			}

			if (useNewMethod == true) 
			{
				camTransform.localPosition = new Vector3 
					(
						Mathf.SmoothDamp (camTransform.localPosition.x, originalPos.x + Random.insideUnitSphere.x * shakeAmount, ref ShakeSmoothVel, Smoothing * Time.deltaTime),
						Mathf.SmoothDamp (camTransform.localPosition.y, originalPos.y + Random.insideUnitSphere.y * shakeAmount, ref ShakeSmoothVel, Smoothing * Time.deltaTime),
						Mathf.SmoothDamp (camTransform.localPosition.z, originalPos.z + Random.insideUnitSphere.z * shakeAmount, ref ShakeSmoothVel, Smoothing * Time.deltaTime)
					);
			}
			
			shakeTimeRemaining -= Time.unscaledDeltaTime * decreaseFactor;
		}
		else
		{
			shakeTimeRemaining = 0f;
			camTransform.localPosition = originalPos;
		}
	}

	public void Shake ()
	{
		shakeTimeRemaining = shakeDuration;
	}
}
