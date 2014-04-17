using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GUITexture))]
public class ScreenFade : MonoBehaviour
{
	private static GameObject fadeObject;
	private static Texture2D fadeTexture;
	private static Color fadeColour;
	private static float fadeSpeed;
	
	private static bool fading = false;
	private static bool doneFading = false;
	
	private static float amountFaded;
	
	void Start()
	{
		Color col = guiTexture.color;
		col.a = 0;
		guiTexture.color = col;
	}
	
	void Update()
	{
		if (fading)
		{
			// Interpolate towards colour
			Color col = guiTexture.color;
			col = Color.Lerp(col, fadeColour, fadeSpeed * Time.deltaTime);
			amountFaded += Time.deltaTime * Mathf.Abs(fadeSpeed);
			guiTexture.color = col;
			
			// Stop when target colour reached
			if (amountFaded >= 1.0f)
			{
				fading = false;
				doneFading = true;
			}
		}
	}
	
	public static void FadeFrom(Color colour)
	{
		if (fadeTexture == null)
			CreateTexture();
		
		fadeObject.guiTexture.color = colour;
	}
	
	public static void FadeTo(Color colour, float speed)
	{
		if (fadeTexture == null)
			CreateTexture();
		
		amountFaded = 0;
		
		// Start fade towards colour
		fadeColour = colour;
		fadeSpeed = speed;
		
		fading = true;
	}
	
	public static bool IsDoneFading()
	{
		return doneFading;
	}
	
	public static void ResetDoneFading()
	{
		doneFading = false;
	}
	
	private static void CreateTexture()
	{
		// Create a persistent game object
		fadeObject = new GameObject("FadeObject");
		GameObject.DontDestroyOnLoad(fadeObject);
		
		// Add this behaviour to game object
		fadeObject.AddComponent<ScreenFade>();
		
		// Add to ignore raycast so it doesn't look like you're always over a GUI element
		fadeObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		
		// Create a texture of desired colour
		fadeTexture = new Texture2D(1, 1);
		fadeTexture.SetPixel(0, 0, Color.white);
		
		// Add texture to fadeObject's guiTexture
		fadeObject.guiTexture.texture = fadeTexture;
		
		// Centre and scale to fullscreen
		fadeObject.guiTexture.transform.position = new Vector3(0.5f, 0.5f, 0.0f);
	}
	
	private static bool ColourEqual(Color a, Color b)
	{
		return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
	}
}
