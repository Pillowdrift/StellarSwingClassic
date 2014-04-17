using UnityEngine;
using System.Collections;

public class ShowOnlyIniOS : MonoBehaviour {

	// Use this for initialization
	void Awake () {
#if !UNITY_IPHONE && !UNITY_EDITOR
		gameObject.active = false;
#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
