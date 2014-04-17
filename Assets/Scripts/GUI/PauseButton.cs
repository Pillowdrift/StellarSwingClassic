using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour
{
	public static bool paused = false;
	
	void ButtonPressed()
	{
		if (GUIController.LevelStarted && !LevelState.Dead && !LevelState.HasFinished && !GameRecorder.playingBack)
		{
			if (Time.timeScale > 0.0f && !paused)
			{
				Time.timeScale = 1.0f - Time.timeScale;
				GUIController.GUILevelPause();
				paused = true;
			}
			else if (paused)
			{
				Time.timeScale = 1.0f - Time.timeScale;
				GUIController.GUILevelPlay();
				paused = false;
			}
		}
	}
}
