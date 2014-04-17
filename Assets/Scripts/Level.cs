using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Level : IComparable<Level>
{
	// Required scores for ranks (any lower than C is a D)
	[System.Serializable]
	public class Ranks
	{
		public int SpeedThreshold = 0;
		public float TimeThreshold = 0;
	}
	
	public string name;
	public int world;
	public int number;
	public Ranks ranks;
	public bool highscores = true;
	
	public int CompareTo(Level _level) 
	{
		/*if (_level == null)
			return 1;
		if (_level.world != world)
			return 1;
		if (_level.number != number)
			return 1;*/
		
		if (_level.world != world)
			return world.CompareTo(_level.world);
		
		// Equal
		//return 0;
		
		return number.CompareTo(_level.number);
	}
}