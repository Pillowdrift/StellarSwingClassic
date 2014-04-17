/* DrawRope.cs - Eddie Cameron
 * ----------------------------
 * Draws a naturally hanging rope between two points
 * ----------------------------
 * Thanks to Alejandro Omar Chocano Vasquez, for the Python script I adapted this from (http://alexvaqp.googlepages.com/)
 * ----------------------------
 */ 
using UnityEngine;
using System.Collections;

public class DrawRope : MonoBehaviour 
{
	LineRenderer lineR;
	public float a = 2f;			// adjust sag of rope. Higher a means less sag
	public int segments = 10;		// how many points to iterate over
	
	public Transform startAnchor;	// either set anchor or point. Setting anchor will override anything you put in point
	public Vector3 startPoint;
	
	public Transform endAnchor;
	public Vector3 endPoint;
	
	void Awake()
	{
		lineR = GetComponent<LineRenderer>();
		RedrawRope();
	}
	
	void Update()
	{
		RedrawRope();
	}

	public void RedrawRope()
	{
		if ( lineR == null )
			lineR = GetComponent<LineRenderer>();
		
		if ( startAnchor != null )
			startPoint = startAnchor.transform.position;
		
		if ( endAnchor != null )
			endPoint = endAnchor.transform.position;
		
		Vector3 between = endPoint - startPoint;
		float heightDiff = between.y;
		
		between.y = 0;
		float dist = between.magnitude * 2.0f;
				
		float lowDist = dist / 2f - heightDiff * a / dist;
		float lowY = startPoint.y - Mathf.Pow ( lowDist, 2f ) / ( 2f * a );
		
		float xStep = between.x / (float)segments;
		float zStep = between.z / (float)segments;
		float distStep = dist / (float)segments;
					
		lineR.SetVertexCount ( segments + 1 );
		for ( int i = 0; i <= 10; i++ )
		{			
			float xI = startPoint.x + i * xStep;
			float zI = startPoint.z + i * zStep;
			
			float yI = lowY + Mathf.Pow ( i * distStep - lowDist, 2f ) / ( 2f * a );
			
			lineR.SetPosition ( i, new Vector3( xI, yI, zI ) );
		}
	}
}