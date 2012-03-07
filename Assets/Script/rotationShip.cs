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
		Vector3 vec = new Vector3(2f, 2f,0f);
		
		this.transform.RotateAround(planet.transform.position,vec, speed);
		
		
		
	
	}
}
