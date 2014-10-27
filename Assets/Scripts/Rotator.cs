using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
	public bool planet = false;
	
	public float angle = 0.0f;
	public Vector3 axis = Vector3.zero;
	
	void Awake()
	{
		axis.Normalize();
	}
	
	void Update()
	{
		if (!planet || LevelSelectGUI.menuState == LevelSelectGUI.MenuState.WORLD_SELECT)
			transform.RotateAround(axis, angle * Time.deltaTime);
	}
	
	void RotateTo(float to)
	{
		angle = Mathf.Lerp(angle, to, Time.deltaTime);
	}
}
