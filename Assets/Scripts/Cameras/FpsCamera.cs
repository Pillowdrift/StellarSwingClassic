using UnityEngine;
using System.Collections;

public class FpsCamera : MonoBehaviour 
{
	const float WORLD_RETURN_RATE = 5.0F;	
	const float CAM_RETURN_RATE = 1.0F;	
	const float SPIN_VELOSITY_DAMPEN_RATE = 0.02F;
	
	public GameObject player;	
	
	//where the camera wants to look
	Vector2 target;
	
	Vector2 lookat;
	
	//interpolation smoothing for rotations created the player changeing direstion
	Vector2 worldInterp;
	
	//interpolation smoothing for rotations created the player rotating the camera
	Vector2 camInterp1;
	
	//Seccond level of camera interpolation
	Vector2 camInterp2;	
	
	public float sensitivityX;
	public float sensitivityY;
	
	
	void Start () 
	{
		
	}
	public void Reset()
	{
		
	}
	
	void Update ()
	{
		AddPitchYaw(ref lookat, transform.position - CamTarget.GetTarget());
		
		if(player.rigidbody.velocity.magnitude > 1)
		{
			AddPitchYaw(ref target, -player.rigidbody.velocity);		
		}	
		
//		if(Input.GetMouseButton(0) && player.rigidbody.velocity.magnitude > 1)
//		{				
//			velocityAngle += Input.GetAxis("Mouse X") * sensitivityX;
//			
//			float spinVelocityDampen = 1-(angle * angle * SPIN_VELOSITY_DAMPEN_RATE * Time.deltaTime);
//			
//								
//			Vector3 TargetVel = Quaternion.Euler(0, angle, 0) *
//								player.rigidbody.velocity *
//								Mathf.Clamp01(spinVelocityDampen);
//			
//			player.transform.LookAt(TargetVel);
//			
//			
//			player.rigidbody.velocity = Vector3.Lerp(player.rigidbody.velocity, TargetVel,0)			
			
			

//			camInterp2 += mouse;
//			
//			// in we spin 365 this makes it so it only spinns back 5
//			shrinkAngle(ref camInterp2);
//			
//			
//			camInterp1 = camInterp2;
//		}
		//else
				
			// camInterp1 interpolates towards 0 and camInterp2 interpolates towards camInterp1
			// provided they both start at the same place this gives camInterp2 a smooth acceleration and 
			// deceleration
//			camInterp1 = Vector2.Lerp(camInterp1, Vector2.zero, CAM_RETURN_RATE * Time.deltaTime); 
//			camInterp2 = Vector2.Lerp(camInterp2, camInterp1, CAM_RETURN_RATE * Time.deltaTime);
		

		Vector3 toLookatPos = transform.position - CamTarget.GetTarget();
		AddPitchYaw(ref lookat, toLookatPos);
		Vector2 midpointGase = Vector2.Lerp(target, lookat, CamTarget.sWeight);
		
		worldInterp = Vector2.Lerp(worldInterp, midpointGase, CamTarget.sRate * Time.deltaTime);			


		
		transform.localEulerAngles = worldInterp;// + camInterp2;
		
		//shrinkAngle(ref target);
		
		transform.position = player.transform.position;
	}
	
	//reduces angles to within the range -180 to 180
	void ShrinkAngle(ref Vector2 a)
	{
		while (a.x < -180) a.x += 360;
        while (a.x > 180) a.x -= 360;
		while (a.y < -180) a.y += 360;
        while (a.y > 180) a.y -= 360;
	}
	
	float AngleDiff(float firstAngle, float secondAngle)
  	{
        float difference = secondAngle - firstAngle;
        while (difference < -180) difference += 360;
        while (difference > 180) difference -= 360;
        return difference;
 	}
	
	//converts a vector3 into a Pitch&Yaw and sets a seccond vector to it 
	Vector3 AddPitchYaw(ref Vector2 outVec, Vector3 inVec)
	{
		Vector3 a = new Vector2(Mathf.Atan2(inVec.y, Mathf.Sqrt(
				(inVec.x * inVec.x)+(inVec.z * inVec.z))) * Mathf.Rad2Deg,
				-Mathf.Atan2(inVec.x, -inVec.z) * Mathf.Rad2Deg);	
		
		// by adding the diffrence in the angles to the anges
		// it eliminates errors caused by crossing -180 and 180
		outVec.x += AngleDiff(outVec.x, a.x);
		outVec.y += AngleDiff(outVec.y, a.y);	
		
		return outVec;
	}
}
