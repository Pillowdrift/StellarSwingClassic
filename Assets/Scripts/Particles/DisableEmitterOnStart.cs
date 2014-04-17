using UnityEngine;
using System.Collections;

public class DisableEmitterOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Disable the particle emitter on start
		GetComponent<ParticleSystem>().enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Enabled
	void EnableParticles(bool v)
	{
		GetComponent<ParticleSystem>().enableEmission = v;
	}
}
