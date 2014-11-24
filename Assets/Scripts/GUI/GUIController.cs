using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIController : MonoBehaviour
{
	public static bool LevelStarted { get; set; }
	
	public static void EndLevel(bool won)
	{
		LevelStarted = false;
		
			if (won)
			{
				if (!GameRecorder.playingBack)
				{
					GUILevelWin();
				}
			}
			else
			{
				GUILevelLose();
			}
	}
	
	public static void GUILevelPlay()
	{
		GUIController.HideText("Paused");
		GUIController.HideText("LevelName");
		
		EnableButton("Pause");
		
		DisableButton("Back");
		DisableButton("Restart");
		
//		DisableButton("DeathRetry");
//		DisableButton("DeathRetryFullscreen");
		
		DisableButton("Previous");
		DisableButton("Next");
	}
	
	public static void GUILevelWin()
	{
		EnableButton("Pause");
		
		EnableButton("Back");
		EnableButton("Restart");
		
//		DisableButton("DeathRetry");
//		DisableButton("DeathRetryFullscreen");
		
		if (LoadLevel.IsPrevLevelAvailable())
			EnableButton("Previous");
		if (LoadLevel.IsNextLevelAvailable())
			EnableButton("Next");
	}
	
	public static void GUILevelLose()
	{
		DisableButton("Pause");
		
		DisableButton("Back");
		DisableButton("Restart");
		
//		EnableButton("DeathRetry");
//		EnableButton("DeathRetryFullscreen");
		
		DisableButton("Previous");
		DisableButton("Next");
	}
	
	public static void GUILevelPause()
	{
		GUIController.ShowText("LevelName");
		GUIController.ShowText("Paused", "Paused");
		
		EnableButton("Pause");
		
		EnableButton("Back");
		EnableButton("Restart");
		
//		DisableButton("DeathRetry");
		DisableButton("DeathRetryFullscreen");
		
		if (LoadLevel.IsPrevLevelAvailable())
			EnableButton("Previous");
		if (LoadLevel.IsNextLevelAvailable())
			EnableButton("Next");
	}
	
	public static void GUIPlaybackRecording()
	{
		ShowText("Replay", "Replay Mode");
		
		DisableButton("Pause");
		
		DisableButton("Back");
		DisableButton("Restart");
		
		EnableButton("ReplayBack");
//		DisableButton("DeathRetryFullscreen");
		
//		DisableButton("DeathRetry");
		
		DisableButton("Previous");
		DisableButton("Next");
	}
	
	public static void EnableButtons()
	{
		GUITexture[] textures = GameObject.FindObjectsOfType(typeof(GUITexture)) as GUITexture[];
		
		foreach (GUITexture texture in textures)
		{
			GUIButton button = texture.GetComponent<GUIButton>();
			
			if (button != null)
			{
				button.enabled = true;
			}
		}
	}
	
	public static void DisableButtons()
	{
		GUITexture[] textures = GameObject.FindObjectsOfType(typeof(GUITexture)) as GUITexture[];
		
		foreach (GUITexture texture in textures)
		{
			GUIButton button = texture.GetComponent<GUIButton>();
			
			if (button != null)
			{
				button.enabled = false;
			}
		}
	}
	
	public static void DisableStars()
	{
		GameObject playLevel = GameObject.Find("PlayLevel");
		if (playLevel != null)
		{
			playLevel.GetComponent<BoxCollider>().enabled = false;
			playLevel.GetComponent<MeshRenderer>().enabled = false;
		}
		
		DisableImage("Star0");
		DisableImage("Star1");
		DisableImage("Star2");
		DisableImage("Star0Locked");
		DisableImage("Star1Locked");
		DisableImage("Star2Locked");
	}
	
	public static void DisableTexts()
	{
		GUIText[] texts = GameObject.FindObjectsOfType(typeof(GUIText)) as GUIText[];
		
		foreach (GUIText text in texts)
		{
			text.enabled = false;
		}
	}
	
	public static bool IsTextEnabled(string objectName)
	{
		GameObject obj = GameObject.Find(objectName);
		if (obj != null)
		{
			GUIText text = obj.GetComponent<GUIText>();
			return text.enabled;
		}
		return false;
	}
	
	public static void ShowText(string objectName)
	{
		GameObject obj = GameObject.Find(objectName);
		if (obj != null)
		{
			GUIText text = obj.GetComponent<GUIText>();
			
			if (text != null)
			{
				text.enabled = true;
			}
		}
	}
	
	public static void ShowText(string objectName, string newText)
	{
		GameObject obj = GameObject.Find(objectName);
		if (obj != null)
		{
			GUIText text = obj.GetComponent<GUIText>();
			if (text != null)
			{
				text.enabled = true;
				text.text = newText;
			}
		}
	}
	
	public static void ShowText(string objectName, string newText, float x, float y)
	{
		GameObject obj = GameObject.Find(objectName);
		
		if (obj == null)
			return;
		
		GUIText text = obj.GetComponent<GUIText>();
		
		if (text != null)
		{
			text.enabled = true;
			text.text = newText;
			text.transform.position = new Vector3(x, y, 1);
        }
	}
	
	public static void HideText(string objectName)
	{
		GameObject obj = GameObject.Find(objectName);
		
		if (obj == null)
			return;
		
		GUIText text = obj.GetComponent<GUIText>();
		
		if (text != null)
            text.enabled = false;
	}
	
	public static string GetText(string objectName)
	{
		GameObject obj = GameObject.Find(objectName);
		
		if (obj == null)
			return "";
		
		GUIText text = obj.GetComponent<GUIText>();
		
		if (text != null)
		{
			return text.text;
		}
		
		return "";
	}
	
	public static void EnableButton(string buttonName)
	{
		GameObject obj = GameObject.Find(buttonName);
		
		obj.layer = 0;
		
		if (obj == null)
			return;
		
		GUIButton button = obj.GetComponent<GUIButton>();
		
		if (button != null)
			button.enabled = true;
	}
	
	public static void DisableButton(string buttonName)
	{
		GameObject obj = GameObject.Find(buttonName);
		
		obj.layer = LayerMask.NameToLayer("Ignore Raycast");
		
		if (obj == null)
			return;
		
		GUIButton button = obj.GetComponent<GUIButton>();
		
		if (button != null)
			button.enabled = false;
	}
	
	public static void EnableImage(string buttonName)
	{
		GameObject obj = GameObject.Find(buttonName);
		
		if (obj == null)
			return;
		
		GUITexture image = obj.GetComponent<GUITexture>();
		
		if (image != null)
			image.enabled = true;
	}
	
	public static void EnableImage(string buttonName, float x, float y)
	{
		GameObject obj = GameObject.Find(buttonName);
		
		if (obj == null)
			return;
		
		GUITexture image = obj.GetComponent<GUITexture>();
		
		if (image != null) {
			image.enabled = true;
			image.transform.position = new Vector3(x, y, 1);
		}
	}	
	
	public static void DisableImage(string buttonName)
	{
		GameObject obj = GameObject.Find(buttonName);
		
		if (obj == null)
			return;
		
		GUITexture image = obj.GetComponent<GUITexture>();
		
		if (image != null)
			image.enabled = false;
	}
	
	public static void StartLevel()
	{
		LevelStarted = true;
	}
}
