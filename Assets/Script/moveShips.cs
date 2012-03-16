using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveShips : MonoBehaviour {
	
	public List<GameObject> planets;
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		int count = Input.touchCount;
		for(int i = 0;i < count; i++) {
			Touch touch  = Input.GetTouch( i );
			int fingerId  = touch.fingerId;
			if ( fingerId >= maxTouches )
			{
				// I'm not sure if this is a bug or how the  SDK reports finger IDs,
				// however, IMO there should only be five finger IDs max.
				if ( !warnedAboutMaxTouches )
				{
					Debug.Log( "Oops! We got a finderId greater than maxTouches: " + touch.fingerId );
					warnedAboutMaxTouches = true;
				}
			}
			Ray cursorRay = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			if(collider.Raycast(cursorRay, out hit, 1000.0f)) {
				Debug.Log ( "Hit detected on object " + name + " at point " + hit.point );
			}
		}
	}
}
