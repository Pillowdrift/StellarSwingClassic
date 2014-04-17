// Rope renderer 2
// Simulates a rope of a particular length held between two points
// Maths from http://members.chello.nl/j.beentjes3/Ruud/catfiles/catenary.pdf

using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
	public int segments = 10;
	
	public Transform start;
	public Transform end;
	public float length = 0.0f;
	
	private LineRenderer lineRenderer;
	
	private float s;
	
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	void Update()
	{
		Vector3 startPos = start.position;
		Vector3 endPos = end.position;
		
		if (startPos.x > endPos.x)
		{
			Vector3 temp = startPos;
			startPos = endPos;
			endPos = temp;
		}
		
		Vector3 current = startPos;
		Vector3 difference = endPos - startPos;
		float dist = difference.magnitude;
		
		float a = difference.y;
		
		//a=10;
		//float length=15;
		//dist=11;
		
		Vector3 xzStep = difference / (float)segments;
		xzStep.y = 0;
		
		// Calculate lowest point (equation 11)
		float aa = a * a;
		
		float L = length;
		float h = Solve_h(a, L, dist);
		
		float root = -Mathf.Sqrt(h * (a + h) * (L * L - aa));
		if (startPos.y < endPos.y)
			root *= 1.0f;
		
		float L1 = -((h * L + root) / a);
		float mu = (2 * h) / (L1 * L1 - h * h);
		
		lineRenderer.SetVertexCount(segments + 1);
		for (int i = 0; i <= segments; ++i)
		{
			// Calculate y of current position
			float x = current.x - startPos.x;			
			float x1 = asinh(mu * L1) / mu;
			float k = startPos.y - h - (1.0f / mu);
			float y = (1.0f / mu) * cosh(mu * x - mu * x1) + k;
			current.y = y;
			
			// Update vertex
			lineRenderer.SetPosition(i, current);
			
			// Update xz position
			current += xzStep;
		}
		
		lineRenderer.SetPosition(segments, endPos);
	}
	
	// Stolen from above paper
	const float MAXERR = 0.0001f;
	const int MAXIT = 1000;
	
	float cosh(float x)
	{
		return (float)Math.Cosh((double)x);
	}
	
	float asinh(float x)
	{
		return (float)Math.Log(x + Math.Sqrt((double)(x * x + 1)));
	}
	
	float atanh(float x)
	{
		return 0.5f * Mathf.Log((1+x)/(1-x));
	}
	
	float Calc_D(float a, float L, float h, float sgn)
	{
		float q=2*sgn*Mathf.Sqrt(h*(a+h)*(L*L-a*a)); // + or - 2* the root used in (11)
		return ((L*L-a*a)*(a+2*h)-L*q)/(a*a)*atanh(a*a/(L*(a+2*h)-q)); // return calculated d from eq (11)
	}
	
	float Solve_h(float a, float L, float d) // Routine to solve h from a, L and d
	{
		int n=1; // Iteration counter (quit if >MAXIT)
		float ta = L * L - a * a;
		float tb = 2 * a;
		float tc = (L+a)/(L-a);
		float td = (float)Math.Log(tc);
		float te = ta * tb * td;
		s=(te<d) ?-1:1;	// Left or right of Y axis ?
		float lower=0, upper=(L-a)/2; // h must be within this range
		
		float TV;
		
		while((upper-lower) > MAXERR && (++n)<MAXIT) // Until range narrow enough or MAXIT
		{
			TV = (lower + upper) / 2;
			if(Calc_D(a,L,TV,s)*s<d*s) upper=TV; else lower=TV; // Narrow the range of possible h
		}
		
		TV = (lower + upper) / 2;
		
		return s*TV; // Returns h (- signals right of Y axis)
	}
	
	float Solve_L(float a, float h, float d) // Routine to solve L from a, h and d
	{
		int n=1; // Iteration counter (quit if >MAXIT)
		float lower=Mathf.Sqrt((d*d+a*a)), upper=2*h+d+a; // L must be within this range
		
		float TV;
		
		while((upper-lower) > MAXERR && (++n)<MAXIT) // Until range narrow enough or MAXIT
		{
			TV = (lower + upper) / 2;
			if(Calc_D(a,TV,h,1)>d) upper=TV; else lower=TV; // Narrow the range of possible L
		}
		
		TV = (lower + upper) / 2;
		return TV; // Returns L
	}
}