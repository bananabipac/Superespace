using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetShip : MonoBehaviour {
	
	
	public int nbShip; //nombre de vaisseaux que possede le vaisseaux au debut de la partie
	public List<GameObject> ships; //liste des vaisseaux sur la planete
	
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
		LimitPop =(int) this.transform.localScale.x * 2; 
		count = 0;
		timePop = 0;
		
		//creation des vaisseaux au depart 
		for(int i = 0 ; i<nbShip ; i++){
			createShip();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//rotation de la planete
		this.transform.RotateAround(this.transform.position,Vector3.up, 0.2f);
		
		
		//partie gestion du combat
		if(fights){	
			startFights();
		}
		
		//partie creation de vaisseaux
		if(ship != null){
			timePop ++;
		
			if(timePop >= repop){
		
				timePop = 0;
			
				if(ships.Count < LimitPop){
					
					createShip();
					
				}
			}
		}
			
	
	}
	
	//fonction qui gere la creation des vaisseaux 
	void createShip (){
		
		float scal = this.transform.localScale.x ;
			
		float min = scal/2.5f  ;
		float max = scal/2.5f +5;
			
		float x = Random.Range(min,max);
		float z = Random.Range(min,max);
		Vector3 vec = new Vector3(x,0,z);
		vec = this.transform.position + vec ; 
			
		
	
			
			
			
			
		GameObject instance = (GameObject) Instantiate(ship,vec, transform.rotation);
		instance.transform.RotateAround(this.transform.position,Vector3.up, Random.Range(0f,360f));
		((rotationShip)instance.GetComponent<rotationShip>()).planet = this.gameObject;
		((rotationShip)instance.GetComponent<rotationShip>()).speed = Random.Range(0.5f,0.8f);
		
		ships.Add(instance);
		
	}
	
	//fonction qui gere le combat entre les vaisseaux
	void startFights(){

		count ++;
		if(count >= vFight){
			count = 0;	
			if(ships.Count >=2){
				int i = 0;
				bool end = false;
				
				while(i<ships.Count && end ==false){
					int j = i+1;
					while(j<ships.Count && end == false){
						
						if(ships[i].tag != ships[j].tag){
							GameObject si = ships[i];
							GameObject sj = ships[i];	
						
							
							ships.RemoveAt(i);
							ships.RemoveAt(j);
								
							
							Vector3 vi = si.transform.position;
							Quaternion ri = si.transform.rotation;
								
							Vector3 vj = sj.transform.position;
							Quaternion rj = sj.transform.rotation;
								
							Destroy(si);
							Destroy(sj);
							Instantiate(explosion, vi, ri);
							Instantiate(explosion, vj, rj);
								
							end = true;
						}else{
							j++;		
						}
						
					}	
				i++;
				}	
				
			}
		}
		
		ship = ships[0].gameObject;
		
		fights = false ;
		
	}
}
