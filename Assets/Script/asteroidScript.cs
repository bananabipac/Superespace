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
		
		//distance = 2.8f;
		//chanceKill = 50;
	
	}
	
	
	void valideDep(GameObject ship){
			
		if(ship != null){
			rotationShip tmp = ship.GetComponent<rotationShip>();
			((rotationShip)ship.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
			if(tmp.super){
				List<GameObject> ships = ship.GetComponent<rotationShip>().ships;
			
				if(ships[0].tag == "red"){	
					tmp.planet.GetComponent<PlanetScript>().shipsRS.Add(ship);
				}else{
					tmp.planet.GetComponent<PlanetScript>().shipsBS.Add(ship);
				}
				int count = ships.Count;
				for(int i  = 0 ; i<count; i++){
					if(ships[0] != null){
						ships[0].transform.position = ship.transform.position;
						ships[0].GetComponent<rotationShip>().planet = tmp.planet; 
						if(ships[0].tag == "red"){
							tmp.planet.GetComponent<PlanetScript>().shipsR.Add(ships[0]);
						}else{
							tmp.planet.GetComponent<PlanetScript>().shipsB.Add(ships[0]);
						}
			
					}else{
						
					}
					
					ships.RemoveAt(0);
				}
				
			}else{
				if(ship.tag == "red"){
					((PlanetScript)tmp.planet.GetComponent<PlanetScript>()).shipsR.Add(ship);
				}else{
					((PlanetScript)tmp.planet.GetComponent<PlanetScript>()).shipsB.Add(ship);
				}
			}
		}else{
			//Debug.Log("erreu : "+i);	
		}

	}
	
	void Update () {
			
			int count = ships.Count;
			for(int i = 0 ; i<count; i++){
				GameObject ship = ships[0];
				if(ship != null){
					if(Vector3.Distance(ship.transform.position, this.transform.position) <= distance){
						rotationShip tmp = ship.gameObject.GetComponent<rotationShip>();
						float scal = tmp.planet.transform.localScale.x ;
								
						float min =  scal/2.5f+1   ;
						float max = scal/2.5f +1.5f;
						
						float z = Random.Range(min,max);
							
						Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), tmp.planet.transform.position);
								
						Vector3 vec = new Vector3(0,0,z);
						vec = quat * vec ;
						vec.y = 0;
					
					
						int kill = Random.Range(0, 101);
						
						if(tmp.super){
								List<GameObject> s = tmp.ships;
								int c = s.Count;
								int x = 0;
								while(x<c){
									if(x< s.Count){	
										int k = Random.Range(0, 101);
										if(k <= chanceKill){
											if(x<0){
												x =0;
											}
											//Debug.Log("count :"+s.Count+" x:"+x);
											GameObject t = s[x];
											Destroy(t);
											
											GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
											expl.transform.position = ship.transform.position;
											s.RemoveAt(x);
											
											//j --;
											if(s.Count <= 0){
												x = c+1;
											}
											
										}else{
											x++;
										}
									}else{
								
										x = c+1;
									}
								}
							
								if(s.Count != c){
									for(int j = 0 ; j<s.Count; j++){
										s[j].transform.position = ship.transform.position;
										s[j].GetComponent<MeshRenderer>().enabled = true;
									
										z = Random.Range(min,max);
											
										quat = Quaternion.AngleAxis(Random.Range(0f, 360f), tmp.planet.transform.position);
												
										vec = new Vector3(0,0,z);
										vec = quat * vec ;
										vec.y = 0;
								
										iTween.MoveTo(s[j],iTween.Hash("position",tmp.planet.transform.position+vec,"time",8f,"oncomplete","valideDep","onCompleteTarget", gameObject,"oncompleteparams", s[j], "easetype", "linear"));	
										shipsE.Add(s[j]);
									}
									Destroy(ship);
									
								
								}else{
									iTween.Stop(ship);
									iTween.MoveTo(ship,iTween.Hash("position",tmp.planet.transform.position+vec,"time",8f,"oncomplete","valideDep","onCompleteTarget", gameObject,"oncompleteparams", ship, "easetype", "linear"));	
									shipsE.Add(ship);
									
							
								}
							ships.RemoveAt(0);
								
						}else{
							if(kill <= chanceKill){
								//iTween.Stop(ship);
							
								ships.RemoveAt(0);
								GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
								expl.transform.position = ship.transform.position;
								Destroy(ship);
								
								
							}else{
								
								iTween.Stop(ship);
								iTween.MoveTo(ship,iTween.Hash("position",tmp.planet.transform.position+vec,"time",8f,"oncomplete","valideDep","onCompleteTarget", gameObject,"oncompleteparams", ship, "easetype", "linear"));	
								shipsE.Add(ship);
								ships.RemoveAt(0);
							}
						}
					}
				}
			}
			for(int j = 0 ; j<shipsE.Count; j++){
				GameObject ship = shipsE[j];
				infoUser info;
				if(ship!= null){
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
}
