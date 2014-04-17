using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class HighScores : MonoBehaviour
{
	// The max amount of highscores saved for each level.
	public const int MaxHighScores = 10;
	
	// High scores version
	public const int VERSION = 1;
	
	// Score type.
	public enum ScoreType
	{
		Speed,
		Time
	}
		
	// Sort recordings by speed.
	protected class SpeedSorter : IComparer<Recording>
	{
		public int Compare(Recording x, Recording y)
		{
			return -x.score.speed.CompareTo(y.score.speed);
		}
	}
	
	// Sort recordings by time.
	protected class TimeSorter : IComparer<Recording>
	{
		public int Compare(Recording x, Recording y)
		{
			return x.score.time.CompareTo(y.score.time);
		}
	}
	
	// Get a sorter for a score type
	static IComparer<Recording> GetSorter(ScoreType type)
	{
		switch (type)
		{
		case ScoreType.Speed:
			return new SpeedSorter();
		case ScoreType.Time:
			return new TimeSorter();
		default:
			return null;
		}
	}
	
	// Get the directory to save the scores.
	static string GetSaveDir(Level level, ScoreType type)
	{
		// Create the directory if it doesn't exist.
		Directory.CreateDirectory(Application.persistentDataPath + "/speed");
		Directory.CreateDirectory(Application.persistentDataPath + "/time");
		Directory.CreateDirectory(Application.persistentDataPath + "/speed/" + level.name);
		Directory.CreateDirectory(Application.persistentDataPath + "/time/" + level.name);
		
		// Get the directory to search.
		switch (type)
		{
		case ScoreType.Speed:
			return Application.persistentDataPath + "/speed/" + level.name;
		case ScoreType.Time:
			return Application.persistentDataPath + "/time/" + level.name;
		default:
			return null;
		}		
	}
	
	// Read the highscores for a level
	public static List<Recording> GetHighScores(Level level, ScoreType type)
	{
		List<Recording> scores = new List<Recording>();
		
		if (level == null)
			return scores;
		
		// Get the save directory.
		string dir = GetSaveDir(level, type);
		
		// Search this directory and order by filename.
		string[] files = Directory.GetFiles(dir);
		for (int i = 0; i < files.Length; ++i)
		{
			// Read the file for the scores.
			Recording rec = Recording.Read(files[i]);
			
			// Add the score.
			scores.Add(rec);
		}
		
		return scores;
	}
	
	// Save scores to disk.
	public static void SaveScores(List<Recording> scores, Level level, ScoreType type)
	{
		if (level == null)
			return;
		
		// Get the directory to save to
		string dir = GetSaveDir(level, type);
		
		// Sort the list
		scores.Sort(GetSorter(type));
		
		// Delete all the recordings in the directory and re-save them.
		string[] files = Directory.GetFiles(dir);
		for (int i = 0; i < files.Length; ++i)
		{
			File.Delete(files[i]);
		}
		
		// Save them.
		for (int i = 0; i < MaxHighScores && i < scores.Count; ++i)
		{
			scores[i].Write(dir + "/" + (i+1).ToString() + ".wifrec");
		}
	}
	
	// Attempt to post a highscore.
	public static void PostScore(Level level, Recording rec)
	{
		// Grab all the top scores
		List<Recording> speedScores = GetHighScores(level, ScoreType.Speed);
		List<Recording> timeScores = GetHighScores(level, ScoreType.Time);
		
		// Check if we are in any of these.
		speedScores.Add(rec);
		timeScores.Add(rec);
		
		// Save them.
		SaveScores(speedScores, level, ScoreType.Speed);
		SaveScores(timeScores, level, ScoreType.Time);
	}
}
