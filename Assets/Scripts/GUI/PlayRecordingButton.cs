using UnityEngine;
using System.Collections;

public class PlayRecordingButton : MonoBehaviour
{
	void ButtonPressed()
	{
		RecordingManager.playRecording = true;
	}
}
