using UnityEngine;
using System.Collections;

public class SlideOnObjects : MonoBehaviour
{
	void EnableTutorial()
	{
		GUIController.ShowText("Tutorial", "Land on objects to slide across them");
	}
	
	void DisableTutorial()
	{
		GUIController.HideText("TutorialText");
		
		Time.timeScale = 1.0f;
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player" && !GameRecorder.playingBack)
		{
			EnableTutorial();
		}
	}
}
