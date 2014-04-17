using UnityEngine;
using System.Collections;

public class TapToGrapple : MonoBehaviour
{
	void EnableTutorial()
	{
		GUIController.DisableButtons();
		
#if UNITY_ANDROID || UNITY_IPHONE
		GUIController.ShowText("Tutorial", "Tap glowing objects to grapple");
#else
		GUIController.ShowText("Tutorial", "Click glowing objects to grapple");
#endif
		
		GameObject.Find("Player").GetComponent<GrapplingHook>().currentTutorial = gameObject;
			
		//outline.enabled = true;
		
		GUIController.EnableImage("Tap");
		
		SendMessage("ActivateSlowDown");
	}
	
	void DisableTutorial()
	{
		GUIController.GUILevelPlay();
		GUIController.HideText("Tutorial");
		GUIController.DisableImage("Tap");
		
		//outline.enabled = false;
		
		SendMessage("DeactivateSlowDown");
	}
	
	void OnTriggerEnter(Collider collider)
	{
//		if (SaveManager.save.worldUnlocked == 1 && SaveManager.save.levelUnlocked == 1)
//		{
			if (collider.tag == "Player" && !GameRecorder.playingBack)
			{
				if (!GameObject.Find("Player").GetComponent<GrapplingHook>().IsGrappling())
					EnableTutorial();
			}
//		}
	}
	
	void Grappled()
	{
		DisableTutorial();
	}
}
