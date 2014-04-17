using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	private Quaternion initialRotation;
	
	void Start()
	{
		initialRotation = transform.rotation;
	}
	
	void Update()
	{
		transform.LookAt(transform.position - Camera.mainCamera.transform.forward, Camera.mainCamera.transform.up);
		transform.rotation *= initialRotation;
	}
}
