using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveShip : MonoBehaviour {
	
	public GameObject planetStart;
	public GameObject planetEnd;
	public List<GameObject> ships;
	public int lvl;
	private GameObject[] l;
	private GameObject user;
	
	private float selectSpeed ;
	private float selectTmp;
	
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	public  Dictionary<int,GameObject> listPlanetStart = new Dictionary<int,GameObject>();
	private Dictionary<int,GameObject> listPlanetEnd = new Dictionary<int,GameObject>();
	public Dictionary<int,GameObject> shipSelect = new Dictionary<int,GameObject>();
	
	private Hashtable link;
	private string dS;
	private string dE;
	
	
	public GUIStyle style;
	public GUISkin skin;

	// Use this for initialization
	void Start () {
		link = ((GestionLink)GetComponent<GestionLink>()).link;	
		l = GameObject.FindGameObjectsWithTag("link");
		user = GameObject.FindWithTag("User");
		
		selectSpeed = 0.4f;
		selectTmp = 0;
		
		style.font = skin.font;

	}
	
	void Update() {
		
	
		//////DEBUUG!!!!!!!!!!!!!!!!//////
		if(Input.GetKeyDown(KeyCode.Space)){
			if(listPlanetStart.ContainsKey(0)){
			}else{
				
				listPlanetStart.Add(0,planetStart);	
				GameObject SelectShip =  Resources.Load("TextSelect")as GameObject;
				Vector3 vec =  planetStart.transform.position;
				vec.y = -20.22636f;
				
				GameObject instance = (GameObject) Instantiate(SelectShip,vec, SelectShip.transform.rotation);
				((TextMesh)instance.GetComponent<TextMesh>()).text = ""+1;
				
				if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="red"){
					instance.transform.RotateAround(Vector3.up, 1.6f);
					Vector3 vt = instance.transform.position;
					vt.x +=5;
					instance.transform.position = vt;
				}else if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="blue"){
					instance.transform.RotateAround(Vector3.up, -1.6f);
					Vector3 vt = instance.transform.position;
					vt.x -=5;
					instance.transform.position = vt;
				}
				
				shipSelect.Add(0,instance);
			}
	
		}
		
		
		/*if(Input.GetKeyUp(KeyCode.Space)){
			if(shipSelect.ContainsKey(0)){
				shipSelect.Remove(0);
			}
			if(listPlanetStart.ContainsKey(0)){
				listPlanetStart.Remove(0);
			}
			deplacement(planetStart,planetEnd, 5);	
		}*/
		
		
		/////END DEBUG!!!!!!!!///
		
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
						listPlanetStart.Add(0,planetStart);	
						if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="red" ||((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="blue" ){
							GameObject SelectShip =  Resources.Load("TextSelect")as GameObject;
							Vector3 vec =  planetStart.transform.position;
							
							vec.y = -20.22636f;
							
							GameObject instance = (GameObject) Instantiate(SelectShip,vec, SelectShip.transform.rotation);
							((TextMesh)instance.GetComponent<TextMesh>()).text = ""+1;
							
							if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="red"){
								instance.transform.RotateAround(Vector3.up, 1.6f);
								Vector3 vt = instance.transform.position;
								vt.x +=5;
								instance.transform.position = vt;
							}else if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="blue"){
								instance.transform.RotateAround(Vector3.up, -1.6f);
								Vector3 vt = instance.transform.position;
								vt.x -=5;
								instance.transform.position = vt;
							}
							
							shipSelect.Add(0,instance);
						}
					}
				}
			}
			//pour la selection du nombre de vaisseau
			if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						PlanetScript scriptTemp = hit.collider.gameObject.GetComponent<PlanetScript>();
						if(scriptTemp.ship.tag == "red" || scriptTemp.ship.tag == "blue"){
							selectTmp += 1*Time.deltaTime;
							if(selectTmp >= selectSpeed){
								selectTmp = 0;
								//Debug.Log ("selection ship : "+shipSelect[fingerId]);
								TextMesh mesh = shipSelect[fingerId].GetComponent<TextMesh>();
								if(scriptTemp.ship.tag == "red"){
									if(int.Parse(mesh.text) +1 > scriptTemp.shipsR.Count-1){
										mesh.text = ""+(scriptTemp.shipsR.Count-1);
									}else{
										mesh.text = ""+(int.Parse(mesh.text)+ 1);
									}
								}else if (scriptTemp.ship.tag == "blue"){
									if(int.Parse(mesh.text) +1 > scriptTemp.shipsB.Count-1){
										mesh.text = ""+(scriptTemp.shipsB.Count-1);
									}else{
										mesh.text = ""+(int.Parse(mesh.text)+ 1);
									}
								}
							}
						}
					}
				}
			}
			//Pour connaitre la planète de d'arrivée, le gameobject est représenté par la variable collider.
			if(touch.phase == TouchPhase.Ended) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "planet") {
						//Debug.Log ("Planete d'arrivée");
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
							//Debug.Log(listPlanetStart[fingerId].name);
							//Debug.Log(listPlanetEnd[fingerId].name);
							if(((GestionLink)GetComponent<GestionLink>()).roadExist(listPlanetStart[fingerId],listPlanetEnd[fingerId])) {
								if(!((GestionLink)GetComponent<GestionLink>()).roadOpen(listPlanetStart[fingerId],listPlanetEnd[fingerId])) {
									for(int i = 0; i < l.Length; i++) {
										if(((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship == null && ((PlanetScript)listPlanetEnd[fingerId].GetComponent<PlanetScript>()).ship == null ){
											
										}else{
											if(((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship.tag == "red") {
												if(user.GetComponent<MoneyScript>().moneyPlayer1 >= 50) {
													if(l[i].name == ""+dS+dE){
														l[i].active = true;
														((Hashtable)link[dS])[dE] = "1";
														user.GetComponent<MoneyScript>().moneyPlayer1 -= 50;
														if(((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship.tag == "red") {
															user.GetComponent<MoneyScript>().incomePlayer1 += 1;
														} else if(((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship.tag == "blue") {
															user.GetComponent<MoneyScript>().incomePlayer2 += 1;
														}
													}
												}
											}
											if(((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship.tag == "blue") {
												if(user.GetComponent<MoneyScript>().moneyPlayer2 >= 50) {
													if(l[i].name == ""+dS+dE){
														l[i].active = true;
														((Hashtable)link[dS])[dE] = "1";
														user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
														if(((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship.tag == "red") {
															user.GetComponent<MoneyScript>().incomePlayer1 += 1;
														} else if(((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship.tag == "blue") {
															user.GetComponent<MoneyScript>().incomePlayer2 += 1;
														}
													}
												}
											}
										}
									
									}
								}
							}
							if(((Hashtable)link[dS])[dE] != null){//si il existe une route entre les 2 planetes
								if((string)((Hashtable)link[dS])[dE] == "1"){//si la route est ouverte
									//Debug.Log("other fights");
									TextMesh mesh = shipSelect[fingerId].GetComponent<TextMesh>();
									deplacement(listPlanetStart[fingerId],listPlanetEnd[fingerId], int.Parse(mesh.text));	
									
								}else{//la route est fermé			
									//Debug.Log("pas de route ouverte");
								}
							}else{//la route n'existe pas
								//Debug.Log("route impossible");	
							}
							
							listPlanetStart.Remove(fingerId);
							listPlanetEnd.Remove(fingerId);
							GameObject tmp = shipSelect[fingerId];
							shipSelect.Remove(fingerId);
							Destroy(tmp);


						}
						
					}
				}
				if(shipSelect.ContainsKey(fingerId)){
					GameObject tmp = shipSelect[fingerId];
					shipSelect.Remove(fingerId);
					Destroy(tmp);

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
			/*GameObject tmp = shipSelect[fingerId];
			shipSelect.Remove(fingerId);
			Destroy(tmp);*/
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
	void valideDeplacement(List<GameObject> shipT){
		for(int i  = 0 ; i<shipT.Count-1; i++){
			if(shipT[i] != null){
			//Debug.Log(ships[i].tag);
				if(shipT[i].tag == "red"){
				
					((PlanetScript)shipT[shipT.Count-1].GetComponent<PlanetScript>()).shipsR.Add(shipT[i]);
				}else{
				
					((PlanetScript)shipT[shipT.Count-1].GetComponent<PlanetScript>()).shipsB.Add(shipT[i]);
				}
			}else{
				//Debug.Log("erreu : "+i);	
			}
			
		}

	}
	
	
	
	//deplace les vaisseaux d'une planete a l'autre
	void deplacement(GameObject start, GameObject end, int nbShip){
		ships = new List<GameObject>();
		float scal = planetEnd.transform.localScale.x ;
				
		float min =  scal/2.5f+1   ;
		float max = scal/2.5f +1.5f;
	
		int nbs = nbShip ; 
		//int nbs ;
		PlanetScript p = (PlanetScript)start.GetComponent<PlanetScript>();
		
		if(p.ship != null){
			/*if(((PlanetScript)start.GetComponent<PlanetScript>()).ship.tag == "red"){
				nbs = ((PlanetScript)start.GetComponent<PlanetScript>()).shipsR.Count/2;
			}else{
			
				nbs = ((PlanetScript)start.GetComponent<PlanetScript>()).shipsB.Count/2;
			}*/
			
			for(int j = 0 ; j<nbs; j++){
				
				if(((PlanetScript)start.GetComponent<PlanetScript>()).ship.tag == "red"){
				
					ships.Add(((PlanetScript)start.GetComponent<PlanetScript>()).shipsR[0]);
					((PlanetScript)start.GetComponent<PlanetScript>()).shipsR.RemoveAt(0);
					
				}else{
				
					ships.Add(((PlanetScript)start.GetComponent<PlanetScript>()).shipsB[0]);
					((PlanetScript)start.GetComponent<PlanetScript>()).shipsB.RemoveAt(0);
				
				}

			
				((rotationShip)ships[j].GetComponent<rotationShip>()).speed = 0;
				((rotationShip)ships[j].GetComponent<rotationShip>()).planet = end;
	
				float z = Random.Range(min,max);
			
				Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), planetEnd.transform.position);
				
				Vector3 vec = new Vector3(0,0,z);
				vec = quat * vec ;
				vec.y = 0;
	 
				if(j == nbs -1){
					ships.Add(end);
					iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",2f,"oncomplete","valideDeplacement","onCompleteTarget", gameObject,"oncompleteparams", ships , "easetype", "linear"));	
					
				}else{
					iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",2f, "easetype", "linear"));
				}
				
				((rotationShip)ships[j].GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
									
			}
		
		}else{
			Debug.Log("null");	
		}
	}
	

	
	
}
