using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour
{
	void ButtonPressed()
	{
		if (GameRecorder.playingBack)
		{
			GameRecorder.Restart();
		}
		else
		{
			GameRecorder.Reset();
			
			Time.timeScale = 1.0f;
			LevelStart.started = false;
			Loading.Load(Application.loadedLevelName);
			GameRecorder.StopPlayback();
			ControlManager.Disabled = false;
		}
	}
}
