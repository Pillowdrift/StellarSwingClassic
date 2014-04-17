using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GrapplingHook : MonoBehaviour
{
	// Structures
	public class GrappleData
	{
		public GameObject gameObject;
		public float maxDistance;
		public Vector3 offset;
	};
	
	public class TouchData
	{
		public int fingerId;
		public float startTime;
	};
	
	public GameObject currentTutorial = null;
	
	public bool ignoreReleaseAfterGrapple = true;
	public bool enableRegrapple = false;
	
	// Variables	
	public float doubleClickTime = 0.1f;
	public bool grappleFollowMoving = true;
	public float minRopeDistanceForNewRope = 1.0f;
	public bool segment = true;
	public bool spiralmode = false;
	public float GrappleExtrusion = 1.0f;
	public float DefaultPullRate = 250.0f;
	public float LineMinWidth = 0.1f;
	public float LineMaxWidth = 2.0f;
	public GameObject grapple;
	public LineRenderer rope;
	
	private float pressTime = 0.0f;
	
	private List<GrappleData> grapples;
	
	public Material ropeMaterial;
	
	// Private Vars
	Camera theCamera;
	GameObject ropeObject;
	GameObject shield;
	private SpringJoint springJoint;
	
	private List<TouchData> localTouches;
	
	// the maximum amount of time between mouse down and up for the click to be used.
	private const float CLICK_TIMER = 0.2f;
	
	//the threshhold time at whitch to discount the click
	private float clickTime;
	private Vector3 test;
	private Ray ray;
	
//	float ropeWidth;
	
//	const float TRANSPARENCY_THRESHOLD = 0.99f;	
//	
//	const float ENABLED_THICKNESS = 0.2f;
//	const float DISABLED_THICKNESS = 1.0f;
//	
//	const float TRANSITION_RATE = 25.0f;
	
	// Use this for initialization
	void Awake()
	{
		localTouches = new List<TouchData>();
		
		// Find the shield
		shield = (GameObject)transform.FindChild("Shield").gameObject;
		
		theCamera = GameObject.Find("TheCamera").camera;
		
		ropeObject = GameObject.Find("Rope");
		rope = ropeObject.GetComponent<LineRenderer>();
		//rope.SetWidth(0.25f, 0.25f);
		rope.enabled = false;
		
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		
		grapples = new List<GrappleData>();
		
		grapple = CreateGrapple();
		springJoint = grapple.GetComponent<SpringJoint>();
		
//		ropeWidth = 0.2f;
		
	}
	
	void Reload()
	{
		Detach();
	}
	
	void Update()
	{
		// Point the shield at the grapple point.
		if(rope.enabled)
		{
			// Point.
			shield.renderer.enabled = true;
			
			// Get the last point on the grapple.
			shield.transform.LookAt(grapples[grapples.Count - 1].gameObject.transform.position);
			shield.transform.Rotate(Vector3.right, 90);
	
//			// Widen the grapple the closer we are to our target.
//			
//			// Get the distance between the .
//			float actualDistance = (transform.position - grapples[grapples.Count - 1].gameObject.transform.position).magnitude;
//			float distanceFration = actualDistance / grapples[grapples.Count - 1].maxDistance;
//			
//			if(distanceFration < TRANSPARENCY_THRESHOLD)
//			{
//				ropeWidth = Mathf.Lerp(ropeWidth, ENABLED_THICKNESS, TRANSITION_RATE * Time.deltaTime);
//			}
//			else
//			{
//				ropeWidth = Mathf.Lerp(ropeWidth, DISABLED_THICKNESS, TRANSITION_RATE * Time.deltaTime);				
//			}
//			
//			rope.SetWidth(ropeWidth, ropeWidth);			

			
		}
		else
		{
			shield.renderer.enabled = false;
		}
		
		if (!ControlManager.MouseOnGUI)
		{
			if (!TutorialCamera.Enabled() && !EndFlagScript.hasFinished && !PauseButton.paused)
			{
#if UNITY_ANDROID || UNITY_IPHONE
				Vector3 pos;
				
				foreach (Touch touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Began)
					{
						// Register touch
						TouchData touchData = new TouchData();
						touchData.fingerId = touch.fingerId;
						touchData.startTime = Time.time;
						
						localTouches.Add(touchData);
					}
						
					if (touch.phase == TouchPhase.Ended)
					{
						TouchData touchData = null;
						foreach (TouchData td in localTouches)
						{
							if (td.fingerId == touch.fingerId)
							{
								touchData = td;
								break;
							}
						}
						
						if (touchData != null)
						{
							pos = touch.position;
							
							localTouches.Remove(touchData);
							
							float startTime = touchData.startTime;
							float endTime = Time.time;
							
							float touchTime = endTime - startTime;
							
							// If the drag was less than the minimum time for grapple/ungrapple
							if (touchTime < CLICK_TIMER)
							{
								if (!IsGrappling())
								{
									TryGrapple(pos);
								}
								else
								{
									Detach();
								}
							}
						}
					}
				}
#else
				if (InputManager.pressed)
				{
					pressTime = Time.time;
				}
				
				if (InputManager.released)
				{
					float touchTime = Time.time - pressTime;
					
					if (touchTime < CLICK_TIMER)
					{
						if (!IsGrappling())
						{
							TryGrapple(Input.mousePosition);
						}
						else
						{
							Detach();
						}
					}
				}
#endif
			}
		}
		
		if (IsGrappling())
		{
			// Shrink rope for "easy mode"
			float dist = (grapple.transform.position - transform.position).magnitude;
			if (dist < springJoint.maxDistance)
				springJoint.maxDistance = dist;
		}
	}
	
	void LateUpdate()
	{
		// If we're grappling
		if(rope.enabled)
		{
			// If the rope can segment
			if(segment)
			{
				RaycastHit hitInfo;
				
				// Check for line of sight with previous grapples
				if(grapples.Count > 1)
				{
					int lastIndex = grapples.Count - 1;
					int oldIndex = lastIndex - 1;
					
					// Get old grapple
					GrappleData old = grapples[oldIndex];
					
					// Calculate ray to grapple, adjusting distance a little so that it doesn't hit the object grapple is grappled on
					Vector3 difference = old.gameObject.transform.position - transform.position;
					Vector3 direction = difference.normalized;
					float distance = difference.magnitude - 0.1f;
					
					// If nothing hit, we have a clear line of sight to the grapple
					Debug.DrawRay(transform.position, direction);
					Ray ray = new Ray(transform.position, direction);
					if(!Physics.Raycast(ray, out hitInfo, distance))
					{
						// Remove last grapple and reattach to the old one
						SetGrapple(grapples[oldIndex]);
						Destroy(grapples[lastIndex].gameObject);
						grapples.RemoveAt(lastIndex);
					}
				}
				
				// Check if the rope has collided with something
				if(Physics.Linecast(transform.position, grapple.transform.position, out hitInfo))
				{
					// Create new grapple if far enough away from the last one
					float grappleDist = (grapple.transform.position - hitInfo.point).magnitude;
					
					if(grappleDist > minRopeDistanceForNewRope)
					{
						//float grapdiff = (grapple.transform.position - hitInfo.point).magnitude;
						Attach(hitInfo, springJoint.maxDistance - grappleDist);
					}
				}
				
				// Reel in if the currently attached object is a puller
				if(grapple.transform.parent != null)
				{
					const float minDistance = 1.0f;
					
					GameObject grappled = grapple.transform.parent.gameObject;
					Pull pull = grappled.GetComponent<Pull>();
					
					if(pull != null || grappled.CompareTag("Pull"))
					{
						float rate = DefaultPullRate;
						
						if(pull != null)
							rate = pull.rate;
						
						float difference = rate * Time.deltaTime;
						
						foreach(GrappleData grappleData in grapples)
						{						
							if(grappleData.maxDistance > minDistance + difference)
								grappleData.maxDistance -= difference;
							else
								grappleData.maxDistance = minDistance;
						}
						
						if(springJoint.maxDistance > minDistance + difference)
							springJoint.maxDistance -= difference;
						else
							springJoint.maxDistance = minDistance;
					}
				}
			}
		}
	}
					
	public void TryGrapple(Vector3 position)
	{
		if (currentTutorial != null)
			currentTutorial.SendMessage("Grappled", SendMessageOptions.DontRequireReceiver);
				
		RaycastHit hit;
		ray = theCamera.ScreenPointToRay(position);
		if (Physics.Raycast(ray, out hit) && (enableRegrapple || rope.enabled == false))
		{
			if(hit.collider.gameObject != gameObject && hit.collider.gameObject.tag != "Ungrappleable")
			{
				if (rope.enabled == true)
					Detach();					
				
				Attach(hit, (transform.position - hit.point).magnitude);
				SoundManager.Play("attach");
			}
		}
	}
	
	public bool IsGrappling()
	{
		return grapples.Count > 0;
	}
	
	public Vector3 GetPos()
	{
		if(IsGrappling())
			return grapple.transform.position;
		
		return Vector3.zero;
	}
	
	public void Reset()
	{
		springJoint.maxDistance = float.MaxValue;	
		rope.enabled = false;
	}
	
	public List<GrappleData> GetGrapples()
	{
		return grapples;
	}
	
	GameObject CreateGrapple()
	{
		GameObject grapple = new GameObject("Grapple");
		grapple.layer = LayerMask.NameToLayer("Grapples");
		Rigidbody grappleBody = grapple.AddComponent<Rigidbody>();
		grappleBody.isKinematic = true;
		grappleBody.useGravity = false;
		SpringJoint spring = grapple.AddComponent<SpringJoint>();
		spring.anchor = Vector3.zero;
		spring.damper = 0;
		spring.spring = 1;
		
		return grapple;
	}
	
	void SetGrapple(GrappleData grappleData)
	{		
		// Move grapple to this position because it's dumb and stupid and dumb
		grapple.transform.position = transform.position;
		
		// Update spring joint
		springJoint.maxDistance = grappleData.maxDistance;		
		springJoint.connectedBody = gameObject.rigidbody;
		
		// Update grapple position and parent it so it follows moving objects
		grapple.transform.position = grappleData.gameObject.transform.position;
		if(grappleFollowMoving)
			grapple.transform.parent = grappleData.gameObject.transform.parent;
		
		// Make rope visible
		rope.enabled = true;
	}
	
	void UnsetGrapple()
	{
		springJoint.connectedBody = null;
	}
	
	public void Attach(RaycastHit hit, float distance)
	{
		if (hit.collider != null && hit.collider.tag == "GrappleTarget")
		{
			if (rope.enabled)
				return;
			else
				distance = (hit.collider.gameObject.GetComponent<GrappleGuide>().GrappleTo() - transform.position).magnitude;
		}
		else
		{
			distance = (hit.point - transform.position).magnitude;
		}
		
		// Update grapple data list
		GrappleData grappleData = new GrappleData();
		grappleData.gameObject = new GameObject("Grapple Placeholder");
		if (hit.collider != null && hit.collider.tag == "GrappleTarget")
		{
			Transform t = hit.collider.gameObject.GetComponent<GrappleGuide>().grappleTo;
			CamTarget.Grappled(t);
			grappleData.gameObject.transform.position = t.position;
		}
		else
		{
			if (hit.collider != null)
				CamTarget.Grappled(hit.collider.transform);
			else
				CamTarget.Grappled(transform);
			grappleData.gameObject.transform.position = hit.point;
		}
		
		grappleData.maxDistance = distance;
		

		
		if (hit.collider != null)
			grappleData.gameObject.transform.parent = (grappleFollowMoving ? hit.collider.gameObject.transform : null);
		
		// Offset position a little so the rope is visible
		// grappleData.gameObject.transform.position += hit.normal * GrappleExtrusion;		
		
		if(grapples.Count == 0)
		{
			grappleData.gameObject.transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);
		}
		else
		{
			grappleData.gameObject.transform.LookAt(grapples[grapples.Count - 1].gameObject.transform, -rigidbody.velocity);
			
			grappleData.offset = hit.normal * GrappleExtrusion;
		}
		
		
					
		// Add grapple data to collection			
		grapples.Add(grappleData);
		
		// Update grapple
		SetGrapple(grappleData);
	}
	
	public void Detach()
	{
		if (rope.enabled)
			CamTarget.UnGrappled();
		
		rope.enabled = false;
		
		foreach(GrappleData grappleData in grapples)
		{
			Destroy(grappleData.gameObject);
		}
		
		grapples.Clear();		
		UnsetGrapple();		
		
	}
}