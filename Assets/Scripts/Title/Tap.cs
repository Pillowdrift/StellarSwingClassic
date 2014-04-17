using UnityEngine;
using System.Collections;

public class Tap : MonoBehaviour
{
	public string parent;
	public bool planet = false;
	public float disableTime = 1.0f;
	
	public static bool enableTap = true;
	
	public bool enableRecordingGUI = false;
	
	private bool enableButton = true;
	
	public static bool transitioning = false;
	
	public static bool enableBack = true;
	
	void Awake()
	{
		enableBack = true;
	}
		
	void OnMouseDown()
	{
		if (enableTap)
		{
			string location = parent;
			
			// Show options screen if first run
			if (Options.firstrun)
			{
				//Options.FirstRun();
				
				//return;
			}
			
			// Update options if we're coming off of the options screen
			if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.OPTIONS)
			{
				Options.enableGUI = false;
				if (!Options.UpdateSettings())
					return;
				Options.Save();
			}
			
			// If this is a planet and a planet is selected, disable
			if (GetComponent<Levels>() != null && LevelSelectGUI.menuState == LevelSelectGUI.MenuState.LEVEL_SELECT)
				return;
			
			if (enabled && enableButton)
			{
				enableButton = false;
				
				switch (tag)
				{
				case "BackButton":
					GameObject playLevel = GameObject.Find("PlayLevel");
					if (playLevel != null)
					{
						playLevel.GetComponent<BoxCollider>().enabled = false;
						playLevel.GetComponent<MeshRenderer>().enabled = false;
					}
					
					if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.WORLD_SELECT)
					{
						location = "TitlePath";
						GetComponent<MeshRenderer>().enabled = false;
						GetComponent<Collider>().enabled = false;
					}
					else if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.LEVEL_SELECT)
					{
						location = "WorldSelect";
						LevelSelectGUI.SetPlanet(null);
						GUIController.DisableStars();
						GUIController.HideText("LevelNameText");
					}
					else if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.RECORDINGS)
					{
						location = "WorldSelect";
					}
					else if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.OPTIONS)
					{
						location = "WorldSelect";
					}
					
					break;
				}
				
				switch (location)
				{
				case "TitlePath":
					LevelSelectGUI.menuState = LevelSelectGUI.MenuState.TITLE;
					break;
				case "WorldSelect":
					LevelSelectGUI.menuState = LevelSelectGUI.MenuState.WORLD_SELECT;
					break;
				case "SelectRecording":
					LevelSelectGUI.menuState = LevelSelectGUI.MenuState.RECORDINGS;
					break;
				case "Options":
					Options.enableGUI = false;
					LevelSelectGUI.menuState = LevelSelectGUI.MenuState.OPTIONS;
					break;
				}
				
				//Camera.mainCamera.gameObject.SendMessage("SetNodes", parent);
				//StartCoroutine("Enable");
				
				//PlanetFader planetFader = GetComponent<PlanetFader>();
				//if (planetFader == null)
				{
					transitioning = true;
					
					GUIController.DisableStars();
					
					PlayButtonFader.FadeOut();
					Camera.mainCamera.gameObject.SendMessage("SetNodes", location);
					
					Options.enableGUI = false;
					
					if (planet)
					{
						LevelSelectGUI.SetPlanet(gameObject);
						LevelSelectGUI.menuState = LevelSelectGUI.MenuState.LEVEL_SELECT;
						
						GameObject playLevel = GameObject.Find("PlayLevel");
						if (playLevel != null)
						{
							playLevel.GetComponent<BoxCollider>().enabled = true;
							playLevel.GetComponent<MeshRenderer>().enabled = true;
						}
					}
					
					if (!enableRecordingGUI)
					{
						RecordingManager.enableGUI = false;
						GUIController.DisableButton("PlayRecording");
					}
					
					StartCoroutine(Enable());
				}
			}
		}
	}
	
	IEnumerator Enable()
	{
		yield return new WaitForSeconds(disableTime);
		
		enableButton = true;
		
		Options.enableGUI = true;
		RecordingManager.enableGUI = enableRecordingGUI;
		
		if (RecordingManager.enableGUI)
		{
			GUIController.EnableButton("PlayRecording");
		}
		
		GameObject backButton = GameObject.Find("ReturnButton");
		if (backButton != null)
		{
			if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.TITLE)
			{
				if (enableBack)
				{
					backButton.GetComponent<MeshRenderer>().enabled = false;
					backButton.GetComponent<Collider>().enabled = false;
				}
			}
			else if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.LEVEL_SELECT)
			{
				if (enableBack)
				{
					backButton.GetComponent<MeshRenderer>().enabled = true;
					backButton.GetComponent<Collider>().enabled = true;
				}
			}
			else
			{
				if (enableBack)
				{
					backButton.GetComponent<MeshRenderer>().enabled = true;
					backButton.GetComponent<Collider>().enabled = true;
				}
			}
		}
	}
}