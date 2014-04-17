using UnityEngine;
using System.Collections;

public class GrappleGuide : MonoBehaviour
{
	public Transform grappleTo;
	
	public void Start()
	{
		Physics.IgnoreCollision(collider, GameObject.Find("Player").collider);
	}
	
	public Vector3 GrappleTo()
	{
		return grappleTo.position;
	}
}
