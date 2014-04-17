using UnityEngine;
using System.Collections;

public class SlowGlow : MonoBehaviour {
	
	public static float glowAmount = 0;
	const float RATE = 20;
	const float AMOUNT = 200;
	const float THRESHOLD = 10;
		
	Vector3 previousVelocity;
	
	// Use this for initialization
	void Start () {
		glowAmount = 0;
		previousVelocity = transform.parent.rigidbody.velocity;
	}
	
	// Update is called once per frame
	void Update () {	
		
		
//		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
//            Camera.main.transform.rotation * Vector3.up);		
		
		//glowAmmount -= RATE * Time.deltaTime;
		glowAmount = Mathf.Lerp(glowAmount, 0, RATE * Time.deltaTime);
		
		if(glowAmount < 0)
			glowAmount = 0;
		
		//Debug.Log(glowAmmount * AMMOUNT);
		
//		Color c = Color.blue;		
//		c.a = Mathf.Clamp01(glowAmmount * AMMOUNT);		
//		
//		renderer.material.color = c;
		
		//glowAmmount = 1.0f;
		
		transform.LookAt(transform.position + previousVelocity);
		GetComponent<ParticleSystem>().startSpeed = transform.parent.rigidbody.velocity.magnitude;
		GetComponent<ParticleSystem>().emissionRate = glowAmount * AMOUNT - THRESHOLD;
		
		//particleEmitter.Emit(Vector3.zero, previousVelosity, 1, 1, Color.white);
		//particleEmitter.Emit(50);
		
		previousVelocity = transform.parent.rigidbody.velocity;
	}
	
	public static void Score(float score)
	{
		glowAmount += score ;	
		//glowAmmount = Mathf.Clamp01(glowAmmount);
	}
}
