//#define LOGGING_ENABLED
	
using UnityEngine;
using System.Collections;

public class Logging : MonoBehaviour
{
	private const int BUTTON_HEIGHT = 40;
	private const int BUTTON_WIDTH = 240;
	
	public Texture pauseImage;
	
	public Font font;
	public GUISkin PauseButtonSkin;
	public GUISkin AllSkin;
	
	private static bool isLogging = true;
	private static string logString = "l";
	
	float score;

	void Start()
	{
		Init();
	}
	
	void Update()
	{
		Init();
		
		AddLineToLog(Application.loadedLevelName);
		AddLineToLog("FPS: " + (1.0f/Time.deltaTime));
		
		if (SaveManager.save != null)
			Logging.AddLineToLog("World unlocked: " + SaveManager.save.worldUnlocked + ", level unlocked: " + SaveManager.save.levelUnlocked);
	}
	
	public static void AddLineToLog(string message)
	{
		logString = logString + message + "\n";
	}
	
	public static void SwitchLogging()
	{
		isLogging = !isLogging;
	}
	
#if LOGGING_ENABLED
	void OnGUI()
	{
		if (isLogging)
		{
			GUI.Label(new Rect(5.0f, 75.0f, Screen.width, Screen.height), logString);
		}
	}
#endif
	
	private void Init()
	{
		logString = "";
	}
}
