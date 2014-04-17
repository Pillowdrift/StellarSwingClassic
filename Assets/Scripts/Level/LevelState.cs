using UnityEngine;
using System.Collections;

public class LevelState : MonoBehaviour
{
	public static bool HasFinished = false;
	public static bool Dead = false;
	
	void Start()
	{
		HasFinished = false;
		Dead = false;
	}
}
