using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

public class Options : MonoBehaviour
{
	private const string optionsfile = "settings.txt";
	
	private const float ORIGINAL_WIDTH = 1024;
	private const float ORIGINAL_HEIGHT = 768;
	
	private readonly int[] AA_LEVELS = { 0, 2, 4, 8 };
	
	public static bool Networking { get { return networking; } }
	public static float Sensitivity { get { return sensitivity; } }
	public static float MasterVolume { get { return mastervolume; } }
	public static float BGMVolume { get { return bgmvolume; } }
	public static float SFXVolume { get { return sfxvolume; } }
	
	public static bool enableGUI = true;
	
	public GUISkin guiSkin;
	public GUISkin pcSkin;
	
	public static bool firstrun = true;
	
	private GUISkin skin;
	
	private static Resolution current;
	
	// Graphics
	private static int currentResolution = 0;
	private static int fullscreen = 0;
	private static int quality = 2;
	
	// Advanced
	private static int aalevel = 2;
	private static int aniso = 1;
	private static int vsync = 1;
	private static int textureres = 2;
	
	// Controls
	private static float sensitivity = 1.0f;
	
	// Sound
	private static float mastervolume = 1.0f;
	private static float bgmvolume = 1.0f;
	private static float sfxvolume = 1.0f;
	
	// Misc
	private static bool advanced = false;
	private static bool networking = true;
	
	// Loading
	private static bool loaded = false;
	
#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", EntryPoint="ClipCursor")]
    static extern bool ClipCursor(ref RECT lpRect);
	
    [DllImport("user32.dll", EntryPoint="ClipCursor")]
    static extern bool ClipCursorNullable(Object obj);
 
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
#endif
	
	public void Awake()
	{
#if UNITY_ANDROID || UNITY_IPHONE
		skin = (GUISkin)Instantiate(guiSkin);
#else
		skin = (GUISkin)Instantiate(pcSkin);
#endif
		
		// Load settings
		Load();
	}
	
	public void Start()
	{
		UpdateSettings();
	}
	
