using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class attackPlanet : MonoBehaviour {
	
	public GameObject planetStart;
//	public GameObject planetEnd;
//	public List<GameObject> ships;
//	public bool triger ; 
//	public bool start;
//	public bool build;
//	public GameObject bul;
//	private int sizeHalo;
	

	// Use this for initialization
	void Start () {
//		triger = false;
//		start = false;
		
		
	
	}
	
	void Update() {
		
		foreach(Touch touch in Input.touches) {
			if(touch.phase == TouchPhase.Ended) 
				//Instantiate (planetStart,Camera.main.ScreenToWorldPoint(touch.position),transform.rotation);
				Debug.Log(Camera.main.ScreenToWorldPoint(touch.position));
		}
	}
	
	/* void OnGUI() {
        Event e = Event.current;
		
		//Debug.Log(e.mousePosition.x);
		
	
			
			//e.mousePosition.x
	
        if (e.isKey){
			
			//deplacement ships
            if(e.keyCode == KeyCode.Space){
				
				triger = true;
			}
			//placement millitaire
            if(e.keyCode == KeyCode.C){
				
				build = true;
			}
			
		}  
    }
	
	void fight(){
		
		((PlanetShip)planetEnd.GetComponent<PlanetShip>()).fights = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		float scal = planetEnd.transform.localScale.x ;
				
		float min = scal/2.5f  ;
		float max = scal/2.5f +5;
		
		if(triger){
			
			
		
			ships = ((PlanetShip)planetStart.GetComponent<PlanetShip>()).ships;
		
			for(int i = 0 ; i<ships.Count ; i++){
			
				((rotationShip)ships[i].GetComponent<rotationShip>()).speed = 0;
				((rotationShip)ships[i].GetComponent<rotationShip>()).planet = planetEnd;
			}
			start = true;
		}
		
		if(start){
		
			for(int j = 0 ; j<ships.Count -1; j++){
				
				
				
				
				
			
				float x = Random.Range(min,max);
				float y = Random.Range(min,max);
				Vector3 vec = new Vector3(x,y,0);
				 
				iTween.MoveTo(ships[j],iTween.Hash("position",planetEnd.transform.position+vec,"time",2f, "easetype", "linear"));	
				
				
				((rotationShip)ships[j].GetComponent<rotationShip>()).speed = Random.Range(0.5f,0.8f);
				((PlanetShip)planetEnd.GetComponent<PlanetShip>()).ships.Add(ships[j]);
				
				
				
				
			}
			
			
			Vector3 vec1 = new Vector3((float)Random.Range(min,max),(float) Random.Range(min,max),0);
			iTween.MoveTo(ships[ships.Count -1],iTween.Hash("position",planetEnd.transform.position+vec1,"time",2f,"oncomplete","fight","onCompleteTarget", gameObject, "easetype", "linear"));	
				
				
			((rotationShip)ships[ships.Count -1].GetComponent<rotationShip>()).speed = Random.Range(0.5f,0.8f);
			((PlanetShip)planetEnd.GetComponent<PlanetShip>()).ships.Add(ships[ships.Count -1]);
			
			start = false;
			triger = false;
		}
		
		if(build){
			
			Vector3 vec = new Vector3(0.7f, 0,-0.7f);
			
			
			//this.planetStart.i
			
			GameObject instance = (GameObject) Instantiate(bul,vec+planetStart.transform.position,transform.rotation);
			//planetStart.AddComponent(GameObject) = instance; 
		}
			
		
			
		
		 
		
		
	
	}*/
	
	
}
