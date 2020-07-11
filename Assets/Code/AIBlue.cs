using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBlue
	:
	CarBase
{
	void Start()
	{
		target = FindObjectOfType<LaggyDriver>();
	}

	void Update()
	{
		var diff = ( target.transform.position +
			target.vel.normalized * predictAmount ) -
			transform.position;
		vel += diff.normalized * accel * Time.deltaTime;
		if( vel.sqrMagnitude != 0.0f )
		{
			transform.rotation = Quaternion.Lerp( transform.rotation,
				Quaternion.LookRotation( vel,Vector3.up ),
				1.1f * Time.deltaTime );
		}
	}

	LaggyDriver target;

	[SerializeField] float predictAmount = 10.0f;
}