	public void OnGUI()
	{
		float scale = Screen.height / ORIGINAL_HEIGHT;
		
		// Center scaled gui
		float offset = (Screen.width / 2.0f) / scale;
		
		Matrix4x4 oldMatrix = GUI.matrix;
		GUI.matrix = Matrix4x4.identity;
		GUI.matrix *= Matrix4x4.Scale(new Vector3(scale, scale, 1));
		GUI.matrix *= Matrix4x4.TRS(new Vector3(offset, 0, 0), Quaternion.identity, Vector3.one);
		
		if (!enableGUI || LevelSelectGUI.menuState != LevelSelectGUI.MenuState.OPTIONS)
			return;
		
		float labelWidth = ORIGINAL_WIDTH * 0.3f;
		
		// Save and switch skin
		GUISkin old = GUI.skin;
		GUI.skin = skin;
		
#if UNITY_IPHONE || UNITY_ANDROID
		
			// Phones
			GUILayout.BeginArea(new Rect(ORIGINAL_WIDTH * -0.45f, ORIGINAL_HEIGHT * 0.35f, ORIGINAL_WIDTH * 0.8f, ORIGINAL_HEIGHT * 0.6f));
			
			// Networking
		/*
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Networking", GUILayout.Width(labelWidth));
			
				if (GUILayout.SelectionGrid((networking ? 1 : 0), new string[] { "Off", "On" }, 2) == 0)
					networking = false;
				else
					networking = true;
			
			GUILayout.EndHorizontal();
		*/
			
			// Sensitivity slider
			GUILayout.BeginHorizontal();
			
				//GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUILayout.Label("Turning sensitivity", GUILayout.Width(labelWidth));
				//GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			
				sensitivity = GUILayout.HorizontalSlider(sensitivity, 0.3f, 1.7f);
			
			GUILayout.EndHorizontal();
			
			// Audio
			//GUILayout.Label("Audio");
			
			// Master volume
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Master Volume", GUILayout.Width(labelWidth));
				mastervolume = GUILayout.HorizontalSlider(mastervolume, 0.0f, 1.0f);
			
			GUILayout.EndHorizontal();
			
			// Music volume
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("BGM Volume", GUILayout.Width(labelWidth));
				bgmvolume = GUILayout.HorizontalSlider(bgmvolume, 0.0f, 1.0f);
			
			GUILayout.EndHorizontal();
			
			// Sound effect volume
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("SFX Volume", GUILayout.Width(labelWidth));
				sfxvolume = GUILayout.HorizontalSlider(sfxvolume, 0.0f, 1.0f);
			
			GUILayout.EndHorizontal();
			
			/*if (GUILayout.Button("WHATEVER YOU DO DONT DO IT"))
			{
				current = Screen.resolutions[currentResolution];
				Screen.SetResolution(current.width, current.height, false);
				UpdateSettings();
				Save();
			}*/
			
			GUILayout.EndArea();
		
#else
		
	#if UNITY_IPHONE || UNITY_ANDROID
			// Phones
			GUILayout.BeginArea(new Rect(ORIGINAL_WIDTH * -0.3f, ORIGINAL_HEIGHT * 0.4f, ORIGINAL_WIDTH * 0.6f, ORIGINAL_HEIGHT * 0.6f));
	#else
			current = Screen.resolutions[currentResolution];
			
			// PC
			GUILayout.BeginArea(new Rect(ORIGINAL_WIDTH * -0.475f, ORIGINAL_HEIGHT * 0.35f, ORIGINAL_WIDTH * 0.425f, ORIGINAL_HEIGHT * 0.65f));
			
			// Resolution
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Resolution", GUILayout.Width(180));
			
				// Previous button
				if (GUILayout.Button("<"))
					currentResolution = Mathf.Clamp(currentResolution-1, 0, Screen.resolutions.Length-1);
			
				// Current selection
				Font oldfont = GUI.skin.label.font;
				TextAnchor oldalign = GUI.skin.label.alignment;
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.skin.label.font = GUI.skin.button.font;
				GUILayout.Label(current.width + ", " + current.height);
				GUI.skin.label.alignment = oldalign;
				GUI.skin.label.font = oldfont;
			
				// Next button
				if (GUILayout.Button(">"))
					currentResolution = Mathf.Clamp(currentResolution+1, 0, Screen.resolutions.Length-1);
	
			GUILayout.EndHorizontal();
			
			// Whether or not to be fullscreen
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Display", GUILayout.Width(180));
				fullscreen = GUILayout.SelectionGrid(fullscreen, new string[] { "Windowed", "Fullscreen" }, 2);
			
			GUILayout.EndHorizontal();
			
			// Quality
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Quality", GUILayout.Width(180));
				
				if (GUILayout.Button("Low"))
					SetQuality(0);
				else if (GUILayout.Button("Medium"))
					SetQuality(1);
				else if (GUILayout.Button("High"))
					SetQuality(2);
				else if (GUILayout.Button("Ultra"))
					SetQuality(3);
			
			GUILayout.EndHorizontal();
			
			// Seperator
			GUILayout.Label("");
	#endif
			
			// Networking
		/*
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Networking", GUILayout.Width(260));
			
				if (GUILayout.SelectionGrid((networking ? 1 : 0), new string[] { "Off", "On" }, 2) == 0)
					networking = false;
				else
					networking = true;
			
			GUILayout.EndHorizontal();
		*/
			
	#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC || UNITY_STANDALONE_LINUX || UNITY_WEBPLAYER
			// Seperator
			GUILayout.Label("");
	#endif
			
			// Controls
			//GUILayout.Label("Controls");
			
			// Sensitivity slider
			GUILayout.BeginHorizontal();
			
				//GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUILayout.Label("Turning sensitivity", GUILayout.Width(260));
				//GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			
				sensitivity = GUILayout.HorizontalSlider(sensitivity, 0.3f, 1.7f);
			
			GUILayout.EndHorizontal();
			
	#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC || UNITY_STANDALONE_LINUX || UNITY_WEBPLAYER
			// Seperator
			GUILayout.Label("");
	#endif
			
			// Audio
			//GUILayout.Label("Audio");
			
			// Master volume
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Master Volume", GUILayout.Width(260));
				mastervolume = GUILayout.HorizontalSlider(mastervolume, 0.0f, 1.0f);
			
			GUILayout.EndHorizontal();
			
			// Music volume
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("BGM Volume", GUILayout.Width(260));
				bgmvolume = GUILayout.HorizontalSlider(bgmvolume, 0.0f, 1.0f);
			
			GUILayout.EndHorizontal();
			
			// Sound effect volume
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("SFX Volume", GUILayout.Width(260));
				sfxvolume = GUILayout.HorizontalSlider(sfxvolume, 0.0f, 1.0f);
			
			GUILayout.EndHorizontal();
			
			/*if (GUILayout.Button("WHATEVER YOU DO DONT DO IT"))
			{
				current = Screen.resolutions[currentResolution];
				Screen.SetResolution(current.width, current.height, false);
				UpdateSettings();
				Save();
			}*/
			
			GUILayout.EndArea();
			
			// Only show advanced on PC
	#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC || UNITY_STANDALONE_LINUX || UNITY_WEBPLAYER
			
			// Advanced area
			GUILayout.BeginArea(new Rect(0.025f * ORIGINAL_WIDTH, 0.35f * ORIGINAL_HEIGHT, 0.425f * ORIGINAL_WIDTH, 0.65f * ORIGINAL_HEIGHT));
		
			GUILayout.BeginHorizontal();
			
				GUILayout.Label("Advanced", GUILayout.Width(160));
			
				if (GUILayout.SelectionGrid((advanced ? 1 : 0), new string[] { "Hide", "Show" }, 2) == 0)
					advanced = false;
				else
					advanced = true;
			
			GUILayout.EndHorizontal();
			
			if (advanced)
			{
				// Seperator
				GUILayout.Label("");
				
				// Anti-aliasing level
				GUILayout.BeginHorizontal();
			
					GUILayout.Label("Anti-aliasing", GUILayout.Width(160));
				
					// Previous button
					if (GUILayout.Button("<"))
						aalevel = Mathf.Clamp(aalevel-1, 0, AA_LEVELS.Length-1);
				
					// Current selection
					oldfont = GUI.skin.label.font;
					oldalign = GUI.skin.label.alignment;
					GUI.skin.label.alignment = TextAnchor.MiddleCenter;
					GUI.skin.label.font = GUI.skin.button.font;
					GUILayout.Label(AA_LEVELS[aalevel] + "x");
					GUI.skin.label.alignment = oldalign;
					GUI.skin.label.font = oldfont;
				
					// Next button
					if (GUILayout.Button(">"))
						aalevel = Mathf.Clamp(aalevel+1, 0, AA_LEVELS.Length-1);
				
				GUILayout.EndHorizontal();
				
				// Anisotropic filtering
				GUILayout.BeginHorizontal();
			
					GUILayout.Label("Filtering", GUILayout.Width(160));
				
					aniso = GUILayout.SelectionGrid(aniso, new string[] { "Off", "On" }, 2);
				
				GUILayout.EndHorizontal();
				
				// Vsync
				GUILayout.BeginHorizontal();
				
					GUILayout.Label("Vertical Sync", GUILayout.Width(160));
				
					vsync = GUILayout.SelectionGrid(vsync, new string[] { "Off", "On" }, 2);
				
				GUILayout.EndHorizontal();
				
				// Texture resolution
				GUILayout.BeginHorizontal();
				
					GUILayout.Label("Texture Res", GUILayout.Width(160));
				
					textureres = GUILayout.SelectionGrid(textureres, new string[] { "Low", "Medium", "High" }, 3);
				
				GUILayout.EndHorizontal();
			}
			
			// Advanced area
			GUILayout.EndArea();
			
	#endif
#endif
		
		// Restore skin
		GUI.skin = old;
		
		// Restore matrix
		GUI.matrix = oldMatrix;
	}
	
