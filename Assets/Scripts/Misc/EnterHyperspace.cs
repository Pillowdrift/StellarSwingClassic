#define GO_BACK_TO_TITLE

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class EnterHyperspace : MonoBehaviour
{
	// Public vars
	public float ShrinkSpeed = 0.99f;
	public float PushPower = 100.0f;
	public float ShrinkPushPower = 20.0f;
	public float BeforeShrinkTime = 2.0f;
	public float AfterShrinkTime = 1.0f;
	public string WorldToLoad = "";
	
	// Private vars
	private float beforeShrinkTimer;
	private float afterShrinkTimer;
	
	private int nextWorld;
	
	// Use this for initialization
	void Start() 
	{
		// Set the timer.
		beforeShrinkTimer = BeforeShrinkTime;
		afterShrinkTimer = AfterShrinkTime;
		
		// Parse level name
		Regex exp = new Regex("World (?<world>.*) Level (?<level>.*)");
		MatchCollection matchList = exp.Matches(WorldToLoad);
		
		if (matchList.Count > 0)
		{
			// Parse level number
			int levelToShow = 0;
			int.TryParse(matchList[0].Groups["level"].ToString(), out levelToShow);
			int.TryParse(matchList[0].Groups["world"].ToString(), out nextWorld);
		}
		
		// At first give a short push.
		rigidbody.AddForce(Vector3.forward * PushPower);
		
		StartCoroutine("DoWarp");
	}
	
	IEnumerator DoWarp()
	{
		SoundManager.Play("warp");
		
		yield return new WaitForSeconds(beforeShrinkTimer);
		
		float shrinkTimer = afterShrinkTimer;
		
		while (shrinkTimer > 0)
		{
			// Update timer
			shrinkTimer -= Time.deltaTime;
			
			// Now shrink the ship really quickly and push it forward more.
			if (transform.localScale.magnitude > 0.1f)
				transform.localScale *= ShrinkSpeed;
			else
				renderer.enabled = false;
			
			rigidbody.AddForce(transform.forward * ShrinkPushPower);
			
			// Wait a frame
			yield return 0;
		}
		
#if GO_BACK_TO_TITLE
		LevelSelectGUI.worldToShow = "World" + (nextWorld - 1);
		LevelSelectGUI.levelToShow = 0;
		LevelSelectGUI.worldTransition = true;
		Application.LoadLevel("Title");
#else
		LevelSelectGUI.worldToShow = "World" + (nextWorld);
		LevelSelectGUI.levelToShow = 0;
		LevelSelectGUI.worldTransition = false;
		Loading.Load(WorldToLoad);
#endif
	}
}
