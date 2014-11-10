using UnityEngine;
using System.Collections;

public class LevelCreator
	: MonoBehaviour
{
	public string creator;

	public void Start()
	{
		GameObject thingy = Tutorial.ShowText("Creator", "Level creator: " + creator,
		                                          0, TextAlignment.Right, TextAnchor.LowerLeft, 0.05f, 0.1f);
		TextFader fader = thingy.AddComponent<TextFader>();
		fader.fadeRate = 1.0f;
		fader.delay = 2.0f;
		fader.FadeOut();
	}
}
