using UnityEngine;
using System.Collections;

public class SavePos : MonoBehaviour
{
	public bool lerpBack = false;
	public float lerpRate = 1.0f;
	
	private Vector3 position;
	private Quaternion rotation;
	private Vector3 scale;
	
	private Vector3 lerpStartPos;
	private Quaternion lerpStartRot;
	private Vector3 lerpStartScale;
	
	private float timeDiff;
	private bool lerping = false;
	
	public void Start()
	{
		position = transform.position;
		rotation = transform.rotation;
		scale = transform.localScale;
	}
	
	public void Update()
	{
		if (lerping)
		{
			timeDiff += Time.deltaTime;
			
			if (timeDiff * lerpRate >= 1.0f)
			{
				lerping = false;
				timeDiff = 1.0f / lerpRate;
			}
			
			transform.position = Vector3.Lerp(transform.position, position, timeDiff * lerpRate);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, timeDiff * lerpRate);
			transform.localScale = Vector3.Lerp(transform.localScale, scale, timeDiff * lerpRate);
		}
	}
	
	public void Reload()
	{
		if (!lerpBack)
		{
			transform.position = position;
			transform.rotation = rotation;
			transform.localScale = scale;
		}
		else
		{
			timeDiff = 0.0f;
			lerping = true;
		}
	}
}
