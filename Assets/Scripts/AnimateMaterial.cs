using UnityEngine;
using System.Collections;

public class AnimateMaterial : MonoBehaviour {
	public Material material;
	public Vector2 speed;
	
	// Update is called once per frame
	void Update () {
		Vector2 v = material.GetTextureOffset("_MainTex");
		
		v += speed * Time.deltaTime;
		
		material.SetTextureOffset("_MainTex", v);	
	}
}
