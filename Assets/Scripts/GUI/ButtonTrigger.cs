using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonTrigger : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	void Update()
	{
		// OnMouseDown for phones
		if (InputManager.pressed || InputManager.released)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(InputManager.currentPosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (InputManager.pressed)
					hit.transform.gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
				else if (InputManager.released)
					hit.transform.gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
#endif
}