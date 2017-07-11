using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorOMeter : MonoBehaviour 
{
	public Rigidbody Player;
	public Transform Vector;
	public Transform Outer;
	public float PlayerVelocityMagnitudeDamp;

	void Start () 
	{
		if (Player == null) 
		{
			Player = GameObject.FindGameObjectWithTag ("Player").GetComponent <Rigidbody> ();
		}
		
	}

	void Update () 
	{
		Vector.localScale = new Vector3
		(
			Vector.localScale.x, 
			Vector.localScale.y, 
			Player.velocity.magnitude * PlayerVelocityMagnitudeDamp
		);

		Outer.rotation = Quaternion.identity * Player.rotation;
	}
}
