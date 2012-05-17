using UnityEngine;
using System.Collections;

public class GUIPlanet : MonoBehaviour {
	
	public GUIStyle style;
	public GUISkin skin;
	private float x;
	private float y;
	private Vector3 vec;
	
	// Use this for initialization
	void Start () {
		
		style.font = skin.font;
		style.alignment = TextAnchor.UpperLeft;
		x = 14f;
		y = -8f;
		//style.alignment = TextAnchor.MiddleCenter;
		//style.alignment = TextAnchor.MiddleCenter;
	}
	
	
	void OnGUI(){
		
		vec = Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position);
		
		if(((PlanetScript)GetComponent<PlanetScript>()).ship != null){
			//rouge contre blanc
			if(((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count >0 && ((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count >0 ){
				GUIplanet(1,1,((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count);
				GUIplanet(2,2,((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count);
				
			}else if(((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count >0 && ((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count >0 ){
				GUIplanet(0,1,((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count);
				GUIplanet(1,2,((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count);
	
			//bleu contre blanc
			}else if(((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count >0 && ((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count >0 ){
				GUIplanet(0,1,((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count);
				GUIplanet(2,2,((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count);
	
			}else{
				if(((PlanetScript)GetComponent<PlanetScript>()).ship.tag == "red"){
					//Camera.mainCamera.ScreenToWorldPoint;
					if(((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count>0){
						GUIplanet(0,0,((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count);
					}else{
						GUIplanet(1,0,((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count);
					}
				}else if(((PlanetScript)GetComponent<PlanetScript>()).ship.tag == "blue"){
					if(((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count>0){
						GUIplanet(1,0,((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count);
					}else{
						GUIplanet(0,0,((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count);
					}
				}else{
					if(((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count>0){
						GUIplanet(1,0,((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count);
					}else if(((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count>0){
						GUIplanet(0,0,((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count);
					}else{
						GUIplanet(2,0,((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count);
					}
				}
			}
		}

	}
	
	void GUIplanet(int color, int position, int count){
		
		//on choisit la couleur utilisé
		if(color ==0){//blue
			style.normal.textColor = new Color(0,0,0.7f,1);	
		}else if(color == 1){ // red
			style.normal.textColor = new Color(0.7f,0,0,1);
		}else if(color == 2){//white
			style.normal.textColor = new Color(0.9f,0.9f,0.9f,1);
		}
		
		
		if(position == 0){//la planete n'est pas en combat
			if(count <10){
				GUI.Label (new Rect (vec.x-(x-6),Screen.height+y -vec.y,50, 35), ""+count, style);
			}else if(count >=100){
				GUI.Label (new Rect (vec.x-(x+6),Screen.height+y -vec.y,50, 35), ""+count, style);
			}else{
				GUI.Label (new Rect (vec.x-x,Screen.height+y -vec.y,50, 35), ""+count, style);
			}	
		}else if(position == 1){//la planete est en combat
			if(count <10){
				GUI.Label (new Rect (vec.x-(x+10),Screen.height+y -vec.y,50, 50), ""+count, style);
			}else if(count >=100){
				GUI.Label (new Rect (vec.x-(x+40),Screen.height+y -vec.y,50, 50), ""+count, style);
			}else{
				GUI.Label (new Rect (vec.x-(x+30),Screen.height+y -vec.y,50, 50), ""+count, style);
			}
			
			style.normal.textColor = new Color(0.7f,0.7f,0.7f,1);
			GUI.Label (new Rect (vec.x-(x-6),Screen.height+y -vec.y,50, 50), "/", style);
				
		}else if(position == 2){
			if(count <10){
				GUI.Label (new Rect (vec.x+15,Screen.height+y -vec.y,50, 50), ""+count, style);
			}else if(count >=100){
				GUI.Label (new Rect (vec.x+15,Screen.height+y -vec.y,50, 50), ""+count, style);
			}else{
				GUI.Label (new Rect (vec.x+15,Screen.height+y -vec.y,50, 50), ""+count, style);
			}

		}
	
	}

}
