using UnityEngine;
using System.Collections;

public class EnableResetFaders : MonoBehaviour
{
	public float disableTime = 1.0f;
	
	void OnMouseDown()
	{			
		ResetFaders.disable = true;
		
		StartCoroutine(Enable());
	}
	
	IEnumerator Enable()
	{
		yield return new WaitForSeconds(disableTime);
		
		ResetFaders.disable = false;
	}
}
