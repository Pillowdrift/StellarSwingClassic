using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml; 
using System.Xml.Serialization; 
using System.Text; 

/// <summary>
/// Recording
/// Serialized to save game
/// Fields can be added (and will assume their default value if not available in the file)
/// but removing or renaming a field will result in an exception when a file is next loaded
/// </summary> 
[Serializable]
public class Save
{
	public const string dir = "";
	public const int VERSION = 5;
	
	[NonSerialized]
	public string filename;
	
	public int worldUnlocked;
	public int levelUnlocked;
	public string playerName = "Name";
	
	public int droneCount = 0;
	
	// The high scores.
	[Serializable]
	public struct LevelHighScore
	{
		public float Score;
		public float Speed;
		public float Time;
		
		public int Stars;
	}
	public LevelHighScore[] highScores = new LevelHighScore[255];
	
	// Options
	public bool OnlineEnabled = true;
	public float BGMSound = 1.0f;
	
	// Get a high score for a level.
	public LevelHighScore GetHighScore(int _levelID) 
	{
		if (_levelID >= highScores.Length || _levelID < 0)
			return new LevelHighScore();
		return highScores[_levelID];
	}
	
	// Update the high score for a level.
	public void UpdateHighScore(LevelHighScore _score, int _levelID) 
	{
		// Get the current scores
		LevelHighScore current = GetHighScore(_levelID);
		
		// Compare them and update them.
		if (_score.Score > current.Score)
			current.Score = _score.Score;
		if (_score.Speed > current.Speed)
			current.Speed = _score.Speed;
		if (_score.Time < current.Time || current.Time == 0)
			current.Time = _score.Time;
		if (_score.Stars > current.Stars)
			current.Stars = _score.Stars;
		
		// Set the score.
		if (_levelID < highScores.Length && _levelID >= 0)
			highScores[_levelID] = current;
	} 
	
	public string Write()
	{
		return Write(filename);
	}
	
	public string Write(string filename)
	{
#if !UNITY_WEBPLAYER
		// Get the full path.
		string fullfilename = Application.persistentDataPath + dir + "/" + filename;
		Directory.CreateDirectory(Application.persistentDataPath + dir);
		
		// Now save.
		
		// Open the file.
		FileStream file = new FileStream(fullfilename, FileMode.OpenOrCreate, FileAccess.Write);
		BinaryWriter writer = new BinaryWriter(file);
#else
		string fullfilename = filename;
		
		MemoryStream ms = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(ms);
#endif
		
		// Write the identifying version number.
		writer.Write(VERSION);
		
		// Write the rest of the data.
		writer.Write(worldUnlocked); // The world unlocked.
		writer.Write(levelUnlocked); // The level unlocked.
		
		// Write the player name
		writer.Write(playerName); 
		
		// Write the options (no longer used)
		writer.Write(false);
		writer.Write(0.0f);
		writer.Write(0.0f);
		
		// Write the high scores
		writer.Write(highScores.Length); // How many high scores there are.
		for (int i = 0; i < highScores.Length; ++i) 
		{
			// Write each element.
			writer.Write(highScores[i].Score); // The score
			writer.Write(highScores[i].Speed); // The speed
			writer.Write(highScores[i].Time); // The time
			writer.Write(highScores[i].Stars); // How many stars
		}
		
		// Write drone count
		writer.Write(droneCount);
		
#if UNITY_WEBPLAYER
		byte[] bytes = ms.ToArray();
		string b64 = Convert.ToBase64String(bytes);
		
		PlayerPrefs.SetString("savefile", b64);
		
		ms.Close();
#endif
		
		// Cool we're done.
		writer.Close();
		
#if !UNITY_WEBPLAYER
		file.Close();
#endif
		
		// Return the full filename just in case something needs it.
		return fullfilename;
	}
	
