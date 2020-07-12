﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarBase
	:
	MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
		body = GetComponent<Rigidbody>();

		var checkpointHolder = GameObject.Find( "Checkpoints" ).transform;
		for( int i = 0; i < checkpointHolder.childCount; ++i )
		{
			checkpoints.Add( checkpointHolder.GetChild( i ).gameObject );
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		body.MovePosition( transform.position + vel * Time.deltaTime );

		RaycastHit hit;
		if( !Physics.Raycast( new Ray( transform.position,Vector3.down ),out hit ) ||
			hit.transform.name == "Grass" )
		{
			var pos = checkpoints[curCheckpoint - 1].transform.position;
			pos.y = 0.0f;
			transform.position = pos;
		}

		vel *= decay;

		if( curBounceForce < 0.05f ) curBounceForce = 0.05f;
		if( curBounceForce < bounceForce ) curBounceForce *= 1.08f;
	}

	public virtual void OnCollisionEnter( Collision coll )
	{
		if( coll.gameObject.tag == "Wall" ||
			coll.gameObject.tag == "Car" )
		{
			// if( canBounce )
			{
				var normal = coll.contacts[0].normal;
				// vel.x *= -Mathf.Abs( normal.x ) * bounceForce;
				// vel.z *= -Mathf.Abs( normal.z ) * bounceForce;
				vel = Vector3.Reflect( vel,normal ) * curBounceForce;
				curBounceForce /= 2.0f;
				// canBounce = false;
				// StartCoroutine( BounceReset( bounceReset ) );
			}

			if( vel.sqrMagnitude > Mathf.Pow( maxSpeed,2 ) )
			{
				vel = vel.normalized * maxSpeed;
			}
		}
	}

	void OnTriggerEnter( Collider coll )
	{
		if( curCheckpoint < checkpoints.Count )
		{
			if( coll.gameObject == checkpoints[curCheckpoint] )
			{
				++curCheckpoint;
			}
		}
		else
		{
			if( coll.gameObject == checkpoints[0] )
			{
				curCheckpoint = 1;
				++lap;
				CompleteLap();
			}
		}
	}

	protected virtual void CompleteLap()
	{
		if( lap >= lapsToComplete &&
			SceneManager.GetActiveScene().name != "Menu" )
		{
			var scorePanel = GameObject.Find( "Canvas" )
				.transform.Find( "ScorePanel" );
			for( int i = 0; i < scorePanel.childCount; ++i )
			{
				var child = scorePanel.GetChild( i );
				var childImg = child.GetComponentInChildren<Image>();
				int place = i + 1;
				if( childImg.sprite == null )
				{
					child.GetComponentInChildren<Text>().text =
						place.ToString() + ". " + gameObject.name;
					childImg.color = Color.white;
					childImg.sprite = thumbnail;

					var levelName = SceneManager.GetActiveScene().name;
					if( name == "Player" &&
						place < PlayerPrefs.GetInt( levelName,11 ) )
					{
						PlayerPrefs.SetInt( levelName,place );
						PlayerPrefs.Save();
					}

					break;
				}
			}

			Destroy( GetComponent<BoxCollider>() );
			body.velocity = Vector3.zero;
			// Destroy( this );
			enabled = false;
		}
	}

	// IEnumerator BounceReset( float t )
	// {
	// 	yield return( new WaitForSeconds( t ) );
	// 	canBounce = true;
	// }

	protected Rigidbody body;

	[SerializeField] protected float accel = 3.0f;
	[SerializeField] float decay = 0.98f;
	const float bounceForce = 1.4f;
	float curBounceForce = bounceForce;
	[SerializeField] Sprite thumbnail = null;

	[HideInInspector] public Vector3 vel = Vector3.zero;
	// bool canBounce = true;
	// const float bounceReset = 0.7f;

	List<GameObject> checkpoints = new List<GameObject>();
	[HideInInspector] public int curCheckpoint = 0;
	[HideInInspector] public int lap = 0;

	const float maxSpeed = 40.0f;
	protected const int lapsToComplete = 3;
}
