using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPurple
	:
	CarBase
{
	void Start()
	{
		targets = FindObjectsOfType<CarBase>();
		target = FindObjectOfType<LaggyDriver>().transform;
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
		yield return ( new WaitForSeconds( delay ) );

		int checkpoint = -1;
		for( int i = 0; i < targets.Length; ++i )
		{
			var newCheckpoint = targets[i].lap * 10 + targets[i].curCheckpoint;
			if( newCheckpoint > checkpoint && targets[i] != this )
			{
				target = targets[i].transform;
				checkpoint = newCheckpoint;
			}
		}

		StartCoroutine( SetTarget( targetReset ) );
	}

	[SerializeField] float targetReset = 4.0f;

	Transform target = null;
	CarBase[] targets;
}
