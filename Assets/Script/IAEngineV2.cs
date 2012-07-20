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
	
	public bool stop ;
	
	private Dictionary<string,int> ponderation = new Dictionary<string, int>();
	
	// Use this for initialization
	void Start () {
		stop = false;
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			user = GameObject.FindGameObjectWithTag("User");
			launchMove = false;
			timer = Time.timeSinceLevelLoad;
		}
	}
	
	/*GameObject instance =(GameObject) Instantiate(Resources.Load("Line")as GameObject);
								instance.transform.position = new Vector3(0,0,0);
								LineRenderer linet = instance.GetComponent<LineRenderer>();
								
								linet.SetPosition(0,hit.collider.gameObject.transform.position);
								linet.SetPosition(1,hit.collider.gameObject.transform.position);
								linet.SetColors(new Color(1,1,1,1),new Color(1,1,1,1));
								listLines.Add(fingerId,instance);
								listPlanetStart.Add (fingerId,hit.collider.gameObject);*/
	
	// Update is called once per frame
	void Update () {
		if(PlayerPrefs.GetString("GameType").Equals("solo")){
			stop = false;
			if(Count.Count >0){
				stop = true;
			}
			/*if(listLines.Count >0){
				stop = true;
			}*/
			if(!stop){ 
				if(Time.timeSinceLevelLoad - timer >= speedIA) {
					checkPlanets();
					findPossibleRoutes();
					calculatePonderation();
					
					chooseRoad();
					
				
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
							if(CountTmp[i] >= Count[i]){
								
								iTween.ValueTo(gameObject,iTween.Hash("from",listPlanetStart[i].transform.position,"to",listPlanetEnd[i].transform.position,"time", vitesseSelect,"onupdate","rendere","oncomplete","delacementFinish","oncompleteparams",i,"easetype","linear"));
								//iTween.MoveTo((GameObject)listMove[i],iTween.Hash("position",listPlanetEnd[i],"time",5f,"oncomplete","delacementFinish","onCompleteTarget", gameObject,"oncompleteparams",i, "easetype", "linear"));
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
		//Debug.Log(i);
		if(listLines.Count > 0){
			listLines[0].GetComponent<LineRenderer>().SetPosition(1, coord);
		}
		
	}
	
	void delacementFinish(int i){
		//int i = 0;
		Debug.Log(i);
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
			
			
			if(int.Parse(val[1])>0){
			
				Count.Add(int.Parse(val[1]));
				CountTmp.Add(0);
				speed.Add(0.2f);
				speedTmp.Add(0);
				
				listPlanetStart.Add(ps);
				listPlanetEnd.Add(pe);
				
				
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