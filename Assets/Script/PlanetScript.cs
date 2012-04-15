using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlanetScript : MonoBehaviour {
	
	
	public int nbShip; //nombre de vaisseaux que possede le vaisseaux au debut de la partie
	public List<GameObject> shipsR; //liste des vaisseaux rouge sur la planete
	public List<GameObject> shipsB; //liste des vaisseaux bleu sur la planete
	public GameObject ship; //Object vaisseaux correspondant au type de vaisseaux que doit construire la planete
	public bool fights; //declanche le combat 
	public int vFight ; //delai entre chaque resolution de combat
	public int repop; // delai entre chaque creation de vaiseaux
	private int count; //timer entre chaque resolution de combat
	private int timePop; //timer entre chaque creation de vaisseaux
	//public GameObject explosion; //object de l'explosion a instantié
	public int LimitPop ;//Limite de population
	public int CaptureCount; //temps de capture requis
	private int CaptureTmp;
	public int SpeedCapture; //vitesse de capture
	public int CaptureTime; //temps e capture en cour
	
	// Use this for initialization
	void Start () {
		
		//LimitPop =(int) this.transform.localScale.x * 2; 
		//Debug
		LimitPop = 50 ;
		vFight = 10;
		repop = 50;
		CaptureCount = 255;
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
				gameObject.light.color = new Color(255,0,0,1);
				CaptureTime = CaptureCount;
			}else if (ship.tag == "blue"){
				gameObject.light.color = new Color(0,0,255,1);
				CaptureTime = -1*CaptureCount;
			}	
		}else{
			gameObject.light.color = new Color(255,255,255,1);
			CaptureTime = 0;
		}
		
		gameObject.light.intensity = 2;
		gameObject.light.range = gameObject.transform.localScale.x * 1.3f;
	}
	
	// Update is called once per frame
	void Update () {
		float t = Mathf.PingPong(Time.time, 1f) / 1f;
        light.color = Color.Lerp(Color.red, Color.blue, t);
		//rotation de la planete
		this.transform.RotateAround(this.transform.position,Vector3.up, 0.1f);
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
					//Debug.Log("capture bleu en cour: "+CaptureTime);
					CaptureTmp = 0;
					CaptureTime -= 1;
					//changement de couleur du halo
					if(CaptureTime <0){
						//gameObject.light.color = new Color(255+CaptureTime,255+CaptureTime,255,1);
						
					}else{
						//gameObject.light.color = new Color(255,255-CaptureTime,255-CaptureTime,1);
						
					}
					Debug.Log("red: "+gameObject.light.color.r+" green: "+gameObject.light.color.g+" blue: "+gameObject.light.color.b);
					
					
					if(CaptureTime <= -1*CaptureCount){
						ship = shipsB[0];
						//Debug.Log("capture bleu terminé: "+CaptureTime);
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
					//Debug.Log("capture rouge en cour: "+CaptureTime);
					CaptureTmp = 0;
					CaptureTime += 1;
					if(CaptureTime >= CaptureCount){
						ship = shipsR[0];
						//Debug.Log("capture rouge terminé: "+CaptureTime);
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
		
		
		
		
		
		
		
		/*if(shipsB.Count >0 && shipsR.Count >0){
			count ++;	
			if(count >= vFight){
				count = 0;
				startFights();
				CaptureTmp = 0;
			}
	
		}else{
			
			if(ship == null){
				if(shipsB>0 && shipR>0){
					
					
					
				}
				
				
				if(shipsB.Count >0){
					ship = shipsB[0];
					gameObject.light.color = Color.blue;
					//ship =(GameObject)  Resources.Load("Shipblue");
				}else if(shipsR.Count >0){
					ship = shipsR[0];
					gameObject.light.color = Color.red;	
				}
				
			}
			 
			
			//partie creation de vaisseaux
			if(ship != null){
				timePop ++;
		
				if(timePop >= repop){
		
					timePop = 0;
				
					if(ship.tag == "red"){
					
						if(shipsR.Count < LimitPop){
					
							createShip();
					
						}
					}else{
					
						if(shipsB.Count < LimitPop){
					
							createShip();
					
						}
					
					}

				}
			}
		}*/
			
			
	
	}
	
	//fonction qui gere la creation des vaisseaux 
	void createShip (){
		
		float scal = this.transform.localScale.x ;
			
		float min = scal/2.5f  ;
		float max = scal/2.5f +1;
			
		float x = Random.Range(min,max);
		float z = Random.Range(min,max);
		Vector3 vec = new Vector3(x,0,z);
		vec = this.transform.position + vec ; 
			
		
	
			
			
			
			
		GameObject instance = (GameObject) Instantiate(ship,vec, transform.rotation);
		instance.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
		((rotationShip)instance.GetComponent<rotationShip>()).planet = this.gameObject;
		((rotationShip)instance.GetComponent<rotationShip>()).speed = Random.Range(0.01f,0.1f);
		
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
			ship = shipsR[0];
			gameObject.light.color = Color.red;
			
		}else if(shipsR.Count ==0){
			ship = shipsB[0];
			gameObject.light.color = Color.blue;
		}
		
		Destroy(sb);
		Destroy(sr);
		
								
							
		/*Vector3 vb = sb.transform.position;
		Quaternion rb = sb.transform.rotation;
							
		Vector3 vr = sr.transform.position;
		Quaternion rr = sr.transform.rotation;*/
		
								
		
		//Instantiate(explosion, vb, rb);
		//Instantiate(explosion, vr, rr);
		
		
	}
	
	
}
