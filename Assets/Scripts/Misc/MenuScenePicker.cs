using UnityEngine;
using System.Collections;

public class MenuScenePicker : MonoBehaviour {
	
	private const int BUTTON_WIDTH = 220;
	private const int BUTTON_HEIGHT = 60;
	
	public static ArrayList SceneNames;
	public Font font;
	
	// Use this for initialization
	void Start () {
		SceneNames = new ArrayList();
		
		//Add scene names here- make sure to add them in to the build in the build settings!
		/*SceneNames.Add("1-1");
		SceneNames.Add("1-2");
		SceneNames.Add("1-3");
		SceneNames.Add("1-4");
		SceneNames.Add("1-5");
		SceneNames.Add("1-6");
		SceneNames.Add("1-7");
		SceneNames.Add("1-8");
		SceneNames.Add("2-1");
		SceneNames.Add("2-2");
		SceneNames.Add("2-3");
		SceneNames.Add("2-4");
		SceneNames.Add("2-5");
		SceneNames.Add("2-6");
		SceneNames.Add("2-7");
		SceneNames.Add("2-8");
		SceneNames.Add("2-9");
		SceneNames.Add("2-10");
		SceneNames.Add("2-11");*/
		
		SceneNames.Add("World 1 Level 1");
		SceneNames.Add("World 1 Level 2");
		SceneNames.Add("World 1 Level 3");
		SceneNames.Add("World 1 Level 4");
		SceneNames.Add("World 1 Level 5");
	    SceneNames.Add("World 2 Level 1");
		SceneNames.Add("World 2 Level 2");
		SceneNames.Add("World 2 Level 3");
		SceneNames.Add("World 2 Level 4");
		SceneNames.Add("World 2 Level 5");
		SceneNames.Add("World 2 Level 6");
		SceneNames.Add("World 3 Level 1");
		SceneNames.Add("World 3 Level 2");
		SceneNames.Add("World 3 Level 3");
		SceneNames.Add("World 3 Level 4");
		SceneNames.Add("World 3 Level 5");
		SceneNames.Add("World 3 Level 6");
		SceneNames.Add("World 3 Level 7");
		SceneNames.Add("World 4 Level 1");
		SceneNames.Add("World 4 Level 2");
		SceneNames.Add("World 4 Level 3");
		SceneNames.Add("World 4 Level 4");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI2() {
		int posCounter = 0;
		int yCounter = 0;
		
		GUI.skin.font = font;
		
		for(int i = 0; i < SceneNames.Count; ++i)
		{
			string name = (string)SceneNames[i];
			//We have a new item so add to the counter
			posCounter++;
			
			//Move to next column if we have 10 items on this one
			if ((posCounter % 6) == 0)
			{
				yCounter++;
				posCounter = 1;
			}
				
			if(GUI.Button(new Rect((BUTTON_WIDTH + 10) * yCounter, (BUTTON_HEIGHT + 10) * posCounter, BUTTON_WIDTH, BUTTON_HEIGHT), name))
			{
				Loading.Load(name);
			}
			
			
		}
		
		GUI.Label(new Rect(Screen.width - 200, Screen.height - 60, 180, 50), "Swinging Ninja Pre-Alpha, Pillowdrift Ltd.");
	}
}
