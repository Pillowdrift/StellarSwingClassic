using UnityEngine;
using System.Collections;

public class AlphaFadein : MonoBehaviour
{
	public float time = 1.0f;
	public float min = 0.0f;
	public float max = 1.0f;
	
	private float coeff = 0.0f;

	void Start()
	{
		if (time == 0)
			coeff = 1.0f;
	}
	
	void Update()
	{
		if (time > 0)
		{
			coeff += Time.deltaTime / time;
			if (coeff > 1.0f)
				coeff = 1.0f;
		}
			
		float alpha = Mathf.Lerp(min, max, coeff);
		
		Color col = renderer.material.color;
		col.a = alpha;
		renderer.material.color = col;
	}
}
