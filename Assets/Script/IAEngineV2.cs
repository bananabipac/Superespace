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
	public int siPlanetPasIAAssezShip ; //si la planete n'est pas a l'IA et assez de vaisseaux
	public int siPlanetPasIAPasAssezShip ; // si planete pas a l'IA et pas assez de vaisseaux
	public int siCaptureEnCourIA ; // si la planete est en cours de capture par l'IA mais moins de 10 vaisseaux
	public int siCaptureEnCourAD; // si la planete est en cours de capture par l'adversaire 
	public int siTrouNoir; //si le trou noire est ouvert et nb vaisseaux requis - 20%
	public int siPasAssezVaisseaux; //si pas assez de vaisseaux ?????
	public int siCombat; //si la planete d'arrivée est en combat + pas assez vaisseaux 
	
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
				
				
				
				
				
				timer = Time.timeSinceLevelLoad;
			}
		}
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
			int pond = valeurBase ; 
			int ps,pe;
			GameObject planetStart = pairs[i][0];
			GameObject planetEnd = pairs[i][1];
			PlanetScript scriptS = planetStart.GetComponent<PlanetScript>();
			PlanetScript scriptE = planetEn.GetComponent<PlanetScript>();
			
			if(int.Parse(planetStart.name) > int.Parse(planetEnd.name)){
				ps = planetEnd.name;
				pe = planetStart.name;
			}else{
				pe = planetEnd.name;
				ps = planetStart.name;
			}
			if(GameObject.Find("a"+ps+pe) != null){//si des asteroides sont sur la route
				pond += siAsteroid;
				
			}
			
			if(planetEnd.tag != IAPlayer){//si la planete n'est pas a l'IA
				if(IAPlayer == "red"){
					if(scriptE.shipsR.Count>0 && scriptE.shipsR.Count<=10 && scriptE.shipsB.Count==0 && scriptE.shipsN.Count == 0){//capture en cours
						pond += siCaptureEnCourIA;
					}else{
						if(scriptE.shipsR.Count>0 && (scriptE.shipsB.Count>0 || scriptE.shipsN.Count > 0)){//combat en cour
							if(scriptE.shipsR.Count<scriptE.shipsB.Count || scriptE.shipsR.Count>0 < scriptE.shipsN.Count){//L'IA est en train de perdre le combat
								pond += siCombat;
							}
						}else if(scriptE.shipsR.Count==0 && (scriptE.shipsB.Count>0 || scriptE.shipsN.Count > 0)){// la planete est a l'adversaire
							if(scriptS.shipsR.Count >= scriptE.shipsB.Count - scriptE.shipsB.Count*marge/100){//si IA assez de vaisseaux
								
							}
						}
						
					}
					
				}else{
					if(scriptE.shipsB>0 && scriptE.shipsB<=10 && scriptE.shipsR==0 && scriptE.shipsN == 0){//capture en cours
						pond += siCaptureEnCourIA;
					}
					
				}
				
				
			}else{//si la planete est a l'IA
				
			}
		}
		
	}
}
