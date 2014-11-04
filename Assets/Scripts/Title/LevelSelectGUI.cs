using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LevelSelectGUI : MonoBehaviour
{
	public enum MenuState
	{
		TITLE,
		WORLD_SELECT,
		LEVEL_SELECT,
		OPTIONS,
		RECORDINGS
	}
	
	public static bool canSpin = true;
	
	public static LevelSelectGUI instance;
	
	public static MenuState menuState;
	
	public float maxDragSpeedForTap = 1.0f;
	
	public Material[] skyboxes;
	
	public static Level currentLevel;
	
	// Set this to "" to show the title screen on load,
	// anything else to attempt to show a world on load
	public static string worldToShow
	{
		get
		{
			return worldToShow2;
		}
		set
		{
			worldToShow2 = value;
		}
	}
	private static string worldToShow2 = "";
	public static int levelToShow = 1;
	public static bool worldTransition = false;
	
	public static string currentWorld;
	public static GameObject currentPlanet;
	
	public Material levelMaterial;
	
	public const float INITIAL_ROTATION = 130; 
	
	private static float startRotation;
	
	private static Levels levels;
	private static int maxLevel;
	
	private static List<GameObject> levelObjects;
	private static GameObject closest;
	private static int closestId = -1;
	
	private static GameObject playLevel;
	
	private Vector3 planetRotationalVelocity = Vector3.zero;
	
	private static float rotationStep;
	
	private static Vector3 rotation;

	private ScrollingCamera scrollingCamera;
	
	public static void LoadSelectedLevel()
	{
		if (closestId >= 0 && closestId < levels.levels.Length)
		{
			// Load level
			currentLevel = levels.levels[closestId];
			
			if (currentLevel != null)
			{
				// Parse level name
				Level level = Levels.GetLevel(levels.levels[closestId].name);
				
				if (level != null)
				{
					// Update level select GUI to show this level name if we press back
					LevelSelectGUI.worldToShow = "World" + level.world;
					LevelSelectGUI.levelToShow = level.number;
				}
				
				Loading.Load(levels.levels[closestId].name, Loading.Transition.FOV);
			}
		}
	}
	
	void Start()
	{
		scrollingCamera = GameObject.FindObjectOfType<ScrollingCamera>();

		instance = this;
	
		canSpin = true;
		
		LevelSelectGUI.menuState = MenuState.TITLE;
		
		AmbientNoise.Play();
		ScreenFade.FadeFrom(new Color(0, 0, 0, 0));
		GUIController.DisableTexts();
		RecordingManager.enableGUI = false;
		
		// Change skybox based on which world is unlocked
		GetComponent<Skybox>().material = skyboxes[SaveManager.save.worldUnlocked-1];
		
		if (worldToShow != "")
		{
			GameObject.Find(worldToShow).SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
			GameObject.Find(worldToShow).SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
			Camera.mainCamera.GetComponent<ScrollingCamera>().FinishImmediate();
			RotateToLevel(levelToShow);
			
			if (worldTransition)
			{
				worldTransition = false;
				StartCoroutine("WorldTransition");
			}
		}
	}
	
	IEnumerator WorldTransition()
	{
		//GameObject oldWorld = GameObject.Find(worldToShow);
		GameObject newWorld;
		GameObject returnButton = GameObject.Find("ReturnButton");
		GameObject playButton = GameObject.Find("PlayLevel");
		GameObject highscoresButton = GameObject.Find("Highscores");
		
		// Disable GUI elements
		Tap.enableBack = false;
		playButton.active = false;
		GameObject.Find("WorldSelectObo").renderer.enabled = false;
		GameObject.Find("Select World").renderer.enabled = false;
		highscoresButton.active = false;
		
		// Work out which world is next
		Regex regex = new Regex("World(?<world>.*)");
		Match match = regex.Match(worldToShow);
		int world = 0;
		int.TryParse(match.Groups["world"].ToString(), out world);
		world += 1;
		worldToShow = "World" + world;
		newWorld = GameObject.Find(worldToShow);
		
		yield return new WaitForSeconds(1.0f);
		
		returnButton.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
		returnButton.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
		
		yield return new WaitForSeconds(1.0f);
		
		newWorld.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
		newWorld.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
		
		yield return new WaitForSeconds(1.0f);
		
		// Enable GUI elements
		Tap.enableBack = true;
		playButton.active = true;
		highscoresButton.active = true;
		GameObject.Find("WorldSelectObo").renderer.enabled = true;
		GameObject.Find("Select World").renderer.enabled = true;
		GameObject.Find("ReturnButton").renderer.enabled = true;
		GameObject.Find("ReturnButton").collider.enabled = true;
		
		//returnButton.renderer.material.color = oldColour;
	}
	
	void Update()
	{
		GUIController.HideText("LevelNameText");
		
		// Remove the stars.
		for (int i = 0; i < 3; ++i)
		{
			GUIController.DisableImage("OverallStar" + (i).ToString());
			GUIController.DisableImage("OverallStar" + (i).ToString() + "Locked");
		}

		if (currentPlanet != null && canSpin)
		{
			// If the highscores are enabled, don't let the user move the level.
			if (!HighScoresGUI.enable)
			{				
				if (InputManager.dragging && !ControlManager.MouseOnGUI)
				{
					planetRotationalVelocity.y = InputManager.frameDifference.x * -0.1f;
				}
				
				// Update rotation
				rotation += planetRotationalVelocity;
				
				if (rotation.y < startRotation)
				{
					rotation.y = startRotation;
					planetRotationalVelocity.y = 0;
				}
				else if (rotation.y > startRotation + ((maxLevel-1) * rotationStep))
				{
					planetRotationalVelocity.y = 0;
					rotation.y = startRotation + ((maxLevel-1) * rotationStep);
				}
				
				currentPlanet.transform.rotation = Quaternion.Euler(rotation);
				
				planetRotationalVelocity *= 0.99f;
				
				if (levelObjects.Count > 0)
				{			
					// Determine closest level
					float distanceSquared = -1.0f;
					
					GameObject oldClosest = closest;
					for (int i = 0; i < levelObjects.Count; ++i)
					{
						GameObject obj = levelObjects[i];
						
						if (obj.renderer.material.color.a == 0)
							Destroy(obj);
						
						Color white = Color.white;
						white.a = obj.renderer.material.color.a;
						obj.renderer.material.color = white;
						
						float dd = (obj.transform.position - scrollingCamera.NodePos).sqrMagnitude;
						
						if (dd < distanceSquared || distanceSquared < 0)
						{
							distanceSquared = dd;
							closest = obj;
							closestId = i;
						}					
					}
					
					// Switch level button
					if (closest != oldClosest)
					{
						if (oldClosest != null)
						{
							PlayLevelButton oldButton = oldClosest.GetComponent<PlayLevelButton>();
		
							if (oldButton != null)
								Destroy(oldClosest.GetComponent<PlayLevelButton>());
						}
						
						closest.AddComponent<PlayLevelButton>();
					}
					
					Color green = Color.green;
					green.a = closest.renderer.material.color.a;
					closest.renderer.material.color = green;
					
					// Set that level as the current level
					currentLevel = levels.levels[closestId];
					
					// Show current level
					//if (Tap.enableButtons)
					if (menuState == MenuState.LEVEL_SELECT)
					{
						Vector3 closestPosition = Camera.mainCamera.WorldToViewportPoint(levelObjects[closestId].transform.position);
						GUIController.ShowText("LevelNameText", levels.levels[closestId].name, closestPosition.x, closestPosition.y);
						
						// Show the stars for this level.
						if (levels.levels[closestId].highscores)
						{
							//int levelId = SaveManager.LevelToLevelID(levels.levels[closestId]);
							//Save.LevelHighScore scores = SaveManager.save.GetHighScore(levelId);
							
							// Show stars
							if (SaveManager.GotWinStar(LevelSelectGUI.currentLevel))
							{
								GUIController.EnableImage("OverallStar0", closestPosition.x + (-0.5f) * 0.2f + 0.25f, closestPosition.y - 0.05f);
								GUIController.DisableImage("OverallStar0Locked");
							}
							else
							{
								GUIController.EnableImage("OverallStar0Locked", closestPosition.x + (-0.5f) * 0.2f + 0.25f, closestPosition.y - 0.05f);
								GUIController.DisableImage("OverallStar0");
							}
							
							if (SaveManager.GotSpeedStar(LevelSelectGUI.currentLevel))
							{
								GUIController.EnableImage("OverallStar1", closestPosition.x + (0) * 0.2f + 0.25f, closestPosition.y - 0.05f);
								GUIController.DisableImage("OverallStar1Locked");
							}
							else
							{
								GUIController.EnableImage("OverallStar1Locked", closestPosition.x + (0) * 0.2f + 0.25f, closestPosition.y - 0.05f);
								GUIController.DisableImage("OverallStar1");
							}
							
							if (SaveManager.GotTimeStar(LevelSelectGUI.currentLevel))
							{
								GUIController.EnableImage("OverallStar2", closestPosition.x + (0.5f) * 0.2f + 0.25f, closestPosition.y - 0.05f);
								GUIController.DisableImage("OverallStar2Locked");
							}
							else
							{
								GUIController.EnableImage("OverallStar2Locked", closestPosition.x + (0.5f) * 0.2f + 0.25f, closestPosition.y - 0.05f);
								GUIController.DisableImage("OverallStar2");
							}
						}
					}
				}
			}
		}
	}
	
	public static void SetPlanet(GameObject planet)
	{
		if (levelObjects == null)
			levelObjects = new List<GameObject>();
		
		if (planet != currentPlanet)
		{
			// Update planet reference
			currentPlanet = planet;	
			
			// Clear old temp gameobjects
			foreach (GameObject obj in levelObjects)
			{
				if (obj != null)
					Destroy(obj);
			}
			
			levelObjects.Clear();
			closest = null;
			closestId = -1;
			
			if (currentPlanet != null)
			{
				currentWorld = planet.name;
				startRotation = currentPlanet.transform.rotation.eulerAngles.y;
				rotation = new Vector3(0, startRotation, 0);
				
				// Add a play button.
				//playLevel = (GameObject)Instantiate(Resources.Load("PlayLevel", typeof(GameObject)));
				//playLevel.transform.parent = currentPlanet.transform;
				//Vector3 playLevelPos = currentPlanet.transform.position;
				//playLevelPos.x += 2.0f;
				//playLevel.transform.position = playLevelPos;
				
				// Get world number
				Regex exp = new Regex("World(?<world>.*)");
				Match match = exp.Match(currentWorld);
				int world = 0;
				int.TryParse(match.Groups["world"].ToString(), out world);
				
				// Load new levels
				levels = currentPlanet.GetComponent<Levels>();
				maxLevel = (SaveManager.save.worldUnlocked > world ? levels.levels.Length : -1);
				if (maxLevel < 0)
				{
					maxLevel = SaveManager.save.levelUnlocked;
				}
				
				if (maxLevel > levels.levels.Length)
					maxLevel = levels.levels.Length;
				
				// Get direction from planet to camera
				Vector3 camDir = currentPlanet.transform.position - Camera.mainCamera.transform.position;
				camDir.Normalize();
				
				// Create gameobjects to represent levels
				rotationStep = 360.0f / levels.levels.Length;
				Vector3 offset = Vector3.forward * currentPlanet.collider.bounds.extents.z * 1.1f;
				
				for (int i = 0; i < levels.levels.Length; ++i)
				{
 					if (levels.levels[i].world > SaveManager.save.worldUnlocked ||
						(levels.levels[i].world == SaveManager.save.worldUnlocked &&
						 levels.levels[i].number > SaveManager.save.levelUnlocked))
					{
						continue;
					}
					
					// Rotate offset
					float thisRotation = rotationStep * -i + INITIAL_ROTATION;
					Quaternion thisQuaternion = Quaternion.Euler(0, thisRotation, 0);
					Vector3 thisOffset = thisQuaternion * offset;
					
					// Create object
					GameObject level = GameObject.CreatePrimitive(PrimitiveType.Plane);
					level.AddComponent<Billboard>();
					level.transform.rotation *= Quaternion.Euler(90, 0, 0);
					level.transform.localScale = 0.01f * Vector3.one;
					level.tag = "Planet";
					level.renderer.material = instance.levelMaterial;
					levelObjects.Add(level);
					
					// Initialise level object
					level.name = levels.levels[i].name;
					level.transform.parent = currentPlanet.transform;
					level.renderer.material.shader = Shader.Find("Transparent/Diffuse");
					level.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
					
					// Fade object in
					Fader fader = level.AddComponent<Fader>();
					fader.fadeRate = 2.0f;
					fader.FadeIn();
					
					// Offset object around planet
					level.transform.position = currentPlanet.transform.position - thisOffset;
				}
			}
		}
	}
	
	public static void RotateToLevel(int index)
	{
		index--;
		
		// Check if in range and set to last level otherwise
		if (index < 0 || index >= levels.levels.Length)
			index = levels.levels.Length - 1;
		
		// Rotate to position
		float yrotation = startRotation + rotationStep * index;
		rotation.y = yrotation;
	}
}
