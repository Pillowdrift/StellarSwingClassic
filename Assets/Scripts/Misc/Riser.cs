using UnityEngine;
using System.Collections;

public class Riser : MonoBehaviour {
	
	// How long for the riser to live for
	public float LifeTime = 0.7f;
	
	public float UpSpeed = 1.0f;
	
	// A timer to make them die.
	private float timer = 0;
	
	// The start color of the material.
	private Color color;
	
	// Use this for initialization
	void Start () {
	}
	
	// Kill the Riser after a while
	void Awake()
	{	
		color = renderer.material.GetColor("_TintColor");
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > LifeTime)
		{
			DestroyImmediate(this.gameObject);
			return;
		}
		
		// Move the riser up slightly.
		transform.Translate(new Vector3(0, 1, 0) * UpSpeed * Time.deltaTime);
		
		// Fade out the alpha.
		renderer.material.SetColor("_TintColor", color * (1 - (timer / LifeTime)));
	}
}
