using UnityEngine;
using System.Collections;

public class EndBeam : MonoBehaviour
{
	float fadeTimer = 0.0f;
	const float FADEOUT_DURATION = 4.0f;
	bool fadeout = false;
	
	float beamStartingWidth;
	
	LineRenderer beam;

	// Use this for initialization
	void Start()
	{
		beam = gameObject.GetComponent<LineRenderer>();
		fadeout = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(!fadeout)
			return;
		
		if(fadeTimer > FADEOUT_DURATION)
		{
			beam.enabled = false;
			fadeout = false;
			return;
		}
		
		float a = 1.0f - (fadeTimer / FADEOUT_DURATION); 
		
		beam.SetColors(new Color(1, 1, 1, a), new Color(1, 1, 1, a));
		beam.SetWidth(a * 10.0f, a * 10.0f);
		
		fadeTimer += Time.deltaTime;
	}
	
	public void StartMining()
	{		
		GetComponent<ParticleSystem>().enableEmission = true;
		beam.enabled = true;
		
		beam.SetPosition(1, transform.position);
		
		transform.LookAt(new Vector3(50, 0, 0));
	}
		
	public void EndMining()
	{		
		GetComponent<ParticleSystem>().enableEmission = false;
		fadeout = true;
	}		
}
