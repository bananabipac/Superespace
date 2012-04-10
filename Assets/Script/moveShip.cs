using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveShip : MonoBehaviour {
	
	public GameObject planetStart;
	public GameObject planetEnd;
	public List<GameObject> ships;
	public int lvl;
	public PlanetLink01 links;
	
	
	//event touch
	//public List<GameObject> planets;
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	private Dictionary<int,GameObject> listPlanetStart = new Dictionary<int,GameObject>();
	private Dictionary<int, GameObject> listPlanetEnd = new Dictionary<int,GameObject>();
	
	
	
	// Use this for initialization
	void Start () {
		//debug
		
		//lvl = 1;
		
	}
	
	void Update() {
		
		/*if(Input.GetKeyDown(KeyCode.Space)){
			Hashtable temp = (Hashtable)links.level[lvl];
				try{//si il existe une route entre les 2 planetes
					if((int)((Hashtable)temp[int.Parse(planetStart.name)])[int.Parse(planetEnd.name)] == 1){//si la route est ouverte
						deplacement();	
					}else{//la route est fermé			
						Debug.Log("pas de route ouverte");
					}
				}catch(System.NullReferenceException e){//la route n'existe pas
					Debug.Log("route impossible");	
				}
						
		 	
		}*/
		
		
		foreach(Touch touch in Input.touches) {
			
			int fingerId  = touch.fingerId;
			if ( fingerId >= maxTouches )
			{
				// I'm not sure if this is a bug or how the  SDK reports finger IDs,
				// however, IMO there should only be five finger IDs max.
				if ( !warnedAboutMaxTouches )
				{
					Debug.Log( "Oops! We got a finderId greater than maxTouches: " + touch.fingerId );
					warnedAboutMaxTouches = true;
				}
			}
			Ray cursorRay = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			//Pour connaitre la planète de départ, le gameobject est représenté par la variable collider.
			if(touch.phase == TouchPhase.Began) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						Debug.Log ("Planete de départ");
						planetStart = hit.collider.gameObject;
						listPlanetStart.Add(fingerId,planetStart);
						
					}
				}
			}
			//Pour connaitre la planète de d'arrivée, le gameobject est représenté par la variable collider.
			if(touch.phase == TouchPhase.Ended) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						Debug.Log ("Planete d'arrivée");
						planetEnd = hit.collider.gameObject;
						if(planetStart != planetEnd) {
							listPlanetEnd.Add(fingerId,planetEnd);
							//verification que les planetes soit liées entre elles
							Hashtable temp = (Hashtable)links.level[lvl];
							try{//si il existe une route entre les 2 planetes
								//if((int)((Hashtable)temp[int.Parse(planetStart.name)])[int.Parse(planetEnd.name)] == 1){//si la route est ouverte
									deplacement(listPlanetStart[fingerId],listPlanetEnd[fingerId]);	
									listPlanetStart.Remove(fingerId);
									listPlanetEnd.Remove(fingerId);
								//}else{//la route est fermé			
								//	Debug.Log("pas de route ouverte");
								//}
							}catch(System.NullReferenceException e){//la route n'existe pas
								Debug.Log("route impossible");	
							}
						}
					}
				}
			}
		}
		
		/*foreach(Touch touch in Input.touches) {
			if(touch.phase == TouchPhase.Ended) 
				//Instantiate (planetStart,Camera.main.ScreenToWorldPoint(touch.position),transform.rotation);
				Debug.Log(Camera.main.ScreenToWorldPoint(touch.position));
			
		}*/
	}
	
	
	
	//ajoute les vaisseaux au tableau de la planete d'arrivé
	void valideDeplacement(){
		
			
		for(int i  = 0 ; i<ships.Count; i++){
			
			if(ships[i].tag == "red"){
				
			((PlanetScript)planetEnd.GetComponent<PlanetScript>()).shipsR.Add(ships[i]);
			}else{
				
			((PlanetScript)planetEnd.GetComponent<PlanetScript>()).shipsB.Add(ships[i]);
			}
			
			
		}
	
	}
	
	//deplace les vaisseaux d'une planete a l'autre
	void deplacement(GameObject start, GameObject end){
		ships = new List<GameObject>();
		float scal = planetEnd.transform.localScale.x ;
				
		float min =  -1 * (scal/2.5f+1)   ;
		float max = scal/2.5f +1;
		
		int nbs ;
		
		
		if(((PlanetScript)start.GetComponent<PlanetScript>()).ship.tag == "red"){
			
			nbs = ((PlanetScript)start.GetComponent<PlanetScript>()).shipsR.Count/2;
		
		
		}else{
			
			nbs = ((PlanetScript)start.GetComponent<PlanetScript>()).shipsB.Count/2;
		}
		
	
		
		
		for(int j = 0 ; j<nbs; j++){
			if(((PlanetScript)start.GetComponent<PlanetScript>()).ship.tag == "red"){
				
				ships.Add(((PlanetScript)start.GetComponent<PlanetScript>()).shipsR[j]);
				((PlanetScript)start.GetComponent<PlanetScript>()).shipsR.RemoveAt(j);
					
			}else{
				
				ships.Add(((PlanetScript)start.GetComponent<PlanetScript>()).shipsB[j]);
				((PlanetScript)start.GetComponent<PlanetScript>()).shipsB.RemoveAt(j);
				
			}
				
			
			
			((rotationShip)ships[j].GetComponent<rotationShip>()).speed = 0;
			((rotationShip)ships[j].GetComponent<rotationShip>()).planet = end;
		
				
			
			float x = Random.Range(min,max);
			float z = Random.Range(min,max);
			while(x< scal/2.5f && x> -1* scal/2.5f && z< scal/2.5f && z> -1* scal/2.5f){
				x = Random.Range(min,max);
				z = Random.Range(min,max);
			}
			
			
			Vector3 vec = new Vector3(x,0,z);
 
			if(j == nbs -1){
				iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",2f,"oncomplete","valideDeplacement","onCompleteTarget", gameObject, "easetype", "linear"));	
				
			}else{
				iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",2f, "easetype", "linear"));
			}
			
			//ships[j].transform.RotateAround(planetEnd.transform.position,Vector3.up, Random.Range(0f,360f));
			((rotationShip)ships[j].GetComponent<rotationShip>()).speed = Random.Range(0.01f,0.1f);
			
		
				
							
		}
		
	
	}
	
	
	
}
