using UnityEngine;
using System.Collections;

public class ShowHighscoresButton : MonoBehaviour
{
	void Update()
	{
		if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.LEVEL_SELECT && LevelSelectGUI.currentLevel != null && LevelSelectGUI.currentLevel.highscores)
		{
			renderer.enabled = true;
			collider.enabled = true;
		}
		else
		{
			renderer.enabled = false;
			collider.enabled = false;
		}
		
		GameObject.Find("HighscoresArea").guiTexture.enabled = HighScoresGUI.enable;			
	}
	
	void OnMouseDown()
	{
		if (!HighScoresGUI.enable)
		{
			HighScoresGUI.enable = true;
		}
		else
		{
			HighScoresGUI.enable = false;
		}
	}
}
