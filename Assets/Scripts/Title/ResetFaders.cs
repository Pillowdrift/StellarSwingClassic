using UnityEngine;
using System.Collections;

public class ResetFaders : MonoBehaviour
{
	public static ResetFaders instance;
	public static bool disable = false;
	
	void Awake()
	{
		if (instance == null)
			instance = this;
	}
}
