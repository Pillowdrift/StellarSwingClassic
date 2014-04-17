using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{	
	// Public vars
	public float distanceFromCamera = 40.0f;
	public float disableTime = 2.0f;
	public bool avoidOcclusion = true;
	
	public float forwardOcclusionSmoothing = 0.5f;
	public float returnOcclusionSmoothing = 0.1f;
	
	public float rotationStep = 5.0f;
	public float maxRotation = 45.0f;
	
	public Vector3 offset = Vector3.zero;
	
	// Private vars
	private ThirdPersonCamera instance;
	
	private float dontPointTimer = 0.0f;
	private float realDistance;
	private GameObject target = null;
	
	private float velf = 0.0f;
	
	// Types
	public struct Plane
	{
		public Vector3 TopLeft;
		public Vector3 TopRight;
		public Vector3 BottomLeft;
		public Vector3 BottomRight;
	}
	
	// Disable the camera pointing at stuff.
	public void DisablePointing()
	{
		dontPointTimer = disableTime;
	}
	
	void Awake()
	{
		target = GameObject.Find("Player");
		realDistance = distanceFromCamera;
	}
	
	void Update()
	{
		Quaternion targetRot = transform.rotation;
		
		// Stop if we have no target.
		if (target == null)
			return;
		
		// If we aren't pointing
		if (!PauseButton.paused)
		{
			dontPointTimer -= Time.deltaTime;
			if (dontPointTimer < 0)
			{
				// Set the rotation.
				Quaternion desired = CalculateDesiredRotation();
				targetRot = Quaternion.Slerp(transform.rotation, desired, CamTarget.sRate * Time.deltaTime);
			}
		}
		
		if (avoidOcclusion)
		{
			// Calculate nearest point of occlusion
			Plane nearPlane = CalculateNearPlane(transform.position);
			float occlusionDistance = FindMinimumOcclusionDistance(target.transform.position, nearPlane);
						
			// Move camera to point of occlusion
			if (occlusionDistance > 0)
				realDistance = Mathf.SmoothDamp(realDistance, occlusionDistance, ref velf, forwardOcclusionSmoothing);
		}
		
		Vector3 targetPos = target.transform.position + offset - transform.forward * distanceFromCamera;// + (transform.forward * -1 * realDistance);
			
		// Move back a bit.
		//transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10.0f);
		transform.position = targetPos;
		transform.rotation = targetRot;
	}
	
	// Get the desired rotation.
	Quaternion CalculateDesiredRotation()
	{
		// The look at position quaternion	
		Quaternion lookAtRot = Quaternion.LookRotation(CamTarget.GetTarget() - target.transform.position);
		
		// The rotation of the player.
		Quaternion forwardRot = Quaternion.LookRotation(target.transform.forward);
		
		// Use the variables in the trigger area to calculate the 
		// desired rotation.
		Quaternion rot = Quaternion.Lerp(forwardRot, lookAtRot, CamTarget.sWeight);
			
		return rot;
	}
	
	Plane CalculateNearPlane(Vector3 position)
	{
		Plane nearPlane = new Plane();
		
		if (camera != null)
		{
			// Get values from camera
			float halfFov = camera.fov * Mathf.Deg2Rad / 2.0f;
			float aspect = camera.aspect;
			float distance = camera.nearClipPlane;
			
			// Calculate width and height of clip plane
			float halfHeight = distance * Mathf.Tan(halfFov);
			float halfWidth = halfHeight * aspect;
			
			// Calculate corners of clip plane	
			nearPlane.BottomRight = position + transform.right * halfWidth;
			nearPlane.BottomRight -= transform.up * halfHeight;
			nearPlane.BottomRight += transform.forward * distance;
			
			nearPlane.BottomLeft = position - transform.right * halfWidth;
			nearPlane.BottomLeft -= transform.up * halfHeight;
			nearPlane.BottomLeft += transform.forward * distance;
			
			nearPlane.TopRight = position + transform.right * halfWidth;
			nearPlane.TopRight += transform.up * halfHeight;
			nearPlane.TopRight += transform.forward * distance;
			
			nearPlane.TopLeft = position - transform.right * halfWidth;
			nearPlane.TopLeft += transform.up * halfHeight;
			nearPlane.TopLeft += transform.forward * distance;
		}
		
		return nearPlane;
	}
	
	float FindMinimumOcclusionDistance(Vector3 position, Plane to)
	{
		float nearestOcclusion = -1.0f;
		
		RaycastHit hitInfo;
		
		if (Physics.Linecast(position, to.TopLeft, out hitInfo) && !hitInfo.collider.gameObject.CompareTag("Player"))
			nearestOcclusion = hitInfo.distance;
		
		if (Physics.Linecast(position, to.TopRight, out hitInfo) && !hitInfo.collider.gameObject.CompareTag("Player"))
			if (hitInfo.distance < nearestOcclusion)
				nearestOcclusion = hitInfo.distance;
		
		if (Physics.Linecast(position, to.BottomLeft, out hitInfo) && !hitInfo.collider.gameObject.CompareTag("Player"))
			if (hitInfo.distance < nearestOcclusion)
				nearestOcclusion = hitInfo.distance;
		
		if (Physics.Linecast(position, to.BottomRight, out hitInfo) && !hitInfo.collider.gameObject.CompareTag("Player"))
			if (hitInfo.distance < nearestOcclusion)
				nearestOcclusion = hitInfo.distance;
		
		return nearestOcclusion;
	}
	
	void OnGUI2() {

	}
}
