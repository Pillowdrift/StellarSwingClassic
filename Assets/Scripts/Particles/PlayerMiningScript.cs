using UnityEngine;
using System.Collections;

public class PlayerMiningScript : MonoBehaviour
{
	
	// Public vars
	public float MiningSpeed = 50.0f;
	
	// Private vars
	private bool mining = false;
	private bool count = false;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Slowly reduce the energy meter.
		if (mining && count) 
		{
			ScoreCalculator.Speed -= MiningSpeed * Time.deltaTime;	
			if (ScoreCalculator.Speed <= 0) 
			{
				ScoreCalculator.Speed = 0;
				EndMining();
			}
		}
	}
	
	// Start Mining
	void StartMining ()
	{
		// Enable the particle emmitter
		transform.FindChild ("Mining").gameObject.SendMessage ("EnableParticles", true);	
		transform.FindChild ("EndBeam").gameObject.SendMessage ("StartMining");

		mining = true;
		count = true;
		SoundManager.Play ("drill");
		
		// Set the glow
		transform.FindChild ("Glow").gameObject.renderer.material.SetColor ("_TintColor", Color.white);
	}	
	
	// Stop mining
	void EndMining ()
	{
		transform.FindChild ("Mining").gameObject.SendMessage ("EnableParticles", false);
		transform.FindChild ("EndBeam").gameObject.SendMessage ("EndMining");

		mining = false;
		SoundManager.StopAll ();
	}
	
	void StopCounting ()
	{
		count = false;
		ScoreCalculator.Speed = 0;
	}
}
