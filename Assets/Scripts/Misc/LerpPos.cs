using UnityEngine;
using System.Collections;

public class LerpPos : MonoBehaviour
{
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
			
			transform.position = Vector3.Lerp(transform.position, position, timeDiff * lerpRate);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, timeDiff * lerpRate);
			transform.localScale = Vector3.Lerp(transform.localScale, scale, timeDiff * lerpRate);
			
			if ((transform.position - position).sqrMagnitude < 0.001f)
			{
				ThirdPersonCamera cameraController = Camera.mainCamera.GetComponent<ThirdPersonCamera>();
				if (cameraController != null)
					cameraController.enabled = false;
				
				SendMessage("RestartLevel");
				
				lerping = false;
			}
		}
	}
	
	public void LerpToStartPos()
	{
		timeDiff = 0.0f;
		lerping = true;
	}
}
