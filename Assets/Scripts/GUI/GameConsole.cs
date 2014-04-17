//#define CONSOLE_ENABLED

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConsole : MonoBehaviour
{
	public const int MAX_LINES = 20;
	
	private static List<string> Lines
	{
		get	
		{
			if (lines == null)
				lines = new List<string>();
			
			return lines;
		}
	}
	
	private static List<string> lines = null;
	
	private static GUIText consoleText;
	
#if CONSOLE_ENABLED
	void Awake()
	{
		// Initialise GUIText for output
		if (consoleText == null)
		{
			GameObject obj = new GameObject("Text");
			obj.transform.parent = transform;
			obj.transform.position = new Vector3(0, 1.0f, 0);
			consoleText = obj.AddComponent<GUIText>();
			consoleText.pixelOffset = new Vector2(5.0f, -256.0f);
		}
		
		// Initialise text buffer
		UpdateConsole();
	}
#endif
	
	public static void Clear()
	{
#if CONSOLE_ENABLED
		Lines.Clear();
		UpdateConsole();
#endif
	}
	
	public static void WriteLine(string line)
	{
#if CONSOLE_ENABLED
		Lines.Add(line);
		UpdateConsole();
#endif
	}
	
#if CONSOLE_ENABLED
	void Update()
	{
		
	}
#endif
	
	private static void UpdateConsole()
	{
#if CONSOLE_ENABLED
		if (consoleText != null)
			consoleText.enabled = true;
		
		if (consoleText != null && Lines.Count > 0)
		{
			// Ensure console output is kept at or below limit
			while (Lines.Count > MAX_LINES)
				Lines.RemoveAt(0);
			
			// Create new string
			string output = "";
			
			// Append all lines to string
			foreach (string line in Lines)
			{
				output += line + "\n";
			}
			
			// Update GUIText
			consoleText.text = output;
		}
#endif
	}
}