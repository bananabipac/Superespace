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
	private int nbShipToLaunch;
	public string IAPlayer;
	
	private Dictionary<string,int> ponderation = new Dictionary<string, int>();
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
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
					/*foreach(GameObject[] pair in pairs) {
						//Debug.Log ("Planet0 "+pair[0].name);	
						//Debug.Log ("Planet1 "+pair[1].name);
					}*/
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
								if(planete.GetComponent<PlanetScript>().ship.tag == IAPlayer){
									if(ind == 0) {
										int nbShip;
										if(IAPlayer == "blue") {
											nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
										}else {
											nbShip = planete.GetComponent<PlanetScript>().shipsR.Count;
										}
										int countEnnemy = 0;
										if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
										}else{
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsB.Count;
										}
										if(pair[1].GetComponent<PlanetScript>().ship.tag != IAPlayer) {
											if (nbShip >= countEnnemy - countEnnemy*marge/100){
												nbShipToLaunch = RandomNumber(Mathf.FloorToInt(countEnnemy - countEnnemy*marge/100),Mathf.FloorToInt(countEnnemy + countEnnemy*marge/100));
												launchMove = true;	
											}else{
												launchMove = false;	
											}
										} else {
											if (nbShip >= countEnnemy) {
												int difference = nbShip - countEnnemy;
												nbShipToLaunch = Mathf.FloorToInt(difference/2);
												launchMove = true;
											} else {
												launchMove = false;	
											}
										}
									}else if(ind == 1) {
										int nbShip;
										if(IAPlayer == "blue") {
											nbShip = pair[1].GetComponent<PlanetScript>().shipsB.Count;
										}else {
											nbShip = pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}
										int countEnnemy = 0;
										if(pair[0].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[0].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[0].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[0].GetComponent<PlanetScript>().shipsN.Count;
										}else{
											countEnnemy	= pair[0].GetComponent<PlanetScript>().shipsB.Count;
										}
										if(pair[0].GetComponent<PlanetScript>().ship.tag != IAPlayer) {
											if (nbShip >= countEnnemy - countEnnemy*marge/100){
												nbShipToLaunch = RandomNumber(Mathf.FloorToInt(countEnnemy - countEnnemy*marge/100),Mathf.FloorToInt(countEnnemy + countEnnemy*marge/100));
												launchMove = true;
											}else{
												launchMove = false;	
											}
										} else {
											if (nbShip >= countEnnemy) {
												int difference = nbShip - countEnnemy;
												nbShipToLaunch = Mathf.FloorToInt(difference/2);
												launchMove = true;
											} else {
												launchMove = false;	
											}
										}
									}
								}
								
								ind++;
							}
						}else{

							for(int i = 0; i < pair.Length; i ++) {
								GameObject planete = pair[i];	
								if(planete.GetComponent<PlanetScript>().ship.tag == IAPlayer){
									if(ind == 0) {
										int nbShip;
										if(IAPlayer == "blue") {
											nbShip = planete.GetComponent<PlanetScript>().shipsB.Count;
										}else {
											nbShip = planete.GetComponent<PlanetScript>().shipsR.Count;
										}
										
										int countEnnemy = 0;
										if(pair[1].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[1].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsN.Count;
										}else{
											countEnnemy	= pair[1].GetComponent<PlanetScript>().shipsB.Count;
										}
										if(pair[1].GetComponent<PlanetScript>().ship.tag != IAPlayer) {
											if (nbShip >= countEnnemy - (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)){
												nbShipToLaunch = RandomNumber(Mathf.FloorToInt(countEnnemy - (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)),Mathf.FloorToInt(countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)));
												launchMove = true;
											}else{
												launchMove = false;	
											}
										} else {
											if (nbShip >= countEnnemy) {
												int difference = nbShip - countEnnemy;
												nbShipToLaunch = Mathf.FloorToInt(difference/2);
												launchMove = true;
											} else {
												launchMove = false;	
											}
										}
									}else if(ind == 1) {
										int nbShip;
										if(IAPlayer == "blue") {
											nbShip = pair[1].GetComponent<PlanetScript>().shipsB.Count;
										}else {
											nbShip = pair[1].GetComponent<PlanetScript>().shipsR.Count;
										}
										int countEnnemy = 0;
										if(pair[0].GetComponent<PlanetScript>().ship.tag == "red") {
											countEnnemy	= pair[0].GetComponent<PlanetScript>().shipsR.Count;
										}else if (pair[0].GetComponent<PlanetScript>().ship.tag == "neutre"){
											countEnnemy	= pair[0].GetComponent<PlanetScript>().shipsN.Count;
										}else{
											countEnnemy	= pair[0].GetComponent<PlanetScript>().shipsB.Count;
										}
										if(pair[0].GetComponent<PlanetScript>().ship.tag != IAPlayer) {
											if (nbShip >= countEnnemy - (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)){
												nbShipToLaunch = RandomNumber(Mathf.FloorToInt(countEnnemy - (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)),Mathf.FloorToInt(countEnnemy + (countEnnemy*marge/100)+(nbShip*asteroid.GetComponent<asteroidScript>().chanceKill/100)));
												launchMove = true;
											}else{
												launchMove = false;
											}
										}else{
											if (nbShip >= countEnnemy) {
												int difference = nbShip - countEnnemy;
												nbShipToLaunch = Mathf.FloorToInt(difference/2);
												launchMove = true;
											} else {
												launchMove = false;
											}
										}
									}
								}
								
								ind++;
							}
							
							
						}

						if(launchMove) {
							int ind2 = 0;
							for(int i = 0; i < pair.Length; i++) {
								GameObject planete = pair[i];
								if(planete.GetComponent<PlanetScript>().ship.tag == IAPlayer) {
									
									if(ind2==0){
										if(planete != pair[1]) {
											user.GetComponent<moveShip>().deplacement(planete,pair[1],nbShipToLaunch);
											if(!user.GetComponent<GestionLink>().roadOpen(planete,pair[1]) && nbShipToLaunch > 0 ){
												user.GetComponent<GestionLink>().openRoad(planete,pair[1]);
												if(IAPlayer == "blue") {
													user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
												}else {
													user.GetComponent<MoneyScript>().moneyPlayer1 -= 50;
												}
											}
										}
									}else if (ind2 == 1) {
										if(planete != pair[0]) {
											user.GetComponent<moveShip>().deplacement(planete,pair[0],nbShipToLaunch);
											if(!user.GetComponent<GestionLink>().roadOpen(planete,pair[0]) && nbShipToLaunch > 0){
												user.GetComponent<GestionLink>().openRoad(planete,pair[0]); 
												if(IAPlayer == "blue") {
													user.GetComponent<MoneyScript>().moneyPlayer2 -= 50;
												}else {
													user.GetComponent<MoneyScript>().moneyPlayer1 -= 50;
												}
											}
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
		if(IAPlayer == "blue") {
			if(user.GetComponent<MoneyScript>().moneyPlayer2 >= 50	) {
				hasEnoughMoney = true;
			}else{
				hasEnoughMoney = false;	
			}
		}else{
			if(user.GetComponent<MoneyScript>().moneyPlayer1 >= 50	) {
				hasEnoughMoney = true;
			}else{
				hasEnoughMoney = false;	
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

	int RandomNumber(int min, int max) {
		return Random.Range(min,max);
	}
	
	void calculatePonderation() {
		for(int i = 0; i < pairs.Count; i++) {
			
			
			
		}
		
	}

}