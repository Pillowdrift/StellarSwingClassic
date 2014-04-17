using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {
	
	// Public variables.
	public float MaxBright = 8;
	public float BrightTime = 1.0f;
	
	// Private variables
	private bool entered = false;
	private float brightTimer = 100.0f;
	
	// Flash when triggered.
	void OnTriggerEnter(Collider collider) {
		entered = true;	
		brightTimer = BrightTime;
		//GameObject portal = GameObject.Find("Portal").GetComponent<Grower>().rate = -500;
		Destroy(GameObject.Find("Clouds"));
		foreach (Renderer renderer in collider.GetComponentsInChildren<Renderer>())
			renderer.enabled = false;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (entered) {
			// Reduce timer.
			brightTimer -= Time.deltaTime;
			if (brightTimer < 0) {
				// Disable the particles.
				for (int i = 0; i < transform.GetChildCount(); ++i) {
					transform.GetChild(i).GetComponent<ParticleSystem>().emissionRate = 0;	
				}
			}
			
			// Set the light brightness
			gameObject.light.intensity = (brightTimer / BrightTime) * MaxBright;
		}
	}
}
