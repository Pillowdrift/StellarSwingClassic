using UnityEngine;
using System.Collections;

public class DropTrigger
	: MonoBehaviour
{
	public bool reset = false;

	public GameObject[] toDrop;

	public void Start()
	{
		if (reset)
		{
			for (int i = 0; i < toDrop.Length; ++i) {
					toDrop [i].AddComponent<SavePos> ();
			}
		}
	}

	public void Reload()
	{
		for (int i = 0; i < toDrop.Length; ++i)
		{
			Destroy(toDrop[i].GetComponent<Rigidbody>());
		}
	}

	public void Fall()
	{
		for (int i = 0; i < toDrop.Length; ++i)
		{
			toDrop[i].AddComponent<Rigidbody>();
		}
	}

	public void OnTriggerEnter()
	{
		Fall();
	}

	public void Grappled()
	{
		Fall();
	}
}
