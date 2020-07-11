using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIYellow
	:
	CarBase
{
	void Start()
	{
		ground = GameObject.Find( "Ground" ).transform;
		for( int i = 0; i < ground.childCount; ++i )
		{
			targets.Add( Vector3.zero );
			FindTarget( i );
		}
	}

	void Update()
	{
		var diff = targets[curTarget] - transform.position;
		diff.y = 0.0f;
		if( diff.sqrMagnitude < Mathf.Pow( targetActivateDist,2 ) )
		{
			++curTarget;
			if( curTarget >= targets.Count ) curTarget = 0;
			targetTries = 0;
		}
		else
		{
			vel += diff.normalized * accel * Time.deltaTime;
			transform.rotation = Quaternion.Lerp( transform.rotation,
				Quaternion.LookRotation( vel,Vector3.up ),
				1.3f * Time.deltaTime );
		}
	}

	public override void OnCollisionEnter( Collision coll )
	{
		base.OnCollisionEnter( coll );

		if( coll.gameObject.tag == "Wall" )
		{
			FindTarget( curTarget );
			if( ++targetTries > maxTries ) --curTarget;
			if( curTarget < 0 ) curTarget = targets.Count - 1;
		}
	}

	void FindTarget( int i )
	{
		var bounds = ground.GetChild( i ).GetComponent<BoxCollider>().bounds;
		targets[i] = new Vector3(
			Random.Range( bounds.min.x,bounds.max.x ),
			transform.position.y,
			Random.Range( bounds.min.z,bounds.max.z )
			);
	}

	List<Vector3> targets = new List<Vector3>();
	int curTarget = 1;
	int targetTries = 0;
	const int maxTries = 25;

	Transform ground;

	[SerializeField] float targetActivateDist = 1.0f;
}
