using UnityEngine;
using System.Collections;

public class RotateThing : MonoBehaviour {

public float rotationAmount = 1.0f;
	
	void Update()
	{
		if (!PauseButton.paused)
			transform.Rotate((rotationAmount * Time.deltaTime), 0, 0);
	}
}
