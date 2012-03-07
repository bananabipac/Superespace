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
			
			float x = Random.Range(0.8f,1.2f);
			float z = Random.Range(0.8f,1.2f);
			Vector3 vec = new Vector3(x,0,z);
			vec = this.transform.position + vec ; 
			GameObject instance = (GameObject) Instantiate(ship,vec, transform.rotation);
			instance.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
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
