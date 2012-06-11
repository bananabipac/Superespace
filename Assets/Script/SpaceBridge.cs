using UnityEngine;
using System.Collections;

public class SpaceBridge : MonoBehaviour {
	public GameObject blackHole1;
	public GameObject blackHole2;
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
	
	}
}
