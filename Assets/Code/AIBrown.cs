using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrown
	:
	CarBase
{
	void Start()
	{
		target = FindObjectOfType<LaggyDriver>();
		if( target == null )
		{
			target = FindObjectOfType<AIMint>();
		}
		StartCoroutine( AISwap( aiSwapTime ) );
	}

	void Update()
	{
		if( !aiTargetMode )
		{
			vel += transform.forward * accel * Time.deltaTime;
		}
		else
		{
			var diff = target.transform.position - transform.position;
			vel += diff.normalized * accel * Time.deltaTime;
			if( vel.sqrMagnitude != 0.0f )
			{
				transform.rotation = Quaternion.Lerp( transform.rotation,
					Quaternion.LookRotation( vel,Vector3.up ),
					1.3f * Time.deltaTime );
			}
		}
	}

	void OnCollisionStay( Collision coll )
	{
		if( !aiTargetMode )
		{
			transform.forward = Vector3.Lerp( transform.forward,
				coll.transform.forward,rotSpeed * Time.deltaTime );
		}
	}

	IEnumerator AISwap( float reset )
	{
		yield return( new WaitForSeconds( reset ) );

		aiTargetMode = !aiTargetMode;

		StartCoroutine( AISwap( reset ) );
	}

	[SerializeField] float rotSpeed = 3.3f;
	[SerializeField] float aiSwapTime = 5.0f;

	bool aiTargetMode = false;
	CarBase target;
}
