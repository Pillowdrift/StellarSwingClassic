using UnityEngine;
using System.Collections;

public class TapToChange : MonoBehaviour {
	
	
	private const int BUTTON_WIDTH = 160;
	private const int BUTTON_HEIGHT = 40;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetMouseButtonDown(0))
			//Loading.LoadLevel("prototype j");
	}
	
	void OnGUI2() {
		//GUI.Label(new Rect(Screen.width/2 - 100 , Screen.height/2 + 100 , 250, 50), "Tap to Begin!");
		
		if (GUI.Button(new Rect(Screen.width/2 - (BUTTON_WIDTH / 2), Screen.height/1.5f, BUTTON_WIDTH, BUTTON_HEIGHT), "Play"))
		{		
			Loading.Load("picker_scene");
		}
		
		if (GUI.Button(new Rect(Screen.width/2 - (BUTTON_WIDTH / 2), Screen.height/1.25f, BUTTON_WIDTH, BUTTON_HEIGHT), "Settings"))
		{		
			Loading.Load("picker_scene");
		}
	}
}
