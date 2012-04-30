using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlanetScript : MonoBehaviour {
	
	
	public int nbShip; //nombre de vaisseaux que possede le vaisseaux au debut de la partie
	public List<GameObject> shipsR; //liste des vaisseaux rouge sur la planete
	public List<GameObject> shipsB; //liste des vaisseaux bleu sur la planete
	public GameObject ship; //Object vaisseaux correspondant au type de vaisseaux que doit construire la planete
	private int vFight ; //delai entre chaque resolution de combat
	public int repop; // delai entre chaque creation de vaiseaux
	private int count; //timer entre chaque resolution de combat
	private int timePop; //timer entre chaque creation de vaisseaux
	//public GameObject explosion; //object de l'explosion a instantié
	public int LimitPop ;//Limite de population
	private int CaptureCount; //temps de capture requis
	private int CaptureTmp;
	private int SpeedCapture; //vitesse de capture
	private float CaptureTime; //temps e capture en cour
	
	//pulsation
	/*private bool pul;//triger pour la pulsation	
	private float sizePul;//taille de la pulsation
	private float countPul;//taille de la pulsation en cour
	private bool invertPul; //pulsation inverse*/
	
	// Use this for initialization
	void Start () {
		
		/*pul = false;
		sizePul = 1;
		countPul = 0;
		invertPul = false;*/
		
		vFight = 8;
		CaptureCount = 1;
		SpeedCapture = 50;
		
		//END Debug
		
		
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
			}	
		}else{
			gameObject.light.color = new Color(1,1,1,1);
			CaptureTime = 0;
		}
		
		gameObject.light.intensity = 2;
		gameObject.light.range = gameObject.transform.localScale.x * 1.3f;
	}
	
	// Update is called once per frame
	void Update () {
		//si la pulsation est enclenché
		/*if(pul){
			
			if(invertPul){
				countPul -= 0.02f;
				gameObject.light.range += 0.04f;
			}else{
				
				countPul += 0.02f;
				
				if(ship.tag == "red"){
					gameObject.light.color = new Color(1,countPul,countPul,1-countPul);
				}
				if(countPul>=sizePul){
					invertPul = true;	
				}
			
			}
			
			countPul = 0;
			if(ship.tag == "red"){
				gameObject.light.color = new Color(1,0,0,1);
				gameObject.light.range = gameObject.transform.localScale.x * 1.3f;
			}
		}*/
		
		//rotation de la planete
		this.transform.RotateAround(this.transform.position,Vector3.up, 0.1f);
		
		//Debug.Log("Planete : "+gameObject.name+" /shipRed : "+shipsR.Count+" /shipBlue : "+shipsB.Count);
		
		if(shipsB.Count > 0 && shipsR.Count >0){//bataille entre les vaissseaux
			count ++;	
			if(count >= vFight){
				count = 0;
				startFights();
				CaptureTmp = 0;
			}
			
		}else if (shipsB.Count>0){//vaisseau bleu present sur la planete
			if(ship == null || ship.tag =="red"){//capture
				CaptureTmp += 1*shipsB.Count;
				if(CaptureTmp >=SpeedCapture){
					
					CaptureTmp = 0;
					CaptureTime -= 0.02f;
					//changement de couleur du halo
					if(CaptureTime <0){
						gameObject.light.color = new Color(1+CaptureTime,1+CaptureTime,1,1);
						
					}else{
						gameObject.light.color = new Color(1,1-CaptureTime,1-CaptureTime,1);
						
					}
					
					
					if(CaptureTime <= -1*CaptureCount){
					
						ship =  Resources.Load("Shipblue")as GameObject;
						gameObject.light.color = new Color(0,0,1,1);
						CaptureTime = -1*CaptureCount;
						Debug.Log("Planete : "+gameObject.name + "capture blue");
					
					}
				}
				
			}else{//creation ship
				timePop ++;
				if(timePop >= repop){
					timePop = 0;
					if(shipsB.Count < LimitPop){
						createShip();
					}
				}
			}
			
		}else if (shipsR.Count>0){//vaisseau rouge present sur la planete
			if(ship == null || ship.tag =="blue"){
				CaptureTmp += 1*shipsR.Count;
				if(CaptureTmp >=SpeedCapture){
					
					CaptureTmp = 0;
					CaptureTime += 0.02f;
					
					
					if(CaptureTime <0){
						gameObject.light.color = new Color(1+CaptureTime,1+CaptureTime,1,1);
						
					}else{
						gameObject.light.color = new Color(1,1-CaptureTime,1-CaptureTime,1);
						
					}
					
					if(CaptureTime >= CaptureCount){
						
						ship =  Resources.Load("Shipred")as GameObject;
						gameObject.light.color = new Color(1,0,0,1);
						CaptureTime = CaptureCount;
						Debug.Log("Planete : "+gameObject.name + "capture red");
						
						//pul = true;
					}
				}
			}else{
				timePop ++;
				if(timePop >= repop){
					timePop = 0;
					if(shipsR.Count < LimitPop){
						createShip();
					}
				}
			}
		}
		
		
		
		
		
		
		
		
	}
	
	//fonction qui gere la creation des vaisseaux 
	void createShip (){
		
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
		
		if(ship.tag == "blue"){
			shipsB.Add(instance);
		}else{
			shipsR.Add(instance);
		}
		
		
	}
	
	//fonction qui gere le combat entre les vaisseaux
	void startFights(){

		GameObject sb = shipsR[0];
		GameObject sr = shipsB[0];	

		shipsB.RemoveAt(0);
		shipsR.RemoveAt(0);

		if(shipsB.Count == 0 && shipsR.Count == 0){
			ship = null;
		}else if(shipsB.Count ==0){
			//ship = shipsR[0];
			//gameObject.light.color = Color.red;
			
		}else if(shipsR.Count ==0){
			//ship = shipsB[0];
			//gameObject.light.color = Color.blue;
		}
		
		Destroy(sb);
		Destroy(sr);
	
	}
	
	
}
