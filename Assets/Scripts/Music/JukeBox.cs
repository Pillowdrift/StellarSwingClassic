using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class JukeBox : MonoBehaviour {
	
	// The static instance
	public static JukeBox Instance = null;
	
	// The AudioSource to play.
	public AudioClip clip;
	
	// Create a static instance.
	void Awake()
	{	
		if (Instance == null)
		{
			Instance = this;
			
			// Make sure this object isn't destroyed when a new scene is loaded.
			DontDestroyOnLoad(this.gameObject);	
			
			// Play the music.
			OnLevelWasLoaded(0);		
		}
		else
		{
			// Play the music.
			Instance.Play(this.clip);
			
			DestroyImmediate(gameObject);
		}
	}
	
	void Update()
	{
		audio.volume = Options.MasterVolume * Options.BGMVolume;
	}
	
	void OnLevelWasLoaded(int levelNum)	
	{
		switch (Application.loadedLevelName)
		{
		case "Tutorial 1":
			Play(SoundManager.sounds["World 1"]);
			break;
		case "Credits":
			Play(SoundManager.sounds["Credits"]);
			break;
		case "Title":
			Play(SoundManager.sounds["Title"]);
			break;
		default:
			if (LevelSelectGUI.currentWorld == "World6")
			{
				Play(SoundManager.sounds["World 6"]);
			}
			else
			{
				Regex exp = new Regex("World (?<world>.*) Level (?<level>.*)");
				Match match = exp.Match(Application.loadedLevelName);
				
				if (match.Success)
				{
					Play(SoundManager.sounds["World " + match.Groups["world"].ToString()]);
				}
			}
			break;
		}
	}
	
	// Force plays some music.
	public void ForcePlay(AudioClip music)
	{
		AudioSource source = GetComponent<AudioSource>();
		
		if (source != null && SaveManager.save != null)		
		{
			clip = music;
			source.loop = true;
			source.audio.clip = clip; 
			source.volume = Options.MasterVolume * Options.BGMVolume;
			source.Play();
		}
	}
	
	// Play some music.
	public void Play(AudioClip music)
	{
		if (clip == music)
			return;
			
		ForcePlay(music);
	}
}
