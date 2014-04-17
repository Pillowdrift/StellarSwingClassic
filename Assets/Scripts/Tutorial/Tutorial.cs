using UnityEngine;
using System.Collections;

public static class Tutorial
{
	private const string PREFIX = "TUTORIALTEXT_";
	
	private const string BIGGUIFONT = "BIGGUI";
	private const string SMALLGUIFONT = "SMALLGUI";
	
	private const string BIGGUIMAT = "BIGGUIMAT";
	private const string SMALLGUIMAT = "SMALLGUIMAT";
	
	private static Font bigFont = null;
	private static Font smallFont = null;
	
	private static Material bigMat = null;
	private static Material smallMat = null;
		
	public static GameObject ShowText(string name, string text, float time, TextAlignment alignment, TextAnchor anchor, float x, float y)
	{
		LoadFonts();
		
		// Destroy old object if existent
		GameObject textObject = GameObject.Find(PREFIX + name);
		MonoBehaviour.Destroy(textObject);
		
		textObject = new GameObject(PREFIX + name);
		
		TutorialText textBehaviour = textObject.GetComponent<TutorialText>();
		
		if (textBehaviour == null)
			textBehaviour = textObject.AddComponent<TutorialText>();
		
		GUIScale textScaler = textObject.GetComponent<GUIScale>();
		GUIText guitext = textObject.guiText;
		guitext.enabled = true;
		
		textObject.transform.position = new Vector3(x, y, 0);
		
		guitext.text = text;
		guitext.anchor = anchor;
		guitext.alignment = alignment;
		//guitext.font = bigFont;
		//guitext.material = bigMat;
		//guitext.fontSize = 48;
		
		if (time > 0)
			textBehaviour.DestroyIn(time);
		
		return textObject;
	}
	
	public static void HideText(string name)
	{
		GameObject testObject = GameObject.Find(PREFIX + name);
		if (testObject != null)
			testObject.SendMessage("DestroyImmed");
	}
	
	private static void LoadFonts()
	{
		bigFont = (Font)Resources.Load("Fonts/" + BIGGUIFONT);
		smallFont = (Font)Resources.Load("Fonts/" + SMALLGUIFONT);
		
		bigMat = (Material)Resources.Load("Fonts/" + BIGGUIMAT);
		smallMat = (Material)Resources.Load("Fonts/" + SMALLGUIMAT);
	}
}