	public static Save Read(string filename)
	{
#if UNITY_WEBPLAYER
		string savefile = PlayerPrefs.GetString("savefile", "");
		
		if (savefile == "")
			return null;
		
		byte[] binary = System.Convert.FromBase64String(savefile);
		MemoryStream ms = new MemoryStream(binary);
		BinaryReader reader = new BinaryReader(ms);
#else		
		string fullfilename = Application.persistentDataPath + dir + "/" + filename;
		
		if (!File.Exists(fullfilename))
			return null;
		
		// Open the file.
		FileStream file = new FileStream(fullfilename, FileMode.Open, FileAccess.Read);
		BinaryReader reader = new BinaryReader(file);
#endif
		
		// Read the version number
		int version = reader.ReadInt32();
		
		// Use the function that corresponds
		Save read;
		switch (version)
		{
		case 1:
			read = Read1(reader);
			break;
		case 2:
			read = Read2(reader);
			break;
		case 3:
			read = Read3(reader);
			break;
		case 4:
			read = Read4(reader);
			break;
		case 5:
			read = Read5(reader);
			break;
		default:
			throw new NotImplementedException();
		}
		read.filename = filename;
		
		// Close the file
		reader.Close();
		
#if UNITY_WEBPLAYER
		ms.Close();
#else
		file.Close();
#endif
		
		return read;
	}
	
	public static Save Read1(BinaryReader reader)
	{
		// Create a new save to read into.
		Save save = new Save();
		
		// Read in the unlocked levels
		save.worldUnlocked = reader.ReadInt32();
		save.levelUnlocked = reader.ReadInt32();
		
		// Read in the high scores.
		int levelCount = reader.ReadInt32();
		save.highScores = new LevelHighScore[levelCount];
		
		// Load the scores
		for (int i = 0; i < levelCount; ++i) 
		{
			save.highScores[i].Score = reader.ReadSingle();
			save.highScores[i].Speed = reader.ReadSingle();
			save.highScores[i].Time = reader.ReadSingle();
			save.highScores[i].Stars = reader.ReadInt32();
		}
		
		// Return the new save.
		return save;
	}
	
	public static Save Read2(BinaryReader reader)
	{
		// Create a new save to read into.
		Save save = new Save();
		
		// Read in the unlocked levels
		save.worldUnlocked = reader.ReadInt32();
		save.levelUnlocked = reader.ReadInt32();
		
		// Read the player name
		save.playerName = reader.ReadString();
		
		// Read in the high scores.
		int levelCount = reader.ReadInt32();
		save.highScores = new LevelHighScore[levelCount];
		
		// Load the scores
		for (int i = 0; i < levelCount; ++i) 
		{
			save.highScores[i].Score = reader.ReadSingle();
			save.highScores[i].Speed = reader.ReadSingle();
			save.highScores[i].Time = reader.ReadSingle();
			save.highScores[i].Stars = reader.ReadInt32();
		}
		
		// Return the new save.
		return save;
	}
	
	public static Save Read3(BinaryReader reader)
	{
		// Create a new save to read into.
		Save save = new Save();
		
		// Read in the unlocked levels
		save.worldUnlocked = reader.ReadInt32();
		save.levelUnlocked = reader.ReadInt32();
		
		// Read the player name
		save.playerName = reader.ReadString();
		
		// Read the options
		save.OnlineEnabled = reader.ReadBoolean();
		
		reader.ReadSingle();
		reader.ReadSingle();
		
		// Read in the high scores.
		int levelCount = reader.ReadInt32();
		save.highScores = new LevelHighScore[levelCount];
		
		// Load the scores
		for (int i = 0; i < levelCount; ++i) 
		{
			save.highScores[i].Score = reader.ReadSingle();
			save.highScores[i].Speed = reader.ReadSingle();
			save.highScores[i].Time = reader.ReadSingle();
			save.highScores[i].Stars = reader.ReadInt32();
		}
		
		// Return the new save.
		return save;
	}
	
