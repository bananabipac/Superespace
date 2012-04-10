using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetLink01 : MonoBehaviour {
	
	public Hashtable level;
	private Hashtable s;

	// Use this for initialization
	void Start () {
		level = new Hashtable();
		
		//LEVEL 1
		s = new Hashtable();
		
		Hashtable temp = new Hashtable();
		temp.Add(1, 1);
		temp.Add(2, 1);
		temp.Add(3, 1);
		s.Add(0,temp);
		
		temp = new Hashtable();
		temp.Add(2, 1);
		temp.Add(4, 1);
		s.Add(1,temp);
		
		temp = new Hashtable();
		temp.Add(3, 1);
		temp.Add(4, 1);
		temp.Add(5, 1);
		s.Add(2,temp);
		
		temp = new Hashtable();
		temp.Add(5, 1);
		s.Add(3,temp);
		
		temp = new Hashtable();
		temp.Add(5, 1);
		s.Add(4,temp);
		
		level.Add(1, s);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	
	}
}
