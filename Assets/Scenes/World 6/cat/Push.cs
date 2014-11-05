using UnityEngine;
using System.Collections;

public class Push
	: MonoBehaviour
{
	public Vector3 lastColliderPos;

	public void OnTriggerEnter(Collider other)
	{
		lastColliderPos = other.transform.position;
	}

	public void OnTriggerStay(Collider other)
	{
		Vector3 diff = other.transform.position - lastColliderPos;
		lastColliderPos = other.transform.position;

		transform.position += diff;
	}
}
