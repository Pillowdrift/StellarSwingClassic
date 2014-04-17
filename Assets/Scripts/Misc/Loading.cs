using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
	public enum Transition
	{
		NONE,
		FOV
	}
	
	const string LOADING_SCENE = "LoadingScene";
	
	static string nextLevel = "";
	static Transition transition = Transition.NONE;
	
	float circleLerp = 0;
	
	float srtatingFov;
	
	const float RATE = 1;
	
	void Start()
	{
		Camera.mainCamera.GetComponent<GUILayer>().enabled = false;
		circleLerp = 0;
		
		srtatingFov = Camera.mainCamera.fov;
		// Orient with camera
		//transform.position = Camera.mainCamera.transform.position + Camera.mainCamera.transform.forward * 10.0f;
		//transform.rotation = Camera.mainCamera.transform.rotation;
		
		// Load splash screen if this is loaded first
		// (by default it's loaded as the first level to preload it)
		//if (nextLevel == "")
			//Application.LoadLevel("Splash");
		
		ScreenFade.FadeFrom(new Color(0, 0, 0, 0));
	}
	
	void Update()
	{
		if (transition == Transition.FOV && circleLerp < 1)
		{				
			Camera.mainCamera.fov = Mathf.Lerp(srtatingFov, 180.0f, 1 - Mathf.Sqrt(1-(circleLerp*circleLerp)));		
			circleLerp += Time.deltaTime * RATE;			
		}
		else if (nextLevel != "")
		{
			if (Application.GetStreamProgressForLevel(nextLevel) == 1)
			{
				Application.LoadLevel(nextLevel);
				nextLevel = "";
			}
			
		}
	}
	
	public static void Load(string name)
	{
		Load(name, Transition.NONE);
	}
	
	public static void Load(string name, Transition transitionToUse)
	{
		nextLevel = name;
		transition = transitionToUse;
		Application.LoadLevelAdditive(LOADING_SCENE);
	}
}
