using UnityEngine;
using System;
using System.Collections;

public class TutorialPoint : MonoBehaviour, IComparable<TutorialPoint>
{
	public string textName;
	public string text;
	public string pcText;
	public float textX, textY;
	public GameObject highlightObject;
	public TutorialCamera.TutorialEvent startEvent = TutorialCamera.TutorialEvent.NONE;
	public TutorialCamera.TutorialEvent endEvent = TutorialCamera.TutorialEvent.NONE;
	public string recordingName = "";
	
	public void Start()
	{
		// Replace ascii \n with linebreak
		string[] splitText = text.Split(new string[] { "\\n" }, System.StringSplitOptions.None);
		text = "";
		foreach (string part in splitText)
		{
			text += part + '\n';
		}
	}
	
	public int CompareTo(TutorialPoint other) 
	{
		return name.CompareTo(other.name);
	}
}
