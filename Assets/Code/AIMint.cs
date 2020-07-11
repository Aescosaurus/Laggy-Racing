using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMint
	:
	CarBase
{
	void Update()
	{
		vel += transform.forward * accel * Time.deltaTime;
	}

	void OnCollisionStay( Collision coll )
	{
		transform.forward = Vector3.Lerp( transform.forward,
			coll.transform.forward,rotSpeed * Time.deltaTime );
	}

	[SerializeField] float rotSpeed = 0.3f;
}
