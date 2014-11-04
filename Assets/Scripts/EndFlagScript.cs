using UnityEngine;
using System;
using System.Collections;

public class EndFlagScript : MonoBehaviour
{
	public const float WAIT_TIME = 0.5f;
	
	private const float LABEL_WIDTH = 120.0f;
	private const float LABEL_HEIGHT = 100.0f;
	
	private const float BUTTON_WIDTH = 200.0f;
	private const float BUTTON_HEIGHT = 80.0f;

	private Vector2 screenSpacePos;
	private float distance;
	
	public float ZoomOut = 1.0f;
	public float ZoomOutTimer = 0.5f;
	
	public float SaveRecordDelay = 1.0f;
	
	public string WorldMapSceneName;
	
	public ThirdPersonCamera aCam;
	
	public static bool hasFinished = false;
	public static bool savedRecording = false;
	
	private GameObject player;
	
	private bool tapped = false;
	private bool uploading = false;
	public bool endingDone = false;
	
	private Save.LevelHighScore score;

	private TutorialCamera tutorialCam;

	private float speedWaitStart = 0;
	private float timeWaitStart = 0;
	
	void Awake()
	{
		player = GameObject.Find("Player");
		tutorialCam = GameObject.FindObjectOfType<TutorialCamera>();
	}
	
	void Start()
	{
		hasFinished = false;
		savedRecording = false;
	}
	
	void Update()
	{
		if (InputManager.pressed)
			tapped = true;
		
		// If the speed text is enabled, display the speed counting up
		if (speedWaitStart > 0)
		{
			float speedCount = Mathf.Max(0.0f, score.Speed);
			float normalisedSpeed = 0;

			if (LevelSelectGUI.currentLevel != null)
				normalisedSpeed = (100.0f * speedCount) / LevelSelectGUI.currentLevel.ranks.SpeedThreshold;
			
			// Clamp normalised speed
			normalisedSpeed = Math.Min(normalisedSpeed, 100.0f);
			
			float waitElapsed = Mathf.Clamp(Mathf.Lerp(0.0f, 1.0f, (Time.time - speedWaitStart) / WAIT_TIME), 0.0f, 1.0f);
			
			float normalisedSpeedCounter = normalisedSpeed * waitElapsed;
			
			GUIController.ShowText("Speed", "Energy: " + Math.Round(normalisedSpeedCounter).ToString() + "%");
		}
		
		// If the time text is enabled, display the time counting up
		if (timeWaitStart > 0)
		{
			float timeCount = score.Time;
			
			float waitElapsed = Mathf.Clamp(Mathf.Lerp(0.0f, 1.0f, (Time.time - timeWaitStart) / WAIT_TIME), 0.0f, 1.0f);
			
			float timeCounter = timeCount * waitElapsed;
			
			GUIController.ShowText("Time", "Time: " + timeCounter.ToString(".#") + "s");
		}
	}
	
	void OnCollisionEnter(Collision collision)		
	{
		if (TutorialCamera.Enabled())
			return;
		
		Collider collider = collision.collider;
		if (collider.gameObject.tag == "Player" && !LevelState.Dead && !LevelState.HasFinished)
		{
			GUIController.HideText("Tutorial");
			
			// Set flag so this doesn't run more than once
			LevelState.HasFinished = true;
			
			// Freeze score values
			ScoreCalculator.End();
			
			//freeze the player
			LevelState.Dead = true;
			collider.rigidbody.velocity = Vector3.zero;
			collider.rigidbody.isKinematic = true;
			
			//make the player face the thing it hit
			collider.transform.LookAt(collider.transform.position + collision.contacts[0].normal, collider.transform.up);
			
			// Send a message to the player to start the mining particles.
			player.SendMessage("StartMining");			
			
			// Switch to a camera that looks at the end thingy.
			ZoomOuterizer zout = Camera.mainCamera.GetComponent<ZoomOuterizer>();
			if (zout != null)
				zout.ZoomOut(ZoomOut, ZoomOutTimer);
			
			// Do level finish routine (scores, stars and audio)
			StartCoroutine("FinishLevel");
		}
    }
	
