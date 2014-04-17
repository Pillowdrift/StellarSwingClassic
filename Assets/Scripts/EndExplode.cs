using UnityEngine;
using System.Collections;

public class EndExplode : MonoBehaviour
{
	GameObject endObject;
	const float EXPLOSION_FORCE = 2;
	
	// Use this for initialization
	void Start()
	{
		
	}
	
	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			float explosionForce = collision.gameObject.rigidbody.velocity.magnitude * EXPLOSION_FORCE;
			Rigidbody[] segments = GetComponentsInChildren<Rigidbody>();
			
			foreach(Rigidbody segment in segments)
			{
				segment.AddExplosionForce(explosionForce, collision.gameObject.transform.position, explosionForce);
			}
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
