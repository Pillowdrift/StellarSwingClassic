using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class LoadLevel : MonoBehaviour
{
	public static bool JustUnlocked { get { return justUnlocked; } set { justUnlocked = value; } }
	
	public string level;
	
	private static bool justUnlocked = false;
	
	private bool pressed = false;
	
	void Start()
	{
		JustUnlocked = false;
	}
	
	public static bool IsPrevLevelAvailable()
	{
		if (SaveManager.save != null)
		{
			Level previous = Levels.GetPrevious(Levels.GetLevel(Application.loadedLevelName));
			
			if (previous != null)
				return true;
		}
		
		return false;
	}
	
	public static bool IsNextLevelAvailable()
	{
		if (SaveManager.save != null)
		{
			Level nextLevel = Levels.GetNext(Levels.GetLevel(Application.loadedLevelName));
			
			if (nextLevel != null &&
				(nextLevel.world < SaveManager.save.worldUnlocked ||
				(nextLevel.world == SaveManager.save.worldUnlocked &&
				 nextLevel.number <= SaveManager.save.levelUnlocked)))
			{
				return true;
			}
		}
		
		return false;
	}
	
	void ButtonPressed()
	{
		if (pressed)
			return;
		
		GameRecorder.Reset();
		
		LoadALevel(level);
		pressed = true;
	}
	
	public static void SetToNext()
	{
		LevelSelectGUI.currentLevel = Levels.GetNext(Levels.GetLevel(Application.loadedLevelName));
	}
	
	public static void LoadALevel(string levelToLoad)
	{
		if (levelToLoad.ToLower() == "next")
		{
			// Load the credits if this is the last level
			if (Levels.IsLastLevel(LevelSelectGUI.currentLevel))
			{
				levelToLoad = "credits";
			}
			
			// Check the current world.
			int currentWorld = LevelSelectGUI.currentLevel.world;
			
			// Why does this use two different functions?
			// No Idea.
			Level nextLevel = Levels.GetNext(Levels.GetLevel(LevelSelectGUI.currentLevel.name));
			
			if (nextLevel != null)
			{
				levelToLoad = nextLevel.name;
				
				// Set current level to new level
				// Load the credits if this is the last level
				LevelSelectGUI.currentLevel = nextLevel;
						
				// If the next level is the first level to a world, then load the transition scene.
				if (JustUnlocked && LevelSelectGUI.currentLevel.number == 1)
				{
					levelToLoad = "World " + currentWorld.ToString() + " To World " + LevelSelectGUI.currentLevel.world.ToString();
				}
			}
		}
		else if (levelToLoad.ToLower() == "previous")
		{
			Level lastLevel = Levels.GetPrevious(Levels.GetLevel(Application.loadedLevelName));
			
			if (lastLevel != null)
			{
				LevelSelectGUI.currentLevel = lastLevel;
				levelToLoad = lastLevel.name;
			}
		}
		
		if (levelToLoad != "")
		{
			if (levelToLoad == "Credits")
			{
				LevelSelectGUI.worldToShow = "World5";
				LevelSelectGUI.levelToShow = -1;
			}
			else
			{
				// Parse level name
				Level level = Levels.GetLevel(levelToLoad);
				
				if (level != null)
				{
					// Update level select GUI to show this level name if we press back
					LevelSelectGUI.worldToShow = "World" + level.world;
					LevelSelectGUI.levelToShow = level.number;
				}
			}
			
			Loading.Load(levelToLoad);
			LevelStart.started = false;
			Time.timeScale = 1.0f;
		}
	}
}
