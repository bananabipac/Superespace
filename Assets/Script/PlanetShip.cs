using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetShip : MonoBehaviour {
	
	
	public int nbShip; //nombre de vaisseaux que possede le vaisseaux au debut de la partie
	public List<GameObject> shipsR; //liste des vaisseaux rouge sur la planete
	public List<GameObject> shipsB; //liste des vaisseaux bleu sur la planete
	public GameObject ship; //Object vaisseaux correspondant au type de vaisseaux que doit construire la planete
	public bool fights; //declanche le combat 
	public int vFight ; //delai entre chaque resolution de combat
	public int repop; // delai entre chaque creation de vaiseaux
	private int count; //timer entre chaque resolution de combat
	private int timePop; //timer entre chaque creation de vaisseaux
	public GameObject explosion; //object de l'explosion a instantié
	public int LimitPop ;//Limite de population

	// Use this for initialization
	void Start () {
		fights = false;
		//LimitPop =(int) this.transform.localScale.x * 2; 
		count = 0;
		timePop = 0;
		vFight = 10;
		repop = 50;
		//creation des vaisseaux au depart 
		for(int i = 0 ; i<nbShip ; i++){
			createShip();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//rotation de la planete
		this.transform.RotateAround(this.transform.position,Vector3.up, 0.1f);
		
		
		
		if(shipsB.Count >0 && shipsR.Count >0){
			
			count ++;	
			if(count >= vFight){
				count = 0;
				
				startFights();
			
			}
	
			
			
		}else{
			
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
		}
			
			
	
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
			
		}else if(shipsR.Count ==0){
			ship = shipsB[0];
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
