using UnityEngine;
using System.Collections;

public class SpaceBridge : MonoBehaviour {
	public GameObject blackHole1;
	public GameObject blackHole2;
	public GameObject planet1;
	public GameObject planet2;
	public bool bridgeOpen = false;
	// Use this for initialization
	void Start () {
		if (bridgeOpen) {
			blackHole1.active = true;
			blackHole2.active = true;			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		blackHole1.transform.RotateAround(blackHole1.transform.position,Vector3.up, 13f * Time.deltaTime);
		blackHole2.transform.RotateAround(blackHole2.transform.position,Vector3.up, 13f * Time.deltaTime);
	
	}
}
