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
		style.alignment = TextAnchor.UpperCenter;
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
			
				style.normal.textColor = new Color(0.7f,0,0,1);
				GUI.Label (new Rect (vec.x-x*2.5f,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count, style);
				
				style.normal.textColor = new Color(1f,1f,1f,1);
				GUI.Label (new Rect (vec.x-(x-10),Screen.height+y -vec.y,35, 35), "/", style);
				
				style.normal.textColor = new Color(1f,1f,1f,1);
				GUI.Label (new Rect (vec.x+15,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count, style);
			//rouge contre bleu
			}else if(((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count >0 && ((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count >0 ){
				style.normal.textColor = new Color(0.7f,0,0,1);
				GUI.Label (new Rect (vec.x-x*2.5f,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count, style);
				
				style.normal.textColor = new Color(1f,1f,1f,1);
				GUI.Label (new Rect (vec.x-(x-10),Screen.height+y -vec.y,35, 35), "/", style);
				
				style.normal.textColor = new Color(0f,0f,0.7f,1);
				GUI.Label (new Rect (vec.x+15,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count, style);
			//bleu contre blanc
			}else if(((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count >0 && ((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count >0 ){
				style.normal.textColor = new Color(0f,0,0.7f,1);
				GUI.Label (new Rect (vec.x-x*2.5f,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count, style);
				
				style.normal.textColor = new Color(1f,1f,1f,1);
				GUI.Label (new Rect (vec.x-(x-10),Screen.height+y -vec.y,35, 35), "/", style);
				
				style.normal.textColor = new Color(1f,1f,1f,1);
				GUI.Label (new Rect (vec.x+15,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count, style);
			}else{
				if(((PlanetScript)GetComponent<PlanetScript>()).ship.tag == "red"){
					//Camera.mainCamera.ScreenToWorldPoint;
					if(((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count>0){
						GUIBlue();
					}else{
						GUIRed();
					}
				}else if(((PlanetScript)GetComponent<PlanetScript>()).ship.tag == "blue"){
					if(((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count>0){
						GUIRed();
					}else{
						GUIBlue();
					}
				}else{
					if(((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count>0){
						GUIRed();
					}else if(((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count>0){
						GUIBlue();
					}else{
						GUINeutre();
					}
				}
			}
		}else{
			
		}
		//GUI.Label (Rect (10, 10, 100, 20), "Hello World!");
		
		
	}
	
	void GUIRed(){
		style.normal.textColor = new Color(0.7f,0,0,1);
		GUI.Label (new Rect (vec.x-x,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsR.Count, style);
	}
	
	void GUIBlue(){
		style.normal.textColor = new Color(0,0,0.7f,1);
		GUI.Label (new Rect (vec.x-x,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsB.Count, style);	
	}
	
	void GUINeutre(){
		style.normal.textColor = new Color(0.9f,0.9f,0.9f,1);
		GUI.Label (new Rect (vec.x-x,Screen.height+y -vec.y,35, 35), ""+((PlanetScript)GetComponent<PlanetScript>()).shipsN.Count, style);
	}
	// Update is called once per frame
	void Update () {
		
			
	
			
	}
}
