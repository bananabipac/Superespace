using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveShip : MonoBehaviour {
	
	public GameObject planetStart;
	public GameObject planetEnd;
	public List<GameObject> ships;
	public int lvl;
	public PlanetLink01 links;
	
	
	
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	private Dictionary<int,GameObject> listPlanetStart = new Dictionary<int,GameObject>();
	private Dictionary<int, GameObject> listPlanetEnd = new Dictionary<int,GameObject>();
	
	
	private Hashtable link;
	private string dS;
	private string dE;
	
	
	
	
	
	// Use this for initialization
	void Start () {
		link = ((GestionLink)GetComponent<GestionLink>()).link;	
		//((GestionLink)GetComponent<GestionLink>()).roadExist(planetStart, planetEnd);
		//((GestionLink)GetComponent<GestionLink>()).roadOpen(planetStart, planetEnd);
	
	}
	
	void Update() {
		
		
		if(Input.GetKeyDown(KeyCode.Space)){
			
			link = ((GestionLink)GetComponent<GestionLink>()).link;
			if(int.Parse(planetStart.name) > int.Parse(planetEnd.name)){
				dS = planetEnd.name;
				dE = planetStart.name;
			}else{
				dE = planetEnd.name;
				dS = planetStart.name;
			}
			if(((Hashtable)link[dS])[dE] != null){//si il existe une route entre les 2 planetes
				if((string)((Hashtable)link[dS])[dE] == "1"){//si la route est ouverte
					deplacement(planetStart,planetEnd);	
				}else{//la route est fermé			
					Debug.Log("pas de route ouverte");
				}
			}else{//la route n'existe pas
				Debug.Log("route impossible");	
			}
		
		}
		
		
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
							link = ((GestionLink)GetComponent<GestionLink>()).link;
							if(int.Parse(listPlanetStart[fingerId].name) > int.Parse(listPlanetEnd[fingerId].name)){
								dS = listPlanetEnd[fingerId].name;
								dE = listPlanetStart[fingerId].name;
							}else{
								dE = listPlanetEnd[fingerId].name;
								dS = listPlanetStart[fingerId].name;
							}
							if(((Hashtable)link[dS])[dE] != null){//si il existe une route entre les 2 planetes
								if((string)((Hashtable)link[dS])[dE] == "1"){//si la route est ouverte
									Debug.Log("other fights");
									deplacement(listPlanetStart[fingerId],listPlanetEnd[fingerId]);	
									
								}else{//la route est fermé			
									Debug.Log("pas de route ouverte");
								}
							}else{//la route n'existe pas
								Debug.Log("route impossible");	
							}
							
							listPlanetStart.Remove(fingerId);
<<<<<<< HEAD
							listPlanetEnd.Remove(fingerId);
							
=======
							listPlanetEnd.Remove(fingerId);	

>>>>>>> 13ac8f595daa9e07f696547f595631becfd2dc43
						}
						
					}
				}
				if(listPlanetStart.ContainsKey(fingerId)){
					listPlanetStart.Remove(fingerId);
				}
				if(listPlanetEnd.ContainsKey(fingerId)){
					listPlanetEnd.Remove(fingerId);	
				}
			}
		}
		
		if(Input.touchCount == 0){
			//Debug.Log("no touch");
			listPlanetStart = new Dictionary<int,GameObject>();
			listPlanetEnd = new Dictionary<int,GameObject>();
		}
		
		if( verifEndGame()){
			Application.LoadLevel("Menu");
		}
		
		/*foreach(Touch touch in Input.touches) {
			if(touch.phase == TouchPhase.Ended) 
				//Instantiate (planetStart,Camera.main.ScreenToWorldPoint(touch.position),transform.rotation);
				Debug.Log(Camera.main.ScreenToWorldPoint(touch.position));
			
		}*/
	}
	
	/*verifie si un des 2 joueurs a perdue
	@return true si un des 2 joueurs a perdu false sinon */
	bool verifEndGame(){
		
		GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");
		
		bool redPlayer = false;
		bool bluePlayer = false;
		int x = 0;
		
		//on verifie si un des 2 joueurs n'a plus de vaisseaux ni de planete
		while((redPlayer == false || bluePlayer == false	) && x < ((GameObject[] )planets).Length){
			PlanetScript p = planets[x].GetComponent<PlanetScript>();
			if(p.ship !=null){
				if(p.ship.tag == "red"){
					redPlayer = true;	
				}
				if(p.ship.tag == "blue"){
					bluePlayer = true;
				}		
			}
			if(p.shipsB.Count > 0 ){
				bluePlayer = true;	
			}
			if(p.shipsR.Count > 0){
				redPlayer = true;	
			}
			x++;
		}
		
		if(redPlayer == false || bluePlayer == false){
			return true;
		}else{
			return false;	
		}
	}
	
	//ajoute les vaisseaux au tableau de la planete d'arrivé
	void valideDeplacement(){
		
			
		for(int i  = 0 ; i<ships.Count; i++){
			if(ships[i] != null){
			//Debug.Log(ships[i].tag);
				if(ships[i].tag == "red"){
				
					((PlanetScript)planetEnd.GetComponent<PlanetScript>()).shipsR.Add(ships[i]);
				}else{
				
					((PlanetScript)planetEnd.GetComponent<PlanetScript>()).shipsB.Add(ships[i]);
				}
			}else{
				Debug.Log("erreu : "+i);	
			}
			
		}
	
	}
	
	//deplace les vaisseaux d'une planete a l'autre
	void deplacement(GameObject start, GameObject end){
		
		ships = new List<GameObject>();
		float scal = planetEnd.transform.localScale.x ;
				
		float min =  scal/2.5f+1   ;
		float max = scal/2.5f +1.5f;
	
		int nbs ; 
		PlanetScript p = (PlanetScript)start.GetComponent<PlanetScript>();
		
		if(p.ship != null){
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
	
				float z = Random.Range(min,max);
			
				Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), planetEnd.transform.position);
				
				Vector3 vec = new Vector3(0,0,z);
				vec = quat * vec ;
	 
				if(j == nbs -1){
					iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",2f,"oncomplete","valideDeplacement","onCompleteTarget", gameObject, "easetype", "linear"));	
					
				}else{
					iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",2f, "easetype", "linear"));
				}
				
				((rotationShip)ships[j].GetComponent<rotationShip>()).speed = Random.Range(0.01f,0.1f);
									
			}
		
		}else{
			Debug.Log("null");	
		}
	}
	
	
	
}
