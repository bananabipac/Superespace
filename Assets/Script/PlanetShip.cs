using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetShip : MonoBehaviour {
	
	public int nbShip;
	public List<GameObject> ships;
	
	public GameObject ship;
	public int deg;
	public bool fights;
	public int vFight ;
	private int count;
	public GameObject explosion;

	// Use this for initialization
	void Start () {
		fights = false;
		deg = 0;
		count = 0;
		
		for(int i = 0 ; i<nbShip ; i++){
			
			float min = this.transform.localScale.x ;
			//Debug.Log(min);
			
			float x = Random.Range(min-5f,min + 5f);
			float z = Random.Range(min-5f,min + 5f);
			Vector3 vec = new Vector3(x,-min,z);
			vec = this.transform.position + vec ; 
			
		
			Vector3 v = new Vector3(2f,2f,0);
			
			
			
			
			GameObject instance = (GameObject) Instantiate(ship,vec, transform.rotation);
			instance.transform.RotateAround(this.transform.position,v, Random.Range(0f,360f));
			((rotationShip)instance.GetComponent<rotationShip>()).planet = this.gameObject;
			((rotationShip)instance.GetComponent<rotationShip>()).speed = Random.Range(0.5f,0.8f);
			
			ships.Add(instance);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		this.transform.RotateAround(this.transform.position,Vector3.up, 0.2f);
		
		
	if(fights){	
		count ++;
		if(count >= vFight){
			count = 0;	
			if(ships.Count >=2){
				int i = 0;
				bool end = false;
				
				while(i<ships.Count && end ==false){
					int j = i+1;
					while(j<ships.Count && end == false){
						
						if(ships[i].tag != ships[j].tag){
							GameObject si = ships[i];
							GameObject sj = ships[i];	
						
							
							ships.RemoveAt(i);
							ships.RemoveAt(j);
								
							
							Vector3 vi = si.transform.position;
							Quaternion ri = si.transform.rotation;
								
							Vector3 vj = sj.transform.position;
							Quaternion rj = sj.transform.rotation;
								
							Destroy(si);
							Destroy(sj);
							Instantiate(explosion, vi, ri);
							Instantiate(explosion, vj, rj);
								
							end = true;
						}else{
							j++;		
						}
						
					}	
				i++;
				}	
				
			}
		}
	}
			
	
	}
}
