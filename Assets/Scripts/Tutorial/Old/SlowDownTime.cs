using UnityEngine;
using System.Collections;

public class SlowDownTime : MonoBehaviour {
	
	// How fast to lerp
	public float LERPSPEED = 10.0f;
	
	// Whether or not we are activated.
	private bool activated = false;
	
	// The current timescale.
	private float timeScale = 1.0f;
	
	public MeshRenderer outline;
	
	// Activates the slow down.
	public void ActivateSlowDown()
	{
		activated = true;
	}
	
	// Deactivate the slow down.
	public void DeactivateSlowDown()
	{
		activated = false;
		Time.timeScale = 1.0f;
	}
	
	void OnTriggerEnter()
	{
		//ActivateSlowDown();
	}
	
	void OnTriggerExit()
	{
		//DeactivateSlowDown();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (activated)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, 0.0f, LERPSPEED * RealTime.realDeltaTime);
			if (Time.timeScale < 0.15f)
			{
				if (Time.timeScale < 0.1f)					
						Time.timeScale = 0;
			}
		}
	}
}
