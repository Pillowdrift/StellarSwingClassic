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

	private float creationTime = 0;
	
	void Start()
	{
		tutCamera = (TutorialCamera)FindObjectOfType(typeof(TutorialCamera));
		creationTime = Time.time;
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

	void Update()
	{
		if (!enabled || !guiTexture.enabled)
			return;
		
		if (Time.time - creationTime < 0.5f)
			return;

		if (tutCamera.Stopped)
		{
			switch (action)
			{
			case TutorialAction.NEXT:
				if (Input.GetButton("NextLevel"))
					ButtonPressed();
				break;
			case TutorialAction.PREV:
				if (Input.GetButton("PreviousLevel"))
					ButtonPressed();
				break;
			case TutorialAction.SKIP:
				if (Input.GetButton("SkipTutorial"))
					ButtonPressed();
				break;
			}
		}
	}
}
