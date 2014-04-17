using UnityEngine;
using System.Collections;

public class BuoyancyController : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		Rigidbody rigidbody = collider.gameObject.GetComponent<Rigidbody>();
		Floater floater = collider.gameObject.GetComponent<Floater>();
		
		if (rigidbody != null)
		{
			if (floater != null)
			{
				float top = transform.position.y + this.collider.bounds.extents.y;
				
				if (top > floater.waterLevel || !floater.enabled)
					floater.waterLevel = top;
				floater.enabled = true;
			}
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		Floater floater = collider.gameObject.GetComponent<Floater>();
		float top = transform.position.y + this.collider.bounds.extents.y;
		if (floater != null && floater.waterLevel == top)
		{
			floater.enabled = false;
		}
	}
}
