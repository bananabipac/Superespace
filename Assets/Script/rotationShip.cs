using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rotationShip : MonoBehaviour {
	
	public GameObject planet;
	public float speed;
	public int life;
	public bool super;
	
	//for huge ship
	public List<GameObject> ships ;
	
	// Use this for initialization
	void Start () {
		ships = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround(planet.transform.position,Vector3.up, speed * Time.deltaTime);
	}
	
	
}
