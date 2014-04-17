#define HORIZONTAL

using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour
{
	private const bool HORIZONTAL = false;
	
	public static bool show = true;
	
	private float maxSize;
	private float factor;
	
	private GUITexture back;
	private GUITexture front;
	private GUITexture mid;
	private GUITexture decofront;
	
	private float currentLength;
	private float actualLength;
	
	public void Start()
	{
		back = GameObject.Find("ScorebarBack").GetComponent<GUITexture>();
		front = GameObject.Find("ScorebarFront").GetComponent<GUITexture>();
		mid = GameObject.Find("ScorebarMid").GetComponent<GUITexture>();
		decofront = GameObject.Find("ScorebarDecoFront").GetComponent<GUITexture>();
		
#if HORIZONTAL
		maxSize = front.pixelInset.width;
#else
		maxSize = front.pixelInset.height;
#endif
		
		factor = Screen.height / 480.0f;
		
		Rect rect = decofront.pixelInset;
		
#if HORIZONTAL
		rect.width *= factor;
#else
		rect.height *= factor;
#endif
		
		decofront.pixelInset = rect;
		
		currentLength = maxSize;
		actualLength = maxSize;
	}
	
	public void Update()
	{
		Rect dimensions;
		
		if (back != null && front != null && LevelStart.started)
			actualLength = Mathf.Clamp(maxSize * (ScoreCalculator.Speed / 100.0f), 0, maxSize);
		
		{
			
			// Update front bar (the actual length)
			dimensions = front.pixelInset;
			
#if HORIZONTAL
			dimensions.width = actualLength * factor;
#else
			dimensions.height = actualLength * factor;
#endif
			
			front.pixelInset = dimensions;
		}
		
		// Update middle bar (the fadey length)
		dimensions = mid.pixelInset;
		
		// Position and scale it at the end of the front bar
#if HORIZONTAL
			dimensions.x = front.pixelInset.x + actualLength * factor;
			dimensions.width = (currentLength - actualLength) * factor;
#else
			dimensions.y = front.pixelInset.y + actualLength * factor;
			dimensions.height = (currentLength - actualLength) * factor;
#endif
		
		// Hide if current length is already less
		if (currentLength < actualLength)
		{
#if HORIZONTAL
			dimensions.width = 0;
#else
			dimensions.height = 0;
#endif
		}
				
		mid.pixelInset = dimensions;
		
		// Update back bar
		dimensions = back.pixelInset;
		
#if HORIZONTAL
		dimensions.x = front.pixelInset.x + currentLength * factor;
		dimensions.width = (maxSize - currentLength) * factor;
#else
		dimensions.y = front.pixelInset.y + currentLength * factor;
		dimensions.height = (maxSize - currentLength) * factor;
#endif
		
		back.pixelInset = dimensions;
		
		currentLength = Mathf.Lerp(currentLength, actualLength, Time.deltaTime * 5.0f);
	}
}
