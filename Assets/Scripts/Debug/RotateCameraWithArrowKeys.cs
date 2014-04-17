using UnityEngine;
using System.Collections;

public class RotateCameraWithArrowKeys : MonoBehaviour {
	
	// The Camera
	public ThirdPersonCamera TheCamera;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Get the rotation for this frame. 
		Vector2 rot = new Vector2();
		if (Input.GetKey(KeyCode.UpArrow)) rot.y += 1.0f;
		if (Input.GetKey(KeyCode.DownArrow)) rot.y -= 1.0f;
		if (Input.GetKey(KeyCode.LeftArrow)) rot.x -= 1.0f;
		if (Input.GetKey(KeyCode.RightArrow)) rot.x += 1.0f;
		rot.Normalize();
		
		// Try and rotate the camera.
		if (!(rot.x == 0 && rot.y == 0))
			TheCamera.DisablePointing();
		TheCamera.transform.Rotate(rot.y, rot.x, 0);
	}
}
