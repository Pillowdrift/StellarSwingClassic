using UnityEngine;
using System.Collections;

public class ReturnToTitle
	: MonoBehaviour
{
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Loading.Load("Title");
		}
	}
}
