using UnityEngine;
using System.Collections;

public class JiteshJoystick : MonoBehaviour {
	
	private Rect _touchArea;
	private Rect _drawArea;
	public GUITexture JoyTexture;
	public ThirdPersonCamera theCam;
	
	private Vector2 _firstTouch;
	private Vector2 _currentTouch;
	
	private bool _isTouch;
	private bool _lastTouch;
	
	// Use this for initialization
	void Start () {
		_touchArea.height = JoyTexture.texture.height;
		_touchArea.width = JoyTexture.texture.width;
		
		_touchArea.x = JoyTexture.transform.position.x;
		_touchArea.y = JoyTexture.transform.position.y;
		
		_drawArea = _touchArea;
		_drawArea.y = Screen.height - _touchArea.height + JoyTexture.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_UNITY_ANDROID
		Vector2 touchPos = Input.GetTouch(0).position;
		
		if (_touchArea.Contains( new Vector3(touchPos.x,
											 touchPos.y,
											 0)))
		{
			if (!_lastTouch)
			{
				// this is the first touch so save the touch vector
				_firstTouch = touchPos;
				_isTouch = true;
			}
			else
			{
				// this is not the first touch
				_isTouch = false;
			}
			
			//calculate the vector between the first touch and where we are now
			
			Vector2 newTouch = touchPos - _firstTouch;
			
			_currentTouch = newTouch;
			_currentTouch.Normalize();
		
			//Camera testing code
			if (!(_currentTouch.x != 0 && _currentTouch.y != 0))
				theCam.DisablePointing();
			theCam.transform.Rotate(_currentTouch.y, -_currentTouch.x, 0);
		}
		
		
		foreach (Touch touch in Input.touches) {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			{
				// if there are touches on the screen
				_lastTouch = true;
			}
			else
			{
				//if there are no touches on the screen
				_lastTouch = false;
			}
        }
#endif		
	}
	
	void OnGUI2() {
		GUI.DrawTexture(_drawArea, JoyTexture.texture);	
		//GUI.Label(new Rect(20, 20, 200, 200), _isTouch ? "True" : "False");
	}
}
