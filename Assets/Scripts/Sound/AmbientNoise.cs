using UnityEngine;
using System.Collections;

public class AmbientNoise : MonoBehaviour
{
	private static AmbientNoise instance;
	
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	
	void Update()
	{
		audio.volume = Options.SFXVolume;
		transform.position = Camera.mainCamera.transform.position;
	}
	
	public static void Play()
	{
		if (instance != null && !instance.audio.isPlaying)
			instance.audio.Play();
	}
	
	public static void Stop()
	{
		if (instance != null)
			instance.audio.Stop();
	}
}
