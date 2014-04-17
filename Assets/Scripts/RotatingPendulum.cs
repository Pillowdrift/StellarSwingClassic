using UnityEngine;
using System.Collections;

public class RotatingPendulum : MonoBehaviour
{
	public float rotationAmount = 1.0f;
	
	void Update()
	{
		if (!PauseButton.paused)
			transform.Rotate(new Vector3(0, 0, rotationAmount * Time.deltaTime));
	}
}
