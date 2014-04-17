using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameRecorder : MonoBehaviour
{
	public const int fps = 20;
	
	public const bool enableLine = true;
	public const float maxLineTime = 0.2f;
	
	public static bool IsRecording { get { return recording; } }
	
	public static Recording current = null;
	
	public static bool played = false;
	
	public static string lastWritten = "";
	
	public static bool playingBack = false;
	public static bool standalone = false;
	
	private static bool recording = false;
	private static bool playback = false;
	
	private static float timeDif = 0.0f;
	private static GameRecorder instance = null;
	
	// State to allow reloading of level
	private static bool reloading = false;
	private static bool wonLast = false;
	private static bool fromMenu = false;
	
	private static GrapplingHook grappleScript;
	
	private float lastTime = 0.0f;
	
	private static int points;
	private static LineRenderer lineRenderer;
	private static Queue<Vector3> linePoints;
	private static Color lineColour = Color.white;
	
	private float time = 0.0f;
	
	void Awake()
	{
		instance = this;
		grappleScript = GetComponent<GrapplingHook>();
		
		if (enableLine && lineRenderer == null)
		{
			GameObject obj = new GameObject("Trail renderer");
			linePoints = new Queue<Vector3>();
			lineRenderer = obj.AddComponent<LineRenderer>();
			lineRenderer.material.shader = Shader.Find("Particles/Additive");
			lineRenderer.SetColors(Color.clear, lineColour);
			points = 1;
			lineRenderer.SetVertexCount(points);
			lineRenderer.SetWidth(0.3f, 0.3f);
			lineRenderer.enabled = true;
		}
	}
	
	void Start()
	{
		if (reloading)
		{
			reloading = false;
			
			lastTime = 0.0f;
			
			if (fromMenu)
			{
				GUIController.GUIPlaybackRecording();
			}
			else
			{
				GUIController.EndLevel(wonLast);
				GUIController.HideText("Text");
				
				if (wonLast)
				{
					EndFlagScript.hasFinished = true;
					
					GUIController.ShowText("Score", ScoreCalculator.finalScoreString);
					GUIController.ShowText("Speed", ScoreCalculator.finalSpeedString);
					GUIController.ShowText("Time", ScoreCalculator.finalTimeString);
				}
				else
				{
					EndDeathScript.hasFinished = true;
				}
			}
			
			GUIController.HideText("Text");
			Camera.mainCamera.GetComponent<ThirdPersonCamera>().enabled = true;
			Playback(current);
		}
	}
	
	void FixedUpdate()
	{
		if (!recording || GameObject.Find("Player").renderer.enabled == false)
			lineRenderer.enabled = false;
		else
			lineRenderer.enabled = true;
		
		// Automatically dequeue old points
		int pointsToKeep = (int)(maxLineTime * fps);
		while (linePoints.Count > pointsToKeep)
		{
			linePoints.Dequeue();
		}
		
		// Add all vertices to line renderer
		int i = 0;
		lineRenderer.SetColors(Color.clear, lineColour);
		lineRenderer.SetVertexCount(linePoints.Count + 1);
		foreach (Vector3 point in linePoints)
		{
			lineRenderer.SetPosition(i, point);
			i++;
		}
		
		if (enableLine && linePoints.Count > 0)
			lineRenderer.SetPosition(i, transform.position);
		
		if (playback && current != null)
		{
			if (!TutorialCamera.Enabled())
			{
				GUIController.GUIPlaybackRecording();
				
				ControlManager.Disabled = true;
				
				GUIController.ShowText("Replay", "Replay Mode");
			}
			
			time += Time.deltaTime;
			
			if (time < current.score.time)
			{
				transform.position = current.GetPosition(time);
				if (!rigidbody.isKinematic)
					rigidbody.velocity = current.GetVelocity(time);
				
				transform.LookAt(transform.position + rigidbody.velocity);
				
				if (current.IsGrappling(time) && current.GetGrapplePos(lastTime) != current.GetGrapplePos(time))
				{
					if (!current.IsGrappling(lastTime))
						SoundManager.Play("attach");
					
					RaycastHit hit = new RaycastHit();
					hit.point = current.GetGrapplePos(time);
					
					grappleScript.Attach(hit, -1);
				}
				else if (!current.IsGrappling(time) && current.IsGrappling(lastTime))
				{
					grappleScript.Detach();
				}
			}
			
			lastTime = time;
		}
	}
	
	public static void StartRecording()
	{
		if (instance != null)
		{
			current = new Recording();
			current.levelName = Application.loadedLevelName;
			current.fps = fps;
			timeDif = 1.0f / (float)fps;
			
			recording = true;
			
			// Start recording coroutine
			instance.StartCoroutine("DoRecord");
		}
	}
	
	public static void StartPlayback(Recording rec)
	{
		playingBack = true;
		current = rec;
		Playback(current);
	}
	
	public static void StartPlaybackTutorial(Recording rec)
	{
		instance.time = 0.0f;
		StopPlayback();
		StartPlayback(rec);
	}
	
	public static void StartPlaybackFromMenu(Recording rec)
	{
		playingBack = true;
		current = rec;
		wonLast = false;
		fromMenu = true;
		reloading = true;
		standalone = true;
		Loading.Load(rec.levelName);
	}
	
	public static void StartPlaybackLevelEnd(Recording rec, bool won)
	{
		playingBack = true;
		current = rec;
		wonLast = won;
		fromMenu = false;
		reloading = true;
		Loading.Load(rec.levelName);
	}
	
	public static void Restart()
	{
		StartPlayback(current);
	}
	
	private static void Playback(Recording rec)
	{
		playingBack = true;
		playback = true;
		played = true;	
	}
	
	public static void StopRecording()
	{
		if (instance != null && recording)
		{
			recording = false;
			instance.AddPoint();
		}
	}
	
	public static void StopPlayback()
	{
		playback = false;
		playingBack = false;
	}
	
	public static Recording Save(bool saveScores, bool flag)
	{
		recording = false;
		
		if (current != null)
		{
			current.score.time = ScoreCalculator.finalTime;
			current.score.score = ScoreCalculator.finalScore;
			current.score.speed = ScoreCalculator.finalSpeed;
			current.playername = SaveManager.save.playerName;
			
			lastWritten = UnixTime.Current.ToString();
			//current.Write(lastWritten);
		}
		
		return current;
	}
	
	public static void Reset()
	{
		StopRecording();
		StopPlayback();
		
		playingBack = false;
		standalone = false;
	}
	
	private IEnumerator DoRecord()
	{		
		// Run until recording = false
		if (recording)
		{
			AddPoint();
			
			yield return new WaitForSeconds(timeDif);
			StartCoroutine("DoRecord");
		}
		
		yield return null;
	}
	
	private void AddPoint()
	{
		// An extra point is always needed for the current position
		points++;
		
		if (enableLine)
		{
			linePoints.Enqueue(transform.position);
		}
		
		current.Add(transform.position, rigidbody.velocity, grappleScript.IsGrappling(), grappleScript.GetPos());
	}
}
