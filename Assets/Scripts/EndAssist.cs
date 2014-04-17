using UnityEngine;
using System.Collections;

public class EndAssist : MonoBehaviour
{
	public Transform target;
	const float RATE = 1;
	
	void Start()
	{
		if (target == null)	
		{
			//Debug.Log("EndAssist target null");
			
			target = GameObject.Find("EndObject").transform;
		}
	}
	
	// Point the player towards the target
	void Update()
	{
		if(rigidbody.isKinematic)
			return;
		
		//Calculate the direction we want to face on a 2D plane
		Vector3 playerPos = transform.position;
		playerPos.y = 0;		
		Vector3 targetPos = target.position;
		targetPos.y = 0;		
		Vector3 targetDir = (targetPos - playerPos);
		targetDir.Normalize();	
		
		//Calculate the direction are going on a 2D plane
		Vector3 playerDir = rigidbody.velocity;
		playerDir.y = 0;
		float player2DSpd = playerDir.magnitude; // save our 2D speed
		playerDir.Normalize();
		
		// interpolate our current direction towards the target direction in 2D
		playerDir = Vector3.Lerp(playerDir, targetDir, RATE * Time.deltaTime);
		playerDir.Normalize();
		
		//Make sure we haven't changed speed
		playerDir *= player2DSpd;
		
		// add back the Y velosity
		playerDir.y = rigidbody.velocity.y;
		
		rigidbody.velocity = playerDir;
	}
}
