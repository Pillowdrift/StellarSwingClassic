using UnityEngine;
using System.Collections;

public class TutorialButton : MonoBehaviour
{
	public enum TutorialAction
	{
		NEXT,
		PREV,
		SKIP
	};
	
	public TutorialAction action;
	
	private TutorialCamera tutCamera;
	
	void Start()
	{
		tutCamera = (TutorialCamera)FindObjectOfType(typeof(TutorialCamera));
	}
	
	void ButtonPressed()
	{
		if (tutCamera.Stopped)
		{
			switch (action)
			{
			case TutorialAction.NEXT:
				tutCamera.Next();
				break;
			case TutorialAction.PREV:
				tutCamera.Prev();
				break;
			case TutorialAction.SKIP:
				GameRecorder.StopPlayback();
				GameObject.Find("Player").SendMessage("Reload");
				tutCamera.Skip();
				break;
			}
		}
	}
}
