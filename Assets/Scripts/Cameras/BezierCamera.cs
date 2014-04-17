using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCamera : MonoBehaviour
{
	// Whether to clean up the list
	public bool cleanup = false;
	
	// Whether to load all available nodes at the start
	public bool loadAtStart = true;
	
	public int NodeCount { get { return cameraNodes.Count; } }
	
	// Camera interpolation state
	private int currentNode = 0;
	private float startTime;
	private List<CameraNode> cameraNodes;
	private List<GameObject> tempObjects;
	
	private Vector3 defaultScale = Vector3.zero;
	
	void Start()
	{
		defaultScale = transform.localScale;
		
		cameraNodes = new List<CameraNode>();
		tempObjects = new List<GameObject>();
		
		if (loadAtStart)
		{
			// Create list of camera nodes
			CameraNode[] nodes = Component.FindObjectsOfType(typeof(CameraNode)) as CameraNode[];
			cameraNodes.AddRange(nodes);
			cameraNodes.Sort();
				
			if (cameraNodes.Count > 0)
			{
				// Set initial time
				startTime = Time.time;
				transform.position = cameraNodes[currentNode].transform.position;
			}
		}
	}
	
	void Update()
	{
		// Update camera position
		if (currentNode < cameraNodes.Count - 1)
		{
			CameraNode current = cameraNodes[currentNode];
			CameraNode next = cameraNodes[currentNode+1];
			
			if ((next.transform.position - transform.position).magnitude < 0.1f)
			{				
				if (cleanup)
				{
					cameraNodes.RemoveAt(currentNode);
				}
				else
				{
					currentNode++;
				}				
				
				if (currentNode >= cameraNodes.Count - 1)
					return;
				
				current = cameraNodes[currentNode];
				next = cameraNodes[currentNode+1];
				
				transform.position = current.transform.position;
			}
			
			float cameraTime = Time.time - startTime;
			float elapsedTime = cameraTime - current.time;
			float totalTime = next.time - current.time;
			float coefficient = elapsedTime / totalTime;
			
			transform.position = Vector3.Lerp(current.transform.position, next.transform.position, coefficient);
			transform.rotation = Quaternion.Lerp(current.transform.rotation, next.transform.rotation, coefficient);
			transform.localScale = defaultScale * Mathf.Lerp(current.objectScale, next.objectScale, coefficient);
			if (camera != null)
				camera.orthographicSize = Mathf.Lerp(current.size, next.size, coefficient);
			else
				Camera.mainCamera.orthographicSize = Mathf.Lerp(current.size, next.size, coefficient);
		}
	}
	
	public void SetBezier(PreviewNode[] nodes)
	{
		
	}
	
	void SetNodes(string parentName)
	{
		for (int i = 0; i < tempObjects.Count; ++i)
			Destroy(tempObjects[i]);
		tempObjects.Clear();
		
		// Reset state of this thingy
		currentNode = 0;
		cameraNodes.Clear();
		
		// Create node at current position
		GameObject tempNode = new GameObject("CamNode");
		tempObjects.Add(tempNode);
		CameraNode node = tempNode.AddComponent<CameraNode>();
		node.time = 0;
		node.size = Camera.mainCamera.orthographicSize;
		node.transform.position = transform.position;
		node.transform.rotation = transform.rotation;
		
		// Load new nodes		
		GameObject parent = GameObject.Find(parentName);
		CameraNode[] nodes = parent.GetComponentsInChildren<CameraNode>();
		cameraNodes.Add(node);
		cameraNodes.AddRange(nodes);
		cameraNodes.Sort();
		
		startTime = Time.time;
	}
	
	void Resync(float time)
	{
		for (int i = 0; i < cameraNodes.Count; ++i)
		{
			if (cameraNodes[i].time == time)
			{
				currentNode = i;
				float timeDiff = Time.time - cameraNodes[i].time;
				startTime += timeDiff;
				break;
			}
		}
	}
}
