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
	// Use this for initialization
	void Start () {
		checkPlanets();
		user = GameObject.FindGameObjectWithTag("User");
		launchMove = false;
		timer = Time.timeSinceLevelLoad;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - timer >= speedIA) {
			checkPlanets();
			findPossibleRoutes();
			checkRoutes(false);
			checkMoney();
			
			if(hasEnoughMoney){
					Debug.Log("enough Money");
				if(pairsClosed.Count>0){
					Debug.Log("road without asteroid find");
					pair = pairsClosed[RandomNumber(0,pairsClosed.Count)];
				}else{
					Debug.Log("no road without asteroid");
					checkRoutes(true);
					if(pairsClosed.Count>0){
						Debug.Log("road with asteroid found");
						pair = pairsClosed[RandomNumber(0,pairsClosed.Count)];
					}else{
					checkRoutes(false);
						Debug.Log("road open find");
						pair = pairsOpened[RandomNumber(0,pairsOpened.Count)];
					}
				}
				Debug.Log("road select :"+pair[0].name+pair[1].name);
						
			} else {
				Debug.Log("Not enough Money");
				if(pairsOpened.Count>0){
					Debug.Log("roadOpenSelect");
					pair = pairsOpened[RandomNumber(0,pairsOpened.Count-1)];
				}
			}
			
			if(pair != null) {
				Debug.Log("road found");
				Debug.Log("planet : "+pair[0].name+" planet : "+pair[1].name);
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
					Debug.Log("no Asteroid");
					foreach(GameObject planete in pair){
						if(planete.GetComponent<PlanetScript>().ship.tag == "blue"){
							if(ind == 0) {
								Debug.Log("ind 0");
								int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
								int countEnnemy = 0;
								if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
								}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
								}
								 
								//Debug("soldier : "+nbShip+" countEnnemy : "+(countEnnemy + countEnnemy*marge/100)); 
								if (nbShip >= countEnnemy + countEnnemy*marge/100){
									Debug.Log("enough Soldier");
									launchMove = true;	
								}else{
									Debug.Log("Not enough");
								}
							}else if(ind == 1) {
								Debug.Log("ind 0");
								int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
								int countEnnemy = 0;
								if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
								}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
								}
								
								//Debug("soldier : "+nbShip+" countEnnemy : "+(countEnnemy + countEnnemy*marge/100)); 
								if (nbShip >= countEnnemy + countEnnemy*marge/100){
									launchMove = true;	
									Debug.Log("enough Soldier");
								}else{
									Debug.Log("not enough Soldier");
								}
								
							}else{
								Debug.Log("no move");
								launchMove = false;	
							}
						}
						
						ind++;
					}
				}else{
					Debug.Log("asteroid on the road");
					
					foreach(GameObject planete in pair){
						if(planete.GetComponent<PlanetScript>().ship.tag == "blue"){
							if(ind == 0) {
								Debug.Log("ind 0 asteroid");
								int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
								
								int countEnnemy = 0;
								if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
								}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
								}
								//Debug("soldier : "+nbShip+" countEnnemy : "+(countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100))); 
								if (nbShip >= countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)){
									launchMove = true;	
									Debug.Log("enough soldier to cross asteroid");
								}else{
									Debug.Log("not enough soldier to cross asteroid");
								}
							}else if(ind == 1) {
								Debug.Log("ind 1 asteroid");
								int nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
								int countEnnemy = 0;
								if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
								}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
									countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
								}
								//Debug("soldier : "+nbShip+" countEnnemy : "+(countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100))); 
								if (nbShip >= countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)){
									launchMove = true;	
									Debug.Log("enough soldier to cross asteroid");
								}else{
									Debug.Log("not enough soldier to cross asteroid");
								}
							}else{
								launchMove = false;	
								Debug.Log("no move asteroid");
							}
						}
						
						ind++;
					}
					
					
				}
				
				if(launchMove) {
					Debug.Log("lauch move true");
					int ind2 = 0;
					foreach(GameObject planete in pair) {
						if(planete.GetComponent<PlanetScript>().ship.tag == "blue") {
							
							if(ind2==0){
								Debug.Log ("move 0");
								user.GetComponent<moveShip>().deplacement(planete,pair[1],planete.GetComponent<PlanetScript>().shipsB.Count);
								if(!user.GetComponent<GestionLink>().roadOpen(planete,pair[1])){
									user.GetComponent<GestionLink>().openRoad(planete,pair[1]); 
									user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
								}
							}else if (ind2 == 1) {
								Debug.Log("move 1");
								user.GetComponent<moveShip>().deplacement(planete,pair[0],planete.GetComponent<PlanetScript>().shipsB.Count);
								if(!user.GetComponent<GestionLink>().roadOpen(planete,pair[1])){
									user.GetComponent<GestionLink>().openRoad(planete,pair[1]); 
									user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
								}
							}
						}
						
					}
				}
			}
			timer = Time.timeSinceLevelLoad;
		}
	}
	void checkRoutes(bool withAsteroid) {
		pairsClosed = new List<GameObject[]>();
		pairsOpened = new List<GameObject[]>();
		string ps,pe;
		foreach(GameObject[] pair in pairs) {
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
		foreach(GameObject planetS in planetsIA) {
			foreach(GameObject planetE in planets) {
				if(user.GetComponent<GestionLink>().roadExist(planetS,planetE)) {
					if(planetE != planetS){
							pairs.Add(new GameObject[] {planetS,planetE});
					}
				}
			}
			
		}
	}
	
	void checkPlanets() {
		planets = GameObject.FindGameObjectsWithTag("planet");
		int indice = 0;
		foreach(GameObject planet in planets) {
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
