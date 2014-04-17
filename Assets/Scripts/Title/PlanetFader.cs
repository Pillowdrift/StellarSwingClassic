using UnityEngine;
using System.Collections;

public class PlanetFader : MonoBehaviour
{	
	public static bool enable = true;
	
	public float fadeRate = 1.0f;
	
	void Update()
	{
		bool fadeout = false;
		
		if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.LEVEL_SELECT && LevelSelectGUI.currentPlanet != gameObject)
		{
			fadeout = true;
		}
		
		Color c = renderer.material.color;
		c.a = Mathf.Lerp(c.a, (fadeout ? 0 : 1), Time.deltaTime * fadeRate);
		if (!fadeout && c.a > 0.95f)
			c.a = 1.0f;
		renderer.material.color = c;
	}
}
