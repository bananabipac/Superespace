using UnityEngine;
using System.Collections;

public class crashScript : MonoBehaviour {
	
	public float time;
	private float timetmp;
	
	// Use this for initialization
	void Start () {
		time = 5;
		timetmp = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		timetmp += 1*Time.deltaTime;
		
		if(timetmp >= time){
			
			Destroy(gameObject);	
			
		}
		
	
	}
}
