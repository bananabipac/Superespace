using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class attackPlanet : MonoBehaviour {
	
	public GameObject planetStart;
	public GameObject planetEnd;
	public List<GameObject> ships;
	
	//event touch
	//public List<GameObject> planets;
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	
	
	

	// Use this for initialization
	void Start () {
		
	}
	
	void Update() {
		
		
		if(Input.GetKeyDown(KeyCode.Space)){
			
			deplacement();
		}
		
		int count = Input.touchCount;
		for(int i = 0;i < count; i++) {
			Touch touch  = Input.GetTouch( i );
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
							deplacement();
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
		float max = scal/2.5f +1;
		
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
			
			((rotationShip)ships[j].GetComponent<rotationShip>()).speed = Random.Range(0.01f,0.1f);
		
				
							
		}
		
	
	}
	
	
	
}
