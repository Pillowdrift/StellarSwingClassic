using UnityEngine;
using System.Collections;

public class GUIButtonFade : MonoBehaviour
{
	public const float normalAlpha = 0.5f;
	public const float pressedAlpha = 1.0f;
	
	private const float ALPHA_MIN = 0.01f;
	
	public float fadeRate = 1.0f;
	
	private GUIButton button;
	
	void Start()
	{
		// Enable guiTexture (but keep it faded out if button disabled)
		guiTexture.enabled = true;
		
		// Set initial alpha to minimum
		SetAlpha(normalAlpha);
		
		// Get attached button
		button = GetComponent<GUIButton>();
		if (button != null && !button.enabled)
		{
			SetAlpha(0);
		}
	}
	
	void Update()
	{
		if (guiTexture != null && button != null)
		{
			// Fade in if pressed
			if (button.enabled && InputManager.held && guiTexture.HitTest(InputManager.currentPosition))
			{
				Color col = guiTexture.color;
				col.a = Mathf.Lerp(col.a, pressedAlpha, RealTime.realDeltaTime * fadeRate);
				guiTexture.color = col;
			}
			// Fade in if enabled
			else if (button.enabled && GetAlpha() < normalAlpha)
			{
				Color col = guiTexture.color;
				col.a = Mathf.Lerp(col.a, normalAlpha, RealTime.realDeltaTime * fadeRate);
				guiTexture.color = col;
				
			}
			// Fade out if disabled
			else if (!button.enabled)
			{
				Color col = guiTexture.color;
				col.a = Mathf.Lerp(col.a, 0.0f, RealTime.realDeltaTime * fadeRate);
				guiTexture.color = col;
			}
			
			if (guiTexture.color.a < ALPHA_MIN)
				guiTexture.enabled = false;
			else
				guiTexture.enabled = true;
		}
		
		/*bool fadeToMin = true;
		
		if ((button == null || button.enabled) && guiTexture.HitTest(InputManager.currentPosition))
		{
			if (InputManager.held)
			{
				SetAlpha(Mathf.Lerp(GetAlpha(), maxAlpha, Time.fixedDeltaTime * fadeRate));
				
				fadeToMin = false;
			}
		}
		
		if (button != null && !button.enabled)
		{
			SetAlpha(Mathf.Lerp(GetAlpha(), 0, Time.fixedDeltaTime * fadeRate * 2.0f));
		}
		else if (fadeToMin)
		{
			SetAlpha(Mathf.Lerp(GetAlpha(), minAlpha, Time.fixedDeltaTime * fadeRate));
		}*/
	}
	
	void SetAlpha(float a)
	{
		Color col = guiTexture.color;
		col.a = a;
		guiTexture.color = col;
	}
	
	float GetAlpha()
	{
		return guiTexture.color.a;
	}
}
