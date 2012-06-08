using UnityEngine;
using System.Collections;

public class asteroidScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/
	
	void OnTriggerEnter(Collider other) {
		rotationShip tmp = other.gameObject.GetComponent<rotationShip>();
		
		float scal = tmp.planet.transform.localScale.x ;
				
		float min =  scal/2.5f+1   ;
		float max = scal/2.5f +1.5f;
		
		float z = Random.Range(min,max);
			
		Quaternion quat = Quaternion.AngleAxis(Random.Range(0f, 360f), tmp.planet.transform.position);
				
		Vector3 vec = new Vector3(0,0,z);
		vec = quat * vec ;
		vec.y = 0;
		iTween.Stop(other.gameObject);
		
		iTween.MoveTo(other.gameObject,iTween.Hash("position",tmp.planet.transform.position+vec,"time",8f,"oncomplete","valideDep","onCompleteTarget", gameObject,"oncompleteparams", other.gameObject, "easetype", "linear"));	
		
    }
	
	
	void valideDep(GameObject ship){
			
		if(ship != null){
			rotationShip tmp = ship.GetComponent<rotationShip>();
			((rotationShip)ship.GetComponent<rotationShip>()).speed = Random.Range(5f,30f);
			if(ship.tag == "red"){
				
				((PlanetScript)tmp.planet.GetComponent<PlanetScript>()).shipsR.Add(ship);
				
			}else{
			
				((PlanetScript)tmp.planet.GetComponent<PlanetScript>()).shipsB.Add(ship);
			}
		}else{
			//Debug.Log("erreu : "+i);	
		}

	}
}
