using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Floater : MonoBehaviour
{
	public float buoyancy = 1.0f;
	public float waterLevel, floatHeight = 1;
	public Vector3 buoyancyCentreOffset;
	public float bounceDamp = 0.05f;
	
	void Awake()
	{
		enabled = false;
	}
	
	void FixedUpdate()
	{		
		Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
		
		// Calculate buoyancy
		float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
		
		if (forceFactor > 0f)
		{
			Vector3 uplift = -Physics.gravity * buoyancy * (forceFactor - rigidbody.velocity.y * bounceDamp) * 0.9f;
			
			if (float.IsNaN(uplift.sqrMagnitude))
				throw new UnityException("Invalid buoyancy");
			
			rigidbody.AddForceAtPosition(uplift, actionPoint);
		}
	}
}
