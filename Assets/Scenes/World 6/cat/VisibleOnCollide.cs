using UnityEngine;
using System.Collections;

public class VisibleOnCollide
	: MonoBehaviour
{
	public void Start()
	{
		Reload();
	}

	public void Reload()
	{
		renderer.enabled = false;
	}

	public void OnCollisionEnter()
	{
		renderer.enabled = true;
	}
}
