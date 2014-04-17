using UnityEngine;

using System.Collections;

 

public class BSplineControlPoint : MonoBehaviour {

 

    public Color color = Color.red;

    

    [HideInInspector]

    public Vector3 cachedPosition;

    

    void Start()

    {

        cachedPosition = transform.position;

    }

    

    void OnDrawGizmos()

    {

        

        cachedPosition = transform.position;

        

        // Draw control point

        Gizmos.color = color;

        Gizmos.DrawSphere(cachedPosition, 0.1f);

        

    }

    

}