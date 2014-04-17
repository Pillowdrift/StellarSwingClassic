// #define ENABLE_BUG_BUTTON

using UnityEngine;
using System.Collections;

public class EndDeathScript : MonoBehaviour
{
	private const float LABEL_WIDTH = 180.0f;
	private const float LABEL_HEIGHT = 100.0f;
	
	private const float BUTTON_WIDTH = 200.0f;
	private const float BUTTON_HEIGHT = 80.0f;
	
	private const float RESET_TIME = 1.0f;

	public float minDistanceForWarning = 10.0f;
	
	public bool explode = true;
	
	private Vector2 screenSpacePos;
	private float distance;
	
	public string WorldMapSceneName;
	
	public ThirdPersonCamera aCam;
	
	public static bool hasFinished = false;
	
	public GameObject playerWarningPrefab;
	
	private GameObject player;
	private GameObject playerWarning;
	
	private GrapplingHook grapplingHook;
	
	void Start()
	{
		hasFinished = false;
		
		player = GameObject.Find("Player");
		grapplingHook = player.GetComponent<GrapplingHook>();
		
		if (playerWarningPrefab != null)
			playerWarning = GameObject.Instantiate(playerWarningPrefab) as GameObject;
	}
	
	void Update()
	{
		if (playerWarning != null)
		{
			// Move death warning under player
			Vector3 position = player.transform.position;
			position.y = transform.position.y + collider.bounds.extents.y;
			playerWarning.transform.position = position;
			
			// Fade its transparency on player distance
			Color colour = playerWarning.renderer.material.color;
			float dist = (player.transform.position - playerWarning.transform.position).magnitude;
			colour.a = minDistanceForWarning / dist;
			playerWarning.renderer.material.color = colour;
		}
	}
	
	void OnTriggerEnter(Collider collided)
	{
		if (collided.tag == "Player" && TutorialCamera.Enabled())
		{
			GameRecorder.StopPlayback();
			if (explode)
			{
				collided.gameObject.SendMessage("Explode");
				SoundManager.Play("crash");
				
				collided.gameObject.SendMessage("Reload");
			}
		
			return;
		}
		
		if (collided.tag == "Player" && !LevelState.Dead)
		{
			// Disable text
			GUIController.DisableTexts();
			
			// Update drone count
			if (SaveManager.save != null)
			{
				SaveManager.save.droneCount++;
				SaveManager.Write();
				
				// Display drone count (disabled)
				/*
				GameObject thingy = Tutorial.ShowText("DroneText", "Drones lost: " + SaveManager.save.droneCount, 0, TextAlignment.Center, TextAnchor.MiddleCenter, 0.5f, 0.5f);
				Mover mover = thingy.AddComponent<Mover>();
				mover.direction = Vector3.up;
				mover.speed = 0.2f;
				TextFader fader = thingy.AddComponent<TextFader>();
				fader.fadeRate = 1.0f;
				fader.FadeOut();
				*/
			}
			
			// Update level state
			GUIController.LevelStarted = false;
			LevelStart.started = false;
			LevelState.Dead = true;
			
			if (explode)
			{
				// Create explosion effect
				collided.gameObject.SendMessage("Explode");
				
				// Crash sound
				SoundManager.Play("crash");
				
				// Disable renderers if exploding
				player.renderer.enabled = false;
				player.transform.FindChild("Shield").renderer.enabled = false;
			}
			
			// Disable default camera controller until level restarts
			ThirdPersonCamera cameraController = Camera.mainCamera.GetComponent<ThirdPersonCamera>();
			if (cameraController != null)
				cameraController.enabled = false;
			
			// Detach grappling hook so we don't keep swinging
			grapplingHook.Detach();
	
			// I don't really remember why
			ControlManager.DisabledFrame = true;
			
			// Pause and then restart the level
			StartCoroutine(PauseBeforeReset());
		}
    }
	
	IEnumerator PauseBeforeReset()
	{
		float time = 0.0f;
		while (time < RESET_TIME && !Input.GetMouseButtonUp(0))
		{
			time += Time.deltaTime;
			yield return null;
		}
		
		// Stop the recording, and also the line it generates
		GameRecorder.StopRecording();
		
		// Reset player
		player.BroadcastMessage("Reload");
		player.renderer.enabled = true;
		
		// Lerp camera back to start transform
		Camera.mainCamera.SendMessage("LerpToStartPos");
	}
}
