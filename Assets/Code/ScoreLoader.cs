using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLoader
	:
	MonoBehaviour
{
	void Start()
	{
		var levelName = transform.Find( "LevelName" ).GetComponent<Text>().text;
		var scoreText = transform.Find( "Score" ).GetComponent<Text>();

		var spot = PlayerPrefs.GetInt( levelName,-1 );
		if( spot > 0 )
		{
			scoreText.text = spot + GeneratePlace( spot );
		}
	}

	string GeneratePlace( int place )
	{
		switch( place )
		{
			case 1:
				return( "st" );
			case 2:
				return( "nd" );
			case 3:
				return( "rd" );
			default:
				return( "th" );
		}
	}
}
