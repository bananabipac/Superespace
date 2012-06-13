using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveShip : MonoBehaviour {
	
	public GameObject planetStart;
	public GameObject planetEnd;
	//public List<GameObject> ships;
	public int lvl;

	private GameObject user;
	
	private float selectSpeed ;

	
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	private  Dictionary<int,GameObject> listPlanetStart = new Dictionary<int,GameObject>();
	private Dictionary<int,GameObject> listPlanetEnd = new Dictionary<int,GameObject>();
	private Dictionary<int,GameObject> shipSelect = new Dictionary<int,GameObject>();
	private Dictionary<int,float> selectCount = new Dictionary<int,float>();
	//public Transform prefabNuke;
	
	// Use this for initialization
	void Start () {
		
		lvl = PlayerPrefs.GetInt("paramLevel");
		
		user = GameObject.FindWithTag("User");
		
		selectSpeed = 0.2f;


	}
	
	void Update() {
		
	
		//////DEBUUG!!!!!!!!!!!!!!!!//////
		if(Input.GetKeyDown(KeyCode.Space)){
			
			deplacement(GameObject.Find("0"), GameObject.Find("1"), 1);
			GetComponent<GestionLink>().openRoad(GameObject.Find("0"), GameObject.Find("1"));
		}
		
		
		if(Input.GetKeyUp(KeyCode.Space)){
			
			
		}
		
		
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
				if(!user.GetComponent<PauseScript>().paused) {
					if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
						if (hit.collider.tag == "planet") {
							Debug.Log ("Planete de départ" + fingerId);
							planetStart = hit.collider.gameObject;
							listPlanetStart.Add(fingerId,planetStart);	
							if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="red" ||((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="blue" ){
								
								if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="red" && ((PlanetScript)planetStart.GetComponent<PlanetScript>()).shipsR.Count>0){
									GameObject SelectShip =  Resources.Load("TextSelectRed")as GameObject;
									Vector3 vec =  planetStart.transform.position;
								
									vec.y = -20.22636f;
								
									GameObject instance = (GameObject) Instantiate(SelectShip,vec, SelectShip.transform.rotation);
									((TextMesh)instance.GetComponent<TextMesh>()).text = ""+0;
									
									instance.transform.RotateAround(Vector3.up, 1.6f);
									Vector3 vt = instance.transform.position;
									vt.x +=5;
									instance.transform.position = vt;
									
									shipSelect.Add(fingerId,instance);
									selectCount.Add(fingerId,0);
									
								}else if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="blue" && ((PlanetScript)planetStart.GetComponent<PlanetScript>()).shipsB.Count>0){
									GameObject SelectShip =  Resources.Load("TextSelectBlue")as GameObject;
									Vector3 vec =  planetStart.transform.position;
								
									vec.y = -20.22636f;
								
									GameObject instance = (GameObject) Instantiate(SelectShip,vec, SelectShip.transform.rotation);
									((TextMesh)instance.GetComponent<TextMesh>()).text = ""+1;
									instance.transform.RotateAround(Vector3.up, -1.6f);
									Vector3 vt = instance.transform.position;
									vt.x -=5;
									instance.transform.position = vt;
									
									shipSelect.Add(fingerId,instance);
									selectCount.Add(fingerId,0);
								}
								
								
							}
						}
					}
				}
			}
			//pour la selection du nombre de vaisseau
			if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
				if(!user.GetComponent<PauseScript>().paused) {
					if(listPlanetStart.ContainsKey(fingerId)){
						if(Physics.Raycast(cursorRay, out hit, 1000.0f)){
							//Debug.Log ("idSelect : "+fingerId);
							if (hit.collider.tag == "planet" && hit.collider.name == listPlanetStart[fingerId].name ) {
								PlanetScript scriptTemp = listPlanetStart[fingerId].GetComponent<PlanetScript>();
								if(scriptTemp.ship.tag == "red" || scriptTemp.ship.tag == "blue"){
									selectCount[fingerId] += 1*Time.deltaTime;
									if(selectCount[fingerId] >= selectSpeed){
										selectCount[fingerId] = 0;
										//Debug.Log ("selection ship : "+shipSelect[fingerId]);
										TextMesh mesh = shipSelect[fingerId].GetComponent<TextMesh>();
										if(scriptTemp.ship.tag == "red"){
											if(int.Parse(mesh.text) +1 > scriptTemp.shipsR.Count){
												mesh.text = ""+(scriptTemp.shipsR.Count);
											}else{
												mesh.text = ""+(int.Parse(mesh.text)+ 1);
											}
										}else if (scriptTemp.ship.tag == "blue"){
											if(int.Parse(mesh.text) +1 > scriptTemp.shipsB.Count){
												mesh.text = ""+(scriptTemp.shipsB.Count);
											}else{
												mesh.text = ""+(int.Parse(mesh.text)+ 1);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			//Pour connaitre la planète de d'arrivée, le gameobject est représenté par la variable collider.
			if(touch.phase == TouchPhase.Ended) {
				
				if(listPlanetStart.ContainsKey(fingerId)){
					if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
						if (hit.collider.tag == "planet" && hit.collider.name != listPlanetStart[fingerId].name ) {
							planetEnd = hit.collider.gameObject;
								listPlanetEnd.Add(fingerId,planetEnd);
								GameObject shipS =((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship; 
								GameObject shipE =((PlanetScript)listPlanetEnd[fingerId].GetComponent<PlanetScript>()).ship; 
								if(shipS.tag == "neutre" && shipE.tag != "neutre"){
									if(((GestionLink)GetComponent<GestionLink>()).roadExist(listPlanetStart[fingerId],listPlanetEnd[fingerId])){
										if(((GestionLink)GetComponent<GestionLink>()).roadOpen(listPlanetStart[fingerId],listPlanetEnd[fingerId])){
											Debug.Log("route deja ouverte");
										}else{
											if(shipE.tag=="red"){
												if(user.GetComponent<MoneyScript>().moneyPlayer1 >=50){
													user.GetComponent<MoneyScript>().moneyPlayer1 -= 50;
													((GestionLink)GetComponent<GestionLink>()).openRoad(listPlanetStart[fingerId],listPlanetEnd[fingerId]);
													Debug.Log("route ouverte rouge");	
												}else{
													Debug.Log("pas assez d'argent joueur rouge");	
												}
											}else if(shipE.tag == "blue"){
												if(user.GetComponent<MoneyScript>().moneyPlayer2 >=50){
													user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
													((GestionLink)GetComponent<GestionLink>()).openRoad(listPlanetStart[fingerId],listPlanetEnd[fingerId]);
													Debug.Log("route ouverte bleu");	
												}else{
													Debug.Log("pas assez d'argent joueur bleu");	
												}
											}
										}
									}else{
										Debug.Log("route inexistante");	
									}
								}else if(shipS.tag != "neutre"){
									if(((GestionLink)GetComponent<GestionLink>()).roadExist(listPlanetStart[fingerId],listPlanetEnd[fingerId])){
										if(((GestionLink)GetComponent<GestionLink>()).roadOpen(listPlanetStart[fingerId],listPlanetEnd[fingerId])){
											Debug.Log("Deplacement");
											TextMesh mesh = shipSelect[fingerId].GetComponent<TextMesh>();
											deplacement(listPlanetStart[fingerId],listPlanetEnd[fingerId], int.Parse(mesh.text));	
										}else{
											if(shipS.tag=="red"){
												if(user.GetComponent<MoneyScript>().moneyPlayer1 >=50){
													user.GetComponent<MoneyScript>().incomePlayer1 += 1;
													user.GetComponent<MoneyScript>().moneyPlayer1 -= 50;
													((GestionLink)GetComponent<GestionLink>()).openRoad(listPlanetStart[fingerId],listPlanetEnd[fingerId]);
													Debug.Log("route ouverte rouge");
													Debug.Log("Deplacement");
													TextMesh mesh = shipSelect[fingerId].GetComponent<TextMesh>();
													deplacement(listPlanetStart[fingerId],listPlanetEnd[fingerId], int.Parse(mesh.text));
												}else{
													Debug.Log("pas assez d'argent joueur rouge");	
												}
											}else if(shipS.tag == "blue"){
												if(user.GetComponent<MoneyScript>().moneyPlayer2 >=50){
													user.GetComponent<MoneyScript>().incomePlayer2 += 1;
													user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
													((GestionLink)GetComponent<GestionLink>()).openRoad(listPlanetStart[fingerId],listPlanetEnd[fingerId]);
													Debug.Log("route ouverte bleu");
													Debug.Log("Deplacement");
													TextMesh mesh = shipSelect[fingerId].GetComponent<TextMesh>();
													deplacement(listPlanetStart[fingerId],listPlanetEnd[fingerId], int.Parse(mesh.text));
												}else{
													Debug.Log("pas assez d'argent joueur bleu");	
												}
											}
										}
									}else{
										Debug.Log("route inexistante");	
									}
									
								}
									
							} else if (hit.collider.tag == "BlackHole"){
								listPlanetEnd.Add(fingerId,hit.collider.gameObject);
								//GameObject shipS =((PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>()).ship;
								//PlanetScript planetScript = (PlanetScript)listPlanetStart[fingerId].GetComponent<PlanetScript>();
								TextMesh mesh = shipSelect[fingerId].GetComponent<TextMesh>();
								deplacement(listPlanetStart[fingerId],hit.collider.gameObject, int.Parse(mesh.text));
								Debug.Log(int.Parse(mesh.text));
		
							}
						}
					
					
				}
				if(!user.GetComponent<PauseScript>().paused) {
					if(shipSelect.ContainsKey(fingerId)){
						GameObject tmp = shipSelect[fingerId];
						shipSelect.Remove(fingerId);
						Destroy(tmp);
					}
					if(selectCount.ContainsKey(fingerId)){
						selectCount.Remove(fingerId);
					}
					if(listPlanetStart.ContainsKey(fingerId)){
						listPlanetStart.Remove(fingerId);
					}
					if(listPlanetEnd.ContainsKey(fingerId)){
						listPlanetEnd.Remove(fingerId);	
					}
				}
			}
			
		}
		
		if( verifEndGame()){
			Application.LoadLevel("Menu");
		}
		
		
		
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
				((rotationShip)shipT[i].GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
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
		infoUser info;
		List<GameObject> ships = new List<GameObject>();
		PlanetScript p = (PlanetScript)start.GetComponent<PlanetScript>();
		int nbs = nbShip ;
		if(p.ship.tag == "red"){
			info = (infoUser) GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>();
		}else{
			info = (infoUser) GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>();
		}
		if(end.tag == "planet") {
			
			
			
			float scal = end.transform.localScale.x ;
					
			float min =  scal/2.5f+1   ;
			float max = scal/2.5f +1.5f;
			
			
			
			if(p.ship != null){
			
				
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
				
					Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), end.transform.position);
					
					Vector3 vec = new Vector3(0,0,z);
					vec = quat * vec ;
					vec.y = 0;
		 
					if(j == nbs -1){
						ships.Add(end);
						iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",info.speedShip,"oncomplete","valideDeplacement","onCompleteTarget", gameObject,"oncompleteparams", ships , "easetype", "linear"));	
						
					}else{
						iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position+vec,"time",info.speedShip, "easetype", "linear"));
					}
					GetComponent<GestionLink>().activeAsteroid(start,end, ships);
					int[] links = GetComponent<GestionLink>().nbRoad();
					user.GetComponent<MoneyScript>().incomePlayer1 = 1 + links[0];
					user.GetComponent<MoneyScript>().incomePlayer2 = 1 + links[1];
					
										
				}
			
			}else{
				Debug.Log("null");	
			}
		} else if (end.tag == "BlackHole") {
			if(p.ship != null){
				if((user.GetComponent<SpaceBridge>().planet1 == start && end.name == "BlackHole1")
					||(user.GetComponent<SpaceBridge>().planet2 == start && end.name == "BlackHole2")){
					for(int j = 0 ; j<nbs; j++){
						
						if(((PlanetScript)start.GetComponent<PlanetScript>()).ship.tag == "red"){
						
							ships.Add(((PlanetScript)start.GetComponent<PlanetScript>()).shipsR[0]);
							((PlanetScript)start.GetComponent<PlanetScript>()).shipsR.RemoveAt(0);
							
						}else{
						
							ships.Add(((PlanetScript)start.GetComponent<PlanetScript>()).shipsB[0]);
							((PlanetScript)start.GetComponent<PlanetScript>()).shipsB.RemoveAt(0);
						
						}
						GameObject[] param = new GameObject[2];
						param[0] = ships[j];
						param[1] = end;
	
						iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position,"time",info.speedShip,"oncomplete","teleport","onCompleteTarget", gameObject,"oncompleteparams", param, "easetype", "linear"));			
					}
				}
			}
		}
		
	}
	
	void teleport(GameObject[] param) {
		GameObject end = param[1];
		Vector3 sortie = new Vector3();
		GameObject[] blackHoles = GameObject.FindGameObjectsWithTag("BlackHole");
		GameObject planeteArrivee = new GameObject();
		float scal = 0f;
		infoUser info = new infoUser();
		
		if(end.name == "BlackHole1") {
			for(int i = 0; i < blackHoles.Length;i++) {
				if(blackHoles[i].name == "BlackHole2") {
					sortie = blackHoles[i].transform.position;	
					scal = user.GetComponent<SpaceBridge>().planet2.transform.localScale.x ;
					planeteArrivee = user.GetComponent<SpaceBridge>().planet2;
				}
			}
		}else if(end.name == "BlackHole2") {
			for(int i = 0; i < blackHoles.Length;i++) {
				if(blackHoles[i].name == "BlackHole1") {
					sortie = blackHoles[i].transform.position;	
					scal = user.GetComponent<SpaceBridge>().planet1.transform.localScale.x ;
					planeteArrivee = user.GetComponent<SpaceBridge>().planet1;
				}
			}
		}
		GameObject ship = param[0];
		
		ship.transform.position = sortie;
		
		if(ship.tag == "red"){
			info = (infoUser) GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>();
		}else{
			info = (infoUser) GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>();
		}
					
		float min =  scal/2.5f+1   ;
		float max = scal/2.5f +1.5f;
		
		((rotationShip)ship.GetComponent<rotationShip>()).speed = 0;
		((rotationShip)ship.GetComponent<rotationShip>()).planet = planeteArrivee;

		float z = Random.Range(min,max);
	
		Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), planeteArrivee.transform.position);
		
		Vector3 vec = new Vector3(0,0,z);
		vec = quat * vec ;
		vec.y = 0;
		
		iTween.MoveTo(ship,iTween.Hash("position",planeteArrivee.transform.position+vec,"time",info.speedShip,"oncomplete","valideDep","onCompleteTarget", gameObject,"oncompleteparams", ship , "easetype", "linear"));	
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

	
}
