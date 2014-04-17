using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Levels : MonoBehaviour
{
	public static List<Level> AllLevels
	{
		get
		{
			if (allLevels == null)
				allLevels = new List<Level>();
			
			return allLevels;
		}
	}
	
	public Level[] levels;
	
	private static List<Level> allLevels = null;
	
	void Awake()
	{
		foreach (Level level in levels)
		{
			// The name will be set automatically if left blank
			if (level.name == "")
				level.name = "World " + level.world + " Level " + level.number;
			
			// Skip if we have already added it
			bool contains = false;
			for (int i = 0; i < AllLevels.Count; ++i)
			{
				if (level.name == AllLevels[i].name)
				{
					contains = true;
					break;
				}
			}
			if (!contains) 
			{
				AllLevels.Add(level);
			}
		}
		
		// Sort levels
		allLevels.Sort();
	}
	
	public static Level GetLevel(string name)
	{
		foreach (Level level in AllLevels)
		{
			if (level.name == name)
				return level;
		}
		
		return null;
	}
	
	public static Level GetPrevious(Level current)
	{
		Level previous = current;
		int index = Levels.AllLevels.IndexOf(current);
		
		do
		{
			index--;
			
			if (index < 0)
				return null;
			
			previous = Levels.AllLevels[index];
		} while (previous.name.Contains("Tutorial"));
		
		return previous;
	}
	
	public static bool IsLastLevel(Level level)
	{
		Level lastLevel = Levels.AllLevels.Last();
		return (lastLevel.world == level.world &&
				lastLevel.number == level.number);
	}
	
	public static Level GetNext(Level current)
	{
		if (AllLevels.Count == 0)
			return new Level();
			
		int index = 0;
		for (int i = 0; i < Levels.AllLevels.Count; ++i) 
		{
			if (Levels.AllLevels[i].world == current.world &&
				Levels.AllLevels[i].number == current.number)
			{
				index = i;
				break;
			}
		}
		if (index+1 < Levels.AllLevels.Count)
			index++;
		else if (index < 0)
			return null;
		
		return AllLevels[index];
	}
}
