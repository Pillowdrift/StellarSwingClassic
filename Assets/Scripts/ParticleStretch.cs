using UnityEngine;
using System.Collections;

public class ParticleStretch : MonoBehaviour
{
	public const float STRETCHSCALE = 0.1f;
	public const float LERPSPEED = 1.0f;
	
	private ParticleRenderer particleRenderer;
	
	void Start()
	{
		particleRenderer = GetComponent<ParticleRenderer>();
	}
	
	void FixedUpdate()
	{
		float length = Camera.mainCamera.velocity.magnitude;
		
		if (length < 0.1f)
		{
			particleRenderer.particleRenderMode = ParticleRenderMode.Billboard;
		}
		else
		{
			particleRenderer.particleRenderMode = ParticleRenderMode.Stretch;
			
			// Add 1 to make sure it never gets down to 0
			GetComponent<ParticleRenderer>().lengthScale = Mathf.Lerp(GetComponent<ParticleRenderer>().lengthScale,
					       length * STRETCHSCALE + 1,
						   LERPSPEED * RealTime.realDeltaTime);
		}
	}
}
