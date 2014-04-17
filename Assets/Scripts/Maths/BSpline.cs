using UnityEngine;

using System.Collections;

 

public class BSpline : MonoBehaviour {

 

    public int n = 2; // Degree of the curve

    

    public BSplineControlPoint[] controlPoints; // The control points.

    

    private Vector3[] cachedControlPoints; // cached control points

    private int[] nV; // Node vector

    

    void Start()

    {

        cachedControlPoints = new Vector3[controlPoints.Length];

        

        CacheControlPoints();

        

        nV = new int[cachedControlPoints.Length + 5];

        

        createNodeVector();

        

    }

    

    // Recursive deBoor algorithm.

    public Vector3 deBoor(int r, int i, float u)

    {

        

        if(r == 0)

        {

            return cachedControlPoints[i];

        }

        else

        {

            

            float pre = (u - nV[i + r]) / (nV[i + n + 1] - nV[i + r]); // Precalculation

            return ((deBoor(r - 1, i, u) * (1 - pre)) + (deBoor(r - 1, i + 1, u) * (pre)));

            

            

        }

        

    }

    

    public void createNodeVector()

    {

        int knoten = 0;

            

        for(int i = 0; i < (n + cachedControlPoints.Length + 1); i++) // n+m+1 = nr of nodes

        {

            if(i > n)

            {

                if(i <= cachedControlPoints.Length)

                {

                    

                    nV[i] = ++knoten;

                }

                else

                {

                    nV[i] = knoten;

                }

            }

            else {

                nV[i] = knoten;

            }

        }

    }

    

    private void CacheControlPoints()

    {

        

        for(int i = 0; i < controlPoints.Length; i++)

        {

            cachedControlPoints[i] = controlPoints[i].cachedPosition;

        }

        

    }

    

    void OnDrawGizmos()

    {

        

        if(controlPoints.Length <= 0) return;

        

        cachedControlPoints = new Vector3[controlPoints.Length];

        

        // Cached the control points

        CacheControlPoints();

        

        if(cachedControlPoints.Length <= 0) return;

        

        // Initialize node vector.

        nV = new int[cachedControlPoints.Length + 5];

        createNodeVector();

        

        

        // Draw the bspline lines

        Gizmos.color = Color.gray;

        

        Vector3 start = cachedControlPoints[0];

        Vector3 end = Vector3.zero;

        

        for(float i = 0.0f; i < nV[n + cachedControlPoints.Length]; i += 0.1f)

        {

            

            for(int j = 0; j < cachedControlPoints.Length; j++)

            {

                if(i >= j)

                {

                    end = deBoor(n, j, i);

                }

            }

    

            Gizmos.DrawLine(start, end);

            start = end;

            

        }

    

    }

    

    

}