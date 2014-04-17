using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HighScoresGUI : MonoBehaviour
{
	public class ScoreCache
	{
		public struct CacheEntry
		{
			public CacheEntry(List<Networking.Score> scores)
			{
				this.scores = scores;
				lastUpdated = DateTime.Now;
			}
			
			public List<Networking.Score> scores;
			public DateTime lastUpdated;
		}
		Dictionary<string, CacheEntry> speedCache = new Dictionary<string, CacheEntry>();
		Dictionary<string, CacheEntry> timeCache = new Dictionary<string, CacheEntry>();
		
		// When a score is received.
		public delegate void ScoresLoaded(List<Networking.Score> scores, string levelname);
		public ScoresLoaded RetrievedCallback;
		
		// Whether or not we are currently loading some scores.
		private bool loadingScores = false;
		private string loadingLevelName = "";
		private HighScores.ScoreType loadingScoreType;
		public bool IsCurrentlyLoading()
		{
			return loadingScores;
		}
		
		// Try to get a score from the cache.
		public void GetScore(string levelname, HighScores.ScoreType type)
		{
			// If we are currently loading scores, wait for that one to finish first.
			if (loadingScores)
			{
				return;
			}
			
			Dictionary<string, CacheEntry> lookup = null;
			if (type == HighScores.ScoreType.Speed)
			{
				lookup = speedCache;
			} else	
			{
				lookup = timeCache;
			}
			
			if (!lookup.ContainsKey(levelname))
			{
				loadingLevelName = levelname;
				loadingScoreType = type;
				Networking.GetScores(LoadedScores, levelname, 
									 type == HighScores.ScoreType.Speed ? Networking.Metric.SPEED : Networking.Metric.TIME,
									 0, 10);
				loadingScores = true;
			}
			else
			{
				RetrievedCallback(lookup[levelname].scores, levelname);
			}
		}
		
		void LoadedScores(List<Networking.Score> scores)
		{
			loadingScores = false;
			if (loadingScoreType == HighScores.ScoreType.Speed)
			{
				speedCache[loadingLevelName] = new CacheEntry(scores);
			}
			else
			{
				timeCache[loadingLevelName] = new CacheEntry(scores);
			}
			RetrievedCallback(scores, loadingLevelName);
		}
	}
	
	// The cache to use
	static ScoreCache cache = new ScoreCache();
	
	// Which scores to show, local or internet.
	enum Locality
	{
		Local,
		Internet
	}
	
	public static bool enable = false;
	
	public GUISkin guiSkin;
	public GUISkin biggerSkin;
	public Texture BG;
	
	Locality locality = Locality.Local;
	HighScores.ScoreType scoreType = HighScores.ScoreType.Speed;
	
	// The old selected level and planet
	GameObject oldPlanet = null;
	Level oldLevel = null;
	
	// A list of replays.
	List<Recording> recordings = null;
	string[] names = null;
	List<Networking.Score> scores = null;
	
	// Selected recording index
	int selectedIndex = 0;
	
	// How much the scrolled view is scrolled
	Vector2 scrolled = Vector2.zero;
	
	// The last mouse position
	Vector3 lastMousePos;
	
	void Awake()
	{
		
	}
	
	void Start()
	{
		cache.RetrievedCallback = OnLoadFromInternet;
	}
		
	// Draw the highscores.
	void OnGUI()
	{
		if (LevelSelectGUI.menuState != LevelSelectGUI.MenuState.LEVEL_SELECT)
			enable = false;
		
		if (!enable) {
			GUIController.HideText("ScoreTypeName");
			return;
		}
			
		// Set skin and store old skin
		GUISkin oldSkin = GUI.skin;
		GUI.skin = Screen.width > 1500 ? biggerSkin : guiSkin;
		
		// Find out whether or not a planet is selected
		if (LevelSelectGUI.currentPlanet == null || !enable)
			return;
		
		// Calculate the how large things need to be.
		float labelPosX = Screen.width / 20;
		float labelPosY = Screen.height / 20;
		float labelWidth = Screen.width - (Screen.width / 10);
		float labelHeight = (Screen.height / 12);
		float buttonX = labelPosX;
		float buttonY = labelPosY;
		float buttonWidth = (Screen.width / 6);
		float buttonHeight = labelHeight;
		float buttonScoreTypeX = labelPosX + buttonWidth;
		float buttonScoreTypeY = buttonY;
		float scrollPosX = labelPosX;
		float scrollPosY = buttonY + buttonHeight;
		float scrollWidth = labelWidth;
		float scrollHeight = (Screen.height / 2.0f);		
		float playButtonX = scrollPosX;
		float playButtonY = scrollPosY + scrollHeight + 5;
		float playButtonWidth = scrollWidth;
		float playButtonHeight = scrollHeight / 4;		
		
		// Update the mouse position
		if (InputManager.currentPosition.x > labelPosX && InputManager.currentPosition.y > (Screen.height - (playButtonY + playButtonHeight)))
		{
			if (InputManager.held)
			{
				if ((lastMousePos - InputManager.currentPosition).y < 10.0f)
					scrolled.y -= (lastMousePos - InputManager.currentPosition).y;
			}
		}
		lastMousePos = InputManager.currentPosition;		
		
		// Draw a texture at the back
		GUI.DrawTexture(new Rect(labelPosX, labelHeight + labelPosY, playButtonX + playButtonWidth - (Screen.width / 20), playButtonY + playButtonHeight - labelHeight - buttonY), BG);
		
		// Draw a label at the top of the menu.
		string label = (locality == Locality.Internet ? "Internet" : "Local") + " " +
					   (scoreType == HighScores.ScoreType.Speed ? "Top Scores" : "Fastest Times");
		GUIController.ShowText("ScoreTypeName", label);
		
		// Draw the play replay button
		if (GUI.Button(new Rect(playButtonX, playButtonY, playButtonWidth, playButtonHeight), "Play Replay"))
		{
			LevelSelectGUI.worldToShow = LevelSelectGUI.currentWorld;
			LevelSelectGUI.levelToShow = LevelSelectGUI.currentLevel.number;
			
			if (locality == Locality.Local && selectedIndex < recordings.Count)
			{
				GameRecorder.StartPlaybackFromMenu(recordings[selectedIndex]);
			}
			else	
			{
				// Grab the recording from the net.
				if (scores != null)
					Networking.GetRecording(LoadRecordingFromInternet, scores[selectedIndex].id);
			}
			
			if (selectedIndex < recordings.Count)
				GameRecorder.StartPlaybackFromMenu(recordings[selectedIndex]);
		}
		
		// Draw the button to switch from local to internet
		// and vice versa.
		string buttonText = "";
		switch (locality)
		{
		case Locality.Local:
			buttonText = "Internet Scores";
			break;
		case Locality.Internet:
			buttonText = "Local Scores";
			break;
		default:
			buttonText = "Wut";
			break;
		}
		
		if (Options.Networking)
		{
			if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), buttonText))
			{
				if (locality == Locality.Local)
					locality = Locality.Internet;
				else
					locality = Locality.Local;
				
				// Refresh the scores
				LoadReplays();
			}
		}
		
		// Button to switch score type.
		string scoreTypeText = "";
		switch (scoreType)
		{
		case HighScores.ScoreType.Speed:
			scoreTypeText = "View Fastest Times";
			break;
		case HighScores.ScoreType.Time:
			scoreTypeText = "View Highest Scores";
			break;
		default:
			scoreTypeText = "Wut";
			break;
		}
		
		if (GUI.Button(new Rect(buttonScoreTypeX, buttonScoreTypeY, buttonWidth, buttonHeight), scoreTypeText))
		{
			if (scoreType == HighScores.ScoreType.Speed)
				scoreType = HighScores.ScoreType.Time;
			else
				scoreType = HighScores.ScoreType.Speed;
			
			// Refresh the scores
			LoadReplays();			
		}
		
		// The scrolling menu stuff
		GUILayout.BeginArea(new Rect(scrollPosX, scrollPosY, scrollWidth, scrollHeight));
		
			scrolled = GUILayout.BeginScrollView(scrolled);
				
				if (names != null)
					selectedIndex = GUILayout.SelectionGrid(selectedIndex, names, 1);
			
			GUILayout.EndScrollView();
		
		GUILayout.EndArea();
		
		// Restore old skin
		GUI.skin = oldSkin;
	}
	
	// Update is called once per frame
	void Update()
	{
		// If online stuff is disabled and somehow we are on internet scores switch back to local.
		if (SaveManager.save != null && !Options.Networking)
			locality = Locality.Local;
			
		// If the selected level has changed then load the recordings for that level.
		if (oldLevel != LevelSelectGUI.currentLevel ||
			oldPlanet != LevelSelectGUI.currentPlanet)
		{
			if (LevelSelectGUI.currentLevel != null)
			{
				oldLevel = LevelSelectGUI.currentLevel;
				oldPlanet = LevelSelectGUI.currentPlanet;
				
				// Reload the replays
				LoadReplays();
			}
		}
	}
	
	void LoadRecordingFromInternet(Recording recording)
	{
		GameRecorder.StartPlaybackFromMenu(recording);		
	}
	
	// Load replays from files.
	void LoadReplays()
	{
		scores = null;
		
		if (locality == Locality.Local) 
		{
			recordings = HighScores.GetHighScores(oldLevel, scoreType);
			
			// Generate an array of names for the GUI stuff
			names = new string[recordings.Count];
			for (int i = 0; i < names.Length; ++i)
			{
				if (scoreType == HighScores.ScoreType.Speed)
					names[i] += recordings[i].playername + ": " + recordings[i].score.speed.ToString(".#") + "%";
				else
					names[i] += recordings[i].playername + ": " + recordings[i].score.time.ToString(".#") + " seconds";
			}
		}
		else
		{
			// We need to get scores from the interwebs.
			names = new string[] { "Retrieving scores..." };			
			cache.GetScore(oldLevel.name, scoreType);
		}
	}
	
	// Called when the scores are retrieved from the internet.
	void OnLoadFromInternet(List<Networking.Score> scores, string levelname)
	{
		// If the currentlevel isn't right then ignore this.
		if (LevelSelectGUI.currentLevel.name != levelname)
			return;
		
		// Create the array to show in the menu.
		names = new string[scores.Count];
		for (int i = 0; i < names.Length; ++i)
		{
			if (scoreType == HighScores.ScoreType.Speed)
				names[i] = scores[i].name + ": " + scores[i].speed.ToString(".#") + "%";
			else
				names[i] = scores[i].name + ": " + scores[i].time.ToString(".#") + " seconds";
		}
					
		this.scores = scores;
	}
}
