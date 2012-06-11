using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceBridge : MonoBehaviour {
	public GameObject blackHole1;
	public GameObject blackHole2;
	public GameObject planet1;
	public GameObject planet2;
	public bool bridgeOpen = false;
	private GameObject[] listPlanets;
	private int nbRed;
	private int nbBlue;
	private bool everActivated = false;
	private float timer;
	private float lastChanceOfOpen;
	private bool timerLaunched = false;
	public float timeBlackHole;
	public float timeLastChanceOfOpen;
	// Use this for initialization
	void Start () {
		if (bridgeOpen) {
			blackHole1.active = true;
			blackHole2.active = true;	
		}
		lastChanceOfOpen = Time.timeSinceLevelLoad;
		listPlanets = GameObject.FindGameObjectsWithTag("planet");
	}
	
	// Update is called once per frame
	void Update () {
		if(bridgeOpen) {
			if(!timerLaunched) {
				timer = Time.timeSinceLevelLoad;
				timerLaunched = true;
			}
			if(Time.timeSinceLevelLoad - timer < timeBlackHole) {
				blackHole1.active = true;
				blackHole2.active = true;	
				blackHole1.transform.RotateAround(blackHole1.transform.position,Vector3.up, 13f * Time.deltaTime);
				blackHole2.transform.RotateAround(blackHole2.transform.position,Vector3.up, 13f * Time.deltaTime);
			} else {
				bridgeOpen = false;
				blackHole1.active = false;
				blackHole2.active = false;
			}
		}else{
			if(!everActivated) {
				for(int i = 0; i < listPlanets.Length;i++) {
					string planet = listPlanets[i].GetComponent<PlanetScript>().ship.tag;
					if(planet == "red") {
						nbRed++;	
					}else if(planet == "blue") {
						nbBlue++;	
					}
				}
				if(nbRed > listPlanets.Length/2 || nbBlue > listPlanets.Length/2) {
					bridgeOpen = true;	
					everActivated = true;
				} else if(Time.timeSinceLevelLoad - lastChanceOfOpen >= timeLastChanceOfOpen) {
					bridgeOpen = true;
					everActivated = true;
				}
				nbRed = 0;
				nbBlue = 0;
			}
		}
		
		
		
	}
}
