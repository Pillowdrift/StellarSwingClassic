using UnityEngine;
using System.Collections;

public class PointAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.mainCamera.transform.position);
		transform.Rotate(Vector3.right, 90);
	}
}
