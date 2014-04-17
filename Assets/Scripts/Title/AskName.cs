using UnityEngine;
using System.Collections;

public class AskName : MonoBehaviour {
	private const int MAX_RANK = 100;
	
	// Whether or not the menu is active.
	private static bool menuActive = false;
	
	// Whether or not we are uploading the score
	public static bool Uploading = false;
	
	// The player name
	private string playername;
	
	// The theme for the gui
	public GUISkin Skin;
	public GUISkin BigSkin;
	
	// Activate the ask name dialog.
	public static void ActivateMenu()
	{
		if (!Options.Networking)
			return;	
		
		// Activate the menu if we have an internet connection
#if UNITY_EDITOR || (!UNITY_IPHONE && !UNITY_ANDROID)
		//if (Network.player.ipAddress.ToString() != "127.0.0.1")
#elif UNITY_IPHONE || UNITY_ANDROID
		if (iPhoneSettings.internetReachability == iPhoneNetworkReachability.ReachableViaWiFiNetwork)
#endif
		{
			menuActive = true;
		}
		//else
		{
			return;
		}
		
		// Activate the text
		GUIController.ShowText("Online", "Post Score Online");		
	}
	
	public static void DeactivateMenu()
	{
		menuActive = false;
		
		// Deactivate the text
		//GUIController.HideText("Online");
	}
	
	// Use this for initialization
	void Start()
	{
		menuActive = false;
		playername = SaveManager.save.playerName;
	}
	
	// If the menu is active then draw it.
	void OnGUI()
	{	
		// Activate the skin
		GUISkin old = GUI.skin;
		GUI.skin = Screen.width > 1500 ? BigSkin : Skin;
		
		if (menuActive)
		{
			GUILayout.BeginArea(new Rect(0, Screen.height * 0.9f, Screen.width, Screen.height * 0.2f));
			
				GUILayout.BeginHorizontal();
					
					GUILayout.FlexibleSpace();
			
					playername = GUILayout.TextField(playername, new GUILayoutOption[] { GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.05f) });
			
					if (GUILayout.Button("Upload", new GUILayoutOption[] { GUILayout.Width(Screen.width * 0.1f), GUILayout.Height(Screen.height * 0.05f) }))
					{
						menuActive = false;
						GUIController.DisableButtons();
						
						SaveManager.save.playerName = playername;
						SaveManager.Write();
						
						// Activate some text stating that the score is being posted.
						GUIController.ShowText("Online", "Posting Score...");
						
						// Get the the rank of the score.
						Networking.SubmitOnline(SaveManager.save.playerName, ScoreCalculator.finalSpeed, ScoreCalculator.finalTime, submitted);
					}
			
					GUILayout.FlexibleSpace();
				
				GUILayout.EndHorizontal();
			
			GUILayout.EndArea();
		}
		
		// Put the skin back
		GUI.skin = old;
	}
	
	// Callback for done uploading scores
	public void submitted(int speedRank, int timeRank, int totalRank)
	{
		Uploading = false;
		
		// Check if valid
		if (speedRank < 0)
		{
			// The name contained some profanity or was rejected for another reason
			GUIController.ShowText("Online3", "Keep it clean!");
			ActivateMenu();
					
			if (((EndFlagScript)Component.FindObjectOfType(typeof(EndFlagScript))).endingDone)
				GUIController.EndLevel(true);
		}
		else
		{
			GUIController.GUILevelWin();
			
			// Posted score
			if (totalRank > MAX_RANK)
				totalRank = MAX_RANK;
			
			// Update the text.
			if (speedRank > MAX_RANK)
				GUIController.ShowText("Online", "Energy: Unranked");
			else
				GUIController.ShowText("Online", "Energy: " + speedRank.ToString() + " / " + totalRank.ToString());
			
			if (timeRank > MAX_RANK)
				GUIController.ShowText("Online2", "Time: Unranked");
			else
				GUIController.ShowText("Online2", "Time: " + timeRank.ToString() + " / " + totalRank.ToString());
					
			if (((EndFlagScript)Component.FindObjectOfType(typeof(EndFlagScript))).endingDone)
				GUIController.EndLevel(true);
		}
	}	
}
