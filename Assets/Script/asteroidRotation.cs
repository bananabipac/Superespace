using UnityEngine;
using System.Collections;

public class asteroidRotation : MonoBehaviour {
	
	public float speed;
	public Vector3 rotation;

	// Use this for initialization
	void Start () {
		speed = Random.Range(20f, 50f);
		rotation = new  Vector3((float)Random.Range(-1,2),(float)Random.Range(-1,2),(float)Random.Range(-1,2));
	
	}
	
	// Update is called once per frame
	void Update () {
	
		this.transform.RotateAround(this.transform.position,rotation, speed * Time.deltaTime);
	}
}
