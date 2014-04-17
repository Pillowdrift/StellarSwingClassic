using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	
	// Public vars
	public float FireTimer = 1.0f;
	public float SmokeTimer = 3.0f;
	
	public ParticleSystem Fire;
	public ParticleSystem Smoke;
	
	// Timers
	private float fireTime;
	private float smokeTime;
	
	// Use this for initialization
	void Start () {
	}
	
	void Awake() 
	{
		fireTime = FireTimer;
		smokeTime = SmokeTimer;
		Smoke.enableEmission = false;
		Fire.enableEmission = true;
	}
	
	// Update is called once per frame
	void Update () {
		fireTime -= Time.deltaTime;
		smokeTime -= Time.deltaTime;
		
		if (fireTime < 0)
		{
			Fire.enableEmission = false;
			Smoke.enableEmission = true;
			if (smokeTime < 0)
			{
				Smoke.enableEmission = false;
				
				Destroy(gameObject);
			}			
		}
	}
}
