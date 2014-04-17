using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float speed;
	public Vector3 direction;
	
	public void Start()
	{
		direction.Normalize();
	}
	
	public void Update()
	{
		transform.position += speed * direction * Time.deltaTime;
	}
}
