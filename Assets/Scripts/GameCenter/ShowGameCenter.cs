using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;

public class ShowGameCenter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUp()
	{
		GameCenterSingleton.Instance.ShowAchievementUI();
	}
}
