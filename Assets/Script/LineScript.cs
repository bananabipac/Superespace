using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineScript : MonoBehaviour {
	
	private LineRenderer player1;
	private LineRenderer player2;
	private Dictionary<int,LineRenderer> listLines = new Dictionary<int,LineRenderer>();
	private Dictionary<int,GameObject> listPlanetStart = new Dictionary<int, GameObject>();
	private int maxTouches = 2;
	private bool warnedAboutMaxTouches = false;
	// Use this for initialization
	void Start () {
		GameObject user1 = GameObject.FindGameObjectWithTag("infoUserRed");
		player1 = user1.GetComponent<LineRenderer>();
		GameObject user2 = GameObject.FindGameObjectWithTag("infoUserBlue");
		player2 = user2.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Touch touch in Input.touches) {
			
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
			if(touch.phase == TouchPhase.Began) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						if(hit.collider.gameObject.GetComponent<PlanetScript>().ship.tag == "red"){
							player1.SetPosition(0,hit.collider.gameObject.transform.position);
							player1.SetPosition(1,hit.collider.gameObject.transform.position);
							listLines.Add(fingerId,player1);
							listPlanetStart.Add (fingerId,hit.collider.gameObject);
						}
						if(hit.collider.gameObject.GetComponent<PlanetScript>().ship.tag == "blue"){
							player2.SetPosition(0,hit.collider.gameObject.transform.position);
							player2.SetPosition(1,hit.collider.gameObject.transform.position);
							listLines.Add(fingerId,player2);
							listPlanetStart.Add (fingerId,hit.collider.gameObject);
						}
					}
				}
			}
			if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
				player1.SetColors(new Color(1,1,1,1), new Color(1,1,1,1));
				player2.SetColors(new Color(1,1,1,1), new Color(1,1,1,1));
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						if(GetComponent<GestionLink>().roadExist(listPlanetStart[fingerId],hit.collider.gameObject)) {
							if(listLines[fingerId].tag == "infoUserRed") {
								player1.SetPosition(1,hit.collider.transform.position);	
								player1.SetColors(new Color(0,1,0,1), new Color(0,1,0,1));
							}
							if(listLines[fingerId].tag == "infoUserBlue"){
								player2.SetPosition(1,hit.collider.transform.position);
								player2.SetColors(new Color(0,1,0,1), new Color(0,1,0,1));
							}
						}
						else {
							Vector3 temp = new Vector3(0f,player1.transform.position.y,0f);
							Vector3 touched = Camera.main.ScreenToWorldPoint(touch.position);
							touched.y = temp.y;
							if(listLines[fingerId].tag == "infoUserRed") {
								player1.SetPosition(1,touched);
								player1.SetColors(new Color(1,0,0,1), new Color(1,0,0,1));
							}
							if(listLines[fingerId].tag == "infoUserBlue") {
								player2.SetPosition(1,touched);
								player2.SetColors(new Color(1,0,0,1), new Color(1,0,0,1));
							}
						}
					} else {
							Vector3 temp = new Vector3(0f,player1.transform.position.y,0f);
							Vector3 touched = Camera.main.ScreenToWorldPoint(touch.position);
							touched.y = temp.y;
							if(listLines[fingerId].tag == "infoUserRed") {
								player1.SetPosition(1,touched);
								player1.SetColors(new Color(1,0,0,1), new Color(1,0,0,1));
							}
							if(listLines[fingerId].tag == "infoUserBlue") {
								player2.SetPosition(1,touched);
								player2.SetColors(new Color(1,0,0,1), new Color(1,0,0,1));
							}
						}
				} else {
					Vector3 temp = new Vector3(0f,player1.transform.position.y,0f);
					Vector3 touched = Camera.main.ScreenToWorldPoint(touch.position);
					touched.y = temp.y;
					if(listLines[fingerId].tag == "infoUserRed") {
						player1.SetPosition(1,touched);
					}
					if(listLines[fingerId].tag == "infoUserBlue") {
						player2.SetPosition(1,touched);
					}
						
				}
			}
			if(touch.phase == TouchPhase.Ended) {
				if(listLines.ContainsKey(fingerId)){
					listLines[fingerId].SetPosition(0,new Vector3(0f,0f,0f));
					listLines[fingerId].SetPosition(1,new Vector3(0f,0f,0f));
					listLines.Remove(fingerId);	
					listPlanetStart.Remove (fingerId);
				}
			}
		}
	}
}
