using UnityEngine;
using System.Collections;

public class SaveManager : MonoBehaviour
{
	public const string dir = "";
	public const string ext = ".wifsav";
	public const string saveFilename = "player1" + ext;
	
	public static Save save;
	
	public static int StarsThisLevel = 0;
	
	void Awake()
	{
		if (save == null)
		{
			save = Save.Read(saveFilename);
			
			// Initialise new save
			if (save == null)
			{
				Create();
			}
		}
	}
	
	public static void Create()
	{
		save = new Save();
		save.filename = saveFilename;
		save.worldUnlocked = 1;
		save.levelUnlocked = 1;
		save.Write();
	}
	
	public static int LevelToLevelID(Level level)
	{
		// Find which level this is.
		int levelID = 0;
		for (levelID = 0; levelID < Levels.AllLevels.Count; ++levelID) 
		{
			if (Levels.AllLevels[levelID].CompareTo(level) == 0)
				break;
		}
		
		return levelID;
	}
	
	public static void UpdateScore(Level level)	
	{
		// Find which level this is.
		int levelID = LevelToLevelID(level);
		
		// Get the score.
		Save.LevelHighScore score = new Save.LevelHighScore();
		score.Score = ScoreCalculator.finalScore;
		score.Speed = ScoreCalculator.finalSpeed;
		score.Time = ScoreCalculator.finalTime;
		
		StarsThisLevel = GetStars(score, level);
		
		// Get the old score from file so we can check which ones we surpassed and award stars accordingly
		Save.LevelHighScore oldScore = save.GetHighScore(levelID);
		
		bool ignoreOldScore = false;
		// if old score was null
		if (oldScore.Time == 0)
			ignoreOldScore = true;
		
		// Get a star for winning.
		score.Stars = 1;
		
		// Star for surpassing score
		if (oldScore.Score >= level.ranks.SpeedThreshold && !ignoreOldScore)
		{
			score.Stars++;
		}
		else if (score.Score >= level.ranks.SpeedThreshold) 
		{
			score.Stars++;
		}
		
		// Star for surpassing time
		if (oldScore.Time <= level.ranks.TimeThreshold && !ignoreOldScore)
		{
			score.Stars++;
		}
		else if (score.Time <= level.ranks.TimeThreshold)
		{
			score.Stars++;
		}
		
		// Update the score.
		if (save != null)
			save.UpdateHighScore(score, levelID);
	}
	
	public static int GetStars(Save.LevelHighScore score, Level level)
	{
		// Get a star for winning
		int stars = 1;
		
		// Get a star for surpassing score
		if (HasSurpassedSpeed(score, level))
		{
			stars++;
		}
		
		// Get a star for surpassing time
		if (HasSurpassedTime(score, level))
		{
			stars++;
		}
		
		return stars;
		
	}
	
	public static bool GotWinStar(Level level)
	{
		// If our unlocked level is greater than this, we've beaten it
		if (save.worldUnlocked > level.world || (save.worldUnlocked == level.world && save.levelUnlocked > level.number))
			return true;
		else
			return false;
	}
	
	public static bool GotSpeedStar(Level level)
	{
		// Get index of level
		int levelID = LevelToLevelID(level);
		
		Save.LevelHighScore highscore = save.GetHighScore(levelID);
		
		return GotWinStar(level) && HasSurpassedSpeed(highscore, level);
	}
	
	public static bool GotTimeStar(Level level)
	{
		// Get index of level
		int levelID = LevelToLevelID(level);
		
		Save.LevelHighScore highscore = save.GetHighScore(levelID);
		
		return GotWinStar(level) && HasSurpassedTime(highscore, level);
	}
	
	public static bool HasSurpassedSpeed(Save.LevelHighScore score, Level level)
	{
		if (level != null)
			return score.Speed >= level.ranks.SpeedThreshold;
	
		return false;
	}
	
	public static bool HasSurpassedTime(Save.LevelHighScore score, Level level)
	{
		if (level != null)
			return score.Time <= level.ranks.TimeThreshold;

		return false;
	}
	
	public static void Beaten(Level level)
	{
		LoadLevel.JustUnlocked = false;
		
		if (save != null)
		{
			Level nextLevel = Levels.GetNext(level);
			
			if (nextLevel != null)
			{
				int worldUnlocked = nextLevel.world;
				int levelUnlocked = nextLevel.number;
				
				if (worldUnlocked > save.worldUnlocked)
				{					
					save.worldUnlocked = worldUnlocked;
					save.levelUnlocked = levelUnlocked;
					
					LoadLevel.JustUnlocked = true;
				}
				else if (levelUnlocked > save.levelUnlocked)
				{
					save.levelUnlocked = levelUnlocked;
					
					LoadLevel.JustUnlocked = true;
				}
			}
		}
	}
	
	public static void Write()
	{
		save.Write();
	}
}
