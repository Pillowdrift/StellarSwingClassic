using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	private const float GAMEPAD_DEFAULT_SENSITIVITY = 10.0f;

	// Whether to render a line between the start and current position of the drag
	public bool enableLine = true;
	
	// The minimum angle change, under which the line's angle will be updated
	public float angleChangeMinimum = 0.1f;
	
	// The maximum change, over which the drag will be reset
	public float angleChangeMaximum = 2.0f;
	
	// The squared distance the mouse must be dragged per frame
	// to ensure that the startposition doesn't reset
	public float sqrResetDeadzone = 1;
	
	// Whether the mouse is held, or has just been pressed or released this frame
	// Dragging is not set until a frame after held so that frameDifference is accurate
	public static bool held = false;
	public static bool pressed = false;
	public static bool released = false;
	public static bool dragging = false;
	
	// The start, current, and previous position of a drag
	// Start position is not valid if held is false
	public static float startTime;
	public static Vector3 startPosition = Vector3.zero;
	public static Vector3 currentPosition = Vector3.zero;
	public static Vector3 previousPosition = Vector3.zero;
	
	// The difference between the start and current position,
	// and the difference between the previous frame and current position
	// Not valid if held is false
	public static Vector3 difference = Vector3.zero;
	public static Vector3 frameDifference = Vector3.zero;
	
	public static float heldTime;
	public static float dragSpeed;
	
	// The line renderer
	private GameObject lineObject;
	private LineRenderer lineRenderer;
	
	// Most recent angle of the line
	private float lineAngle;
	
	void Awake()
	{		
		// Create line renderer if line enabled
		if (enableLine)
		{
			lineObject = new GameObject();
			lineRenderer = lineObject.AddComponent<LineRenderer>();
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetWidth(0.05f, 0.05f);
			lineRenderer.enabled = false;
		}
	}
	
	void Update()
	{
		// Update input state
		held = Input.GetMouseButton(0) ||  Input.GetButton("Grapple");
		pressed = Input.GetMouseButtonDown(0) || Input.GetButtonDown("Grapple");
		released = Input.GetMouseButtonUp(0) || Input.GetButtonUp("Grapple");
		
		if (pressed)
			held = true;
		else if (released)
			held = false;
		
		if (held && !pressed)
			dragging = true;
		else
			dragging = false;

		if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
		{
			dragging = true;
		}
		
		// Set previous position to last position and update current position
		previousPosition = currentPosition;
		currentPosition = Input.mousePosition;
		
		// Update frame difference
		frameDifference = currentPosition - previousPosition;

		// Add gamepad input
		frameDifference.x += Input.GetAxis("Horizontal") * GAMEPAD_DEFAULT_SENSITIVITY;
		
		// Calculate line angle
		float angle = Vector3.Angle(currentPosition, previousPosition);
		
		if (pressed)
		{
			startTime = Time.time;
		}
		
		if (held)
		{
			heldTime = Time.time - startTime;
			dragSpeed = difference.magnitude / heldTime;
			
			// If this isn't a new press
			if (!pressed)
			{
				// If the line angle is greater than one
				float angleDifference = Mathf.Abs(lineAngle - angle);
				if (angleDifference > angleChangeMaximum)
				{
					// Reset the line
					startPosition = currentPosition;
					lineAngle = angle;
				}
			}
		}
		
		// Calculate directional difference
		//Vector3 oldDirection = (currentPosition - startPosition).normalized;
		//Vector3 newDirection = frameDifference.normalized;
		//float cosDifference = Vector3.Dot (oldDirection, newDirection);
		
		// Determine initial position
		// (either when the mouse is first pressed or when the frame difference is less than a deadzone)
		if (pressed || frameDifference.sqrMagnitude < sqrResetDeadzone)
		{
			startPosition = currentPosition;
		}
		
		// Update overall difference
		difference = currentPosition - startPosition;
	}
}
