using UnityEngine;
using System.Collections;

public class MoveMeshUV : MonoBehaviour {
	
	MeshFilter mFilter;
	Mesh mesh;
	
	// Use this for initialization
	void Start () {
		mFilter = GetComponent<MeshFilter>();
		mesh = mFilter.mesh;
	}
	
	// Update is called once per frame
	void Update () {
		float offset = Time.time * 0.5f;
		renderer.material.mainTextureOffset = new Vector2(offset, 0);
	}
}
