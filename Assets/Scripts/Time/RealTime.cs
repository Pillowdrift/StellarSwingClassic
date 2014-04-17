using UnityEngine;
using System.Collections;

public class RealTime : MonoBehaviour
{
	public static float lastUpdate;
	public static float realDeltaTime = 0.0f;
	
	void Start()
	{
		lastUpdate = Time.realtimeSinceStartup;	
	}
	
	void Update()
	{
		realDeltaTime = Time.realtimeSinceStartup - lastUpdate;
		lastUpdate = Time.realtimeSinceStartup;
	}
}
