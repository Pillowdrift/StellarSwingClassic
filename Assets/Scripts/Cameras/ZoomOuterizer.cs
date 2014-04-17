using UnityEngine;
using System.Collections;

public class ZoomOuterizer : MonoBehaviour {
	
	// Private Variables.
	private bool zoomingOut = false;
	private float zoomingOutVal = 0.0f;
	private float zoomingOutTime = 0.0f;
	private float zoomingOutTimer = 0.0f;
	private Vector3 originalCameraPosition = Vector3.zero;
	
	public void ZoomOut(float _val, float _time) {
		// Slowly start zooming out.
		zoomingOut = true;
		zoomingOutVal = _val;
		zoomingOutTime = _time;
		zoomingOutTimer = _time;
		originalCameraPosition = Camera.mainCamera.transform.position;
		Camera.mainCamera.GetComponent<ThirdPersonCamera>().enabled = false;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// If we are zooming out, start.
		if (zoomingOut) {
			zoomingOutTimer -= RealTime.realDeltaTime;
			if (zoomingOutTimer < 0) {
				zoomingOut = false;
			}			
			Camera.mainCamera.transform.position = originalCameraPosition;
			Camera.mainCamera.transform.Translate(new Vector3(0, 0, -(1-(zoomingOutTimer / zoomingOutTime)) * zoomingOutVal));
		}
	}
}
