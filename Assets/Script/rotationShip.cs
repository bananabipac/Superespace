using UnityEngine;
using System.Collections;

public class rotationShip : MonoBehaviour {
	
	public GameObject planet;
	public float speed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround(planet.transform.position,Vector3.up, speed * Time.deltaTime);
	}
}
