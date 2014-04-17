using UnityEngine;
using System.Collections;

// ?????????
public class TitleBackButton : MonoBehaviour
{
	void OnMouseDown()
	{
		if (enabled)
		{
			enabled = false;
			GameObject.Find("AndroidBackButtonControl").GetComponent<AndroidBackButtonControl>().enabled = false;
			
			if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.WORLD_SELECT)
			{
				// Go back to the title screen
			}
			else if (LevelSelectGUI.menuState == LevelSelectGUI.MenuState.LEVEL_SELECT)
			{
				// Go back to world select
			}
			
			StartCoroutine("Enable");
		}
	}
	
	IEnumerator Enable()
	{
		yield return new WaitForSeconds(0);
		
		enabled = true;
		GameObject.Find("AndroidBackButtonControl").GetComponent<AndroidBackButtonControl>().enabled = true;
		
	}
}
