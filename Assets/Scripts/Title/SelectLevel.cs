using UnityEngine;
using System.Collections;

public class SelectLevel : MonoBehaviour
{
	public string levelName;
	
	void OnMouseDown()
	{
		Loading.Load(levelName);
	}
}
