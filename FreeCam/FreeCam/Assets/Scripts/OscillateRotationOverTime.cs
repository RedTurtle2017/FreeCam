using UnityEngine;
using System.Collections;

public class OscillateRotationOverTime : MonoBehaviour 
{
	public float time;
	public bool changePos;
	public bool changeRotation;
	public posspace PosSpace;
	public enum posspace
	{
		local,
		world
	}

	public rotspace RotSpace;
	public enum rotspace
	{
		local,
		world
	}

	public mathfunctionpos MathFunctionPos;
	public enum mathfunctionpos
	{
		sin, 
		cos
	}

	public mathfunctionrot MathFunctionRot;
	public enum mathfunctionrot
	{
		sin, 
		cos
	}

	[Header ("Change Rotation Values")]
	public Vector3 amountRot;
	public Vector3 frequencyRot;
	public Vector3 offsetRot;

	[Header ("Change Position Values")]
	public Vector3 amountPos;
	public Vector3 frequencyPos;
	public Vector3 offsetPos;

	void Update () 
	{
		time += Time.deltaTime;

		if (changeRotation == true)
		{
			if (RotSpace == rotspace.local)
			{
				if (MathFunctionRot == mathfunctionrot.sin) 
				{
					transform.localRotation = Quaternion.Euler 
				(
						amountRot.x * Mathf.Sin (time * frequencyRot.x) + offsetRot.x, 
						amountRot.y * Mathf.Sin (time * frequencyRot.y) + offsetRot.y, 
						amountRot.z * Mathf.Sin (time * frequencyRot.z) + offsetRot.z
					);
				}

				if (MathFunctionRot == mathfunctionrot.cos) 
				{
					transform.localRotation = Quaternion.Euler 
					(
						amountRot.x * Mathf.Cos (time * frequencyRot.x) + offsetRot.x, 
						amountRot.y * Mathf.Cos (time * frequencyRot.y) + offsetRot.y, 
						amountRot.z * Mathf.Cos (time * frequencyRot.z) + offsetRot.z
					);
				}
			}

			if (RotSpace == rotspace.world) 
			{
				if (MathFunctionRot == mathfunctionrot.sin) 
				{
					transform.rotation = Quaternion.Euler 
					(
						amountRot.x * Mathf.Sin (time * frequencyRot.x) + offsetRot.x, 
						amountRot.y * Mathf.Sin (time * frequencyRot.y) + offsetRot.y, 
						amountRot.z * Mathf.Sin (time * frequencyRot.z) + offsetRot.z
					);
				}

				if (MathFunctionRot == mathfunctionrot.cos) 
				{
					transform.rotation = Quaternion.Euler 
						(
							amountRot.x * Mathf.Cos (time * frequencyRot.x) + offsetRot.x, 
							amountRot.y * Mathf.Cos (time * frequencyRot.y) + offsetRot.y, 
							amountRot.z * Mathf.Cos (time * frequencyRot.z) + offsetRot.z
						);
				}
			}
		}

		if (changePos == true) 
		{
			if (PosSpace == posspace.local) 
			{
				if (MathFunctionPos == mathfunctionpos.sin)
				{
					transform.localPosition = new Vector3 
						(
							amountPos.x * Mathf.Sin (time * frequencyPos.x) + offsetPos.x, 
							amountPos.y * Mathf.Sin (time * frequencyPos.y) + offsetPos.y, 
							amountPos.z * Mathf.Sin (time * frequencyPos.z) + offsetPos.z
						);
				}

				if (MathFunctionPos == mathfunctionpos.cos)
				{
					transform.localPosition = new Vector3 
						(
							amountPos.x * Mathf.Cos (time * frequencyPos.x) + offsetPos.x, 
							amountPos.y * Mathf.Cos (time * frequencyPos.y) + offsetPos.y, 
							amountPos.z * Mathf.Cos (time * frequencyPos.z) + offsetPos.z
						);
				}
			}

			if (PosSpace == posspace.world) 
			{
				if (MathFunctionPos == mathfunctionpos.sin) 
				{
					transform.position = new Vector3 
						(
							amountPos.x * Mathf.Sin (time * frequencyPos.x) + offsetPos.x, 
							amountPos.y * Mathf.Sin (time * frequencyPos.y) + offsetPos.y, 
							amountPos.z * Mathf.Sin (time * frequencyPos.z) + offsetPos.z
						);
				}

				if (MathFunctionPos == mathfunctionpos.cos) 
				{
					transform.position = new Vector3 
						(
							amountPos.x * Mathf.Cos (time * frequencyPos.x) + offsetPos.x, 
							amountPos.y * Mathf.Cos (time * frequencyPos.y) + offsetPos.y, 
							amountPos.z * Mathf.Cos (time * frequencyPos.z) + offsetPos.z
						);
				}
			}
		}
	}
}
