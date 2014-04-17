using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrappleRenderer : MonoBehaviour
{
	private GameObject ropeObject;
	private LineRenderer rope;
	private GrapplingHook grapplingHook;
	
	void Start()
	{
		ropeObject = GameObject.Find("Rope");
		rope = ropeObject.GetComponent<LineRenderer>();
//		rope.SetWidth(0.3f, 0.3f);
//		rope.enabled = false;
		grapplingHook = gameObject.GetComponent<GrapplingHook>();
	}
	
	void LateUpdate()
	{
		List<GrapplingHook.GrappleData> grapples = grapplingHook.GetGrapples();
		
		if (grapples != null)
		{
			rope.SetVertexCount(grapples.Count + 1);
			
			// Update rope position	
			int i = 0;
			foreach (GrapplingHook.GrappleData grap in grapples)
			{
				rope.SetPosition(i++, grap.gameObject.transform.TransformPoint(0, 0.3f, 0));
				//rope.SetPosition(i++, grap.gameObject.transform.position);
			}
			
			rope.SetPosition(i++, transform.position);
		}
	}
}
