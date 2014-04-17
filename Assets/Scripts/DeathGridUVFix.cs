using UnityEngine;
using System.Collections;

public class DeathGridUVFix : MonoBehaviour {
	
	const float scale = 0.1f;

	// Use this for initialization
	void Start () 
	{		
		float x = Mathf.Round(transform.localScale.x * scale);
		float z = Mathf.Round(transform.localScale.z * scale);	

        Mesh mesh = GetComponent<MeshFilter>().mesh;
		
    	Vector2[] uvs = mesh.uv;
		
		
        for (int i = 0; i < uvs.Length; i++) 
		{
            uvs[i].x *= x;
			uvs[i].y *= z;			
        }
		
        mesh.uv = uvs;
    }
}
