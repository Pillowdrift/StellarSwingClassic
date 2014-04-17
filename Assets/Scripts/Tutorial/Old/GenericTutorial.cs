using UnityEngine;
using System.Collections;

public class GenericTutorial : MonoBehaviour
{
	public string text = "";
	
	void Start()
	{
		// Replace ascii \n with linebreak
		string[] splitText = text.Split(new string[] { "\\n" }, System.StringSplitOptions.None);
		text = "";
		foreach (string part in splitText)
		{
			text += part + '\n';
		}
	}
	
	void EnableTutorial()
	{
		GUIController.ShowText("Tutorial", text);
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
