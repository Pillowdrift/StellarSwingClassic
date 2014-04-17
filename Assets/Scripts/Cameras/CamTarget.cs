using UnityEngine;
using System.Collections;

public class CamTarget : MonoBehaviour
{
	public float weight = 1;
	public float rate = 1;
	public float returnRate = 1;
	public Transform target = null;
	// if the grapple if attached to this gameobject proform Grappled
	public Transform disableOnceGrappledTo = null;
	public bool disableOnceGrappled = false;
	public Transform afterGrappleTarget = null;
	public bool disableOnceUnGrappled = false;
	public Transform afterUnGrappleTarget = null;
	
	public static float sWeight = 0;
	public static float sRate = 1;
	static Transform sTarget = null;
	static bool sDisableOnceGrappled = false;
	static Transform sAfterGrappleTarget = null;
	static bool sDisableOnceUnGrappled = false;
	static Transform sAfterUnGrappleTarget = null;
	static int sLastActivatedID = 0;
	static Transform sGrappleAttachedTo = null;
	
	void Start()
	{
		if(disableOnceGrappledTo == null)
			disableOnceGrappledTo = target;
			
		Disable(1);
	}
	
	void Update()
	{
	}
	
	// When you go into the zone.
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Player")
		{
			Enable();
		}
	}
	
	// When you exit the zone.
	void OnTriggerExit(Collider other)
	{
		if(sLastActivatedID == gameObject.GetInstanceID())
		{
			Disable(returnRate);
		}
	}
	
	
	public static void Grappled(Transform grappleTarget)
	{	
		if(sDisableOnceGrappled)
			Disable(1);
		if(sAfterGrappleTarget != null)
			sTarget = sAfterGrappleTarget;
		
		sGrappleAttachedTo = grappleTarget;
	}
	
	public static void UnGrappled()
	{
		if(sDisableOnceUnGrappled)
			Disable(1);
		
		if(sAfterUnGrappleTarget != null)
			sTarget = sAfterUnGrappleTarget;
		
		sGrappleAttachedTo = null;
	}
	
	// Get the target to look at.
	public static Vector3 GetTarget()
	{
		if(sTarget != null)
			return sTarget.transform.position;
		else
			return Vector3.zero;
	}
	
	static void Disable(float returnRate)
	{
		sWeight = 0;
		sRate = returnRate;
		sTarget = null;
		sDisableOnceGrappled = false;
		sAfterGrappleTarget = null;
		sDisableOnceUnGrappled = false;
		sAfterUnGrappleTarget = null;
	}
	
	void Enable()
	{
		sLastActivatedID = gameObject.GetInstanceID();
		sWeight = weight;
		sRate = rate;
		sDisableOnceGrappled = disableOnceGrappled;
		sAfterGrappleTarget = afterGrappleTarget;
		sDisableOnceUnGrappled = disableOnceUnGrappled;
		sAfterUnGrappleTarget = afterUnGrappleTarget;
		
		if(target != null)
			sTarget = target;
		
		if(disableOnceGrappledTo == sGrappleAttachedTo)
			Grappled(disableOnceGrappledTo);
	}
}
