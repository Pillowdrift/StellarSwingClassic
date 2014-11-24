using UnityEngine;
using System.Collections;

public class Dasher : MonoBehaviour
{
	public float power = 150.0f;
	
	// The prefab of the object to destroy.
	public GameObject Riser;
	
	// The time interval between spawning.
	public float SpawnInterval = 0.3f;
	
	// A timer for creating the rising objects.
	private float timer = 0;
	
	// Generate the rising objects that come out of the flipper.
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > SpawnInterval)
		{
			// Spawn a Riser
			GameObject riser = (GameObject)Instantiate(Riser, transform.position, transform.rotation);
			riser.transform.localScale = new Vector3 (transform.lossyScale.x * 1000,
													  transform.lossyScale.y * 1000,
													  transform.lossyScale.z * 1000);
			timer = 0;
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		Rigidbody body = collider.rigidbody;
		
		if (body != null)
		{
			body.velocity = new Vector3(body.velocity.x, body.velocity.y, power);
		}
	}
}
