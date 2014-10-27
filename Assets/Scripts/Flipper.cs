using UnityEngine;
using System.Collections;

public class Flipper : MonoBehaviour
{
	public float power = 10.0f;
	
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
			float dp = power - body.velocity.y;

			float lastSpeedSqr = body.velocity.sqrMagnitude;

			body.velocity = new Vector3(body.velocity.x, power, body.velocity.z);

			float speedSqr = body.velocity.sqrMagnitude;

			// Notify scorecalucator of energy added (k = (1/2)mv^2)
			float k0 = 0.5f * body.mass * lastSpeedSqr;
			float k1 = 0.5f * body.mass * speedSqr;
			float dk = k1 - k0;

			ScoreCalculator.addEnergy(dk);
		}
	}
}
