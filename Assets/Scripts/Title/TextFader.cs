using UnityEngine;
using System.Collections;

public class TextFader : MonoBehaviour
{
	public float fadeRate = 1.0f;
	public float delay = 0.0f;
	
	public bool invisibleOnStart = false;
	
	bool fadeout = false;
	bool fadein = false;

	private float startTime;

	void Start()
	{
		startTime = Time.time;
	}
	
	void Update()
	{
		if (Time.time < startTime + delay)
			return;

		if (fadeout || fadein)
		{
			Color c = guiText.material.color;
			c.a = Mathf.Lerp(c.a, (fadeout ? 0 : 1), Time.deltaTime * fadeRate);
			guiText.material.color = c;
		}
	}
	
	public void FadeOut()
	{
		fadeout = true;
		fadein = false;
		if (collider != null)
			collider.enabled = false;
	}
	
	public void FadeIn()
	{
		fadein = true;
		fadeout = false;
		if (collider != null)
			collider.enabled = true;
	}
}
