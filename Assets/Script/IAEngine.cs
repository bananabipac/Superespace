using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAEngine : MonoBehaviour {
	private GameObject user;
	private GameObject[] planets;
	private List<GameObject> planetsIA = new List<GameObject>();
	private float timer;
	private List<GameObject[]> pairs = new List<GameObject[]>();
	private List<GameObject[]> pairsClosed = new List<GameObject[]>();
	private List<GameObject[]> pairsOpened = new List<GameObject[]>();
	public float speedIA;
	private bool hasEnoughMoney;
	private bool launchMove;
	public float marge;
	GameObject[] pair = new GameObject[2];
	private bool isUpdating;
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			checkPlanets();
			user = GameObject.FindGameObjectWithTag("User");
			launchMove = false;
			timer = Time.timeSinceLevelLoad;
			isUpdating  = false;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			if(Time.timeSinceLevelLoad - timer >= speedIA) {
				if(!isUpdating) {
					checkPlanets();
					findPossibleRoutes();
					checkRoutes(false);
					checkMoney();
					isUpdating = true;
				}else{
					if(hasEnoughMoney){
						if(pairsClosed.Count>0){
							pair = pairsClosed[RandomNumber(0,pairsClosed.Count)];
						}else{
							checkRoutes(true);
							if(pairsClosed.Count>0){
								pair = pairsClosed[RandomNumber(0,pairsClosed.Count)];
							}else{
								checkRoutes(false);
								pair = pairsOpened[RandomNumber(0,pairsOpened.Count)];
							}
						}
								
					} else {
						if(pairsOpened.Count>0)
							pair = pairsOpened[RandomNumber(0,pairsOpened.Count-1)];
					}
					if(pair != null) {
						string ps,pe;
						if(int.Parse(pair[0].name) > int.Parse(pair[1].name)){
							ps = pair[1].name;
							pe = pair[0].name;
						}else{
							pe = pair[1].name;
							ps = pair[0].name;
						}
						GameObject asteroid = GameObject.Find ("a"+ps+pe);
						int ind = 0;
						if(asteroid == null) {
							for(int i = 0; i < pair.Length; i++){
								GameObject planete = pair[i];
								if(planete.GetComponent<PlanetScript>().ship.tag == "blue"){
									if(ind == 0) {
										int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
										int countEnnemy = 0;
										if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
										}
										if (nbShip >= countEnnemy + countEnnemy*marge/100){
											launchMove = true;	
										}
									}else if(ind == 1) {
										int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
										int countEnnemy = 0;
										if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
										}
										if (nbShip >= countEnnemy + countEnnemy*marge/100){
											launchMove = true;
										}
										
									}else{
										launchMove = false;	
									}
								}
								
								ind++;
							}
						}else{
							
							for(int i = 0; i < pair.Length; i ++) {
								GameObject planete = pair[i];	
								if(planete.GetComponent<PlanetScript>().ship.tag == "blue"){
									if(ind == 0) {
										int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
										
										int countEnnemy = 0;
										if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
										}
										if (nbShip >= countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)){
											launchMove = true;
										}
									}else if(ind == 1) {
										int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
										int countEnnemy = 0;
										if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
										}
										if (nbShip >= countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)){
											launchMove = true;
										}
									}else{
										launchMove = false;
									}
								}
								
								ind++;
							}
							
							
						}
						
						if(launchMove) {
							int ind2 = 0;
							for(int i = 0; i < pair.Length; i++) {
								GameObject planete = pair[i];
								if(planete.GetComponent<PlanetScript>().ship.tag == "blue") {
									
									if(ind2==0){
										
										if(!user.GetComponent<GestionLink>().roadOpen(planete,pair[1]) && planete != pair[1]){
											user.GetComponent<moveShip>().deplacement(planete,pair[1],planete.GetComponent<PlanetScript>().shipsB.Count);
											user.GetComponent<GestionLink>().openRoad(planete,pair[1]); 
											user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
										}
									}else if (ind2 == 1) {
										
										if(!user.GetComponent<GestionLink>().roadOpen(planete,pair[0]) && planete != pair[0]){
											user.GetComponent<moveShip>().deplacement(planete,pair[0],planete.GetComponent<PlanetScript>().shipsB.Count);
											user.GetComponent<GestionLink>().openRoad(planete,pair[0]); 
											user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
										}
									}
								}
								
							}
						}
					}
					timer = Time.timeSinceLevelLoad;
					reinitVar();
					isUpdating = false;
				}
			}
		}
	}
	void reinitVar() {
		pairs = new List<GameObject[]>();
		planetsIA = new List<GameObject>();
	}
	void checkRoutes(bool withAsteroid) {
		pairsClosed = new List<GameObject[]>();
		pairsOpened = new List<GameObject[]>();
		string ps,pe;
		for(int i = 0; i < pairs.Count; i++) {
			GameObject[] pair = pairs[i];
			if(int.Parse(pair[0].name) > int.Parse(pair[1].name)){
				ps = pair[1].name;
				pe = pair[0].name;
			}else{
				pe = pair[1].name;
				ps = pair[0].name;
			}
			if(user.GetComponent<GestionLink>().roadOpen(pair[0],pair[1]))	{
				if(GameObject.Find("a"+ps+pe) != null){
					if(withAsteroid){
						pairsOpened.Add(pair);
					}
				}else{
					if(!withAsteroid){
						pairsOpened.Add(pair);
					}
				}
			}else{
				if(GameObject.Find("a"+ps+pe) != null){
					if(withAsteroid){
						pairsClosed.Add(pair);
					}
				}else{
					if(!withAsteroid){
						pairsClosed.Add(pair);
					}
				}
			}
			
		}
		
		
	}
	
	
	void checkMoney() {
		if(user.GetComponent<MoneyScript>().moneyPlayer2 >= 50	) {
			hasEnoughMoney = true;
		}else{
			hasEnoughMoney = false;	
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
	
	void checkPlanets() {
		planets = GameObject.FindGameObjectsWithTag("planet");
		int indice = 0;
		for(int i = 0; i < planets.Length; i++){
			GameObject planet = planets[i];
			if(planet.GetComponent<PlanetScript>().ship.tag == "blue") {
				//Debug.Log(planet.tag);
				planetsIA.Add(planet);
				indice++;
				//Debug.Log("planete IA: "+planet.name);
			}
		}	
		
	}
	
	int RandomNumber(int min, int max) {
		return Random.Range(min,max);
	}
	
}
