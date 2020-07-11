using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPink
	:
	CarBase
{
	void Start()
	{
		ground = GameObject.Find( "Ground" ).transform;
		for( int i = 0; i < ground.childCount; ++i )
		{
			targets.Add( ground.GetChild( i ).transform );
		}
	}

	void Update()
	{
		vel += transform.forward * accel * Time.deltaTime;

		transform.forward = Vector3.Lerp( transform.forward,
			Vector3.Lerp( targets[curTarget].forward,targets[nextTarget].forward,lookAheadPercent ),
			rotSpeed * Time.deltaTime );
	}

	void OnTriggerStay( Collider coll )
	{
		for( int i = 0; i < targets.Count; ++i )
		{
			if( targets[i] == coll.transform )
			{
				curTarget = i;
				nextTarget = i + 1;
				if( nextTarget >= targets.Count ) nextTarget = 0;
			}
		}
	}

	Transform ground;

	[SerializeField] float rotSpeed = 0.3f;
	[SerializeField] float lookAheadPercent = 0.2f;

	List<Transform> targets = new List<Transform>();
	int curTarget = 1;
	int nextTarget = 2;
}
