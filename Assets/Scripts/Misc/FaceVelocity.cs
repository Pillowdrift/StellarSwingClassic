using UnityEngine;
using System.Collections;

public class FaceVelocity : MonoBehaviour {
	
	// Keep track of position for the speed.
	Vector3 oldPos;
	
	void Start() {
		oldPos = Vector3.zero;	
	}
	
	// Update is called once per frame
	void Update () {
		// Calculate velocity
		Vector3 vel = transform.position - oldPos;
		oldPos = transform.position;
		
		// Face our velocity
		transform.LookAt(transform.position + vel);
	}
}
