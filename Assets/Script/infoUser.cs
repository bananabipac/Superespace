using UnityEngine;
using System.Collections;

public class infoUser : MonoBehaviour {
	
	public int powerMin;
	public int powerMax;
	public int lifeShip;
	public float speedShip;
	 
	// Use this for initialization
	void Start () {
		powerMin = 2;
		powerMax = 7;
		lifeShip = 5;
		
		speedShip = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