	public static void FirstRun()
	{
		Tap.enableTap = false;
	}
	
	public static bool UpdateSettings()
	{
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC || UNITY_STANDALONE_LINUX || UNITY_WEBPLAYER
		// Update display mode
		Resolution current = Screen.resolutions[currentResolution];
					
		// Set display mode
		if (current.width != Screen.width ||
			current.height != Screen.height ||
			(fullscreen == 1) != Screen.fullScreen)
		{
			Screen.SetResolution(current.width, current.height, fullscreen == 1);
		}
		
		// Update quality settings
		if (textureres > 0)
			QualitySettings.SetQualityLevel(quality);
		else
			QualitySettings.SetQualityLevel(quality);
		
		// Custom
		QualitySettings.antiAliasing = aalevel;
		QualitySettings.anisotropicFiltering = (aniso > 0 ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable);
		QualitySettings.vSyncCount = vsync;
#endif
		
		
		return true;
	}
			
	public static void Save()
	{
		
#if UNITY_WEBPLAYER
		PlayerPrefs.SetFloat("sensitivity", sensitivity);
		PlayerPrefs.SetFloat("mastervolume", mastervolume);
		PlayerPrefs.SetFloat("bgmvolume", bgmvolume);
		PlayerPrefs.SetFloat("sfxvolume", sfxvolume);
		PlayerPrefs.SetInt("firstrun", System.Convert.ToInt32(firstrun));
		PlayerPrefs.SetString("resolution", Screen.resolutions[currentResolution].width + "x" + Screen.resolutions[currentResolution].height);
		PlayerPrefs.SetInt("fullscreen", fullscreen);
		PlayerPrefs.SetInt("quality", quality);
		PlayerPrefs.SetInt("aalevel", aalevel);
		PlayerPrefs.SetInt("aniso", aniso);
		PlayerPrefs.SetInt("vsync", vsync);
		PlayerPrefs.SetInt("textureres", textureres);
		PlayerPrefs.SetInt("advanced", System.Convert.ToInt32(advanced));
		PlayerPrefs.SetInt("networking", System.Convert.ToInt32(networking));
#else
		using (StreamWriter w = File.CreateText(Application.persistentDataPath + "/" + optionsfile))
		{
			w.WriteLine("sensitivity=" + sensitivity);
			w.WriteLine("mastervolume=" + mastervolume);
			w.WriteLine("bgmvolume=" + bgmvolume);
			w.WriteLine("sfxvolume=" + sfxvolume);
			w.WriteLine("firstrun=" + firstrun);
			
#	if UNITY_STANDALONE_WIN || UNITY_STANDALONE_MAC || UNITY_STANDALONE_LINUX
			w.WriteLine("resolution=" + Screen.resolutions[currentResolution].width + "x" + Screen.resolutions[currentResolution].height);
			w.WriteLine("fullscreen=" + fullscreen);
			w.WriteLine("quality=" + quality);
			w.WriteLine("aalevel=" + aalevel);
			w.WriteLine("aniso=" + aniso);
			w.WriteLine("vsync=" + vsync);
			w.WriteLine("textureres=" + textureres);
			w.WriteLine("advanced=" + advanced);
			w.WriteLine("networking=" + networking);
#	endif
		}
#endif
	}
	
