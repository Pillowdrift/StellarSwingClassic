using UnityEngine;
using System.Collections;

public class EndCollisionCalculator : MonoBehaviour
{
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			//float herp = Vector3.Dot(col.contacts[0].normal,col.relativeVelocity) * rigidbody.mass;
		}
	}
}
