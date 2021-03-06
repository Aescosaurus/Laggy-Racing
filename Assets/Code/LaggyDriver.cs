﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaggyDriver
	:
	CarBase
{
	void Start()
	{
		lapText = GameObject.Find( "Canvas" ).transform.Find( "LapText" )
			.GetComponent<Text>();

		var scorePanel = GameObject.Find( "Canvas" )
			.transform.Find( "ScorePanel" );
		for( int i = 0; i < scorePanel.childCount; ++i )
		{
			var child = scorePanel.GetChild( i );
			child.GetComponentInChildren<Text>().text = "";
			child.GetComponentInChildren<Image>().color = invis;
		}

		StartCoroutine( LapFlash( lapFlashInterval ) );
	}

	void Update()
	{
		StartCoroutine( ApplyVel( Input.GetAxis( "Accelerate" ) * accel,lagginess ) );

		StartCoroutine( ApplyDir( Input.GetAxis( "Turn" ) * accel,lagginess ) );

		if( Input.GetAxis( "Cancel" ) > 0.0f )
		{
			SceneManager.LoadScene( "Menu" );
		}
	}

	IEnumerator ApplyVel( float vel,float lag )
	{
		yield return( new WaitForSeconds( lag ) );

		this.vel += transform.forward * vel * accel;
	}

	IEnumerator ApplyDir( float turn,float lag )
	{
		yield return( new WaitForSeconds( lag ) );

		transform.Rotate( transform.up,turn * turnSpeed );
	}

	protected override void CompleteLap()
	{
		base.CompleteLap();

		lapText.text = "Lap " + ( lap + 1 ).ToString();
		StartCoroutine( LapFlash( lapFlashInterval ) );

		if( lap >= lapsToComplete )
		{
			lapText.text = "";
			var canv = GameObject.Find( "Canvas" ).transform;
			canv.Find( "ScorePanel" ).gameObject.SetActive( true );
			canv.Find( "BackButton" ).gameObject.SetActive( true );
		}
	}

	IEnumerator LapFlash( float interval )
	{
		if( ++curFlash > flashCount )
		{
			curFlash = 0;
			lapText.color = Color.white;
		}
		else
		{
			lapText.color = ( lapText.color == Color.white ? invis : Color.white );
			yield return ( new WaitForSeconds( interval ) );
			StartCoroutine( LapFlash( lapFlashInterval ) );
		}
	}

	[SerializeField] float lagginess = 1.0f;
	[SerializeField] float turnSpeed = 1.2f;

	Text lapText;
	const float lapFlashInterval = 0.5f;
	int curFlash = 0;
	const int flashCount = 6;
	readonly Color invis = new Color( 1.0f,1.0f,1.0f,0.0f );
}
