using UnityEngine;
using System.Collections;

public class TapToRelease : MonoBehaviour
{
	private GrapplingHook grapplingHook;
	
	void Start()
	{
		grapplingHook = GameObject.Find("Player").GetComponent<GrapplingHook>();
	}
	
	void EnableTutorial()
	{
		GUIController.DisableButtons();
		
#if UNITY_ANDROID || UNITY_IPHONE
		GUIController.ShowText("Tutorial", "Tap again anywhere to release");
#else
		GUIController.ShowText("Tutorial", "Click again anywhere to release");
#endif
		
		GUIController.EnableImage("Tap2");
		
		GameObject.Find("Player").GetComponent<GrapplingHook>().currentTutorial = gameObject;
		
		SendMessage("ActivateSlowDown");
	}
	
	void DisableTutorial()
	{
		GUIController.GUILevelPlay();
		GUIController.HideText("Tutorial");
		GUIController.DisableImage("Tap2");
		
		SendMessage("DeactivateSlowDown");
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player" && !GameRecorder.playingBack)
		{
			if (GameObject.Find("Player").GetComponent<GrapplingHook>().IsGrappling())
			{
				EnableTutorial();
				StartCoroutine(WaitForUngrapple());
			}
		}
	}
	
	IEnumerator WaitForUngrapple()
	{
		while (grapplingHook.IsGrappling())
		{
			yield return null;
		}
		
		DisableTutorial();
	}
}
