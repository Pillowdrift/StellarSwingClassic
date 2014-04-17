using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedSprite : MonoBehaviour {
	
	// A struct for a individual animation.
	[System.Serializable]
	public class SpriteAnimation {
		public string name = "Unnamed";
		public float fps = 30;
		public int xOffset = 0;
		public int yOffset = 0;
		public int frameWidth = 100;
		public int frameHeight = 100;
		public Texture texture = null;
		public int xStartTile = 0;
		public int yStartTile = 0;
		public int frameCount = 1;
	}
	
	// A list containing all the animations for this object.
	public List<SpriteAnimation> Animations = new List<SpriteAnimation>();
	
	// The current animation
	public string CurrentAnimation;
	
	// A dictionary to store all the animations by name.
	private Dictionary<string, SpriteAnimation> animations = new Dictionary<string, SpriteAnimation>();
	
	// The timer.
	private float timer = 0;
	
	// Whether or not the sprite is flipped horizontally
	public bool FlippedHor = false;
	public bool FlippedVer = false;
	
	// Public functions
	public void ChangeAnimation(string _name, bool _resetTimer) {
		CurrentAnimation = _name;
		if (_resetTimer)
			timer = 0;
	}
	
	public void SetFPS(float fps)
	{
		SpriteAnimation current;
		if (animations.TryGetValue(CurrentAnimation, out current))
		{
			// Change the time passed to be as if it was at this fps.
			float frame = timer * current.fps;
			current.fps = fps;
			timer = frame / current.fps;
		}
	}
	
	// Use this for initialization
	void Start () {
		// Fill out the dictionary.
		animations.Clear();
		foreach (SpriteAnimation anim in Animations) {
			animations.Add(anim.name, anim);	
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Increment the timer
		timer += Time.deltaTime;
		
		// Set the UVs for this object.
		UpdateRect();
	}
	
	// Calculate the area of the image that should be shown at this time.
	void UpdateRect() {
		// If the current animation cannot be found, then don't do anything.
		if (!animations.ContainsKey(CurrentAnimation))
			return;
			
		// Get the current animation.
		SpriteAnimation current = animations[CurrentAnimation];
		
		// Calculate which frame we are on.
		int frame = (int)(timer * current.fps) % current.frameCount;
		
		// Get the area on the texture to use as the UVs
		int x = (current.xStartTile * current.frameWidth) + current.xOffset + (current.frameWidth * (frame % (current.texture.width / current.frameWidth)));
		int y = (current.yStartTile * current.frameHeight) + current.yOffset + (current.frameHeight * (frame / (current.texture.width / current.frameWidth)));
		
		// Get the mesh filter and mesh.
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.mesh;
		
		// Set the texture.
		gameObject.renderer.material.mainTexture = current.texture;
		
		// Set the uvs.
		Vector2[] uvs = new Vector2[4];
		if (!FlippedHor && !FlippedVer) {
			uvs[3].x = (float)x / current.texture.width;
			uvs[3].y = 1 - ((float)y / current.texture.height);
			uvs[0].x = (float)(x + current.frameWidth) / current.texture.width;
			uvs[0].y = 1 - ((float)y / current.texture.height);
			uvs[1].x = (float)(x + current.frameWidth) / current.texture.width;
			uvs[1].y = 1 - ((float)(y + current.frameHeight) / current.texture.height);
			uvs[2].x = (float)x / current.texture.width;
			uvs[2].y = 1 - ((float)(y + current.frameHeight) / current.texture.height);
		} else if (FlippedVer && !FlippedHor) {
			uvs[2].x = (float)x / current.texture.width;
			uvs[2].y = 1 - ((float)y / current.texture.height);
			uvs[1].x = (float)(x + current.frameWidth) / current.texture.width;
			uvs[1].y = 1 - ((float)y / current.texture.height);
			uvs[0].x = (float)(x + current.frameWidth) / current.texture.width;
			uvs[0].y = 1 - ((float)(y + current.frameHeight) / current.texture.height);
			uvs[3].x = (float)x / current.texture.width;
			uvs[3].y = 1 - ((float)(y + current.frameHeight) / current.texture.height);
		} else if (FlippedHor && !FlippedVer) {
			uvs[0].x = (float)x / current.texture.width;
			uvs[3].y = 1 - ((float)y / current.texture.height);
			uvs[3].x = (float)(x + current.frameWidth) / current.texture.width;
			uvs[0].y = 1 - ((float)y / current.texture.height);
			uvs[2].x = (float)(x + current.frameWidth) / current.texture.width;
			uvs[1].y = 1 - ((float)(y + current.frameHeight) / current.texture.height);
			uvs[1].x = (float)x / current.texture.width;
			uvs[2].y = 1 - ((float)(y + current.frameHeight) / current.texture.height);			
		}
		
		mesh.uv = uvs;
	}
}
