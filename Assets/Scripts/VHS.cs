using UnityEngine;
using System.Collections;

public class VHS : MonoBehaviour
{
	public bool disableIfNoRecordings = false;
	
	private Color initial;
	private Color transparent;
	
	private Tap tap;
	private RecordingManager recordingManager;
	
	void Start()
	{
		tap = GetComponent<Tap>();
		recordingManager = GameObject.FindObjectOfType(typeof(RecordingManager)) as RecordingManager;
		
		initial = renderer.material.color;
		transparent = initial;
		transparent.a = 0;
	}
	
	void Update()
	{
		if (recordingManager.RecordingCount <= 0 && disableIfNoRecordings)
		{
			renderer.material.color = transparent;
			tap.enabled = false;
		}
		else
		{
			renderer.material.color = initial;
			tap.enabled = true;
		}
	}
}
