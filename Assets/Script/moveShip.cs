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
	private bool redPlayer = false;
	private bool bluePlayer = false;
	
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	private  Dictionary<int,GameObject> listPlanetStart = new Dictionary<int,GameObject>();
	private Dictionary<int,GameObject> listPlanetEnd = new Dictionary<int,GameObject>();
	private Dictionary<int,GameObject> shipSelect = new Dictionary<int,GameObject>();
	private Dictionary<int,float> selectCount = new Dictionary<int,float>();
	private Dictionary<int,float> vitesseSelect = new Dictionary<int,float>();
	//public Transform prefabNuke;
	
	//Debug
	private int p = -1;
	private int pe = -1;
	
	// Use this for initialization
	void Start () {
		
		lvl = PlayerPrefs.GetInt("paramLevel");
		
		user = GameObject.FindWithTag("User");
		
		selectSpeed = 0.2f;


	}
	
	void Update() {
		
		//////DEBUUG!!!!!!!!!!!!!!!!/////
				if(Input.GetKeyDown(KeyCode.Keypad0)){	
					if(p == -1){
						p = 0;
						
					}else if(pe == -1){
						pe = 0;
					}
				}
				if(Input.GetKeyDown(KeyCode.Keypad1)){	
					if(p == -1){
						p = 1 ;
						
					}else if(pe == -1){
						pe = 1;
					}
				}
				if(Input.GetKeyDown(KeyCode.Keypad2)){
					if(p == -1){
						p = 2 ;
						
					}else if(pe == -1){
						pe = 2;
					}
					
				}
				if(Input.GetKeyDown(KeyCode.Keypad3)){
					if(p == -1){
						p = 3 ;
						
					}else if(pe == -1){
						pe = 3;
					}
					
				}
				if(Input.GetKeyDown(KeyCode.Keypad4)){
					if(p == -1){
						p = 4 ;
						
					}else if(pe == -1){
						pe = 4;
					}
					
				}
				if(Input.GetKeyDown(KeyCode.Keypad5)){
					if(p == -1){
						p =  5;
						
					}else if(pe == -1){
						pe = 5;
					}
					
				}
				if(Input.GetKeyDown(KeyCode.Keypad6)){
					if(p == -1){
						p =  6;
						
					}else if(pe == -1){
						pe = 6;
					}
					
				}
				if(Input.GetKeyDown(KeyCode.Keypad7)){
					if(p == -1){
						p =  7;
						
					}else if(pe == -1){
						pe = 7;
					}
					
				}
				if(Input.GetKeyDown(KeyCode.Keypad8)){
					if(p == -1){
						p =  8;
						
					}else if(pe == -1){
						pe = 8;
					}
					
				}
				if(Input.GetKeyDown(KeyCode.Keypad9)){
					if(p == -1){
						p =  9;
						
					}else if(pe == -1){
						pe = 9;
					}
					
				}
		
		if(p!= -1 && pe != -1){
			deplacement(GameObject.Find(""+p), GameObject.Find(""+pe), 7);
			GetComponent<GestionLink>().openRoad(GameObject.Find(""+p), GameObject.Find(""+pe));
			p = -1;
			pe = -1;
			
		}

		/////END Debug!!!!!!!!///			
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
				//Pour connaitre la plan�te de d�part, le gameobject est repr�sent� par la variable collider.
				if(touch.phase == TouchPhase.Began) {
					if(!user.GetComponent<PauseScript>().paused2) {
						if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
							if (hit.collider.tag == "planet") {
								Debug.Log ("Planete de d�part" + fingerId);
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
										vitesseSelect.Add(fingerId, selectSpeed);
										
									}else if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="blue" && ((PlanetScript)planetStart.GetComponent<PlanetScript>()).shipsB.Count>0){
										if(PlayerPrefs.GetString("GameType").Equals("versus")) {
											GameObject SelectShip =  Resources.Load("TextSelectBlue")as GameObject;
											Vector3 vec =  planetStart.transform.position;
										
											vec.y = -20.22636f;
										
											GameObject instance = (GameObject) Instantiate(SelectShip,vec, SelectShip.transform.rotation);
											((TextMesh)instance.GetComponent<TextMesh>()).text = ""+0;
											instance.transform.RotateAround(Vector3.up, -1.6f);
											Vector3 vt = instance.transform.position;
											vt.x -=5;
											instance.transform.position = vt;
											
											shipSelect.Add(fingerId,instance);
											selectCount.Add(fingerId,0);
											vitesseSelect.Add(fingerId, selectSpeed);
										}
									}
									
									
								}
							}
						}
					}
				}
				//pour la selection du nombre de vaisseau
				if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
					if(!user.GetComponent<PauseScript>().paused2) {
						if(listPlanetStart.ContainsKey(fingerId)){
							if(Physics.Raycast(cursorRay, out hit, 1000.0f)){
								//Debug.Log ("idSelect : "+fingerId);
								if (hit.collider.tag == "planet" && hit.collider.name == listPlanetStart[fingerId].name ) {
									PlanetScript scriptTemp = listPlanetStart[fingerId].GetComponent<PlanetScript>();
									if(scriptTemp.ship.tag == "red" || scriptTemp.ship.tag == "blue"){
										selectCount[fingerId] += 1*Time.deltaTime;
										if(selectCount[fingerId] >= vitesseSelect[fingerId]){
											selectCount[fingerId] = 0;
											if( vitesseSelect[fingerId] > 0.001){
												vitesseSelect[fingerId] = vitesseSelect[fingerId]-0.01f;
											}
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
				//Pour connaitre la plan�te de d'arriv�e, le gameobject est repr�sent� par la variable collider.
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
													if(PlayerPrefs.GetString("GameType").Equals("versus")) {
														if(user.GetComponent<MoneyScript>().moneyPlayer2 >=50){
															user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
															((GestionLink)GetComponent<GestionLink>()).openRoad(listPlanetStart[fingerId],listPlanetEnd[fingerId]);
															Debug.Log("route ouverte bleu");	
														}else{
															Debug.Log("pas assez d'argent joueur bleu");	
														}
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
													if(PlayerPrefs.GetString("GameType").Equals("versus")) {
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
					if(!user.GetComponent<PauseScript>().paused2) {
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
						if(vitesseSelect.ContainsKey(fingerId)){
							vitesseSelect.Remove(fingerId);	
						}
					
					}
				}
				
			}
		if( verifEndGame()){
			GetComponent<Launch_EndScript>().endGame = true;
			user.GetComponent<stats>().SendData();
			if(bluePlayer) {
				GetComponent<Launch_EndScript>().winner = "blue";
			} else {
				GetComponent<Launch_EndScript>().winner = "red";
			}
		}
		
		
		
	}
	
	/*verifie si un des 2 joueurs a perdue
	@return true si un des 2 joueurs a perdu false sinon */
	bool verifEndGame(){
		
		GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");
		redPlayer = false;
		bluePlayer = false;
		
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
	
	//ajoute les vaisseaux au tableau de la planete d'arriv�
	void valideDeplacement(GameObject shipT){
		
		if(shipT != null){
			
			((rotationShip)shipT.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
			if(shipT.tag == "red"){
				((PlanetScript)shipT.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>()).shipsR.Add(shipT);
			}else{
			
				((PlanetScript)shipT.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>()).shipsB.Add(shipT);
			}
			
			//shipT.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>().refreshShip();
		}else{
			//Debug.Log("erreu : "+i);	
		}

	}
	
	//ajoute les  gros vaisseaux au tableau de la planete d'arriv�
	void valideDeplacementHugeShip(GameObject shipG){
		
		List<GameObject> ships = shipG.GetComponent<rotationShip>().ships;
		
		if(ships[0].tag == "red"){	
			shipG.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>().shipsRS.Add(shipG);
		}else{
			shipG.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>().shipsBS.Add(shipG);
		}
		int count = ships.Count;
		for(int i  = 0 ; i<count; i++){
			if(ships[0] != null){
				ships[0].transform.position = shipG.transform.position;
				ships[0].GetComponent<rotationShip>().planet = shipG.GetComponent<rotationShip>().planet; 
				if(ships[0].tag == "red"){
					shipG.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>().shipsR.Add(ships[0]);
				}else{
					shipG.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>().shipsB.Add(ships[0]);
				}
				//shipG.GetComponent<rotationShip>().planet.GetComponent<PlanetScript>().refreshShip();
	
			}
			
			ships.RemoveAt(0);
		}
		
		
	}
		
	//deplace les vaisseaux d'une planete a l'autre
	public void deplacement(GameObject start, GameObject end, int nbShip){
		infoUser info;
		List<GameObject> ships = new List<GameObject>();
		//List<GameObject> shipsG = new List<GameObject>();
		PlanetScript p = (PlanetScript)start.GetComponent<PlanetScript>();
		
		
		int nbs = nbShip ;
		
		if(p.ship.tag == "red"){
			info = (infoUser) GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>();
			if(p.shipsR.Count < nbShip){
				nbs=p.shipsR.Count;	
			}
		}else{
			info = (infoUser) GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>();
			if(p.shipsB.Count < nbShip){
				nbs=p.shipsB.Count;	
			}
		}
		
		
		if(end.tag == "planet") {
			
			float scal = end.transform.localScale.x ;		
			float min =  scal/2.5f+1   ;
			float max = scal/2.5f +1.5f;
			
			int t = 1;
			int c = 1;

			if(p.ship != null){
			
				
				for(int j = nbs-1 ; j>=0; j--){
					
					float z = Random.Range(min,max);
				
					Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), end.transform.position);
					
					Vector3 vec = new Vector3(0,0,z);
					vec = quat * vec ;
					vec.y = 0;
					
					if(p.ship.tag == "red"){
						((rotationShip)p.shipsR[0].GetComponent<rotationShip>()).speed = 0;
						((rotationShip)p.shipsR[0].GetComponent<rotationShip>()).planet = end;
					}else{
						((rotationShip)p.shipsB[0].GetComponent<rotationShip>()).speed = 0;
						((rotationShip)p.shipsB[0].GetComponent<rotationShip>()).planet = end;
					}
					
					
					if(nbs>= 10*t){
					
						if(p.ship.tag == "red"){
							p.shipsRS[0].GetComponent<rotationShip>().ships.Add(p.shipsR[0]);
							p.shipsR.RemoveAt(0);
						}else{
							p.shipsBS[0].GetComponent<rotationShip>().ships.Add(p.shipsB[0]);
							p.shipsB.RemoveAt(0);
						}
						
					}else{
						if(p.ship.tag == "red"){
							p.shipsR[0].GetComponent<MeshRenderer>().enabled = true;
							ships.Add(p.shipsR[0]);
							iTween.MoveTo(p.shipsR[0],iTween.Hash("position",end.transform.position+vec,"time",info.speedShip,"oncomplete","valideDeplacement","onCompleteTarget", gameObject,"oncompleteparams", p.shipsR[0], "easetype", "linear"));	
							p.shipsR.RemoveAt(0);
						}else{
							p.shipsB[0].GetComponent<MeshRenderer>().enabled = true;
							ships.Add(p.shipsB[0]);
							iTween.MoveTo(p.shipsB[0],iTween.Hash("position",end.transform.position+vec,"time",info.speedShip,"oncomplete","valideDeplacement","onCompleteTarget", gameObject,"oncompleteparams", p.shipsB[0], "easetype", "linear"));	
							p.shipsB.RemoveAt(0);
						}
						
					}
					
					if(c == 10*t){
						if(p.ship.tag == "red"){
							p.shipsRS[0].GetComponent<rotationShip>().planet = end;
							//p.shipsRS[0].GetComponent<rotationShip>().ships.Add(end);
							ships.Add(p.shipsRS[0]);
							iTween.MoveTo(p.shipsRS[0],iTween.Hash("position",end.transform.position+vec,"time",info.speedShip,"oncomplete","valideDeplacementHugeShip","onCompleteTarget", gameObject,"oncompleteparams", p.shipsRS[0] , "easetype", "linear"));
							p.shipsRS.RemoveAt(0);
							
						}else{
							p.shipsBS[0].GetComponent<rotationShip>().planet = end;
							//p.shipsBS[0].GetComponent<rotationShip>().ships.Add(end);
							ships.Add(p.shipsBS[0]);
							iTween.MoveTo(p.shipsBS[0],iTween.Hash("position",end.transform.position+vec,"time",info.speedShip,"oncomplete","valideDeplacementHugeShip","onCompleteTarget", gameObject,"oncompleteparams", p.shipsBS[0] , "easetype", "linear"));
							p.shipsBS.RemoveAt(0);
						}
						t++;
					}
					
					c++;
						
										
				}
				GetComponent<GestionLink>().activeAsteroid(start,end, ships);
				int[] links = GetComponent<GestionLink>().nbRoad();
				user.GetComponent<MoneyScript>().incomePlayer1 = 1 + links[0];
				user.GetComponent<MoneyScript>().incomePlayer2 = 1 + links[1];
				
			
			}else{
				Debug.Log("null");	
			}
		} else if (end.tag == "BlackHole") {
			if(p.ship != null){
				if((user.GetComponent<SpaceBridge>().planet1 == start && end.name == "BlackHole1")
					||(user.GetComponent<SpaceBridge>().planet2 == start && end.name == "BlackHole2")){
					for(int j = 0 ; j<nbs; j++){
						
						if(p.ship.tag == "red"){
						
							ships.Add(p.shipsR[0]);
							p.shipsR.RemoveAt(0);
							
						}else{
						
							ships.Add(p.shipsB[0]);
							p.shipsB.RemoveAt(0);
						
						}
						GameObject[] param = new GameObject[2];
						param[0] = ships[j];
						param[1] = end;
	
						iTween.MoveTo(ships[j],iTween.Hash("position",end.transform.position,"time",info.speedShip,"oncomplete","teleport","onCompleteTarget", gameObject,"oncompleteparams", param, "easetype", "linear"));			
					}
				}
			}
		}
	
		start.GetComponent<PlanetScript>().playSound();
		//p.refreshShip();
		
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
