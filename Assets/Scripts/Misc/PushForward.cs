using UnityEngine;
using System.Collections;

public class PushForward : MonoBehaviour {
	
	// Public variables.
	public float Power = 1.0f;
	public Vector3 Dir = Vector3.forward;
	
	// Use this for initialization
	void Start () {
		// Push the rigid body in this direction.
		rigidbody.AddRelativeForce(Dir * Power);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
