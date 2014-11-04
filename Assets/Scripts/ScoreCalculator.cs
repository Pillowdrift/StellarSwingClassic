using UnityEngine;
using System;
using System.Collections;

public class ScoreCalculator : MonoBehaviour
{	
	public static string TimeString
	{
		get
		{
			TimeSpan ts = TimeSpan.FromSeconds(time);
			
			string format = "{1:D2}.{2:D2}";
			if (ts.Minutes > 0)
				format = "{0:D2}:" + format;
			
			return String.Format(format, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
		}
	}
	
	public static float Energy { get { return energy; } set {energy = value; } }
	public static float Speed { get { return speed; } set {speed = value; } }
	
	public static float finalScore = 0.0f;
	public static float finalTime = 0.0f;
	public static float finalSpeed = 0.0f;
	
	public static string finalScoreString = "";
	public static string finalTimeString = "";	
	public static string finalSpeedString = "";	
	
	public static float energy;
	
	public static float LastEnergyDelta { get { return lastEnergy - energy; } }
	
	static float time;
	static float lastEnergy;
	static float speed = 0;
	
	static float endHeight;	
	static float maxEnergy;	
	
	void Start()
	{
		GameObject endObj = GameObject.Find("EndObject");
		GameObject player = GameObject.Find("Player");
		
//		float endRadius = endObj.GetComponent<SphereCollider>().radius;
//		//convert the local bottom of the end object into world space
//		float endWorldHeight = endObj.transform.TransformPoint(0,-endRadius,0).y;
		
		endHeight = -endObj.transform.position.y;// player.transform.position.y - endWorldHeight;
		
		float kineticEnergy = 0.5f * rigidbody.mass * player.GetComponent<PlayerMovements>().startingVelocity.sqrMagnitude;
		float maxGravEnergy = rigidbody.mass * (-Physics.gravity.y) * endHeight;	
		
		maxEnergy = kineticEnergy + maxGravEnergy;
	}
	
	void FixedUpdate()
	{
		if (!GameRecorder.playingBack && !LevelState.Dead)
		{
			time += Time.deltaTime;
			
			float kineticEnergy = 0.5f * rigidbody.mass * rigidbody.velocity.sqrMagnitude;
			float gpe = rigidbody.mass * (-Physics.gravity.y) * (endHeight + transform.position.y);		
			//float gravitationalPotentialEnergy = rigidbody.mass * (-Physics.gravity.y) * transform.position.y;		
			energy = kineticEnergy + gpe;
			
			float energyDelta = lastEnergy - energy;
					
			if(energyDelta > 0)
			{
				EnergyLossParticleController.Score(energyDelta);
				if(energyDelta > 1)
				{
					transform.LookAt(transform.position + rigidbody.velocity);
				}
			}
			
			speed = (energy / maxEnergy) * 100.0f;
			
			if (!LevelStart.started)
				speed = 100.0f;
			
			lastEnergy = energy;
			
			//speed = rigidbody.velocity.magnitude;	
		}
	}
	
	public static void Reset()
	{
		if (!GameRecorder.playingBack)
		{				
			energy = 0.0f;
			time = 0.0f;
			lastEnergy = 0.0f;
			speed = 0;
			
			finalScore = 0.0f;
			finalTime = 0.0f;
			finalSpeed = 0.0f;
		}
	}
	
	public static void End()
	{
		if (!GameRecorder.playingBack)
		{
			finalScore = energy;
			finalTime = time;
			finalSpeed = speed;
			
			finalScoreString = "Score: " + ((int)energy).ToString();
			finalTimeString = "Time: " + TimeString;
			finalSpeedString = "Speed: " + ((int)speed).ToString() + "%";

			Debug.Log("Final energy (raw): " + Speed);
		}
	}
}