	public static Save Read4(BinaryReader reader)
	{
		// Create a new save to read into.
		Save save = new Save();
		
		// Read in the unlocked levels
		save.worldUnlocked = reader.ReadInt32();
		save.levelUnlocked = reader.ReadInt32();
		
		// Read the player name
		save.playerName = reader.ReadString();
		
		// Read the options
		save.OnlineEnabled = reader.ReadBoolean();
		
		reader.ReadSingle();
		reader.ReadSingle();
		
		// Read in the high scores.
		int levelCount = reader.ReadInt32();
		save.highScores = new LevelHighScore[levelCount];
		
		// Load the scores
		for (int i = 0; i < levelCount; ++i) 
		{
			save.highScores[i].Score = reader.ReadSingle();
			save.highScores[i].Speed = reader.ReadSingle();
			save.highScores[i].Time = reader.ReadSingle();
			save.highScores[i].Stars = reader.ReadInt32();
		}
		
		// Return the new save.
		return save;
	}
	
	public static Save Read5(BinaryReader reader)
	{
		// Create a new save to read into.
		Save save = new Save();
		
		// Read in the unlocked levels
		save.worldUnlocked = reader.ReadInt32();
		save.levelUnlocked = reader.ReadInt32();
		
		// Read the player name
		save.playerName = reader.ReadString();
		
		// Read the options
		save.OnlineEnabled = reader.ReadBoolean();
		
		reader.ReadSingle();
		reader.ReadSingle();
		
		// Read in the high scores.
		int levelCount = reader.ReadInt32();
		save.highScores = new LevelHighScore[levelCount];
		
		// Load the scores
		for (int i = 0; i < levelCount; ++i) 
		{
			save.highScores[i].Score = reader.ReadSingle();
			save.highScores[i].Speed = reader.ReadSingle();
			save.highScores[i].Time = reader.ReadSingle();
			save.highScores[i].Stars = reader.ReadInt32();
		}
		
		// Load drone count
		save.droneCount = reader.ReadInt32();
		
		// Return the new save.
		return save;
	}
	
	/* The following metods came from the referenced URL */ 
	public static string UTF8ByteArrayToString(byte[] characters) 
	{      
	  UTF8Encoding encoding = new UTF8Encoding(); 
	  string constructedString = encoding.GetString(characters); 
	  return (constructedString); 
	} 
	
	public static byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
	  UTF8Encoding encoding = new UTF8Encoding(); 
	  byte[] byteArray = encoding.GetBytes(pXmlString); 
	  return byteArray; 
	} 
	
	// Here we serialize our UserData object of myData 
	string SerializeObject(object pObject) 
	{ 
	  string XmlizedString = null; 
	  MemoryStream memoryStream = new MemoryStream(); 
	  XmlSerializer xs = new XmlSerializer(typeof(Save)); 
	  XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
	  xs.Serialize(xmlTextWriter, pObject); 
	  memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
	  XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
	  return XmlizedString; 
	} 
	
	// Here we deserialize it back into its original form 
	public static object DeserializeObject(string pXmlizedString) 
	{ 
	  XmlSerializer xs = new XmlSerializer(typeof(Save)); 
	  MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
	  XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
	  return xs.Deserialize(memoryStream); 
	} 
	
#if !UNITY_WEBPLAYER
	void CreateXML(string filelocation, string data) 
	{ 
	  StreamWriter writer; 
	  FileInfo t = new FileInfo(filelocation); 
	  if(!t.Exists) 
	  { 
	     writer = t.CreateText(); 
	  } 
	  else 
	  { 
	     t.Delete(); 
	     writer = t.CreateText(); 
	  } 
	  writer.Write(data); 
	  writer.Close(); 
	  Debug.Log("File written."); 
	} 
#endif
	
	public static string LoadXML(string filelocation) 
	{ 
	  StreamReader r = File.OpenText(filelocation); 
	  string _info = r.ReadToEnd(); 
	  r.Close(); 
	  return _info;
	} 
}
