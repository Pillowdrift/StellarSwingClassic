using UnityEngine;

public class BezierTest : MonoBehaviour
{
	private float t = 0.0f;
	private Vector3 p0, p1, p2, p3;
	
	void Start()
	{
		p0 = new Vector3(-5.0f, 0.0f, 0.0f);
		p1 = Random.insideUnitSphere * 2.0f;
		p2 = Random.insideUnitSphere * 2.0f;
		p3 = new Vector3(5.0f, 0.0f, 5.0f);
	}
	
	void Update()
	{
		transform.position = Bezier.BezierInterp(t, p0, p1, p2, p3);
		
		t += 0.001f;
		if (t > 1.0f)
			t = 0.0f;
	}
}