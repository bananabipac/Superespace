using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlanetScript : MonoBehaviour {
	
	
	public int nbShip; //nombre de vaisseaux que possede le vaisseaux au debut de la partie
	public List<GameObject> shipsR; //liste des vaisseaux rouge sur la planete
	public List<GameObject> shipsB; //liste des vaisseaux bleu sur la planete
	public List<GameObject> shipsN; //liste des vaisseaux neutre sur la planete
	public GameObject ship; //Object vaisseaux correspondant au type de vaisseaux que doit construire la planete
	private float vFight ; //delai entre chaque resolution de combat
	public int repop; // delai entre chaque creation de vaiseaux
	private float count; //timer entre chaque resolution de combat
	private float timePop; //timer entre chaque creation de vaisseaux
	//public GameObject explosion; //object de l'explosion a instantié
	public int LimitPop ;//Limite de population
	private int CaptureCount; //temps de capture requis
	private int CaptureTmp;
	private int SpeedCapture; //vitesse de capture
	public float CaptureTime; //temps e capture en cour
	private GameObject user;
	//pulsation
	/*private bool pul;//triger pour la pulsation	
	private float sizePul;//taille de la pulsation
	private float countPul;//taille de la pulsation en cour
	private bool invertPul; //pulsation inverse*/
	public List<GameObject> shipsRS = new List<GameObject>();
	public List<GameObject> shipsBS = new List<GameObject>();
	public List<GameObject> shipsNS = new List<GameObject>();
	
	private int sr;
	private int sb;
	private int sn;
	
	
	// Use this for initialization
	void Start () {
		
		/*pul = false;
		sizePul = 1;
		countPul = 0;
		invertPul = false;*/
		
		user = GameObject.FindGameObjectWithTag("User");
		
		vFight = 0.4f;
		CaptureCount = 1;
		SpeedCapture = 50;
		
		//END //Debug
		
		
		CaptureTmp = 0;	
		timePop = 0;
		count = 0;	
		CaptureTime = 0;
		
	
		
		//creation des vaisseaux au depart 
		for(int i = 0 ; i<nbShip ; i++){
			createShip();
		}
	
	
		if(ship != null){
			if(ship.tag == "red"){
				gameObject.light.color = new Color(1,0,0,1);
				CaptureTime = CaptureCount;
			}else if (ship.tag == "blue"){
				gameObject.light.color = new Color(0,0,1,1);
				CaptureTime = -1*CaptureCount;
			}else{//ship neutre
				gameObject.light.color = new Color(1,1,1,1);
				CaptureTime = 0;
			}
		}else{
			gameObject.light.color = new Color(1,1,1,1);
			CaptureTime = 0;
		}
		
		gameObject.light.intensity = 2;
		gameObject.light.range = gameObject.transform.localScale.x * 1.3f;
		
		sr = shipsR.Count;
		sb = shipsB.Count;
		sn = shipsN.Count;
		
		refreshShip();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(sr != shipsR.Count){
			refreshShip();	
			sr = shipsR.Count;
		}else if(sb != shipsB.Count){
			refreshShip();	
			sb = shipsB.Count;
		}else if(sn != shipsN.Count){
			refreshShip();	
			sn = shipsN.Count;
		}
		
		//rotation de la planete
		this.transform.RotateAround(this.transform.position,Vector3.up, 7f * Time.deltaTime);
		
		if(shipsB.Count > 0 && shipsR.Count >0 || shipsN.Count > 0 && shipsR.Count >0 || shipsB.Count > 0 && shipsN.Count >0 ){//bataille entre les vaissseaux
			count  += 1*Time.deltaTime;
			
			if(count >= vFight){
				count = 0;
				startFights();
				CaptureTmp = 0;
			}
			
		}else if (shipsB.Count>0 && ship.tag != "blue"){//vaisseau bleu present sur la planete
			if(CaptureTime > -1*CaptureCount && Time.deltaTime>0){
				CaptureTmp += 1*shipsB.Count;
				if(CaptureTmp >=SpeedCapture){
					CaptureTmp = 0;
					CaptureTime -= 0.7f*Time.deltaTime;
					//changement de couleur du halo
					if(CaptureTime <0){
						gameObject.light.color = new Color(1+CaptureTime,1+CaptureTime,1,1);
					}else{
						gameObject.light.color = new Color(1,1-CaptureTime,1-CaptureTime,1);
						
					}
					
					if(CaptureTime <= -1*CaptureCount){
						ship =  Resources.Load("Shipblue")as GameObject;
						user.GetComponent<stats>().nbCaptureBlue ++;
						gameObject.light.color = new Color(0,0,1,1);
						CaptureTime = -1*CaptureCount;
						((GestionLink)user.GetComponent<GestionLink>()).changeColor(gameObject);
						int[] links = ((GestionLink)user.GetComponent<GestionLink>()).nbRoad();
						user.GetComponent<MoneyScript>().incomePlayer1 = 1 + links[0];
						user.GetComponent<MoneyScript>().incomePlayer2 = 1 + links[1];
						////Debug.Log("Planete : "+gameObject.name + "capture blue");
						
					}
				}
			}else{
				if(ship.tag == "blue"){
					timePop += 10*Time.deltaTime;
					if(timePop >= repop){
						timePop = 0;
						if(shipsB.Count < LimitPop){
							createShip();
						}
					}
				}
				
			}
		}else if (shipsR.Count>0 && ship.tag != "red"){//vaisseau rouge present sur la planete
			
			if(CaptureTime < CaptureCount && Time.deltaTime>0){
				CaptureTmp += 1*shipsR.Count;
				if(CaptureTmp >=SpeedCapture){
						
					CaptureTmp = 0;
					CaptureTime += 0.7f*Time.deltaTime;
						
					if(CaptureTime <0){
						gameObject.light.color = new Color(1+CaptureTime,1+CaptureTime,1,1);
							
					}else{
						gameObject.light.color = new Color(1,1-CaptureTime,1-CaptureTime,1);
							
					}
							
					if(CaptureTime >= CaptureCount){
						ship =  Resources.Load("Shipred")as GameObject;
						user.GetComponent<stats>().nbCaptureRed++;
						gameObject.light.color = new Color(1,0,0,1);
						CaptureTime = CaptureCount;
						//Debug.Log("Planete : "+gameObject.name + "capture red");
						((GestionLink)user.GetComponent<GestionLink>()).changeColor(gameObject);
						int[] links = ((GestionLink)user.GetComponent<GestionLink>()).nbRoad();
						user.GetComponent<MoneyScript>().incomePlayer1 = 1 + links[0];
						user.GetComponent<MoneyScript>().incomePlayer2 = 1 + links[1];
						//pul = true;	
					}
				}
			}else{
				if(ship.tag == "red"){
					timePop += 10*Time.deltaTime;
					if(timePop >= repop){
						timePop = 0;
						if(shipsR.Count < LimitPop){
							createShip();
						}
					}
				}
				
			}
		}else{
			timePop += 10*Time.deltaTime;
			if(timePop >= repop){
				timePop = 0;
				if(ship.tag == "red" && shipsR.Count < LimitPop){
					createShip();
				}else if(ship.tag == "blue" && shipsB.Count < LimitPop){
					createShip();
				}
			}
		}
	}
	
	//fonction qui gere la creation des vaisseaux 
	public void createShip (){
		infoUser infoRed = GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>();
		infoUser infoBlue = GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>();
		
		float scal = this.transform.localScale.x ;
			
		float min = scal/2.5f +1 ;
		float max = scal/2.5f +1.5f;
			
		float z = Random.Range(min,max);
		Vector3 vec = new Vector3(0,0,z);
		vec = this.transform.position + vec ; 
				
			
		GameObject instance = (GameObject) Instantiate(ship,vec, transform.rotation);
		instance.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
		((rotationShip)instance.GetComponent<rotationShip>()).planet = this.gameObject;
		((rotationShip)instance.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
		((rotationShip)instance.GetComponent<rotationShip>()).super = false;
	
		if(ship.tag == "blue"){
			
			((rotationShip)instance.GetComponent<rotationShip>()).life = infoBlue.lifeShip;
			shipsB.Add(instance);
		}else if(ship.tag == "red"){
			((rotationShip)instance.GetComponent<rotationShip>()).life = infoRed.lifeShip;
			shipsR.Add(instance);
		}else{
			((rotationShip)instance.GetComponent<rotationShip>()).life = 3;
			shipsN.Add(instance);
		}
		
		/*if(ship.tag == "blue"){
			////Debug.Log(shipsBS.Count);
			if(shipsB.Count >= 10*shipsBS.Count + 10){
				GameObject shipGros = (GameObject)Instantiate(Resources.Load("ShipblueG")as GameObject, vec ,transform.rotation); 
				//GameObject instanceS = Resources.Load("Shipred")as GameObject;
				shipGros.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
				((rotationShip)shipGros.GetComponent<rotationShip>()).planet = this.gameObject;
				((rotationShip)shipGros.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
				shipGros.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
				for(int i = 0 + 10*shipsBS.Count; i<10 + 10*shipsBS.Count; i++){		
					shipsB[i].GetComponent<rotationShip>().speed = 0;
					shipsB[i].GetComponent<MeshRenderer>().enabled = false;
				}
				shipsBS.Add(shipGros);
			}
		}
		
		if(ship.tag == "red"){
			if(shipsR.Count >= 10*shipsRS.Count + 10){
				GameObject shipGros = (GameObject)Instantiate(Resources.Load("ShipredG")as GameObject, vec ,transform.rotation); 
				//GameObject instanceS = Resources.Load("Shipred")as GameObject;
				shipGros.GetComponent<rotationShip>().super = true;
				shipGros.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
				((rotationShip)shipGros.GetComponent<rotationShip>()).planet = this.gameObject;
				((rotationShip)shipGros.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
				shipGros.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
				
				for(int i = 0 + 10*shipsRS.Count; i<10 + 10*shipsRS.Count; i++){		
					shipsR[i].GetComponent<rotationShip>().speed = 0;
					shipsR[i].GetComponent<MeshRenderer>().enabled = false;
				}
				shipsRS.Add(shipGros);
			}
		}
		
		if(ship.tag == "neutre"){
			if(shipsN.Count >= 10*shipsNS.Count + 10){
				GameObject shipGros = (GameObject)Instantiate(Resources.Load("ShipNG")as GameObject, vec ,transform.rotation); 
				//GameObject instanceS = Resources.Load("Shipred")as GameObject;
				shipGros.GetComponent<rotationShip>().super = true;
				shipGros.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
				((rotationShip)shipGros.GetComponent<rotationShip>()).planet = this.gameObject;
				((rotationShip)shipGros.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
				shipGros.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
				
				for(int i = 0 + 10*shipsNS.Count; i<10 + 10*shipsNS.Count; i++){		
					shipsN[i].GetComponent<rotationShip>().speed = 0;
					shipsN[i].GetComponent<MeshRenderer>().enabled = false;
				}
				shipsNS.Add(shipGros);
			}
		}*/
		//refreshShip();
		
	}
	
	void createSuperShip(int color){
		float scal = this.transform.localScale.x ;
			
		float min = scal/2.5f +1 ;
		float max = scal/2.5f +1.5f;
			
		float z = Random.Range(min,max);
		Vector3 vec = new Vector3(0,0,z);
		vec = this.transform.position + vec ; 
		
		if(color == 0){//blue
			GameObject shipGros = (GameObject)Instantiate(Resources.Load("ShipblueG")as GameObject, vec ,transform.rotation); 
			//GameObject instanceS = Resources.Load("Shipred")as GameObject;
			shipGros.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
			((rotationShip)shipGros.GetComponent<rotationShip>()).planet = this.gameObject;
			((rotationShip)shipGros.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
			shipGros.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
			for(int i = 0 + 10*shipsBS.Count; i<10 + 10*shipsBS.Count; i++){		
				shipsB[i].GetComponent<rotationShip>().speed = 0;
				shipsB[i].GetComponent<MeshRenderer>().enabled = false;
			}
			shipsBS.Add(shipGros);
			
		}
		
		if(color == 1){//red
			GameObject shipGros = (GameObject)Instantiate(Resources.Load("ShipredG")as GameObject, vec ,transform.rotation); 
			//GameObject instanceS = Resources.Load("Shipred")as GameObject;
			shipGros.GetComponent<rotationShip>().super = true;
			shipGros.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
			((rotationShip)shipGros.GetComponent<rotationShip>()).planet = this.gameObject;
			((rotationShip)shipGros.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
			shipGros.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
			
			for(int i = 0 + 10*shipsRS.Count; i<10 + 10*shipsRS.Count; i++){		
				shipsR[i].GetComponent<rotationShip>().speed = 0;
				shipsR[i].GetComponent<MeshRenderer>().enabled = false;
			}
			shipsRS.Add(shipGros);
			
		}
		
		if(color == 2){//neutre
			GameObject shipGros = (GameObject)Instantiate(Resources.Load("ShipNG")as GameObject, vec ,transform.rotation); 
			//GameObject instanceS = Resources.Load("Shipred")as GameObject;
			shipGros.GetComponent<rotationShip>().super = true;
			shipGros.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
			((rotationShip)shipGros.GetComponent<rotationShip>()).planet = this.gameObject;
			((rotationShip)shipGros.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
			shipGros.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
			
			for(int i = 0 + 10*shipsNS.Count; i<10 + 10*shipsNS.Count; i++){		
				shipsN[i].GetComponent<rotationShip>().speed = 0;
				shipsN[i].GetComponent<MeshRenderer>().enabled = false;
			}
			shipsNS.Add(shipGros);
			
		}
		
	}
	
	//fonction qui gere le combat entre les vaisseaux
	void startFights(){
		infoUser infoUserB =(infoUser) GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>();
		infoUser infoUserR =(infoUser) GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>();
		
		if(shipsB.Count>0 && shipsR.Count>0){
			int iB = Random.Range(0,shipsB.Count-1);
			int iR = Random.Range(0,shipsR.Count-1);
			GameObject sb = shipsB[iB];
			GameObject sr = shipsR[iR];
			
			
			((rotationShip)sb.GetComponent<rotationShip>()).life -= Random.Range(infoUserR.powerMin,infoUserR.powerMax);
			
			((rotationShip)sr.GetComponent<rotationShip>()).life -= Random.Range(infoUserB.powerMin,infoUserB.powerMax); 
		
			if(((rotationShip)sb.GetComponent<rotationShip>()).life<=0){
				shipsB.RemoveAt(iB);
				GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
				expl.transform.position = sb.transform.position;
				user.GetComponent<stats>().destroyShipBBattle ++;
				
				Destroy(sb);	
			}
			if(((rotationShip)sr.GetComponent<rotationShip>()).life<=0){
				shipsR.RemoveAt(iR);
				GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
				expl.transform.position = sr.transform.position;
				user.GetComponent<stats>().destroyShipRBattle ++;
				Destroy(sr);
			}
		}else if(shipsB.Count <= 0){
			int iN = Random.Range(0,shipsN.Count-1);
			int iR = Random.Range(0,shipsR.Count-1);
			GameObject sn = shipsN[iN];
			GameObject sr = shipsR[iR];
			
			
			((rotationShip)sn.GetComponent<rotationShip>()).life -= Random.Range(infoUserR.powerMin,infoUserR.powerMax); 
			((rotationShip)sr.GetComponent<rotationShip>()).life -= Random.Range(2,7); 
			
			if(((rotationShip)sn.GetComponent<rotationShip>()).life<=0){
				shipsN.RemoveAt(iN);
				GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
				expl.transform.position = sn.transform.position;
				user.GetComponent<stats>().destroyShipNBattle ++;
				Destroy(sn);	
			}
			if(((rotationShip)sr.GetComponent<rotationShip>()).life<=0){
				shipsR.RemoveAt(iR);
				GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
				expl.transform.position = sr.transform.position;
				user.GetComponent<stats>().destroyShipRBattle ++;
				Destroy(sr);
			}
			
		}else if(shipsR.Count <= 0){
			int iN = Random.Range(0,shipsN.Count-1);
			int iB = Random.Range(0,shipsB.Count-1);
			GameObject sb = shipsB[iB];
			GameObject sn = shipsN[iN];
			
			
			((rotationShip)sb.GetComponent<rotationShip>()).life -= Random.Range(2,7); 
			((rotationShip)sn.GetComponent<rotationShip>()).life -= Random.Range(infoUserB.powerMin,infoUserB.powerMax); 
			
			if(((rotationShip)sb.GetComponent<rotationShip>()).life<=0){
				shipsB.RemoveAt(iB);
				GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
				expl.transform.position = sb.transform.position;
				user.GetComponent<stats>().destroyShipBBattle ++;
				Destroy(sb);	
			}
			if(((rotationShip)sn.GetComponent<rotationShip>()).life<=0){
				shipsN.RemoveAt(iN);
				GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
				expl.transform.position = sn.transform.position;
				user.GetComponent<stats>().destroyShipNBattle ++;
				Destroy(sn);
			}
		}
		
		//refreshShip();
		
	}
	
	public void refreshShip(){
	
		int t = 1 ;
		bool r = false;
		bool b = false;
		bool n = false;
		
		if(shipsR.Count == 0){
			r = true;	
		}
		if(shipsB.Count == 0){
			b = true;	
		}
		if(shipsN.Count == 0){
			n = true;	
		}
		
		while(!r || !b || !n){
			int s ;
			if((t*10 - 10)<0){
				s =0;	
			}else{
				s=(t*10 - 10);
			}
			if(!r){	
				if(shipsR.Count < 10*t){
					r = true;
					if(shipsRS.Count >= t){
						for(int i = t-1; i<shipsRS.Count ; i++){
							Destroy(shipsRS[0]);					
							shipsRS.RemoveAt(0);
							
						}
					}
					for(int j = s; j<t*10; j++){
						if(j < shipsR.Count){
							if(shipsR[j] != null){
								shipsR[j].GetComponent<MeshRenderer>().enabled = true;
								shipsR[j].GetComponent<rotationShip>().speed = Random.Range(5f,30f);
							}
						}
						
					}

				}else{
					if(shipsRS.Count < t){
						createSuperShip(1);
					}else{
						for(int i  = s; i<t*10; i++){
							if(shipsR[i] != null){
								shipsR[i].GetComponent<MeshRenderer>().enabled = false;
								shipsR[i].GetComponent<rotationShip>().speed = 0;
							}
						}
					}
				}
			}
			
			if(!b){
				if(shipsB.Count < 10*t){
					b = true;
					if(shipsBS.Count >= t){
						for(int i = t-1; i<shipsBS.Count ; i++){
							Destroy( shipsBS[0]);
							shipsBS.RemoveAt(0);
							
						}
					}
					
					for(int j = s; j<t*10; j++){
						if(j < shipsB.Count){
							if(shipsB[j] != null){
								shipsB[j].GetComponent<MeshRenderer>().enabled = true;
								shipsB[j].GetComponent<rotationShip>().speed = Random.Range(5f,30f);
							}
						}
						
					}

				}else{
					if(shipsBS.Count < t){
						createSuperShip(0);
					}else{
						for(int i  = s; i<t*10; i++){
							if(shipsB[i] != null){
								shipsB[i].GetComponent<MeshRenderer>().enabled = false;
								shipsB[i].GetComponent<rotationShip>().speed = 0;
							}
						}
					}	
				}
				
			}
			
			if(!n){	
				if(shipsN.Count < 10*t){
					n = true;
					
					if(shipsNS.Count >= t){
						for(int i = t-1; i<shipsNS.Count ; i++){
							Destroy(shipsNS[0]);
							shipsNS.RemoveAt(0);
							
						}
					}
					
					for(int j = s; j<t*10; j++){
						if(j < shipsN.Count){
							if(shipsN[j] != null){
								shipsN[j].GetComponent<MeshRenderer>().enabled = true;
								shipsN[j].GetComponent<rotationShip>().speed = Random.Range(5f,30f);
							}
						}
						
					}

				}else{
					if(shipsNS.Count < t){
						createSuperShip(2);
					}else{
						for(int i  = s; i<t*10; i++){
							if(shipsN[i] != null){
								shipsN[i].GetComponent<MeshRenderer>().enabled = false;
								shipsN[i].GetComponent<rotationShip>().speed = 0;
							}
						}
					}	
				}
			}
			
			t++;
			
		}
		
	}
	
	public void playSound(){
		audio.Play();	
	}
	
	
	
}