	private void SetQuality(int level)
	{
		level = Mathf.Clamp(level, 0, 3);
		
		quality = level;
		
		aalevel = level;
		aniso = (level > 0 ? 1 : 0);
		vsync = 1;
		textureres = Mathf.Clamp(level, 0, 2);
	}
	
	private static void Load()
	{
		if (!loaded)
		{
			loaded = true;
			
#if UNITY_WEBPLAYER
			// Load
			sensitivity = PlayerPrefs.GetFloat("sensitivity", sensitivity);
			mastervolume = PlayerPrefs.GetFloat("mastervolume", mastervolume);
			bgmvolume = PlayerPrefs.GetFloat("bgmvolume", bgmvolume);
			sfxvolume = PlayerPrefs.GetFloat("sfxvolume", sfxvolume);
			firstrun = System.Convert.ToBoolean(PlayerPrefs.GetInt("firstrun", System.Convert.ToInt32(firstrun)));
			fullscreen = PlayerPrefs.GetInt("fullscreen", fullscreen);
			quality = PlayerPrefs.GetInt("quality", quality);
			aalevel = PlayerPrefs.GetInt("aalevel", aalevel);
			aniso = PlayerPrefs.GetInt("aniso", aniso);
			vsync = PlayerPrefs.GetInt("vsync", vsync);
			textureres = PlayerPrefs.GetInt("textureres", textureres);
			advanced = System.Convert.ToBoolean(PlayerPrefs.GetInt("advanced", System.Convert.ToInt32(advanced)));
			networking = System.Convert.ToBoolean(PlayerPrefs.GetInt("networking", System.Convert.ToInt32(networking)));
			
			string[] size = PlayerPrefs.GetString("resolution",
				Screen.resolutions[currentResolution].width + "x" + Screen.resolutions[currentResolution].height).Split('x');
			
			float x, y;
								
			if (float.TryParse(size[0], out x) && float.TryParse(size[1], out y))
			{
				currentResolution = 0;
				for (int i = 0; i < Screen.resolutions.Length; ++i)
				{
					Resolution resolution = Screen.resolutions[i];
					
					if (resolution.width == x && resolution.height == y)
					{
						currentResolution = i;
						break;
					}
				}
			}
#else			
			if (!File.Exists(Application.persistentDataPath + "/" + optionsfile))
			{
				// Set resolution
				currentResolution = Screen.resolutions.Length-1;
				fullscreen = 0;
				UpdateSettings();
				
				// Create new settings file
				Save();
			}
			else
			{
				string[] allLines = File.ReadAllLines(Application.persistentDataPath + "/" + optionsfile);
				
				foreach (string line in allLines)
				{
					string[] parts = line.Split('=');
					
					if (parts.Length > 1)
					{
						// Switch by setting name
						switch (parts[0])
						{
						case "resolution":
							string[] size = parts[1].Split('x');
							if (size.Length > 1)
							{
								float x, y;
								
								if (float.TryParse(size[0], out x) && float.TryParse(size[1], out y))
								{
									currentResolution = 0;
									for (int i = 0; i < Screen.resolutions.Length; ++i)
									{
										Resolution resolution = Screen.resolutions[i];
										
										if (resolution.width == x && resolution.height == y)
										{
											currentResolution = i;
											break;
										}
									}
								}
							}
							break;
						case "fullscreen":
							int.TryParse(parts[1], out fullscreen);
							break;
						case "quality":
							int.TryParse(parts[1], out quality);
							break;
						case "aalevel":
							int.TryParse(parts[1], out aalevel);
							break;
						case "aniso":
							int.TryParse(parts[1], out aniso);
							break;
						case "vsync":
							int.TryParse(parts[1], out vsync);
							break;
						case "textureres":
							int.TryParse(parts[1], out textureres);
							break;
						case "sensitivity":
							float.TryParse(parts[1], out sensitivity);
							break;
						case "mastervolume":
							float.TryParse(parts[1], out mastervolume);
							break;
						case "bgmvolume":
							float.TryParse(parts[1], out bgmvolume);
							break;
						case "sfxvolume":
							float.TryParse(parts[1], out sfxvolume);
							break;
						case "advanced":
							bool.TryParse(parts[1], out advanced);
							break;
						case "networking":
							bool.TryParse(parts[1], out networking);
							break;
						case "firstrun":
							bool.TryParse(parts[1], out firstrun);
							break;
						}
					}
				}
			}
			
			// Get index of current resolution if it's listed
			currentResolution = 0;
			for (int i = 0; i < Screen.resolutions.Length; ++i)
			{
				Resolution resolution = Screen.resolutions[i];
				
				if (resolution.width == Screen.width && resolution.height == Screen.height)
				{
					currentResolution = i;
					break;
				}
			}
#endif
			
			UpdateSettings();
		}
	}
}