using UnityEngine;
using System.Collections;

public class Antigrav : MonoBehaviour
{
	public Vector3 force = Vector3.zero;

	public void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			Physics.gravity = 40.0f * transform.up;
		}
	}
}
