using UnityEngine;
using System.Collections;

public class EnergyLossParticleController : MonoBehaviour {
	
	public static float glowAmount = 0;
	const float RATE = 15;
	const float AMOUNT = 100f;
	const float THRESHOLD = 0.01f;
	
	void Start () {
		glowAmount = 0;
	}
	
	void Update () {	
		glowAmount = Mathf.Lerp(glowAmount, 0, RATE * Time.deltaTime);
		
		if(glowAmount < 0)
			glowAmount = 0;
		
		GetComponent<ParticleSystem>().emissionRate = (glowAmount - THRESHOLD) * AMOUNT;
	}
	
	public static void Score(float score)
	{
		glowAmount += score ;	
	}
}
