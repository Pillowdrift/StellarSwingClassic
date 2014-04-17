using UnityEngine;
using System.Collections;

public class UnlockAchievement : MonoBehaviour {
	
	public string AchievementKey;

	// Use this for initialization
	void Start () {
		GameCenterSingleton.Instance.AddAchievementProgress(AchievementKey, 100.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
