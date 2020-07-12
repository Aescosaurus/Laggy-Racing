using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITeal
	:
	CarBase
{
	void Start()
	{
		targets = FindObjectsOfType<CarBase>();
		StartCoroutine( SetTarget( targetReset ) );
	}

	void Update()
	{
		var diff = target.position - transform.position;
		vel += diff.normalized * accel * Time.deltaTime;
		if( vel.sqrMagnitude != 0.0f )
		{
			transform.rotation = Quaternion.Lerp( transform.rotation,
				Quaternion.LookRotation( vel,Vector3.up ),
				1.3f * Time.deltaTime );
		}
	}

	IEnumerator SetTarget( float delay )
	{
		float dist = 999999.0f;
		for( int i = 0; i < targets.Length; ++i )
		{
			float newDist = ( targets[i].transform.position -
				transform.position ).sqrMagnitude;
			if( newDist < dist && targets[i] != this )
			{
				target = targets[i].transform;
				dist = newDist;
			}
		}

		yield return ( new WaitForSeconds( delay ) );

		StartCoroutine( SetTarget( targetReset ) );
	}

	[SerializeField] float targetReset = 6.0f;

	Transform target = null;
	CarBase[] targets;
}
