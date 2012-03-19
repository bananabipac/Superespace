using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveShips_origin : MonoBehaviour {
	
	//public List<GameObject> planets;
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	
	void Update () {
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
			//Pour connaitre la planète de départ, le gameobject est représenté par la variable collider.
			if(touch.phase == TouchPhase.Began) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						Debug.Log ("Planete de départ");	
					}
				}
			}
			//Pour connaitre la planète de d'arrivée, le gameobject est représenté par la variable collider.
			if(touch.phase == TouchPhase.Ended) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						Debug.Log ("Planete d'arrivée");	
					}
				}
			}
		}
	}
}
