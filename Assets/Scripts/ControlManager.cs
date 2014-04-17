using UnityEngine;
using System.Collections;

public class ControlManager : MonoBehaviour
{
	// Set this if you want to disable controls until you enable them again
	public static bool Disabled
	{
		get
		{
			return controlsDisabled;
		}
		set
		{
			controlsDisabled = value;
			
			if (controlsDisabled)
			{
				DisableControls();
			}
			else
			{
				EnableControls();
			}
		}
	}
	
	// Set this to disable controls for this frame
	public static bool DisabledFrame
	{
		get
		{
			return lastDisabled == Time.frameCount;
		}
		set
		{
			lastDisabled = (value ? Time.frameCount : -1);
			
			if (lastDisabled == Time.frameCount)
			{
				DisableControls();
			}
			else
			{
				EnableControls();
			}
		}
	}
	
	public static bool MouseOnGUI
	{
		get
		{
//			UnityEngine.Rect rect = GUILayoutUtility.GetLastRect();
	//		if (rect != null && rect.Contains(InputManager.currentPosition))
//				return true;
			
			guiLayer = Camera.mainCamera.GetComponent<GUILayer>();
			
			if (guiLayer != null)
			{
				GUIElement element = guiLayer.HitTest(Input.mousePosition);
				
				if (element != null)
				{
					// If this isn't text
					if (element.guiText == null && element.enabled)
					{
						// If invisible, false
						if (element.guiTexture != null && element.guiTexture.color.a <= 0.0f)
							return false;
						
						return true;
					}
				}
			}
			
			return false;
		}
	}
	
	private static GrapplingHook grapplingHook;
	private static PlayerMovements playerMovement;
	
	private static bool controlsDisabled = false;
	private static int lastDisabled = -1;
	
	private static GUILayer guiLayer;
	
	void Awake()
	{
		grapplingHook = gameObject.GetComponent<GrapplingHook>();
		playerMovement = gameObject.GetComponent<PlayerMovements>();
	}
	
	private static void DisableControls()
	{
		if (grapplingHook != null && playerMovement != null)
		{
			grapplingHook.enabled = false;
			playerMovement.enabled = false;
		}
	}
	
	private static void EnableControls()
	{
		if (!controlsDisabled)
		{
			if (grapplingHook != null && playerMovement != null)
			{
				grapplingHook.enabled = true;
				playerMovement.enabled = true;
			}
		}
	}
}
