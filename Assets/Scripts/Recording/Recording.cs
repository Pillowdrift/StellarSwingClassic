using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System;
using System.Xml; 
using System.Xml.Serialization; 
using System.Text; 

/// <summary>
/// Recording
/// Serialized to save recordings
/// Fields can be added (and will assume their default value if not available in the file)
/// but removing or renaming a field will result in an exception when a file is next loaded
/// </summary> 
[System.Serializable]
public class Recording
{
	[System.Serializable]
	public struct Score
	{
		public float time;
		public float score;
		public float speed;
		
		public void Write(BinaryWriter writer)
		{
			writer.Write(time);
			writer.Write(score);
			writer.Write(speed);
		}
		public void Read(BinaryReader reader)
		{
			time = reader.ReadSingle();
			score = reader.ReadSingle();
			speed = reader.ReadSingle();
		}
	}
	
	[System.Serializable]
	public struct State
	{
		public bool grappling;
		public float3 grapplepos;
		public float3 position;
		public float3 velocity;
		
		public void Write(BinaryWriter writer)
		{
			writer.Write(grappling);
			grapplepos.Write(writer);
			position.Write(writer);
			velocity.Write(writer);
		}
		public void Read(BinaryReader reader)
		{
			grappling = reader.ReadBoolean();
			grapplepos.Read(reader);
			position.Read(reader);
			velocity.Read(reader);
		}
	}
	
	[System.Serializable]
	public struct float3
	{
		public float x, y, z;
		
		public Vector3 ToVector()
		{
			return new Vector3(x, y, z);
		}
		
		public void Write(BinaryWriter writer)
		{
			writer.Write(x);
			writer.Write(y);
			writer.Write(z);
		}
		public void Read(BinaryReader reader)
		{
			x = reader.ReadSingle();
			y = reader.ReadSingle();
			z = reader.ReadSingle();
		}
	}
	
	public const string dir = "/recordings";
	public const int VERSION = 2;
	public string SaveDir
	{
		get
		{
			return Application.persistentDataPath + dir;
		}
	}
	
	public bool flagged = false;
	public int fps = 0;
	public Score score;
	public string levelName;
	public List<State> states = new List<State>();
	public string playername = "Steve";
	
	public void Add(Vector3 position, Vector3 velocity, bool grappling, Vector3 grapple)
	{
		State state;
		
		state.position.x = position.x;
		state.position.y = position.y;
		state.position.z = position.z;
		
		state.velocity.x = velocity.x;
		state.velocity.y = velocity.y;
		state.velocity.z = velocity.z;
		
		state.grappling = grappling;
		
		state.grapplepos.x = grapple.x;
		state.grapplepos.y = grapple.y;
		state.grapplepos.z = grapple.z;
		
		if (states == null)
		{
			states = new List<State>();
		}
		
		states.Add(state);
	}
	
	public Vector3 GetPosition(float time)
	{
		if (states.Count == 0)
			return Vector3.zero;
		
		float frame = fps * time;
		
		int firstframe = (int)frame;
		int nextFrame = firstframe + 1;
		float lerpCoefficient = frame - (float)firstframe;
		
		if (firstframe < 0)
			return states[0].position.ToVector();
		else if (firstframe >= states.Count || nextFrame >= states.Count)
			return states[states.Count-1].position.ToVector();
		
		Vector3 last = states[firstframe].position.ToVector();
		Vector3 next = states[nextFrame].position.ToVector();
		
		Vector3 v = Vector3.Lerp(last, next, lerpCoefficient);
		
		v = last;
		
		return v;
	}
	
