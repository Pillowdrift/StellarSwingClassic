using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour {
	
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Player") 		
		{
			Loading.Load(Application.loadedLevelName);
		}
	}
}
