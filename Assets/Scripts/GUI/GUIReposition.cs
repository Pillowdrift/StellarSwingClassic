using UnityEngine;
using System.Collections;

public class GUIReposition : MonoBehaviour
{
	public Vector3 anchor = Vector3.zero;
	
	public float horzResolution = 800;
	public float vertResolution = 480;
	
	private Vector3 initialPosition;
	
	private float horzRatio;
	private float vertRatio;
	
	void Start()
	{
		// Store initial position
		initialPosition = transform.position;
		
		// Scale to screen resolution
		// Design for horzResolution*vertResolution and then the pixel positions will be the same on every screen
		horzRatio = horzResolution / Screen.width;
		vertRatio = vertResolution / Screen.height;
		
		// Translate to preserve pixel distance on different resolutions
		transform.position = new Vector3((initialPosition.x - anchor.x) * horzRatio, 1.0f - ((1.0f - (initialPosition.y + anchor.y)) * vertRatio), 1.0f);
		transform.position += new Vector3(anchor.x, -anchor.y, anchor.z);
	}
}
