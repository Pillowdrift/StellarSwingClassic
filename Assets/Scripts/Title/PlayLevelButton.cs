using UnityEngine;
using System.Collections;

public class PlayLevelButton : MonoBehaviour
{
	// Public vars
	public float TimeDelay = 0.5f;
	
	// Private vars
	private Vector3 originalScale;
	private bool pressed = false;
	private float timer = 0.0f;
	
	// Use this for initialization
	void Start()
	{
		originalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, originalScale, 5.0f * Time.deltaTime);
		
		// Load the level once the button is back to it's original size.
		if (pressed)
		{
			timer -= Time.deltaTime;
			if (timer < 0)
			{
				gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
				Camera.mainCamera.GetComponent<GUILayer>().enabled = false;
				LevelSelectGUI.worldToShow = LevelSelectGUI.currentWorld;
				LevelSelectGUI.levelToShow = LevelSelectGUI.currentLevel.number;
				LevelSelectGUI.LoadSelectedLevel();
				pressed = false;
			}
		}
	}
	
	void OnMouseDown()
	{
		// If the highscores menu is open, then we can't be pressed.
		if (!HighScoresGUI.enable)
		{
			LevelSelectGUI.canSpin = false;
			
			GameObject highscoresbutton = GameObject.Find("Highscores");
			if (highscoresbutton != null)
				highscoresbutton.active = false;
			GameObject rButton = GameObject.Find("ReturnButton");
			if (rButton != null)
				rButton.active = false;
			transform.localScale *= 1.1f;
			pressed = true;
			timer = TimeDelay;
		}
	}
}
