using UnityEngine;
using System.Collections;

public class UnparentAndFollow : MonoBehaviour {
	
	// Private Vars
	private Transform oldParent;
	
	// Use this for initialization
	void Start () {
		// Save our parenr
		oldParent = transform.parent;
		
		// Detach
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = oldParent.transform.position;
	}
}
