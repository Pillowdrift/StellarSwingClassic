using UnityEngine;
using System.Collections;

public class GrappleGuideParent : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{			
		transform.LookAt(transform.position + transform.parent.rigidbody.velocity);
	
	}
}
