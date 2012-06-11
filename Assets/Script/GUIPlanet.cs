using UnityEngine;
using System.Collections;

public class GUIPlanet : MonoBehaviour {
	
	public GUIStyle style;
	public GUISkin skin;
	private float x;
	private float y;
	private Vector3 vec;
	private GameObject user;
	//private int nbShipsB;
	//private int nbShipsR;
	//private int nbShipsN;
	
	// Use this for initialization
	void Start () {
		
		style.font = skin.font;
		style.alignment = TextAnchor.UpperLeft;
		x = 14f;
		y = -8f;
		user = GameObject.FindWithTag("User");
		//PlanetScript tmp = GetComponent<PlanetScript>();
		//nbShipsB = tmp.shipsB.Count;
		//nbShipsR = tmp.shipsR.Count;
		//nbShipsN = tmp.shipsN.Count;
		
	}
	
	
	void OnGUI(){
		int shipsR;
		int shipsB;
		int shipsN;
		if(!user.GetComponent<PauseScript>().paused) {
			PlanetScript planet = GetComponent<PlanetScript>();
			shipsR = planet.shipsR.Count;
			shipsB = planet.shipsB.Count;
			shipsN = planet.shipsN.Count;
			vec = Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position);
			
			if(planet.ship != null){
				
				//rouge contre bleu
				if(shipsR >0 && shipsB >0 ){
					GUIplanet(1,1,shipsR);
					GUIplanet(0,2,shipsB);
				
				//rouge contre blanc
				}else if(shipsR >0 && shipsN >0 ){
					GUIplanet(1,1,shipsR);
					GUIplanet(2,2,shipsN);
		
				//bleu contre blanc
				}else if(shipsN >0 && shipsB >0 ){
					GUIplanet(2,1,shipsN);
					GUIplanet(0,2,shipsB);
					
		
				}else{
					if(planet.ship.tag == "red"){
						//Camera.mainCamera.ScreenToWorldPoint;
						if(shipsB>0){
							GUIplanet(0,0,shipsB);
						}else{
							GUIplanet(1,0,shipsR);
						}
					}else if(planet.ship.tag == "blue"){
						if(shipsR>0){
							GUIplanet(1,0,shipsR);
						}else{
							GUIplanet(0,0,shipsB);
						}
					}else{
						if(shipsR>0){
							GUIplanet(1,0,shipsR);
						}else if(shipsB>0){
							GUIplanet(0,0,shipsB);
						}else{
							GUIplanet(2,0,shipsN);
						}
					}
				}
			}
		}

	}
	
	void GUIplanet(int color, int position, int count){
		Matrix4x4 matrixBackup = GUI.matrix;
		//on choisit la couleur utilisé
		
		if(color ==0){//blue
			style.normal.textColor = new Color(0,0,0.7f,1);	
		}else if(color == 1){ // red
			style.normal.textColor = new Color(0.7f,0,0,1);
		}else if(color == 2){//white
			style.normal.textColor = new Color(0.9f,0.9f,0.9f,1);
			
		}
		
		
		if(position == 0){//la planete n'est pas en combat
			if(color ==0){//blue
				if(count <10){
					GUIUtility.RotateAroundPivot(-90f,new Vector3((vec.x-x)+14, (Screen.height+y -vec.y)+8, 5));
					GUI.Label (new Rect (vec.x-(x-6),Screen.height+y -vec.y,50, 35), ""+count, style);
				}else if(count >=100){
					GUIUtility.RotateAroundPivot(-90f,new Vector3((vec.x-x)+15, (Screen.height+y -vec.y)+7.5f, 5));
					GUI.Label (new Rect (vec.x-(x+6),Screen.height+y -vec.y,50, 35), ""+count, style);
				}else{
					GUIUtility.RotateAroundPivot(-90f,new Vector3((vec.x-x)+14, (Screen.height+y -vec.y)+8, 5));
					GUI.Label (new Rect (vec.x-x,Screen.height+y -vec.y,50, 35), ""+count, style);					
				}
			}else if(color == 1){
				if(count <10){
					GUIUtility.RotateAroundPivot(90f,new Vector3((vec.x-x)+14, (Screen.height+y -vec.y)+8, 5));
					GUI.Label (new Rect (vec.x-(x-6),Screen.height+y -vec.y,50, 35), ""+count, style);
				}else if(count >=100){
					GUIUtility.RotateAroundPivot(90f,new Vector3((vec.x-x)+14, (Screen.height+y -vec.y)+8, 5));
					GUI.Label (new Rect (vec.x-(x+6),Screen.height+y -vec.y,50, 35), ""+count, style);
				}else{
					GUIUtility.RotateAroundPivot(90f,new Vector3((vec.x-x)+14, (Screen.height+y -vec.y)+8, 5));
					GUI.Label (new Rect (vec.x-x,Screen.height+y -vec.y,50, 35), ""+count, style);
				}
				
			}else if(color == 2){				
				if(count <10){
					GUI.Label (new Rect (vec.x-(x-6),Screen.height+y -vec.y,50, 35), ""+count, style);
				}else if(count >=100){
					GUI.Label (new Rect (vec.x-(x+6),Screen.height+y -vec.y,50, 35), ""+count, style);
				}else{
					GUI.Label (new Rect (vec.x-x,Screen.height+y -vec.y,50, 35), ""+count, style);
				}		
			}
		
			GUI.matrix = matrixBackup;
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
