using UnityEngine;
using System.Collections;

public class DisablePlanet : MonoBehaviour {
	
	// The levels component.
	Levels levels;
	
	// Use this for initialization
	void Start () {
		// Grab our levels component.
		levels = gameObject.GetComponent<Levels>();
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePlanetState();
	}
	
	// Update which planets are unlocked.
	void UpdatePlanetState() {
		// Check if we can show ourselves.
		bool unlocked = true;
		for (int i = 0; i < levels.levels.Length; ++i) {
			// I might have just made a big mistake adding this line
			if (SaveManager.save != null) {
				if (levels.levels[i].world > SaveManager.save.worldUnlocked) {
					unlocked = false;	
				}
			}
		}
		
		// Now show ourselves if we need to, otherwise hide ourselves.
		gameObject.renderer.enabled = unlocked;
		gameObject.collider.enabled = unlocked;
	}
}
