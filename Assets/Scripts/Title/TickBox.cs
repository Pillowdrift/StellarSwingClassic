using UnityEngine;
using System.Collections;

public class TickBox : MonoBehaviour {
	
	// Public vars
	public Texture On;
	public Texture Off;
	
	public bool Value;
	
	// When clicked
	void OnMouseUp()
	{
		Value = !Value;
		SendMessage("OnValueChange", Value);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Value)
		{
			renderer.material.mainTexture = On;
		}
		else
		{
			renderer.material.mainTexture = Off;
		}
	}
}