	IEnumerator FinishLevel()
	{
		Debug.Log("Level finished!");

		// If this is a tutorial level, skip to the real level so scores and stuff are right
		if (tutorialCam != null)
		{
			LevelSelectGUI.currentLevel = Levels.GetLevel(tutorialCam.level);
			Debug.Log("In tutorial: skipping to level " + LevelSelectGUI.currentLevel.name);
		}
		
		// Update save
		// Don't save if we are replaying a recording.
		if (!GameRecorder.playingBack)
		{
			if (LevelSelectGUI.currentLevel != null)
			{
				SaveManager.Beaten(LevelSelectGUI.currentLevel);
				SaveManager.UpdateScore(LevelSelectGUI.currentLevel);
				SaveManager.Write();	
			}
		}

		// Wait time between stars unless tapped to skip
		const bool playting = true;
		const bool playfail = false;	
		
		// Which stars are unlocked
		bool scoreStar = false;
		bool timeStar = false;

		Debug.Log("Updating next level");
		
		hasFinished = true;
		
		// Calculate scores
		Debug.Log("Calculating scores");

		score = new Save.LevelHighScore();
		
		if (!GameRecorder.playingBack)
		{
	        score.Score = ScoreCalculator.finalScore;
	        score.Speed = ScoreCalculator.finalSpeed;
	        score.Time = ScoreCalculator.finalTime;
		}
		else
		{
	        score.Score = GameRecorder.current.score.score;
	        score.Speed = GameRecorder.current.score.speed;
	        score.Time = GameRecorder.current.score.time;
		}
		
		// Save recording
		Debug.Log("Save recording!");

		if (!GameRecorder.playingBack)
		{
			savedRecording = true;
			GameRecorder.StopRecording();
			Recording rec = GameRecorder.Save(true, false);
		
			// Upload score to online thing
			if(!GameRecorder.playingBack)
			{
				HighScores.PostScore(LevelSelectGUI.currentLevel, rec);

			}
			// Disable buttons so user doesn't disrupt score uploading
			GUIController.DisableButtons();
		}
		
		// Check which stars have been earnt
		Debug.Log("Checking speed star!");

		scoreStar = SaveManager.HasSurpassedSpeed(score, LevelSelectGUI.currentLevel);
		timeStar = SaveManager.HasSurpassedTime(score, LevelSelectGUI.currentLevel);
		
		// Reset press tracker so player can choose to skip
		tapped = false;
		
		// Let the camera pan back a bit
		yield return new WaitForSeconds(tapped ? 0 : WAIT_TIME);
		
		// Show first star
		Debug.Log("Showing speed star!");

		GUIController.EnableImage("Star0Locked");
		
		// Let the player know what this star's for
		//yield return new WaitForSeconds(tapped ? 0 : waitTime);
		GUIController.ShowText("Won", "Won");
		
		// Award level beaten star
		yield return new WaitForSeconds(tapped ? 0 : WAIT_TIME);
		GUIController.EnableImage("Star0");
		
		if (!tapped && playting)
			SoundManager.Play("ting");
		
		// Show score star
		yield return new WaitForSeconds(tapped ? 0 : WAIT_TIME);		
		GUIController.EnableImage("Star1Locked");
		
		// Tell the player their score
		//yield return new WaitForSeconds(tapped ? 0 : waitTime);
		// Show to one decimal place (".#")
		//f/loat normalisedSpeed = (100.0f * score.Speed) / LevelSelectGUI.currentLevel.ranks.SpeedThreshold;
		//GUIController.ShowText("Speed", "Energy: " + ((int)normalisedSpeed).ToString() + "%");

		// Updated in Update() but needs to be enabled here like this
		speedWaitStart = Time.time;
		GUIController.ShowText ("Speed", "");
		
		// Award score star if score threshold beaten
		yield return new WaitForSeconds(tapped ? 0 : WAIT_TIME);
		if (scoreStar)
			GUIController.EnableImage("Star1");
		
		player.SendMessage("StopCounting");
		
		if (!tapped)
		{
			if (scoreStar && playting)
				SoundManager.Play("ting");
			else if (scoreStar && playfail)
				SoundManager.Play("fail");
		}
		
		// Show time star
		yield return new WaitForSeconds(tapped ? 0 : WAIT_TIME);		
		GUIController.EnableImage("Star2Locked");

		// Shown in Update() but needs to be enabled here like this
		timeWaitStart = Time.time;
		GUIController.ShowText("Time", "");
		
		// Award star if time threshold beaten
		yield return new WaitForSeconds(tapped ? 0 : WAIT_TIME);
		if (timeStar)
			GUIController.EnableImage("Star2");
		
		if (!tapped)
		{
			if (timeStar && playting)
				SoundManager.Play("ting");
			else if (!timeStar && playfail)
				SoundManager.Play("fail");
		}
		
		yield return new WaitForSeconds(tapped ? 0 : WAIT_TIME);
		
		ScoreCalculator.Speed = 0;
		player.SendMessage("EndMining");
		
		// Show overall
		if (!GameRecorder.playingBack)
		{
			Level thisLevel = Levels.GetLevel(Application.loadedLevelName);
			if (thisLevel != null && thisLevel.highscores)
			{
				// Activate highscores menu.
				AskName.ActivateMenu();
			}
			
			GUIController.ShowText("Overall", "Overall");
			
			if (SaveManager.GotWinStar(LevelSelectGUI.currentLevel))
				GUIController.EnableImage("OverallStar0");
			else
				GUIController.EnableImage("OverallStar0Locked");
			
			if (SaveManager.GotSpeedStar(LevelSelectGUI.currentLevel))
				GUIController.EnableImage("OverallStar1");
			else
				GUIController.EnableImage("OverallStar1Locked");
			
			if (SaveManager.GotTimeStar(LevelSelectGUI.currentLevel))
				GUIController.EnableImage("OverallStar2");
			else
				GUIController.EnableImage("OverallStar2Locked");
		}
		
		bool worldBeaten = true;
		foreach (Level level in Levels.AllLevels)
		{
			if (level.world == LevelSelectGUI.currentLevel.world)
			{
				if (!SaveManager.GotWinStar(level) ||
					!SaveManager.GotSpeedStar(level) ||
					!SaveManager.GotTimeStar(level))
				{
					worldBeaten = false;
				}
			}
		}
		
		if (worldBeaten)
		{
			if (LevelSelectGUI.currentLevel.world == 1)
			{
				GameCenterSingleton.Instance.AddAchievementProgress("w_1_o", 100.0f);
			}
			if (LevelSelectGUI.currentLevel.world == 2)
			{
				GameCenterSingleton.Instance.AddAchievementProgress("w_2_o", 100.0f);
			}
			if (LevelSelectGUI.currentLevel.world == 3)
			{
				GameCenterSingleton.Instance.AddAchievementProgress("w_3_o", 100.0f);
			}
			if (LevelSelectGUI.currentLevel.world == 4)
			{
				GameCenterSingleton.Instance.AddAchievementProgress("w_4_o", 100.0f);
			}
			if (LevelSelectGUI.currentLevel.world == 5)
			{
				GameCenterSingleton.Instance.AddAchievementProgress("w_5_o", 100.0f);
			}
		}
		
		if (!uploading)
			GUIController.EndLevel(true);
		
		endingDone = true;
	}
}
