using UnityEngine;
using System.Collections;

public static class Helper
{
	public struct ClipPlanePoints
	{
		public Vector3 UpperLeft;
		public Vector3 UpperRight;
		public Vector3 LowerLeft;
		public Vector3 LowerRight;
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		do
		{
			if (angle < -360)
				angle += 360;
			else if (angle > 360)
				angle -= 360;
		}
		while (angle < -360 || angle > 360);
		
		return Mathf.Clamp(angle, min, max);
	}
	
	public static ClipPlanePoints ClipPlaneAtNear(Vector3 position)
	{
		ClipPlanePoints clipPlanePoints = new ClipPlanePoints();
		
		if (Camera.mainCamera != null)
		{
			// Get values from camera
			Transform transform = Camera.mainCamera.transform;
			float halfFov = Camera.mainCamera.fov * Mathf.Deg2Rad / 2.0f;
			float aspect = Camera.mainCamera.aspect;
			float distance = Camera.mainCamera.nearClipPlane;
			
			// Calculate width and height of clip plane
			float halfHeight = distance * Mathf.Tan(halfFov);
			float halfWidth = halfHeight * aspect;
			
			// Calculate corners of clip plane
			clipPlanePoints.LowerRight = position + transform.right * halfWidth;
			clipPlanePoints.LowerRight -= transform.up * halfHeight;
			clipPlanePoints.LowerRight += transform.forward * distance;
			
			clipPlanePoints.LowerLeft = position - transform.right * halfWidth;
			clipPlanePoints.LowerLeft -= transform.up * halfHeight;
			clipPlanePoints.LowerLeft += transform.forward * distance;
			
			clipPlanePoints.UpperRight = position + transform.right * halfWidth;
			clipPlanePoints.UpperRight += transform.up * halfHeight;
			clipPlanePoints.UpperRight += transform.forward * distance;
			
			clipPlanePoints.UpperLeft = position - transform.right * halfWidth;
			clipPlanePoints.UpperLeft += transform.up * halfHeight;
			clipPlanePoints.UpperLeft += transform.forward * distance;
		}
		
		return clipPlanePoints;
	}
}
