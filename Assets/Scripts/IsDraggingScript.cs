using UnityEngine;
using System.Collections;

public class IsDraggingScript : MonoBehaviour
{
	public GameObject player;
	
	void Start()
	{
#if UNITY_ANDROID || UNITY_IPHONE
		GUIController.ShowText("Tutorial", "Swipe left or right to turn");
#else
		GUIController.ShowText("Tutorial", "Drag left or right to turn");
#endif
	}
	
	// Update is called once per frame
	void Update()
	{
		if (GameRecorder.playingBack || !LevelStart.started)
		{
			GUIController.HideText("Tutorial");
			enabled = false;
			return;
		}
		
		if (LevelStart.started)
		{
			PlayerMovements playerMovements = player.GetComponent<PlayerMovements>();
			
			if(Mathf.Abs(playerMovements.angle) > 1.0f && Time.timeScale > 0.0f)
			{
				GUIController.HideText("Tutorial");
				enabled = false;
			}
		}
	}
	
	void OnDisable()
	{
		
	}
}
