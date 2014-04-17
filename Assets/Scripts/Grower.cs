using UnityEngine;
using System.Collections;

public class Grower : MonoBehaviour
{
	public float rate = 1.0f;
	public float min = 0.0f;
	public float max = 10.0f;
	
	private float currentScale = 1.0f;
	private Vector3 initialScale = Vector3.zero;
	
	void Start()
	{
		initialScale = transform.localScale;
	}
	
	void Update()
	{
		if (currentScale > min && currentScale < max)
			currentScale += Time.deltaTime * rate;
		
		transform.localScale = initialScale * currentScale;
	}
}
