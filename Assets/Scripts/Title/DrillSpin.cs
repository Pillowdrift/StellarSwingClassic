using UnityEngine;
using System.Collections;

public class DrillSpin : MonoBehaviour
{
	private const float DRILL_SPEED = 360.0f;
	
	public void Update()
	{
		transform.Rotate(0, 0, -DRILL_SPEED * Time.deltaTime);
	}
}
