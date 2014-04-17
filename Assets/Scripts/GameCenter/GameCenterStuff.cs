using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterStuff : MonoBehaviour {
	
	public GameCenterSingleton instance;
	
	// Use this for initialization
	void Start () {
#if UNITY_IPHONE
		instance = GameCenterSingleton.Instance;
#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public static void Auth()
	{
		
	}
	
}
