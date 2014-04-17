using UnityEngine;
using System.Collections;

public class AndroidBackButtonControl : MonoBehaviour
{
	void Update()
	{
		if (enabled)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.TITLE)
				{
					Application.Quit();
				}
				else
				{
					GameObject.Find("ReturnButton").SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
					GameObject.Find("ReturnButton").SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
