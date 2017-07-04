using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public GameObject Shot;
	public Transform ShotSpawn;
	public float fireRate;
	public float nextFire;

	void Start () 
	{
		
	}

	void Update () 
	{
		if (Input.GetMouseButton (0)) 
		{
			Shoot ();
		}
	}

	void Shoot ()
	{
		if (Time.time > nextFire) 
		{
			Instantiate (Shot, ShotSpawn.position, ShotSpawn.rotation);
			nextFire = Time.time + fireRate;
		}
	}
}
