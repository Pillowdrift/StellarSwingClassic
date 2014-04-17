using UnityEngine;
using System.Collections;

public class GUIImageScale : MonoBehaviour
{
	private float width;
	private float height;
	
	private float x;
	private float y;
	
	void Start()
	{
		// Get their original size.
		if (guiTexture != null)
		{
			width = guiTexture.pixelInset.width;
			height = guiTexture.pixelInset.height;
			x = guiTexture.pixelInset.x;
			y = guiTexture.pixelInset.y;
		}
	}
	
	void Update()
	{
		if (guiTexture != null)
		{
			float scale = (float)(Screen.width / (float)GUIScale.WIDTH);
			Rect rect = guiTexture.pixelInset;
			rect.x = x * scale;
			rect.y = y * scale;
			rect.width = width * scale;
			rect.height = height * scale;
			guiTexture.pixelInset = rect;
		}
	}
}
