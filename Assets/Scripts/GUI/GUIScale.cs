using UnityEngine;
using System.Collections;

public class GUIScale : MonoBehaviour {
	
	// The screen size threshold.
	public const int WIDTH = 1024;
	public const int HEIGHT = 768;
	
	// Use this for initialization
	void Start () {
		// Save the original font.fuck
		/*oldFont = guiText.font;
		oldMaterial = guiText.material;
		
		if (Screen.width < WIDTH || Screen.height < HEIGHT)
		{
			guiText.font = small;
			guiText.material = smallMaterial;
		}
		else
		{
			guiText.font = oldFont;
			guiText.material = oldMaterial;
		}*/
		float widthScale = (float)Screen.width / WIDTH;
		
		guiText.fontSize = (int)(32.0f * widthScale);
	}
}
