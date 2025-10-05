using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateSetter
	:
	MonoBehaviour
{
	void Start()
	{
		Application.targetFrameRate = 60;
	}
}
