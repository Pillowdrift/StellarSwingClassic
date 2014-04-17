using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TutorialCamera : MonoBehaviour
{
	public enum TutorialEvent
	{
		NONE,
		LOAD_PRESET_LEVEL,
		LOAD_NEXT_LEVEL,
		START_PLAYER,
		PLAY_RECORDING,
		RESET_PLAYER
	}
	
	public string level;
	public GameObject parent;
	
	private List<TutorialPoint> points;
	
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	
	private Vector3 nextPosition;
	private Quaternion nextRotation;
	
	private float currentLerpCoeff = 1.0f;
	
	private int index = 0;
	
	private GUIButton nextButton;
	private GUIButton lastButton;
	private GUIButton skipButton;
	
	private GameObject theCamera;
	
	private static bool tutorialEnabled = false;
	
	private bool moving = false;
	
	private bool init = false;
		
	public IEnumerator Start()
	{
		tutorialEnabled = true;
		
		points = new List<TutorialPoint>();
		
		nextButton = GameObject.Find("TutorialNext").GetComponent<GUIButton>();
		lastButton = GameObject.Find("TutorialBack").GetComponent<GUIButton>();
		skipButton = GameObject.Find("TutorialSkip").GetComponent<GUIButton>();
		
		lastButton.enabled = false;
		nextButton.enabled = false;
		skipButton.enabled = false;
		
		if (level != "")
		{
			//Application.LoadLevelAdditive(level);
			//StartCoroutine(LoadDummyLevel(level));
			AsyncOperation async = Application.LoadLevelAdditiveAsync(level);
			yield return async;
		}
		
		nextButton.enabled = true;
		skipButton.enabled = true;
		
		// Disable camera
		theCamera = GameObject.Find("TheCamera");
		if (theCamera != null)		
		{
			theCamera.active = false;
		}
		
		// Freeze player
		GameObject player = GameObject.Find("Player");
		if (player != null)
		{
			player.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		
		if (parent != null)
		{
			LoadPoints(parent);
		}
		
		init = true;
		
		//Tutorial.ShowText("Tap", "Tap to continue", 0, TextAlignment.Center, TextAnchor.LowerLeft, 0.0f, 0.0f);
	}
	
	IEnumerator LoadDummyLevel(string level)
	{
		// Wait a frame
		yield return null;
		
		GameObject.Find("Player").AddComponent<TutorialPlayer>();
	}
	
	public void Update()
	{
		if (!init)
			return;
		
		// Show or hide buttons
		if (index > 0)
			lastButton.enabled = true;
		else
			lastButton.enabled = false;
		
		if (index < points.Count-1)
			nextButton.enabled = true;
		else
			nextButton.enabled = false;
		
		// Update lerp coefficient
		currentLerpCoeff += Time.deltaTime;
		
		if (currentLerpCoeff > 1.0f)
			currentLerpCoeff = 1.0f;
		
		if (moving && currentLerpCoeff >= 1.0f)
		{
			moving = false;
			
			HandleEvent(points[index].endEvent);
			
			if (index < points.Count)
			{
				Tutorial.ShowText(points[index].textName, points[index].text, 0, TextAlignment.Center, TextAnchor.MiddleCenter, points[index].textX, points[index].textY);
			}
		}
		
		Vector3 newPos = Vector3.Lerp(lastPosition, nextPosition, Mathf.SmoothStep(0, 1, currentLerpCoeff));
		Quaternion newRot = Quaternion.Lerp(lastRotation, nextRotation, Mathf.SmoothStep(0, 1, currentLerpCoeff));
		
		if (!float.IsNaN(newRot.x))
		{
			transform.position = newPos;
			transform.rotation = newRot;
		}
	}
	
	public bool Stopped
	{
		get
		{
			return (currentLerpCoeff >= 1.0f);
		}
	}
	
	public void Next()
	{
		NextPoint();
		if (index < points.Count)
		{
			Tutorial.HideText(points[index].textName);
		}
	}
	
	public void Prev()
	{
		PreviousPoint();
		if (index < points.Count)
		{
			Tutorial.HideText(points[index].textName);
		}
	}
	
	public void Skip()
	{
		// Disable buttons
		lastButton.enabled = false;
		nextButton.enabled = false;
		skipButton.enabled = false;
		
		// Load final point
		EndPoint();
		if (index < points.Count)
		{
			Tutorial.HideText(points[index].textName);
		}
	}
	
	public void LoadPoints(GameObject list)
	{
		points = new List<TutorialPoint>(list.GetComponentsInChildren<TutorialPoint>());
		points.Sort();
			
		if (points.Count > 0)
		{
			moving = true;
			
			currentLerpCoeff = 1.0f;
			
			// Just sit at the first point until a button is pressed
			lastPosition = points[0].transform.position;
			lastRotation = points[0].transform.rotation;
			
			nextPosition = lastPosition;
			nextRotation = lastRotation;
			
			transform.position = nextPosition;
			transform.rotation = nextRotation;
		}
	}
	
	private void NextPoint()
	{
		transform.position = nextPosition;
		transform.rotation = nextRotation;
		
		lastPosition = nextPosition;
		lastRotation = nextRotation;
		
		index++;
		if (index < points.Count)
		{
			currentLerpCoeff = 0.0f;
			
			nextPosition = points[index].transform.position;
			nextRotation = points[index].transform.rotation;
			
			StartEvent();
		}
		
		moving = true;
	}
	
	private void PreviousPoint()
	{
		transform.position = nextPosition;
		transform.rotation = nextRotation;
				
		lastPosition = nextPosition;
		lastRotation = nextRotation;
		
		index--;
		if (index >= 0 && index < points.Count)
		{
			currentLerpCoeff = 0.0f;
			
			nextPosition = points[index].transform.position;
			nextRotation = points[index].transform.rotation;
			
			StartEvent();
		}
		
		moving = true;
	}
	
	private void EndPoint()
	{
		// Final point
		index = points.Count-1;
		
		// Set up interpolation
		currentLerpCoeff = 0.0f;
		
		lastPosition = transform.position;
		lastRotation = transform.rotation;
		
		nextPosition = points[index].transform.position;
		nextRotation = points[index].transform.rotation;
		
		moving = true;
	}
	
	public static bool Enabled()
	{
		return tutorialEnabled;
		//return GameObject.FindObjectOfType(typeof(TutorialCamera)) != null;
	}
	
	private void StartEvent()
	{
		HandleEvent(points[index].startEvent);
	}
	
	private void HandleEvent(TutorialEvent tutorialEvent)
	{
		switch (tutorialEvent)
		{
		case TutorialEvent.NONE:
			break;
		case TutorialEvent.LOAD_PRESET_LEVEL:
			LevelSelectGUI.currentLevel = Levels.GetLevel(level);
			Loading.Load(level);
			break;
		case TutorialEvent.LOAD_NEXT_LEVEL:
			LoadLevel.LoadALevel("next");
			break;
		case TutorialEvent.START_PLAYER:
			GameRecorder.StopPlayback();
			
			tutorialEnabled = false;
			
			// Disable buttons
			lastButton.enabled = false;
			nextButton.enabled = false;
			skipButton.enabled = false;
			
			// Get rid of mainCamera tag
			tag = "";
			
			// Unfreeze camera
			{
				theCamera.active = true;
				theCamera.GetComponent<LevelStart>().Start();
			}
			
			// Unfreeze player
			GameObject player = GameObject.Find("Player");
			if (player != null)
			{
				player.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
			
			// Disable tutorial camera
			gameObject.active = false;
			break;
		case TutorialEvent.PLAY_RECORDING:
			string test = "Recordings/" + points[index].recordingName;
			TextAsset ta = (TextAsset)Resources.Load(test);
			Stream s = new MemoryStream(ta.bytes);
			BinaryReader br = new BinaryReader(s);
			GameRecorder.StartPlaybackTutorial(Recording.Read(br));
			break;
		case TutorialEvent.RESET_PLAYER:
			GameRecorder.StopPlayback();
			GameObject.Find("Player").BroadcastMessage("Reload");
			break;
		}
		
	}
}
