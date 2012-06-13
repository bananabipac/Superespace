using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class asteroidScript : MonoBehaviour {
	
	public List<GameObject> ships ;
	public List<GameObject> shipsE;

	public float distance; 
	public int chanceKill;
	
	// Use this for initialization
	void Start () {
		ships = new List<GameObject>();
		shipsE = new List<GameObject>();
		
		distance = 2.8f;
		chanceKill = 50;
	
	}
	
	
	void valideDep(GameObject ship){
			
		if(ship != null){
			rotationShip tmp = ship.GetComponent<rotationShip>();
			((rotationShip)ship.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
			if(ship.tag == "red"){
				
				((PlanetScript)tmp.planet.GetComponent<PlanetScript>()).shipsR.Add(ship);
				
			}else{
			
				((PlanetScript)tmp.planet.GetComponent<PlanetScript>()).shipsB.Add(ship);
			}
		}else{
			//Debug.Log("erreu : "+i);	
		}

	}
	
	void Update () {
			for(int i = 0 ; i<ships.Count; i++){
				GameObject ship = ships[i];
				if(Vector3.Distance(ship.transform.position, this.transform.position) <= distance){
					int kill = Random.Range(0, 101);
					if(kill <= chanceKill){
						
						ships.RemoveAt(i);
						Destroy(ship);
						i=i-1;
					}else{
						rotationShip tmp = ship.gameObject.GetComponent<rotationShip>();
						float scal = tmp.planet.transform.localScale.x ;
								
						float min =  scal/2.5f+1   ;
						float max = scal/2.5f +1.5f;
						
						float z = Random.Range(min,max);
							
						Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), tmp.planet.transform.position);
								
						Vector3 vec = new Vector3(0,0,z);
						vec = quat * vec ;
						vec.y = 0;
						iTween.Stop(ship);
						
						
						
						iTween.MoveTo(ship,iTween.Hash("position",tmp.planet.transform.position+vec,"time",8f,"oncomplete","valideDep","onCompleteTarget", gameObject,"oncompleteparams", ship, "easetype", "linear"));	
						shipsE.Add(ship);
						ships.RemoveAt(i);
					}
				}
			}
			for(int j = 0 ; j<shipsE.Count; j++){
				GameObject ship = shipsE[j];
				infoUser info;
				if(ship.tag == "red"){
					info = (infoUser) GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>();
				}else{
					info = (infoUser) GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>();
				}
				if(Vector3.Distance(ship.transform.position, this.transform.position) >= distance){
					rotationShip tmp = ship.GetComponent<rotationShip>();
				
					float scal = tmp.planet.transform.localScale.x ;
							
					float min =  scal/2.5f+1   ;
					float max = scal/2.5f +1.5f;
					
					float z = Random.Range(min,max);
						
					Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), tmp.planet.transform.position);
							
					Vector3 vec = new Vector3(0,0,z);
					vec = quat * vec ;
					vec.y = 0;
					iTween.Stop(ship);
					
					iTween.MoveTo(ship,iTween.Hash("position",tmp.planet.transform.position+vec,"time",info.speedShip,"oncomplete","valideDep","onCompleteTarget", gameObject,"oncompleteparams", ship, "easetype", "linear"));	
					shipsE.RemoveAt(j);
				}
			}
	}
}
