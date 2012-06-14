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
	
	public GameObject rightArrow1;
	public GameObject rightArrow2;
	public GameObject leftArrow1;
	public GameObject leftArrow2;
	// Use this for initialization
	void Start () {
		if (bridgeOpen) {
			blackHole1.active = true;
			blackHole2.active = true;
			leftArrow1.active = true;
			leftArrow2.active = true;
			rightArrow1.active = true;
			rightArrow2.active = true;
			iTween.MoveTo(leftArrow1,iTween.Hash("x",-26,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
			iTween.MoveTo(leftArrow2,iTween.Hash("x",-25,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
			iTween.MoveTo(rightArrow1,iTween.Hash("x",26,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
			iTween.MoveTo(rightArrow2,iTween.Hash("x",25,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
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
				leftArrow1.active = false;
				leftArrow2.active = false;
				rightArrow1.active = false;
				rightArrow2.active = false;
				iTween.Stop (leftArrow1);
				iTween.Stop (leftArrow2);
				iTween.Stop (rightArrow1);
				iTween.Stop (rightArrow2);
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
					leftArrow1.active = true;
					leftArrow2.active = true;
					rightArrow1.active = true;
					rightArrow2.active = true;
					iTween.MoveTo(leftArrow1,iTween.Hash("x",-26,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
					iTween.MoveTo(leftArrow2,iTween.Hash("x",-25,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
					iTween.MoveTo(rightArrow1,iTween.Hash("x",26,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
					iTween.MoveTo(rightArrow2,iTween.Hash("x",25,"time",0.5,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.linear));
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
