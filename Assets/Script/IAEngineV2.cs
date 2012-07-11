using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAEngineV2 : MonoBehaviour {

	private GameObject user;
	private GameObject[] planets;
	private List<GameObject> planetsIA = new List<GameObject>();
	private float timer;
	private List<GameObject[]> pairs = new List<GameObject[]>();
	//private List<GameObject[]> pairsClosed = new List<GameObject[]>();
	//private List<GameObject[]> pairsOpened = new List<GameObject[]>();
	public float speedIA;
	private bool hasEnoughMoney;
	private bool launchMove;
	public float marge;
	GameObject[] pair = new GameObject[2];
	private int nbShipToLaunch;
	public string IAPlayer;
	
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

	
	private Dictionary<string,int> ponderation = new Dictionary<string, int>();
	
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			user = GameObject.FindGameObjectWithTag("User");
			launchMove = false;
			timer = Time.timeSinceLevelLoad;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			if(Time.timeSinceLevelLoad - timer >= speedIA) {
				checkPlanets();
				findPossibleRoutes();
				calculatePonderation();
				
				chooseRoad();
				
			
				reinitVar();
				
				
				timer = Time.timeSinceLevelLoad;
			}
		}
	}
	void reinitVar() {
		pairs = new List<GameObject[]>();
		planetsIA = new List<GameObject>();
		ponderation.Clear();
		
	}
	void checkPlanets() {
		planets = GameObject.FindGameObjectsWithTag("planet");
		int indice = 0;
		for(int i = 0; i < planets.Length; i++){
			GameObject planet = planets[i];
			if(planet.GetComponent<PlanetScript>().ship.tag == IAPlayer) {
				//Debug.Log(planet.tag);
				planetsIA.Add(planet);
				indice++;
				//Debug.Log("planete IA: "+planet.name);
			}
		}	

	}
	
	void findPossibleRoutes() {
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
	
	void calculatePonderation() {
		for(int i = 0; i < pairs.Count; i++) {
			
			bool ast = false; 
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
				Debug.Log("Asteroid");
				
			}
			
			if(scriptE.ship.tag != IAPlayer){//si la planete n'est pas a l'IA
				if(IAPlayer == "red"){
					if( scriptS.shipsR.Count>(10+(10*marge)/100) && scriptE.shipsR.Count>0 && scriptE.shipsR.Count<=10 && scriptE.shipsB.Count==0 && scriptE.shipsN.Count == 0){//capture en cours
						pond += siCaptureEnCourIA;
						nbShip +=Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsR.Count);
						Debug.Log("CaptureEnCourIA");
					}else{
						if(scriptE.shipsR.Count>0 && (scriptE.shipsB.Count>0 || scriptE.shipsN.Count > 0)){//combat en cour
							if(scriptE.shipsB.Count> scriptE.shipsR.Count ){
								pond += siCombatPlanetAD;
								Debug.Log("CombatPlanetAD");
								
								nbShip += Mathf.FloorToInt((scriptE.shipsB.Count + (scriptE.shipsB.Count*marge)/100)- scriptE.shipsR.Count);
							
							}else if(scriptE.shipsN.Count> scriptE.shipsR.Count ){
								pond += siCombatPlanetAD;
								Debug.Log("CombatPlanetAD");
								
								nbShip += Mathf.FloorToInt((scriptE.shipsN.Count + (scriptE.shipsN.Count*marge)/100)- scriptE.shipsR.Count);	
								
							}
						}else if(scriptE.shipsR.Count==0 && (scriptE.shipsB.Count>0 || scriptE.shipsN.Count > 0)){// la planete est a l'adversaire
							if(scriptE.shipsB.Count > 0 && scriptE.shipsN.Count == 0){//planete bleu
								if(scriptS.shipsR.Count >= scriptE.shipsB.Count - scriptE.shipsB.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsB.Count + scriptE.shipsB.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									Debug.Log("PlanetPasIAPasAssezShip");
								}
								
							}else if(scriptE.shipsN.Count > 0 && scriptE.shipsB.Count == 0){//planete neutre
								if(scriptS.shipsR.Count >= scriptE.shipsN.Count - scriptE.shipsN.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsN.Count +scriptE.shipsN.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									Debug.Log("PlanetPasIAPasAssezShip");
								}
								
								
							}
						}
						
					}
					
				}else{
					if( scriptS.shipsB.Count>(10+(10*marge)/100) && scriptE.shipsB.Count>0 && scriptE.shipsB.Count<=10 && scriptE.shipsR.Count==0 && scriptE.shipsN.Count == 0){//capture en cours
						pond += siCaptureEnCourIA;
						nbShip += Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsB.Count  );
						Debug.Log("CaptureEnCourIA");
						
					}else{
						if(scriptE.shipsB.Count>0 && (scriptE.shipsR.Count>0 || scriptE.shipsN.Count > 0)){//combat en cour
							if(scriptE.shipsR.Count> scriptE.shipsB.Count ){
								pond += siCombatPlanetAD;
								Debug.Log("CombatPlanetAD");
								
								nbShip += Mathf.FloorToInt((scriptE.shipsR.Count + (scriptE.shipsR.Count*marge)/100)- scriptE.shipsB.Count);	
								
							}else if(scriptE.shipsN.Count> scriptE.shipsB.Count ){
								pond += siCombatPlanetAD;
								Debug.Log("CombatPlanetAD");
								
								nbShip += Mathf.FloorToInt((scriptE.shipsN.Count + (scriptE.shipsN.Count*marge)/100)- scriptE.shipsB.Count);	
								
							}
						}else if(scriptE.shipsB.Count==0 && (scriptE.shipsR.Count>0 || scriptE.shipsN.Count > 0)){// la planete est a l'adversaire
							if(scriptE.shipsR.Count > 0 && scriptE.shipsN.Count == 0){//planete bleu
								if(scriptS.shipsB.Count >= scriptE.shipsR.Count - scriptE.shipsR.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsR.Count + scriptE.shipsR.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									Debug.Log("PlanetPasIAPasAssezShip");
								}
								
							}else if(scriptE.shipsN.Count > 0 && scriptE.shipsR.Count == 0){//planete neutre
								if(scriptS.shipsB.Count >= scriptE.shipsB.Count - scriptE.shipsN.Count*marge/100){//si IA assez de vaisseaux 
									pond += siPlanetPasIAAssezShip;
									Debug.Log("PlanetPasIAAssezShip");
									nbShip += Mathf.FloorToInt(scriptE.shipsN.Count + scriptE.shipsN.Count*marge/100);
								}else{//si IA pas assez de vaisseaux 
									pond += siPlanetPasIAPasAssezShip;
									Debug.Log("PlanetPasIAPasAssezShip");
								}
								
								
							}
						}
						
					}
				}
			}else{//si la planete est a l'IA
				pond += siPlanetIA;
				Debug.Log("PlanetIA");
				if(IAPlayer =="red"){
					
					if(scriptE.shipsB.Count > 0){//la planete est attaqué	
						if(scriptE.shipsR.Count >0){//perte du combat
							if(scriptE.shipsB.Count>scriptE.shipsR.Count){
								pond += siCombatPlaneteIA;
								Debug.Log("CombatPlaneteIA");
								
								nbShip += Mathf.FloorToInt((scriptE.shipsB.Count + (scriptE.shipsB.Count*marge)/100)- scriptE.shipsR.Count);	
								
							}
						}else{//la planete est en cours de capture par l'ennemie
							pond += siCaptureEnCourAD;
							Debug.Log("CaptureEnCourAD");
							nbShip += Mathf.FloorToInt(scriptE.shipsB.Count + (scriptE.shipsB.Count*marge)/100);
						}
					}else{
						if(scriptE.shipsR.Count <= 10){
							pond += siPlanetIAPasAssezShip;
							Debug.Log("PlanetIAPasAssezShip");
							nbShip += Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsR.Count  );
							
						}
						
					}
					
				}else{
					if(scriptE.shipsR.Count > 0){//la planete est attaqué	
						if(scriptE.shipsB.Count >0){//perte du combat
							if(scriptE.shipsR.Count>scriptE.shipsB.Count){
								pond += siCombatPlaneteIA;
								Debug.Log("CombatPlaneteIA");
								nbShip += Mathf.FloorToInt((scriptE.shipsR.Count + (scriptE.shipsR.Count*marge)/100)- scriptE.shipsB.Count);
							}
						}else{//la planete est en cours de capture par l'ennemie
							pond += siCaptureEnCourAD;
							Debug.Log("CaptureEnCourAD");
							nbShip += Mathf.FloorToInt(scriptE.shipsR.Count + (scriptE.shipsR.Count*marge)/100);
						}
					}else{
						if(scriptE.shipsB.Count <= 10){
							pond += siPlanetIAPasAssezShip;
							Debug.Log("PlanetIAPasAssezShip");
							
							nbShip += Mathf.FloorToInt((10+(10*marge)/100) - scriptE.shipsB.Count);
							Debug.Log(nbShip);
							
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
						Debug.Log("NotEnoughMOney");
					}
				}else{
					if(user.GetComponent<MoneyScript>().moneyPlayer2 <50){
						pond += notEnoughMoney;	
						Debug.Log("NotEnoughMOney");
					}
					
				}
			}	
			
		
			ponderation.Add(""+planetStart.name+planetEnd.name+"-"+nbShip, pond);
			
			Debug.Log(""+planetStart.name+planetEnd.name+" pond : "+pond);
		}
		
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
		
		string[] val = l[rand].Split('-');
		Debug.Log("deplacement choisit: "+val[0]);
		GameObject ps = GameObject.Find(""+val[0][0]);
		GameObject pe = GameObject.Find(""+val[0][1]);
		//Debug.Log(
		if(val[1] != null && val[1] != ""){
			
			if(user.GetComponent<GestionLink>().roadExist(ps, pe) && !user.GetComponent<GestionLink>().roadOpen(ps, pe)){
				user.GetComponent<GestionLink>().openRoad(ps,pe);
				if(IAPlayer =="red"){
					user.GetComponent<MoneyScript>().moneyPlayer1 -= 50;
				}else{
					user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
				}
			}
			
			user.GetComponent<moveShip>().deplacement(ps,pe, int.Parse(val[1]));
			
		}
		
	}

}
