using UnityEngine;
using System.Collections;

public class SaveOnlineFeatures : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TickBox>().Value = SaveManager.save.OnlineEnabled;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnValueChange(bool v)
	{
		SaveManager.save.OnlineEnabled = v;
		SaveManager.Write();
	}
}
