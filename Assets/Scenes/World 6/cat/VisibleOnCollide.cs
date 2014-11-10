using UnityEngine;
using System.Collections;

public class VisibleOnCollide
	: MonoBehaviour
{
	public GameObject[] objs;

	public void Start()
	{
		Reload();
	}

	public void Reload()
	{
		for (int i = 0; i < objs.Length; ++i)
			objs[i].renderer.enabled = false;
	}
	
	public void OnCollisionEnter()
	{
		for (int i = 0; i < objs.Length; ++i)
			objs[i].renderer.enabled = true;
	}
	
	public void OnTriggerEnter()
	{
		for (int i = 0; i < objs.Length; ++i)
			objs[i].renderer.enabled = true;
	}
}
