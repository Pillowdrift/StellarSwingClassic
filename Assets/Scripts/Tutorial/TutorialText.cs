using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GUIText))]
[RequireComponent (typeof(GUIScale))]
public class TutorialText : MonoBehaviour
{
	public void DestroyIn(float time)
	{
		StartCoroutine(Destroy(time));
	}
	
	private void DestroyImmed()
	{
		Destroy(gameObject);
	}
	
	private IEnumerator Destroy(float time)
	{
		yield return new WaitForSeconds(time);
		
		Destroy(gameObject);
	}
}
