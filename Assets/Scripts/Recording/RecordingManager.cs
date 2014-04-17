using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class RecordingManager : MonoBehaviour
{
	public static string extension = ".wifrec";
	public static string recordingsDir;
	
	public static bool enableGUI = false;
	public static bool playRecording = false;
	
	private List<string> recordingNames;
	private List<string> fullFilenames;
	private List<string> recordingDates;
	
	private GameObject recordingTextParent;
	
	private Recording current = null;
	private int currentRecording = -1;
	
	private Vector2 scrollPos = Vector2.zero;
	private int selGridInt = 0;
	
	void Start()
	{
		recordingsDir = Application.persistentDataPath + Recording.dir + "/";
		GameRecorder.Reset();
		
		playRecording = false;
		recordingNames = new List<string>();
		fullFilenames = new List<string>();
		recordingDates = new List<string>();
		StartCoroutine("LoadRecordings");
		
#if FALSE
		const bool dostuff = false;
		if (dostuff)
		{
			// Load all recordings and export scores
			StreamWriter all = new StreamWriter("D:\\Dropbox\\Pillowdrift\\Testing\\won.txt");
			all.WriteLine("Level name\tWon\tTotal");
			foreach (string dir in Directory.GetDirectories(recordingsDir))
			{
				string levelname = Path.GetFileNameWithoutExtension(dir);
				
				int won = 0;
				int total = 0;
				
				StreamWriter stream = new StreamWriter("D:\\Dropbox\\Pillowdrift\\Testing\\Scores\\" + levelname + ".txt");
				
				stream.WriteLine("Score\tSpeed\tTime");
				
				foreach (string file in Directory.GetFiles(dir))
				{
					Recording rec = Recording.Read(file);
					
					// If a win
					if (rec.score.time > 0)
					{
						stream.WriteLine(rec.score.score.ToString() + "\t" + rec.score.speed.ToString() + "\t" + rec.score.time.ToString());
						
						won++;
					}
					
					total++;
				}
				
				all.WriteLine(levelname + "\t" + won.ToString() + "\t" + total.ToString());
				
				stream.Close();
			}
			all.Close();
		}
#endif
	}
	
	void Update()
	{
		if (currentRecording != selGridInt)
		{
			currentRecording = selGridInt;
			if (selGridInt < fullFilenames.Count)
				current = Recording.Read(fullFilenames[selGridInt]);
			else
				current = null;
		}
		
		if (playRecording && current != null)
		{
			// Play back recording
			playRecording = false;
			GameRecorder.StartPlaybackFromMenu(current);
		}
	}
	
	void OnGUI()
	{
		if (enableGUI)
		{
			// Draw recording selection
			float selectionGuiX = 25.0f;
			float selectionGuiY = Screen.height * 0.3f;
			float selectionGuiWidth = Screen.width * 0.4f;
			float selectionGuiHeight = Screen.height - selectionGuiY - 25.0f;
			
			GUILayout.BeginArea(new Rect(selectionGuiX, selectionGuiY, selectionGuiWidth, selectionGuiHeight));
			
				scrollPos = GUILayout.BeginScrollView(scrollPos);
				
					selGridInt = GUILayout.SelectionGrid(selGridInt, recordingDates.ToArray(), 1);
				
				GUILayout.EndScrollView();
			
			GUILayout.EndArea();
			
			// Draw recording stats
			if (current != null)
			{
				float statsX = selectionGuiX + selectionGuiWidth + 25.0f;
				float statsY = selectionGuiY;
				float statsWidth = Screen.width * 0.4f;
				float statsHeight = selectionGuiHeight;
				
				GUILayout.BeginArea(new Rect(statsX, statsY, statsWidth, statsHeight));
				
					GUILayout.Label("Level: " + current.levelName.ToString() +
									"\nScore: " + current.score.score.ToString() +
									"\nSpeed: " + current.score.speed.ToString() +
									"\nTime: " + current.score.time.ToString() +
									(current.flagged ? "\nBUG" : ""));
				
				GUILayout.EndArea();
			}
		}
	}
	
	IEnumerator LoadRecordings()
	{/*
		// Load recording names
		DirectoryInfo dir = new DirectoryInfo(recordingsDir);
		if (dir.Exists)
		{
			foreach (FileInfo file in dir.GetFiles("*.wifrec", SearchOption.AllDirectories))
			{
				string filename = Path.GetFileNameWithoutExtension(file.Name);
				float timestamp;
				
				if (float.TryParse(filename, out timestamp))
				{
					recordingNames.Insert(0, filename);
					fullFilenames.Insert(0, file.FullName);
					recordingDates.Insert(0, (UnixTime.Epoch.AddSeconds(timestamp)).ToString());
				}
			}
		}
		*/
		yield return null;
	}
	 
	public int RecordingCount { get { return recordingNames.Count; } }
	public List<string> RecordingNames { get { return recordingNames; } }
}
