using UnityEngine;
using System;
using System.Collections;

public class CameraNode : MonoBehaviour, IComparable<CameraNode>
{
	public float time = -1.0f;
	public float size = 4.0f;
	public float objectScale = 1.0f;
	
	public void Start()
	{
		// Throw an exception if the time is not set
		if (time < 0)
			throw new Exception();
	}
	
	public int CompareTo(CameraNode other)
	{
		return -other.time.CompareTo(time);
	}
}
