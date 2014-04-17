using UnityEngine;
using System.Collections;

public class DragToTurn : MonoBehaviour
{
	private bool tutorialEnabled = false;
	public GUITexture finger;
	
	const float SWIPE_DISTANCE = 0.25f;
	const float SWIPE_DURATION = 3.0f;
	float time = 0;
	
	void EnableTutorial()
	{
		GUIController.DisableButtons();
		GUIController.ShowText("TutorialText");
		
		finger.enabled = true;
		
		Time.timeScale = 0.0f;
		tutorialEnabled = true;
	}
	
	void DisableTutorial()
	{
		GUIController.GUILevelPlay();
		GUIController.HideText("TutorialText");
		GUIController.DisableImage("DragIcon");
		
		Time.timeScale = 1.0f;
		tutorialEnabled = false;
	}
	
	void HideDrag()
	{
		finger.enabled = false;
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player" && !GameRecorder.playingBack)
		{
			EnableTutorial();
		}
	}
	
	void OnTriggerLeave(Collider collider)
	{
		if (collider.tag == "Player" && !GameRecorder.playingBack)
		{
			HideDrag();
		}
	}
	
	
	void Update()
	{
		if (tutorialEnabled)
		{
			if (InputManager.pressed)
			{
				DisableTutorial();
			}
		}
		
		if(finger.enabled || LevelState.HasFinished)
		{				
			finger.transform.position = new Vector3(Mathf.SmoothStep(0.5f - SWIPE_DISTANCE, 0.5f + SWIPE_DISTANCE, Mathf.SmoothStep(0,1,time)), finger.transform.position.y, 0);			
			
			time += 1.0f/SWIPE_DURATION * RealTime.realDeltaTime;
			
			if(time > 1)
				time -= 1;
		}
		else
			time = 0;
	}
}
