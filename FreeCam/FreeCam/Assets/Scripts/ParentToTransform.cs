using UnityEngine;

public class ParentToTransform : MonoBehaviour
{
	public Transform ParentObject;
	public string InstantiatedObject = "Instantiated Bullets";
	public bool OnCall;

	void OnEnable () 
	{
		if (OnCall == false) 
		{
			ParentObject = GameObject.Find (InstantiatedObject).transform;
			gameObject.transform.parent = ParentObject.transform;
		}
	}

	public void ParentNow ()
	{
		ParentObject = GameObject.Find (InstantiatedObject).transform;
		gameObject.transform.parent = ParentObject.transform;
	}
}
