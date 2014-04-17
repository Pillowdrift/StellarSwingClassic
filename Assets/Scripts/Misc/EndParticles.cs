using UnityEngine;
using System.Collections;

public class EndParticles : MonoBehaviour
{
	private ParticleSystem.Particle[] particles;
	
	void LateUpdate()
	{
		particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
		GetComponent<ParticleSystem>().GetParticles(particles);
		
		// Accelerate all particles towards centre
		for (int i = 0; i < particles.Length; ++i)
		{
			const float G = 1000.0f;
			
			Vector3 diff = transform.position - particles[i].position;
			
			float rSQuared = diff.sqrMagnitude;
			float r = diff.magnitude;
			
			if (r < 1.0f)
			{
				particles[i].lifetime = 0;
				particles[i].position = transform.position;
				particles[i].velocity = Vector3.zero;
				continue;
			}
			
			float F = G / rSQuared;
			
			float a = F;
			
			//float squareSeconds = Time.deltaTime * Time.deltaTime;
			
			//a *= squareSeconds;
			
			Vector3 acceleration = (transform.position - particles[i].position).normalized * a;
			
			particles[i].velocity += acceleration;
		}
		
		GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
	}
}
