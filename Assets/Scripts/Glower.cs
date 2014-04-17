using UnityEngine;
using System.Collections;

public class Glower : MonoBehaviour
{
	public bool alpha = false;
	
	public float min = 0.5f;
	public float max = 1.5f;
	public float rate = 2.0f;
	
	public string matname = "";
	
	void Update()
	{
		float col = min + (max - min) * 0.5f * (1.0f + Mathf.Sin(Time.time * rate));
		
		Material mat = null;
		
		foreach (Material material in renderer.materials)
		{
			if (material.name == matname || material.name == matname + " (Instance)")
			{
				mat = material;
				break;
			}
		}
		
		if (mat != null)
		{
			Color color = mat.color;
			if (alpha)
			{
				color.a = col;
			}
			else
			{
				color.r = col;
				color.g = col;
				color.b = col;
			}
			
			mat.color = color;
		}
	}
}
