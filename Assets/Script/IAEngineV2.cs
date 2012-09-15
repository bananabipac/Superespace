using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAEngineV2 : MonoBehaviour {

	private GameObject user;
	private GameObject[] planets;
	private List<GameObject> planetsIA = new List<GameObject>();
	private float timer;
	private List<GameObject[]> pairs = new List<GameObject[]>();
	public float speedIA;
	private bool hasEnoughMoney;
	private bool launchMove;
	public float marge;
	GameObject[] pair = new GameObject[2];
	private int nbShipToLaunch;
	public string IAPlayer;
	public float vitesseSelect;
	
	private List<GameObject> listLines = new List<GameObject>();
	private List<GameObject> listPlanetStart = new List<GameObject>();
	private List<GameObject> listPlanetEnd = new List<GameObject>();
	private List<GameObject> GuiSelect = new List<GameObject>();
	
	private List<int> Count = new List<int>();
	private List<int> CountTmp = new List<int>();
	private List<float> speed = new List<float>();
	private List<float> speedTmp = new List<float>();
	
	//variable pour les pouvoire
	private GameObject power;
	private GameObject plaE;
	private bool powerLaunch ; 
	
	
	//valeur de ponderation
	public int valeurBase ; //valeur de base
	public int siAsteroid ; //si des asteroides sont sur la route
	public int siPlanetIA ;//si la planete d'arrivé est a l'IA
	public int siPlanetIAPasAssezShip ;//si la planete d'arrivé est a l'IA
	public int siPlanetPasIAAssezShip ; //si la planete n'est pas a l'IA et assez de vaisseaux
	public int siPlanetPasIAPasAssezShip ; // si planete pas a l'IA et pas assez de vaisseaux
	public int siCaptureEnCourIA ; // si la planete est en cours de capture par l'IA mais moins de 10 vaisseaux
	public int siCaptureEnCourAD; // si la planete est en cours de capture par l'adversaire 
	public int siTrouNoir; //si le trou noire est ouvert et nb vaisseaux requis - 20%
	public int siPasAssezVaisseaux; //si pas assez de vaisseaux ?????
	public int siCombatPlanetAD; //si la planete d'arrivée est a l'adversaire en combat + pas assez vaisseaux 
	public int siCombatPlaneteIA; //si la planete d'arrivée est a L'IA en combat + pas assez de vaisseaux
	public int notEnoughMoney; //si l'IA n'a pas assez d'argent pour ouvrir la route
	public int trouNoir; //si le trou noir est ouvert et assez de vaisseaux 
	public int chanceUsePower;
	public int chanceUpgrade;
	public bool stop ;
	private int choseUpgrade;
	
	public bool upgrade;
	private GUIPlayers script;
	private Dictionary<string,int> ponderation = new Dictionary<string, int>();
	
	// Use this for initialization
	void Start () {
		stop = false;
		power = null;
		powerLaunch = false;
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			user = GameObject.FindGameObjectWithTag("User");
			launchMove = false;
			timer = Time.timeSinceLevelLoad;
			script = user.GetComponent<GUIPlayers>();
		}
		upgrade = false;
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Space)){	
			movePower("nuke", GameObject.Find("0"));
			//user.GetComponent<stats>().SendData();
		}
		
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			stop = false;
			if(Count.Count >0){
				stop = true;
			}
			
			if(powerLaunch){
				stop = true;	
			}
			/*if(listLines.Count >0){
				stop = true;
			}*/
			if(!stop){ 
				if(Time.timeSinceLevelLoad - timer >= speedIA) {
					if(!upgrade){
						if(IAPlayer =="red" && (script.lvlAttack1 != 3 || script.lvlLife1 != 3 || script.lvlSpeed1 != 3)){
							int rand = Random.Range(0, 101);
							if(rand <= chanceUpgrade){
								upgrade = true;	
								bool t = false;
								while(!t){
									choseUpgrade = Random.Range(1,4);
									if(choseUpgrade == 1 && script.lvlAttack1<3){
										t=true;		
									}else if(choseUpgrade == 2 && script.lvlLife1<3){
										t=true;	
									}else if(choseUpgrade == 3 && script.lvlSpeed1<3){
										t=true;	
									}
								}
							}
						}else if(IAPlayer == "blue" && (script.lvlAttack2 != 3 || script.lvlLife2 != 3 || script.lvlSpeed2 != 3) ){
							int rand = Random.Range(0, 101);
							if(rand <= chanceUpgrade){
								upgrade = true;	
								bool t = false;
								while(!t){
									choseUpgrade = Random.Range(1,4);
									if(choseUpgrade == 1 && script.lvlAttack2<3){
										t=true;		
									}else if(choseUpgrade == 2 && script.lvlLife2<3){
										t=true;	
									}else if(choseUpgrade == 3 && script.lvlSpeed2<3){
										t=true;	
									}
								}
							}
							
						}
						
					}
					
					
					
					checkPlanets();
					findPossibleRoutes();
					calculatePonderation();
					
					
					if(upgrade){
						
						if(choseUpgrade == 2){
							if(IAPlayer == "red"){
								if( 100 * (script.lvlLife1+1) <= user.GetComponent<MoneyScript>().moneyPlayer1){
									(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).lifeShip += 2;
									user.GetComponent<MoneyScript>().moneyPlayer1 -= (100 * (script.lvlLife1+1));
									script.lvlLife1++;
									upgrade = false;
									GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeRed")as GameObject);
									Vector3 vec = GameObject.Find("vie1").transform.position;
									vec.x -= 1.5f;
									vec.z -= 1.4f;
									upgradeFeedBack.transform.position = vec;
								}
							}else{
								if( 100 * (script.lvlLife2+1) <= user.GetComponent<MoneyScript>().moneyPlayer2){
									(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).lifeShip += 2;
									user.GetComponent<MoneyScript>().moneyPlayer2 -= (100 * (script.lvlLife2+1));
									script.lvlLife2++;
									upgrade = false;
									GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeBlue")as GameObject);
									Vector3 vec = GameObject.Find("vie2").transform.position;
									vec.x += 1.5f;
									vec.z += 1.4f;
									upgradeFeedBack.transform.position = vec;
								}
							}
						}else if(choseUpgrade == 1){
							if(IAPlayer == "red"){
								if( 100 * (script.lvlAttack1+1) <= user.GetComponent<MoneyScript>().moneyPlayer1) {
									(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMin += 2;
									(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMax += 2;
									user.GetComponent<MoneyScript>().moneyPlayer1 -= (100 * (script.lvlAttack1+1));
									script.lvlAttack1++;
									upgrade = false;
									GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeRed")as GameObject);
									Vector3 vec = GameObject.Find("attaque1").transform.position;
									vec.x -= 1.5f;
									vec.z -= 1.4f;
									upgradeFeedBack.transform.position = vec;
									
								}
							}else{
								if( 100 * (script.lvlAttack2+1) <= user.GetComponent<MoneyScript>().moneyPlayer2) {
									(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).powerMin += 2;
									(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).powerMax += 2;
									user.GetComponent<MoneyScript>().moneyPlayer2 -= (100 * (script.lvlAttack2+1));
									script.lvlAttack2++;
									upgrade = false;
									GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeBlue")as GameObject);
									Vector3 vec = GameObject.Find("attaque2").transform.position;
									vec.x += 1.5f;
									vec.z += 1.4f;
									upgradeFeedBack.transform.position = vec;
									
								}
							}
							
						
						}else if(choseUpgrade ==3){
							if(IAPlayer=="red") {
								if( 100 * (script.lvlSpeed1+1) <= user.GetComponent<MoneyScript>().moneyPlayer1) {
									(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).speedShip -= 0.2f;
									user.GetComponent<MoneyScript>().moneyPlayer1 -= (100 * (script.lvlSpeed1+1));
									script.lvlSpeed1++;
									upgrade = false;
									GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeRed")as GameObject);
									Vector3 vec = GameObject.Find("vitesse1").transform.position;
									vec.x -= 1.5f;
									vec.z -= 1.4f;
									upgradeFeedBack.transform.position = vec;
								}
							}else{
								if( 100 * (script.lvlSpeed2+1) <= user.GetComponent<MoneyScript>().moneyPlayer2) {
									(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).speedShip -= 0.2f;
									user.GetComponent<MoneyScript>().moneyPlayer2 -= (100 * (script.lvlSpeed2+1));
									script.lvlSpeed2++;
									upgrade = false;
									GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeBlue")as GameObject);
									Vector3 vec = GameObject.Find("vitesse2").transform.position;
									vec.x += 1.5f;
									vec.z += 1.4f;
									upgradeFeedBack.transform.position = vec;
								}
							}
						}
							
					}
					
					if(!powerLaunch){
						chooseRoad();
					}
					
					reinitVar();
					
					
					timer = Time.timeSinceLevelLoad;
				}
			}else{
				
				for(int i =0; i<Count.Count; i++){
					if(CountTmp[i] < Count[i]){
						speedTmp[i] += 1*Time.deltaTime;
						if(speedTmp[i] > speed[i]){
							if( speed[i] > 0.001){
								speed[i] = speed[i]-0.01f;
							}
							speedTmp[i] = 0;
							
							CountTmp[i] ++;
							GuiSelect[i].GetComponent<TextMesh>().text = ""+CountTmp[i];
							if(IAPlayer == "red"){
								if(CountTmp[i] >= Count[i] || CountTmp[i] >= listPlanetStart[i].GetComponent<PlanetScript>().shipsR.Count){
								
									iTween.ValueTo(gameObject,iTween.Hash("from",listPlanetStart[i].transform.position,"to",listPlanetEnd[i].transform.position,"time", vitesseSelect,"onupdate","rendere","oncomplete","delacementFinish","oncompleteparams",i,"easetype","linear"));
									//iTween.MoveTo((GameObject)listMove[i],iTween.Hash("position",listPlanetEnd[i],"time",5f,"oncomplete","delacementFinish","onCompleteTarget", gameObject,"oncompleteparams",i, "easetype", "linear"));
								}	
							}else{
								if(CountTmp[i] >= Count[i] || CountTmp[i] >= listPlanetStart[i].GetComponent<PlanetScript>().shipsB.Count){
									
									iTween.ValueTo(gameObject,iTween.Hash("from",listPlanetStart[i].transform.position,"to",listPlanetEnd[i].transform.position,"time", vitesseSelect,"onupdate","rendere","oncomplete","delacementFinish","oncompleteparams",i,"easetype","linear"));
									//iTween.MoveTo((GameObject)listMove[i],iTween.Hash("position",listPlanetEnd[i],"time",5f,"oncomplete","delacementFinish","onCompleteTarget", gameObject,"oncompleteparams",i, "easetype", "linear"));
								}	
							}
						}
					}	
				}
			}
		}
	}
	void reinitVar() {
		pairs = new List<GameObject[]>();
		planetsIA = new List<GameObject>();
		ponderation.Clear();
		
	}
	
	void rendere(Vector3 coord){
		////Debug.Log(i);
		if(listLines.Count > 0){
			listLines[0].GetComponent<LineRenderer>().SetPosition(1, coord);
		}
		
	}
	void delacementFinish(int i){
		//int i = 0;
		////Debug.Log(i);
		if(i<listPlanetStart.Count && i<listPlanetEnd.Count){
			if(user.GetComponent<GestionLink>().roadExist(listPlanetStart[i], listPlanetEnd[i]) && !user.GetComponent<GestionLink>().roadOpen(listPlanetStart[i], listPlanetEnd[i])){
				user.GetComponent<GestionLink>().openRoad(listPlanetStart[i],listPlanetEnd[i]);
				if(IAPlayer =="red"){
					user.GetComponent<MoneyScript>().moneyPlayer1 -= 50;
				}else{
					user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
				}
			}
			
			user.GetComponent<moveShip>().deplacement(listPlanetStart[i],listPlanetEnd[i],Count[i]);
			listPlanetStart.RemoveAt(i);
			listPlanetEnd.RemoveAt(i);
			
			GameObject tmp = listLines[i];
			Destroy(tmp);
			listLines.RemoveAt(i);
			
			tmp = GuiSelect[i];
			Destroy(tmp);
			GuiSelect.RemoveAt(i);
			
			Count.RemoveAt(i);
			CountTmp.RemoveAt(i);
			speed.RemoveAt(i);
			speedTmp.RemoveAt(i);
		}
	
	}
	void checkPlanets() {
		planets = GameObject.FindGameObjectsWithTag("planet");
		int indice = 0;
		for(int i = 0; i < planets.Length; i++){
			GameObject planet = planets[i];
			if(planet.GetComponent<PlanetScript>().ship.tag == IAPlayer) {
				////Debug.Log(planet.tag);
				planetsIA.Add(planet);
				indice++;
				////Debug.Log("planete IA: "+planet.name);
			}
		}	

	}
	/// <summary>
	/// Finds the possible routes.
	/// </summary>
	void findPossibleRoutes() {
		SpaceBridge bridge = user.GetComponent<SpaceBridge>();
		if(bridge.bridgeOpen){
			if(IAPlayer == "red"){
				if(bridge.planet1.GetComponent<PlanetScript>().ship.tag == "red"){
					pairs.Add(new GameObject[]{bridge.planet1,bridge.planet2});
				}
				
				if(bridge.planet2.GetComponent<PlanetScript>().ship.tag == "red"){
					pairs.Add(new GameObject[]{bridge.planet2,bridge.planet1});
				}
			}
		}
		for(int i = 0; i < planetsIA.Count; i++){
			GameObject planetD = planetsIA[i];
			for(int j = 0; j < planets.Length; j++) {
				GameObject planetE = planets[j];
				if(user.GetComponent<GestionLink>().roadExist(planetD,planetE)) {
					if(planetE != planetD){
						pairs.Add(new GameObject[] {planetD,planetE});
					}
				}
			}

		}
		
	}
	/// <summary>
	/// Calculates the ponderation.
	/// </summary>
	void calculatePonderation() {
		for(int i = 0; i < pairs.Count; i++) {
			
			bool ast = false;
			bool sabo = false;
			bool nuke = false;
			bool crash = false;
			int nbShip = 0;
			
			int pond = valeurBase ; 
			string ps,pe;
			GameObject planetStart = pairs[i][0];
			GameObject planetEnd = pairs[i][1];
			PlanetScript scriptS = planetStart.GetComponent<PlanetScript>();
			PlanetScript scriptE = planetEnd.GetComponent<PlanetScript>();
			
			if(int.Parse(planetStart.name) > int.Parse(planetEnd.name)){
				ps = planetEnd.name;
				pe = planetStart.name;
			}else{
				pe = planetEnd.name;
				ps = planetStart.name;
			}
			if(GameObject.Find("a"+ps+pe) != null){//si des asteroides sont sur la route
				pond += siAsteroid;
				ast = true;
				////Debug.Log("Asteroid");
				
			}
			
			if((planetStart.name == user.GetComponent<SpaceBridge>().planet1.name || planetStart.name == user.GetComponent<SpaceBridge>().planet2.name) && (planetEnd.name == user.GetComponent<SpaceBridge>().planet1.name || planetEnd.name == user.GetComponent<SpaceBridge>().planet2.name)){
				pond += trouNoir;
			}
			
			if(scriptE.ship.tag != IAPlayer){//si la planete n'est pas a l'IA
				if(IAPlayer == "red"){
					if( scriptS.shipsR.Count>(10+(10*marge)/100) && scriptE.shipsR.Count>0 && scriptE.shipsR.Count<=10 && scriptE.shipsB.Count==0 && scriptE.shipsN.Count == 0){//capture en cours
						pond += siCaptureEnCourIA;
						nbShip +=Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsR.Count);
						////Debug.Log("CaptureEnCourIA");
					}else{
						if(scriptE.shipsR.Count>0 && (scriptE.shipsB.Count>0 || scriptE.shipsN.Count > 0)){//combat en cour
							if(scriptE.shipsB.Count> scriptE.shipsR.Count ){
								pond += siCombatPlanetAD;
								////Debug.Log("CombatPlanetAD");
								
								
								if(user.GetComponent<MoneyScript>().moneyPlayer1 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){//L'IA Lance sabotage
									////Debug.Log("sabotage");
									movePower("sabotage", planetEnd);
								}else{//l'IA n'a pas Assez d'argent
									nbShip += Mathf.FloorToInt((scriptE.shipsB.Count + (scriptE.shipsB.Count*marge)/100)- scriptE.shipsR.Count);
								}
							
							}else if(scriptE.shipsN.Count> scriptE.shipsR.Count ){
								pond += siCombatPlanetAD;
								////Debug.Log("CombatPlanetAD");
								
								
								if(user.GetComponent<MoneyScript>().moneyPlayer1 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){//L'IA lance sabotage
									////Debug.Log("sabotage")
									movePower("sabotage", planetEnd);
								}else{//l'IA n'a pas assez d'argent
									nbShip += Mathf.FloorToInt((scriptE.shipsN.Count + (scriptE.shipsN.Count*marge)/100)- scriptE.shipsR.Count);
								}
								
							}
						}else if(scriptE.shipsR.Count==0 && (scriptE.shipsB.Count>0 || scriptE.shipsN.Count > 0)){// la planete est a l'adversaire
							if(scriptE.shipsB.Count > 0 && scriptE.shipsN.Count == 0){//planete bleu
								if(scriptS.shipsR.Count >= scriptE.shipsB.Count - scriptE.shipsB.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									////Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsB.Count + scriptE.shipsB.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									////Debug.Log("PlanetPasIAPasAssezShip");
									
									if(user.GetComponent<MoneyScript>().moneyPlayer1 >=800 && !powerLaunch && !upgrade){//L'IA lance nuke
										////Debug.Log("nuke");
										movePower("nuke", planetEnd);
									}else if(user.GetComponent<MoneyScript>().moneyPlayer1 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){
										//movePower("crash", planetEnd);
											if(scriptE.shipsB.Count <= 10){
												////Debug.Log("crash");
												movePower("crash", planetEnd);
											}else{
												////Debug.Log("sabotage");
												movePower("sabotage", planetEnd);
											}
										
										
									}
									
								}
								
							}else if(scriptE.shipsN.Count > 0 && scriptE.shipsB.Count == 0){//planete neutre
								if(scriptS.shipsR.Count >= scriptE.shipsN.Count - scriptE.shipsN.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									////Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsN.Count +scriptE.shipsN.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									////Debug.Log("PlanetPasIAPasAssezShip");
									if(user.GetComponent<MoneyScript>().moneyPlayer1 >=800 && !powerLaunch  && !upgrade){//L'IA lance nuke
										////Debug.Log("nuke");
										movePower("nuke", planetEnd);
									}else if(user.GetComponent<MoneyScript>().moneyPlayer1 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){
										////Debug.Log("sabotage");
										movePower("sabotage", planetEnd);
									}
								}
								
								
							}
						}
						
					}
					
				}else{
					if( scriptS.shipsB.Count>(10+(10*marge)/100) && scriptE.shipsB.Count>0 && scriptE.shipsB.Count<=10 && scriptE.shipsR.Count==0 && scriptE.shipsN.Count == 0){//capture en cours
						pond += siCaptureEnCourIA;
						nbShip += Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsB.Count  );
						////Debug.Log("CaptureEnCourIA");
						
					}else{
						if(scriptE.shipsB.Count>0 && (scriptE.shipsR.Count>0 || scriptE.shipsN.Count > 0)){//combat en cour
							if(scriptE.shipsR.Count> scriptE.shipsB.Count ){
								pond += siCombatPlanetAD;
								////Debug.Log("CombatPlanetAD");
								if(user.GetComponent<MoneyScript>().moneyPlayer2 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){//L'IA lance sabotage
									////Debug.Log("sabotage");
									movePower("sabotage", planetEnd);
								}else{//l'IA n'a pas assez d'argent
									nbShip += Mathf.FloorToInt((scriptE.shipsR.Count + (scriptE.shipsR.Count*marge)/100)- scriptE.shipsB.Count);	
								}
									
							}else if(scriptE.shipsN.Count> scriptE.shipsB.Count ){
								pond += siCombatPlanetAD;
								////Debug.Log("CombatPlanetAD");
								if(user.GetComponent<MoneyScript>().moneyPlayer2 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){//L'IA lance sabotage
									////Debug.Log("sabotage");
									movePower("sabotage", planetEnd);
								}else{
									nbShip += Mathf.FloorToInt((scriptE.shipsN.Count + (scriptE.shipsN.Count*marge)/100)- scriptE.shipsB.Count);	
								}
							}
						}else if(scriptE.shipsB.Count==0 && (scriptE.shipsR.Count>0 || scriptE.shipsN.Count > 0)){// la planete est a l'adversaire
							if(scriptE.shipsR.Count > 0 && scriptE.shipsN.Count == 0){//planete bleu
								if(scriptS.shipsB.Count >= scriptE.shipsR.Count - scriptE.shipsR.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									////Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsR.Count + scriptE.shipsR.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									////Debug.Log("PlanetPasIAPasAssezShip");
									if(user.GetComponent<MoneyScript>().moneyPlayer2 >=800 && !powerLaunch && !upgrade){//L'IA lance nuke
										////Debug.Log("nuke");
										movePower("nuke", planetEnd);
									}else if(user.GetComponent<MoneyScript>().moneyPlayer2 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){
										//movePower("crash", planetEnd);
										
										if(scriptE.shipsR.Count <= 10){
											////Debug.Log("crash");
											movePower("crash", planetEnd);
										}else{
											////Debug.Log("sabotage");
											movePower("sabotage", planetEnd);
										}
										
									}
								}
								
							}else if(scriptE.shipsN.Count > 0 && scriptE.shipsR.Count == 0){//planete neutre
								if(scriptS.shipsB.Count >= scriptE.shipsB.Count - scriptE.shipsN.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									////Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsN.Count + scriptE.shipsN.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									////Debug.Log("PlanetPasIAPasAssezShip");
									if(user.GetComponent<MoneyScript>().moneyPlayer2 >=800 && !powerLaunch && !upgrade ){//L'IA lance nuke
										////Debug.Log("nuke");
										movePower("nuke", planetEnd);
									}else if(user.GetComponent<MoneyScript>().moneyPlayer2 >=200 && !powerLaunch && !upgrade && Random.Range(0,101) <= chanceUsePower){
										////Debug.Log("sabotage");
										movePower("sabotage", planetEnd);
										
									}
								}
								
								
							}
						}
						
					}
				}
			}else{//si la planete est a l'IA
				pond += siPlanetIA;
			////Debug.Log("PlanetIA");
				if(IAPlayer =="red"){
					
					if(scriptE.shipsB.Count > 0){//la planete est attaqué	
						if(scriptE.shipsR.Count >0){//perte du combat
							if(scriptE.shipsB.Count>scriptE.shipsR.Count){
								pond += siCombatPlaneteIA;
								////Debug.Log("CombatPlaneteIA");
								
								nbShip += Mathf.FloorToInt((scriptE.shipsB.Count + (scriptE.shipsB.Count*marge)/100)- scriptE.shipsR.Count);	
								
							}
						}else{//la planete est en cours de capture par l'ennemie
							pond += siCaptureEnCourAD;
							////Debug.Log("CaptureEnCourAD");
							nbShip += Mathf.FloorToInt(scriptE.shipsB.Count + (scriptE.shipsB.Count*marge)/100);
						}
					}else{
						if(scriptE.shipsR.Count <= 10){
							pond += siPlanetIAPasAssezShip;
							////Debug.Log("PlanetIAPasAssezShip");
							nbShip += Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsR.Count  );
							
						}
						
					}
					
				}else{
					if(scriptE.shipsR.Count > 0){//la planete est attaqué	
						if(scriptE.shipsB.Count >0){//perte du combat
							if(scriptE.shipsR.Count>scriptE.shipsB.Count){
								pond += siCombatPlaneteIA;
								////Debug.Log("CombatPlaneteIA");
								nbShip += Mathf.FloorToInt((scriptE.shipsR.Count + (scriptE.shipsR.Count*marge)/100)- scriptE.shipsB.Count);
							}
						}else{//la planete est en cours de capture par l'ennemie
							pond += siCaptureEnCourAD;
							////Debug.Log("CaptureEnCourAD");
							nbShip += Mathf.FloorToInt(scriptE.shipsR.Count + (scriptE.shipsR.Count*marge)/100);
						}
					}else{
						if(scriptE.shipsB.Count <= 10){
							pond += siPlanetIAPasAssezShip;
							////Debug.Log("PlanetIAPasAssezShip");
							
							nbShip += Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsB.Count);
							////Debug.Log(nbShip);
							
						}
						
					}
					
				}
					
				
			}
			
			if(ast){
				nbShip += Mathf.FloorToInt((nbShip * marge) /100);
			}
			
			if(user.GetComponent<GestionLink>().roadExist(planetStart, planetEnd) && !user.GetComponent<GestionLink>().roadOpen(planetStart, planetEnd)){
				if(IAPlayer == "red"){
					if(user.GetComponent<MoneyScript>().moneyPlayer1 <50){
						pond += notEnoughMoney;	
						////Debug.Log("NotEnoughMOney");
					}
				}else{
					if(user.GetComponent<MoneyScript>().moneyPlayer2 <50){
						pond += notEnoughMoney;	
						////Debug.Log("NotEnoughMOney");
					}
					
				}
			}
			
			if(IAPlayer == "red"){
				if(scriptS.shipsR.Count <nbShip){
					nbShip = scriptS.shipsR.Count;	
				}
			}else{
				if(scriptS.shipsB.Count <nbShip){
					nbShip = scriptS.shipsB.Count;	
				}
			}
			ponderation.Add(""+planetStart.name+planetEnd.name+"-"+nbShip, pond);
			
			////Debug.Log(""+planetStart.name+planetEnd.name+" pond : "+pond);
		}
		
	}
	
	void movePower(string selec, GameObject planetE){
		////Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
		powerLaunch = true;
		if(selec == "crash"){
			if(IAPlayer == "red"){
				power = user.GetComponent<GUIPlayers>().crash1;
			}else{
				power = user.GetComponent<GUIPlayers>().crash2;
			}
		}else if(selec == "sabotage"){
			if(IAPlayer == "red"){
				power = user.GetComponent<GUIPlayers>().sabotage1;
			}else{
				power = user.GetComponent<GUIPlayers>().sabotage2;
			}
		}else if(selec == "nuke"){
			if(IAPlayer == "red"){
				power = user.GetComponent<GUIPlayers>().nuke1;
			}else{
				power = user.GetComponent<GUIPlayers>().nuke2;
			}
		}
		
		plaE = planetE;
		//iTween.MoveTo(gameObject,iTween.Hash("from",power,"to",planetE.transform.position,"time", vitesseSelect,"oncomplete","delacementFinishPower","oncompleteparams",selec,"easetype","linear"));
		iTween.MoveTo(power,iTween.Hash("position",planetE.transform.position,"time",vitesseSelect,"oncomplete","delacementFinishPower","onCompleteTarget", gameObject,"oncompleteparams",selec, "easetype", "linear"));
		
	}
	
	void delacementFinishPower(string powerSelect){
		GUIPlayers info = user.GetComponent<GUIPlayers>();
		if(powerSelect == "crash"){
			plaE.GetComponent<PlanetScript>().repop += info.crash;
			GameObject expl = (GameObject)Instantiate(Resources.Load("crash")as GameObject);
			expl.transform.position = plaE.transform.position;
			if(IAPlayer == "red"){
				user.GetComponent<MoneyScript>().moneyPlayer1-= info.crashPrice;
				power.transform.position = info.posCrash1;
				user.GetComponent<stats>().nbCrashRed ++;
			}else{
				user.GetComponent<MoneyScript>().moneyPlayer2-= info.crashPrice;
				power.transform.position = info.posCrash2;
				user.GetComponent<stats>().nbCrashBlue ++;
			}
		}else if(powerSelect =="sabotage"){
			if(plaE.GetComponent<PlanetScript>().ship.tag =="neutre"){
				int deleteShip = (int)Mathf.Floor(plaE.GetComponent<PlanetScript>().shipsN.Count*info.sabotage/100);
				for(int ships = 0; ships < deleteShip; ships++) {
					GameObject temp = plaE.GetComponent<PlanetScript>().shipsN[0];
					plaE.GetComponent<PlanetScript>().shipsN.RemoveAt(0);
					GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
					expl.transform.position = temp.transform.position;
					Destroy (temp);
					
					user.GetComponent<stats>().destroyShipNPower ++;
					
				}
			}else if(plaE.GetComponent<PlanetScript>().ship.tag =="red"){
				int deleteShip = (int)Mathf.Floor(plaE.GetComponent<PlanetScript>().shipsR.Count*info.sabotage/100);
				//Debug.Log("DELETESHIP :"+deleteShip);
				for(int ships = 0; ships < deleteShip; ships++) {
					GameObject temp = plaE.GetComponent<PlanetScript>().shipsR[0];
					plaE.GetComponent<PlanetScript>().shipsR.RemoveAt(0);
					GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
					expl.transform.position = temp.transform.position;
					user.GetComponent<stats>().destroyShipRPower ++;
					Destroy (temp);
					
				}
			}else if(plaE.GetComponent<PlanetScript>().ship.tag =="blue"){
				int deleteShip = (int)Mathf.Floor(plaE.GetComponent<PlanetScript>().shipsB.Count*info.sabotage/100);
				for(int ships = 0; ships < deleteShip; ships++) {
					GameObject temp = plaE.GetComponent<PlanetScript>().shipsB[0];
					plaE.GetComponent<PlanetScript>().shipsB.RemoveAt(0);
					GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
					expl.transform.position = temp.transform.position;
					user.GetComponent<stats>().destroyShipBPower ++;
					Destroy (temp);
				}
			}
			
			if(IAPlayer == "red"){
				user.GetComponent<MoneyScript>().moneyPlayer1-= info.sabotagePrice;
				power.transform.position = info.posSabotage1;
				user.GetComponent<stats>().nbSabotageRed ++;
			}else{
				user.GetComponent<MoneyScript>().moneyPlayer2-= info.sabotagePrice;	
				power.transform.position = info.posSabotage2;
				user.GetComponent<stats>().nbSabotageBlue ++;
			}
			
		}else if(powerSelect =="nuke"){
			if(plaE.GetComponent<PlanetScript>().ship.tag == "neutre"){
				int deleteShip = plaE.GetComponent<PlanetScript>().shipsN.Count;
				GameObject apo = (GameObject)Instantiate(Resources.Load("apocalypse")as GameObject);
				apo.transform.position = plaE.transform.position;
				for(int ships = 0; ships < deleteShip; ships++) {
					GameObject temp = plaE.GetComponent<PlanetScript>().shipsN[0];
					plaE.GetComponent<PlanetScript>().shipsN.RemoveAt(0);
					GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
					expl.transform.position = temp.transform.position;
					user.GetComponent<stats>().destroyShipNPower ++;
					Destroy (temp);
				}
			}else if(plaE.GetComponent<PlanetScript>().ship.tag == "red"){
				int deleteShip = plaE.GetComponent<PlanetScript>().shipsR.Count;
				GameObject apo = (GameObject)Instantiate(Resources.Load("apocalypse")as GameObject);
				apo.transform.position = plaE.transform.position;
				for(int ships = 0; ships < deleteShip; ships++) {
					GameObject temp = plaE.GetComponent<PlanetScript>().shipsR[0];
					plaE.GetComponent<PlanetScript>().shipsR.RemoveAt(0);
					GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
					expl.transform.position = temp.transform.position;
					user.GetComponent<stats>().destroyShipRPower ++;
					Destroy (temp);
				}
			}else if(plaE.GetComponent<PlanetScript>().ship.tag == "blue"){
				int deleteShip = plaE.GetComponent<PlanetScript>().shipsB.Count;
				GameObject apo = (GameObject)Instantiate(Resources.Load("apocalypse")as GameObject);
				apo.transform.position = plaE.transform.position;
				for(int ships = 0; ships < deleteShip; ships++) {
					GameObject temp = plaE.GetComponent<PlanetScript>().shipsB[0];
					plaE.GetComponent<PlanetScript>().shipsB.RemoveAt(0);
					GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
					expl.transform.position = temp.transform.position;
					user.GetComponent<stats>().destroyShipBPower ++;
					Destroy (temp);
				}
			}
			
			if(IAPlayer == "red"){
				user.GetComponent<MoneyScript>().moneyPlayer1-= info.nukePrice;	
				power.transform.position = info.posNuke1;
				user.GetComponent<stats>().nbNukeRed ++;
			}else{
				user.GetComponent<MoneyScript>().moneyPlayer2-= info.nukePrice;
				power.transform.position = info.posNuke2;
				user.GetComponent<stats>().nbNukeBlue ++;
			}
		}
		
		power = null;
		plaE = null;
		powerLaunch = false ;
		
	}

	void chooseRoad(){
		List<string> l = new List<string>();
		int max = 0;
		
		foreach(KeyValuePair<string,int> pair in ponderation){
			if(pair.Value > max){
				max = pair.Value;	
			}
			
		}
		
		foreach(KeyValuePair<string,int> pair in ponderation){
			if(pair.Value == max){
				l.Add(pair.Key);
			}
			
		}
		
		int rand = Random.Range(0, l.Count);	
		SpaceBridge bridge = user.GetComponent<SpaceBridge>();
		if( rand < l.Count){
			if( l[rand] != null ){
				string[] val = l[rand].Split('-');
				////Debug.Log("deplacement choisit: "+val[0]);
				GameObject ps = GameObject.Find(""+val[0][0]);
				GameObject pe = GameObject.Find(""+val[0][1]);
				////Debug.Log(
				
				if(val[1] != null && val[1] != ""){
					
					
					if(int.Parse(val[1])>0){
					
						Count.Add(int.Parse(val[1]));
						CountTmp.Add(0);
						speed.Add(0.2f);
						speedTmp.Add(0);
						
						listPlanetStart.Add(ps);
						
						if(bridge.planet1.name == ps.name && bridge.planet2.name == pe.name){
							listPlanetEnd.Add(bridge.blackHole1);
						}else if(bridge.planet2.name == ps.name && bridge.planet1.name == pe.name){
							listPlanetEnd.Add(bridge.blackHole2);
						}else{
							listPlanetEnd.Add(pe);
						}
						
						
						GameObject instance =(GameObject) Instantiate(Resources.Load("Line")as GameObject);
						instance.transform.position = new Vector3(0,0,0);
						LineRenderer linet = instance.GetComponent<LineRenderer>();
						linet.SetColors(new Color(1,1,1,1),new Color(1,1,1,1));
						linet.SetPosition(0,ps.transform.position);
						linet.SetPosition(1,ps.transform.position);
						
						listLines.Add(instance);
						
						if(IAPlayer == "red"){
							GameObject SelectShip =  Resources.Load("TextSelectRed")as GameObject;
							Vector3 vec =  ps.transform.position;
						
							vec.y = -20.22636f;
						
							GameObject instanceSelect = (GameObject) Instantiate(SelectShip,vec, SelectShip.transform.rotation);
							((TextMesh)instanceSelect.GetComponent<TextMesh>()).text = ""+0;
							
							instanceSelect.transform.RotateAround(Vector3.up, 1.6f);
							Vector3 vt = instanceSelect.transform.position;
							vt.x +=5;
							instanceSelect.transform.position = vt;
							GuiSelect.Add(instanceSelect);
						}else{
							GameObject SelectShip =  Resources.Load("TextSelectBlue")as GameObject;
							Vector3 vec =  ps.transform.position;
						
							vec.y = -20.22636f;
						
							GameObject instanceSelect = (GameObject) Instantiate(SelectShip,vec, SelectShip.transform.rotation);
							((TextMesh)instanceSelect.GetComponent<TextMesh>()).text = ""+0;
							instanceSelect.transform.RotateAround(Vector3.up, -1.6f);
							Vector3 vt = instanceSelect.transform.position;
							vt.x -=5;
							instanceSelect.transform.position = vt;
							GuiSelect.Add(instanceSelect);
							
						}
					}
				}
			}
		}
		
	}

}
