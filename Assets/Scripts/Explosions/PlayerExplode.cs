using UnityEngine;
using System.Collections;

public class PlayerExplode : MonoBehaviour
{
	public GameObject ExplosionPrefab;
	
	// Explode
	void Explode()
	{
		Instantiate(ExplosionPrefab, transform.position, transform.rotation);
		
		SoundManager.Play("bang");
	}
}
