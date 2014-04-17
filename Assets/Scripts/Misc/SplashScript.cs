using UnityEngine;
using System.Collections;

public class SplashScript : MonoBehaviour
{
	public float fadeStartsIn = 1.0f;
	
	private bool startedFade = false;
	
	void Start()
	{		
		StartCoroutine("FadeStart");
	}
	
	void Update()
	{
		// Switch to the fade straight away if the user presses a button
		if (!startedFade && Input.GetMouseButton(0))
		{
			startedFade = true;
			ScreenFade.FadeFrom(new Color(0, 0, 0, 0));
			ScreenFade.FadeTo(new Color(0, 0, 0, 1.0f), 1.0f);
		}
		
		if (ScreenFade.IsDoneFading())
		{
			ScreenFade.ResetDoneFading();
			StartCoroutine("LevelChange");
		}
	}
	
	IEnumerator FadeStart()
	{
		yield return new WaitForSeconds(fadeStartsIn);
		
		if (!startedFade)
		{
			startedFade = true;
			ScreenFade.FadeFrom(new Color(0, 0, 0, 0));
			ScreenFade.FadeTo(new Color(0, 0, 0, 1.0f), 1.0f);
		}
	}
	
	IEnumerator LevelChange()
	{
		yield return new WaitForSeconds(0.2f);
		
		Application.LoadLevel("Title");
	}
}
