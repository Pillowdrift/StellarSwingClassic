using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI2() {
		if (GUI.Button(new Rect((Screen.width / 2) -25 , Screen.height - 50, 100, 20), "Play"))
			Loading.Load("prototype 1");
	}
}