	public Vector3 GetVelocity(float time)
	{
		if (states.Count == 0)
			return Vector3.zero;
		
		float frame = fps * time;
		
		int firstframe = (int)frame;
		int nextFrame = firstframe + 1;
		float lerpCoefficient = frame - (float)firstframe;
		
		if (firstframe < 0)
			return states[0].velocity.ToVector();
		else if (firstframe >= states.Count || nextFrame >= states.Count)
			return states[states.Count-1].velocity.ToVector();
		
		Vector3 last = states[firstframe].velocity.ToVector();
		Vector3 next = states[nextFrame].velocity.ToVector();
		
		return Vector3.Lerp(last, next, lerpCoefficient);
	}
	
	public bool IsGrappling(float time)
	{
		int frame = (int)(fps * time);
		
		if (frame < 0 || frame >= states.Count)
			return false;
		
		return states[frame].grappling;
	}
	
	public Vector3 GetGrapplePos(float time)
	{
		Vector3 pos = new Vector3(0, 0, 0);
		
		int frame = (int)(fps * time);
		
		if (frame >= 0 && frame < states.Count)
		{
			pos.x = states[frame].grapplepos.x;
			pos.y = states[frame].grapplepos.y;
			pos.z = states[frame].grapplepos.z;
		}
		
		return pos;
	}
	
	public string Write(string filename)
	{
		string fullfilename = filename;
		
		// Write the recording to file.
		FileStream file = new FileStream(fullfilename, FileMode.OpenOrCreate, FileAccess.Write);
		Write(file);
		file.Close();
		
		return fullfilename;
	}
	
	public void Write(Stream stream)
	{
		BinaryWriter writer = new BinaryWriter(stream);
		
		// Write the version number
		writer.Write(VERSION);
		
		// Write the flagged value
		writer.Write(flagged);
		
		// Write the fps
		writer.Write(fps);
		
		// Write the score
		score.Write(writer);
		
		// Write the level name
		writer.Write(levelName);
		
		// Write the players name.
		writer.Write(playername);
		
		// Write all the states.
		writer.Write(states.Count); // How many states there are.
		for (int i = 0; i < states.Count; ++i) 	
		{
			states[i].Write(writer);
		}
		
		writer.Close();
	}
	
	public static Recording Read(string filename)
	{
		// Open the file.
		FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
		BinaryReader reader = new BinaryReader(file);
		
		Recording rec = Read(reader);
		
		reader.Close();
		file.Close();
		
		return rec;
	}
	
	public static Recording Read(BinaryReader reader)
	{
		// Read the version of the recording.
		Recording read = null;
		int version = reader.ReadInt32();
		switch (version)
		{
		case 1:
			read = Read1(reader);
			break;
		case 2:
			read = Read2(reader);
			break;
		default:
			throw new NotImplementedException();
		}
		
		return read;
	}
	
	public static Recording Read1(BinaryReader reader)
	{	
		// Create a recording
		Recording rec = new Recording();
		
		// Read the flagged value
		rec.flagged = reader.ReadBoolean();
		
		// Read the fps
		rec.fps = reader.ReadInt32();
		
		// Read the score.
		rec.score.Read(reader);
		
		// Read the level name
		rec.levelName = reader.ReadString();
		
		// Read all the states.
		int stateCount = reader.ReadInt32();
		for (int i = 0; i < stateCount; ++i)
		{
			State state = new State();
			state.Read(reader);
			rec.states.Add(state);
		}
		
		return rec;
	}
	
	public static Recording Read2(BinaryReader reader)
	{	
		// Create a recording
		Recording rec = new Recording();
		
		// Read the flagged value
		rec.flagged = reader.ReadBoolean();
		
		// Read the fps
		rec.fps = reader.ReadInt32();
		
		// Read the score.
		rec.score.Read(reader);
		
		// Read the level name
		rec.levelName = reader.ReadString();
		
		// Read the players name
		rec.playername = reader.ReadString();
		
		// Read all the states.
		int stateCount = reader.ReadInt32();
		for (int i = 0; i < stateCount; ++i)
		{
			State state = new State();
			state.Read(reader);
			rec.states.Add(state);
		}
		
		return rec;
	}	
}
