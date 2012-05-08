using UnityEngine;
using System.Collections;

public class infoUser : MonoBehaviour {
	
	public int powerMin;
	public int powerMax;
	public int lifeShip;
	public float speedShip;
	 
	// Use this for initialization
	void Start () {
		powerMin = 1;
		powerMax = 3;
		lifeShip = 2;
		
		speedShip = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
