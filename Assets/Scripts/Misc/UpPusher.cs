using UnityEngine;
using System.Collections;

public class UpPusher : MonoBehaviour
{
	public float strength = 0.0f;
	public Vector3 direction = Vector3.zero;
	
	private Vector3 force;
	
	public void Start()
	{
		force = strength * direction.normalized;
	}
	
	public void OnTriggerStay(Collider collider)
	{
		//collider.rigidbody.AddForce(force);
	}
	
	public void OnTriggerEnter()
	{
		Physics.gravity *= -1;
	}
	
	public void OnTriggerExit()
	{
		Physics.gravity *= -1;
	}
}
