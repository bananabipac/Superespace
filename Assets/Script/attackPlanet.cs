using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class attackPlanet : MonoBehaviour {
	
	public GameObject planetStart;
	public GameObject planetEnd;
	public List<GameObject> ships;
	

	// Use this for initialization
	void Start () {
		
	}
	
	void Update() {
		
		foreach(Touch touch in Input.touches) {
			if(touch.phase == TouchPhase.Ended) 
				//Instantiate (planetStart,Camera.main.ScreenToWorldPoint(touch.position),transform.rotation);
				Debug.Log(Camera.main.ScreenToWorldPoint(touch.position));
			
		}
	}
	
	void OnGUI() {
        Event e = Event.current;
		
        if (e.isKey){
			
			//deplacement ships
            if(e.keyCode == KeyCode.Space){
				
				deplacement();
			}
			
			
		}  
    }
	
	
	
	
	//ajoute les vaisseaux au tableau de la planete d'arrivé
	void valideDeplacement(){
		
			
		for(int i  = 0 ; i<ships.Count; i++){
			
			if(ships[i].tag == "red"){
				
			((PlanetShip)planetEnd.GetComponent<PlanetShip>()).shipsR.Add(ships[i]);
			}else{
				
			((PlanetShip)planetEnd.GetComponent<PlanetShip>()).shipsR.Add(ships[i]);
			}
			
		}
	
	}
	
	//deplace les vaisseaux d'une planete a l'autre
	void deplacement(){
		ships = new List<GameObject>();
		float scal = planetEnd.transform.localScale.x ;
				
		float min = scal/2.5f  ;
		float max = scal/2.5f +5;
		
		int nbs ;
		
		
		if(((PlanetShip)planetStart.GetComponent<PlanetShip>()).ship.tag == "red"){
			
			nbs = ((PlanetShip)planetStart.GetComponent<PlanetShip>()).shipsR.Count/2;
		
		}else{
			
			nbs = ((PlanetShip)planetStart.GetComponent<PlanetShip>()).shipsB.Count/2;
		}
		
		
		for(int j = 0 ; j<nbs; j++){
			if(((PlanetShip)planetStart.GetComponent<PlanetShip>()).ship.tag == "red"){
				
				ships.Add(((PlanetShip)planetStart.GetComponent<PlanetShip>()).shipsR[j]);
				((PlanetShip)planetStart.GetComponent<PlanetShip>()).shipsR.RemoveAt(j);
					
			}else{
				
				ships.Add(((PlanetShip)planetStart.GetComponent<PlanetShip>()).shipsB[j]);
				((PlanetShip)planetStart.GetComponent<PlanetShip>()).shipsB.RemoveAt(j);
				
			}
				
			
			
			((rotationShip)ships[j].GetComponent<rotationShip>()).speed = 0;
			((rotationShip)ships[j].GetComponent<rotationShip>()).planet = planetEnd;
		
				
			
			float x = Random.Range(min,max);
			float z = Random.Range(min,max);
			Vector3 vec = new Vector3(x,0,z);
 
			if(j == nbs -1){
				iTween.MoveTo(ships[j],iTween.Hash("position",planetEnd.transform.position+vec,"time",2f,"oncomplete","valideDeplacement","onCompleteTarget", gameObject, "easetype", "linear"));	
				
			}else{
				iTween.MoveTo(ships[j],iTween.Hash("position",planetEnd.transform.position+vec,"time",2f, "easetype", "linear"));
			}
			
			((rotationShip)ships[j].GetComponent<rotationShip>()).speed = Random.Range(0.5f,0.8f);
		
				
							
		}
		
	
	}
	
	
	
}
