using UnityEngine;
using System.Collections;

public class TriggerArea : MonoBehaviour {

	public GameObject LookAtMe;
	public Vector3 VectorToLookAt;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Player") {
			LookAtMe.transform.position = VectorToLookAt;
		}
	}
}
