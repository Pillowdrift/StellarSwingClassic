using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Networking : MonoBehaviour
{
	public class Score
	{
		public int id;
		public string name;
		public float speed;
		public float time;
	}
	
	public enum Metric
	{
		SPEED,
		TIME
	}
	
	public delegate void HighscoreSubmitted(int speedRank, int timeRank, int totalRank);
	public delegate void ScoresRetrieved(List<Score> scores);
	public delegate void RecordingRetrieved(Recording recording);
	public delegate void RankRetrieved(int rank, int total);
	
	private static Networking instance;
	
	private static HighscoreSubmitted highscoreCallback;
	private static ScoresRetrieved retrievalCallback;
	private static RecordingRetrieved recordingCallback;
	private static RankRetrieved rankCallback;
	
	void Awake()
	{
		instance = this;
	}
	
	void Start()
	{
		instance = this;
	}
	
	public static void SubmitOnline(string name, float speed, float time, HighscoreSubmitted callback)
	{
		const string url = "http://www.pillowdrift.com/swift/";
		
		highscoreCallback = callback;
		
		System.Net.ServicePointManager.Expect100Continue = false;
		
		Recording current = GameRecorder.current;
		
		if (current != null)
		{
			// Encode recording to base 64
			MemoryStream ms = new MemoryStream();
			current.Write(ms);
			string recording = Convert.ToBase64String(ms.ToArray());
			
			
			if (recording != "")
			{
				// Create POST request
				WWWForm form = new WWWForm();
				
				form.AddField("name", name);
				form.AddField("level", current.levelName);
				form.AddField("recording", recording);
				form.AddField("time", time.ToString());
				form.AddField("speed", speed.ToString());
				
				WWW www = new WWW(url, form);
				
				form.AddField("Expect", "");
				
				instance.StartCoroutine("WaitForSubmission", www);
			}
		}
	}
	
	public static void GetScores(ScoresRetrieved callback, string level, Metric metric, int start, int count)
	{
		retrievalCallback = callback;
		
		string url = "http://www.pillowdrift.com/swift/?type=highscores&metric={0}&start={1}&count={2}&level={3}";
		string metricString;
		
		if (metric == Metric.SPEED)
			metricString = "speed";
		else if (metric == Metric.TIME)
			metricString = "time";
		else return;
		
		// Make web request
		WWW www = new WWW(String.Format(url, metricString, start, count, level.Replace(" ", "%20")));
		
		instance.StartCoroutine("WaitForRetrieval", www);
	}
	
	public static void GetRank(RankRetrieved callback, string level, Metric metric, float score)
	{
		rankCallback = callback;
		
		string url = "http://www.pillowdrift.com/swift/?type=rank&metric={0}&level={1}&{0}={2}";
		string metricString;
		
		if (metric == Metric.SPEED)
			metricString = "speed";
		else if (metric == Metric.TIME)
			metricString = "time";
		else return;
		
		// Make web request
		WWW www = new WWW(String.Format(url, metricString, level.Replace(" ", "%20"), score));
		
		instance.StartCoroutine("WaitForRank", www);
	}
	
	// THIS DOESNT WORK
/*	public static void GetRank(RankRetrieved callback, Metric metric, int id)
	{
		rankCallback = callback;
		
		string url = "http://www.pillowdrift.com/swift/?type=rank&metric={0}&id={1}";
		string metricString;
		
		if (metric == Metric.SPEED)
			metricString = "speed";
		else if (metric == Metric.TIME)
			metricString = "time";
		else return;
		
		// Make web request
		WWW www = new WWW(String.Format(url, metricString, id));
		
		instance.StartCoroutine("WaitForRank", www);
	}*/
	
	public static void GetRecording(RecordingRetrieved callback, int id)
	{
		recordingCallback = callback;
		
		string url = "http://www.pillowdrift.com/swift/?type=recording&id={0}";
		
		WWW www = new WWW(String.Format(url, id));
		instance.StartCoroutine("WaitForRecording", www);
	}
	
	IEnumerator WaitForSubmission(WWW www)
	{
		yield return www;
		
		int speedRank = 0;
		int timeRank = 0;
		int totalRank = 0;
		
		Regex exp = new Regex("Speed rank: (?<speedrank>.*), Time rank: (?<timerank>.*), Total: (?<totalrank>.*)");
		Match match = exp.Match(www.text);
		
		if (match.Success)
		{
			int.TryParse(match.Groups["speedrank"].ToString(), out speedRank);
			int.TryParse(match.Groups["timerank"].ToString(), out timeRank);
			int.TryParse(match.Groups["totalrank"].ToString(), out totalRank);
		}
		
		highscoreCallback(speedRank, timeRank, totalRank);
	}
	
	IEnumerator WaitForRetrieval(WWW www)
	{
		List<Score> scores = new List<Score>();
		
		// Wait for request
		yield return www;
		
		// Parse scores
		if (www.isDone)
		{
			string[] eachScore = www.text.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
			
			foreach (string score in eachScore)
			{
				Regex exp = new Regex("ID: (?<id>.*), Name: (?<name>.*), Time: (?<time>.*), Speed: (?<speed>.*)");
				MatchCollection matchList = exp.Matches(score);
				
				foreach (Match match in matchList)
				{
					Score thisScore = new Score();
					
					thisScore.name = match.Groups["name"].ToString();
					
					if (int.TryParse(match.Groups["id"].ToString(), out thisScore.id) &&
						float.TryParse(match.Groups["speed"].ToString(), out thisScore.speed) &&
						float.TryParse(match.Groups["time"].ToString(), out thisScore.time))
					{
						scores.Add(thisScore);
					}
				}
			}
		}
		
		// Return scores
		retrievalCallback(scores);
	}
	
	IEnumerator WaitForRecording(WWW www)
	{
		yield return www;
		
		byte[] recBytes = Convert.FromBase64String(www.text);
				
		// Create stream from result (sorry, probably hacky)
		MemoryStream stream = new MemoryStream();
		
		// Write string to stream
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write(recBytes);
		writer.Flush();
		
		// Reset stream
		stream.Position = 0;
		
		// Read stream to recording
		BinaryReader reader = new BinaryReader(stream);
		Recording rec = Recording.Read(reader);
		
		recordingCallback(rec);
	}
	
	IEnumerator WaitForRank(WWW www)
	{
		int rank = 0;
		int total = 0;
		
		// Wait for request
		yield return www;
		
		// Parse scores
		if (www.isDone)
		{
			Regex exp = new Regex("Rank: (?<rank>.*), Total: (?<total>.*)");
			MatchCollection matchList = exp.Matches(www.text);
			
			foreach (Match match in matchList)
			{
				int.TryParse(match.Groups["rank"].ToString(), out rank);
				int.TryParse(match.Groups["total"].ToString(), out total);
				rankCallback(rank, total);
			}
		}
	}
}