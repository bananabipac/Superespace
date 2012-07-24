using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineScript : MonoBehaviour {
	// Resources.Load("Shipred")as GameObject;
	
	private Dictionary<int,GameObject> listLines = new Dictionary<int,GameObject>();
	private Dictionary<int,GameObject> listPlanetStart = new Dictionary<int, GameObject>();
	private int maxTouches = 2;
	private bool warnedAboutMaxTouches = false;
	private GameObject user;
	// Use this for initialization
	void Start () {
		user = GameObject.FindWithTag("User");
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
					if(!user.GetComponent<PauseScript>().paused2) {
						if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
							if (hit.collider.tag == "planet" && hit.collider.gameObject.GetComponent<PlanetScript>().ship.tag !="neutre" ) {
								if(PlayerPrefs.GetString("GameType").Equals("solo")) {
									if(hit.collider.gameObject.GetComponent<PlanetScript>().ship.tag == "red"){
										GameObject instance =(GameObject) Instantiate(Resources.Load("Line")as GameObject);
										instance.transform.position = new Vector3(0,0,0);
										LineRenderer linet = instance.GetComponent<LineRenderer>();
										
										linet.SetPosition(0,hit.collider.gameObject.transform.position);
										linet.SetPosition(1,hit.collider.gameObject.transform.position);
										linet.SetColors(new Color(1,1,1,1),new Color(1,1,1,1));
										listLines.Add(fingerId,instance);
										listPlanetStart.Add (fingerId,hit.collider.gameObject);
									}
								}else{
									GameObject instance =(GameObject) Instantiate(Resources.Load("Line")as GameObject);
									instance.transform.position = new Vector3(0,0,0);
									LineRenderer linet = instance.GetComponent<LineRenderer>();
									
									linet.SetPosition(0,hit.collider.gameObject.transform.position);
									linet.SetPosition(1,hit.collider.gameObject.transform.position);
									linet.SetColors(new Color(1,1,1,1),new Color(1,1,1,1));
									listLines.Add(fingerId,instance);
									listPlanetStart.Add (fingerId,hit.collider.gameObject);
								}
							}
						}
					}
					
				}
				if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					if(!user.GetComponent<PauseScript>().paused2) {
						if(listPlanetStart.ContainsKey(fingerId)){
							Vector3 touched = Camera.main.ScreenToWorldPoint(touch.position);
							touched.y = listPlanetStart[fingerId].transform.position.y;
							LineRenderer linet = listLines[fingerId].GetComponent<LineRenderer>();
							if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
								//Debug.Log (hit.collider.tag);
								if ((hit.collider.tag == "planet" || hit.collider.tag == "BlackHole") && hit.collider.name != listPlanetStart[fingerId].name) {
									if(GetComponent<GestionLink>().roadExist(listPlanetStart[fingerId],hit.collider.gameObject)) {
										if(GetComponent<GestionLink>().roadOpen(listPlanetStart[fingerId],hit.collider.gameObject)) {
											linet.SetPosition(1,hit.collider.gameObject.transform.position);
											linet.SetColors(new Color(0,1,0,1),new Color(0,1,0,1));
										}else{
											if(listPlanetStart[fingerId].GetComponent<PlanetScript>().ship.tag == "red") {
												if(user.GetComponent<MoneyScript>().moneyPlayer1 >= 50) {
													linet.SetPosition(1,hit.collider.gameObject.transform.position);
													linet.SetColors(new Color(0,1,0,1),new Color(0,1,0,1));
												}else{
													linet.SetPosition(1,hit.collider.gameObject.transform.position);
													linet.SetColors(new Color(1,0,0,1),new Color(1,0,0,1));
												}
											}else{
												if(user.GetComponent<MoneyScript>().moneyPlayer2 >= 50) {
													linet.SetPosition(1,hit.collider.gameObject.transform.position);
													linet.SetColors(new Color(0,1,0,1),new Color(0,1,0,1));
												}else{
													linet.SetPosition(1,hit.collider.gameObject.transform.position);
													linet.SetColors(new Color(1,0,0,1),new Color(1,0,0,1));
												}
											}
										}
									} else {
										if(listPlanetStart[fingerId] == user.GetComponent<SpaceBridge>().planet1) {
											if(hit.collider.name == "BlackHole1") {
												linet.SetPosition(1,hit.collider.gameObject.transform.position);
												linet.SetColors(new Color(0,1,0,1),new Color(0,1,0,1));
											}else{
												linet.SetPosition(1,touched);
												linet.SetColors(new Color(1,0,0,1),new Color(1,0,0,1));	
											}	
										}else if(listPlanetStart[fingerId] == user.GetComponent<SpaceBridge>().planet2) {
											if(hit.collider.name == "BlackHole2") {
												linet.SetPosition(1,hit.collider.gameObject.transform.position);
												linet.SetColors(new Color(0,1,0,1),new Color(0,1,0,1));
											}else{
												linet.SetPosition(1,touched);
												linet.SetColors(new Color(1,0,0,1),new Color(1,0,0,1));	
											}	
										}else{
											linet.SetPosition(1,touched);
											linet.SetColors(new Color(1,0,0,1),new Color(1,0,0,1));
										}
									}
								} else {
									linet.SetPosition(1,touched);
									linet.SetColors(new Color(1,1,1,1),new Color(1,1,1,1));	
								}
							} else {
								linet.SetPosition(1,touched);
								linet.SetColors(new Color(1,1,1,1),new Color(1,1,1,1));		
							}
						}
					}
				}
				if(touch.phase == TouchPhase.Ended) {
					if(!user.GetComponent<PauseScript>().paused2) {
						if(listLines.ContainsKey(fingerId)){
							GameObject tmp = listLines[fingerId];
							listLines.Remove(fingerId);
							Destroy(tmp);
						}
						if(listPlanetStart.ContainsKey(fingerId)){
							listPlanetStart.Remove(fingerId);
						}
					}
					
				}
			}
	}
}
