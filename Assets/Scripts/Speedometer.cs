using UnityEngine;
using System.Collections;

public class Speedometer : MonoBehaviour
{	
	public Texture speedo;
	public Texture needle;
	
	// The x and y as multiples of screen width and height (0..1)
	public float x = 0.0f;
	public float y = 0.0f;
	
	// The width and height as multiples of input texture width and height (0..1)
	public float width = 1.0f;
	public float height = 1.0f;
	
	// Whether to stop the speedometer from rotating more than once
	public bool limit = true;
	
	// How much to scale the speed
	public float speedScale = 1.0f;
	
	// Smoothing factor for movement
	public float needleSmoothing = 0.1f;
	
	private Rigidbody playerBody;
	private Rect position;
	
	private float visibleRotation = 0.0f;
	private float vel = 0.0f;
	
	// The rotation of the furthest point around the speedometer
	private const float max = 237.0f;
	
	void Awake()
	{
		playerBody = GameObject.Find("Player").GetComponent<Rigidbody>();
		position = new Rect(Screen.width * x, Screen.height * y, speedo.width * width, speedo.height * height);
	}
	
	void OnGUI2()
	{
		// Draw speedo back
		GUI.DrawTexture(new Rect(Screen.width * x, Screen.height * y, speedo.width * width, speedo.height * height), speedo, ScaleMode.ScaleToFit, true, 0);
		
		// Rotate needle
		if (playerBody != null)
		{
			// Adjust velocity for needle
			Vector3 adjustedVelocity = new Vector3(playerBody.velocity.x, playerBody.velocity.y, playerBody.velocity.z);
			
			// Scale speed
			float speed = adjustedVelocity.magnitude * speedScale;
			
			// Interpolate towards value
			visibleRotation = Mathf.SmoothDamp(visibleRotation, speed, ref vel, needleSmoothing);
			
			// Clamp
			if (limit)
				visibleRotation = Mathf.Clamp(visibleRotation, 0, max);
			
			GUIUtility.RotateAroundPivot(visibleRotation, new Vector2(position.x + position.width / 2.0f, position.y + position.height / 2.0f));
		}
		
		// Draw needle
		GUI.DrawTexture(position, needle, ScaleMode.ScaleToFit, true, 0);
	}
}
