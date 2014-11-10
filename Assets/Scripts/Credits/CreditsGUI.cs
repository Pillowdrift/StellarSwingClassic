using UnityEngine;
using System.Collections;

public class CreditsGUI : MonoBehaviour
{
	private Vector3 initialPosition;
	
	private const string credits = 	  "Credits\n\n"
									+ "2014 Pillowdrift LTD.\n"
									+ "Stellar Swing and all \n characters and artwork\n"
									+ "are trademarks of Pillowdrift LTD. \n"
									+ "\n"
									+ "Lead Programmer\n"
									+ "Caitlin Wilks\n"
									+ "\n"
									+ "Level Designer\n"
									+ "Jitesh Rawal\n"
									+ "\n"
									+ "Prototyper\n"
									+ "Richard Webster-Noble\n"
									+ "\n"
									+ "Programmer\n"
									+ "Grant Livingston\n"
									+ "\n"
									+ "Artwork\n"
									+ "Tom Duke\n"
									+ "Joe Brammer\n"
									+ "\n"
									+ "Sound\n"
									+ "Freesound.org\n"
									+ "Black Boe\n"
									+ "DJ Chronos\n"
									+ "sandyrb\n"
									+ "\n"
									+ "Stellar Swing OST by\n"
									+ "Campbell Logan\n"
									+ "\n"
									+ "Extra world soundtrack\n"
									+ "Bwarch\n"
									+ "\n"
									+ "A note on the extra world:\n"
									+ "The following are user contributed levels.\n"
									+ "They are not part of the main game.\n"
									+ "Do not feel that it is necessary to beat them.\n"
									+ "\n"
									+ "Extra world levels\n"
									+ "Holy\n"
									+ "spitznagl\n"
									+ "Willhart\n"
									+ "Rena\n"
									+ "Granix\n"
									+ "Jesuiscontent\n"
									+ "strawberrydoll (original level designer)\n"
									+ "\n \n"
									+ "Thanks for playing!\nNow challenge extra!\n\n"
									+ "www.pillowdrift.com";
	
	// rate of scrolling
	private const float rate = 0.1f;
	
	// Use this for initialization
	void Start()
	{
		initialPosition = transform.position;
		guiText.text = credits;
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.Translate(0, rate * Time.deltaTime, 0);
		
		if (transform.position.y > 3.5f)
			transform.position = initialPosition;
		
		// tap to get to menu
		if (Input.GetMouseButton(0))
		{
			if (SaveManager.save != null &&
				SaveManager.save.worldUnlocked == 5)
			{
				// World5 so it'll show THE NEXT world (6)
				SaveManager.save.worldUnlocked = 6;
				SaveManager.save.levelUnlocked = 1;

				LevelSelectGUI.worldToShow = "World5";
				LevelSelectGUI.levelToShow = 0;
				LevelSelectGUI.worldTransition = true;
				Application.LoadLevel("Title");
			}
			else
			{
				Loading.Load("Title");
			}
		}
	}
}
