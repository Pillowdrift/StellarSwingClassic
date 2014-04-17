using UnityEngine;
using System.Collections;

public class TurnSmoothly : MonoBehaviour
{
	void EnableTutorial()
	{
		GUIController.ShowText("Tutorial", "Turn smoothly to avoid losing speed");
	}
	
	void DisableTutorial()
	{
		GUIController.HideText("Tutorial");
	}
	
	void OnTriggerEnter(Collider collider)
	{
		//if (SaveManager.save.worldUnlocked == 2 && SaveManager.save.levelUnlocked == 4)
		{
			if (collider.tag == "Player" && !GameRecorder.playingBack)
			{
				EnableTutorial();
			}
		}
	}
}
