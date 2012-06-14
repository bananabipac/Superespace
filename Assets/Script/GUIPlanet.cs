using UnityEngine;
using System.Collections;

public class GUIPlanet : MonoBehaviour {
	
	public GUIStyle style;
	public GUISkin skin;
	private float x;
	private float y;
	private GameObject user;
	
	
	private int shipsRT;
	private int shipsBT;
	private int shipsNT;
	
	private float refresh;
	private float tmp;
	
	private PlanetScript planet;
	
	private GameObject textR;
	private GameObject textB;
	private GameObject textN;
	private GameObject textVs;
	
	private bool boolR;
	private bool boolB;
	private bool boolN;
	
	private Vector3 vecR;
	
	// Use this for initialization
	void Start () {
		
		style.font = skin.font;
		style.alignment = TextAnchor.UpperLeft;
		x = 14f;
		y = -8f;
		user = GameObject.FindWithTag("User");
		planet = GetComponent<PlanetScript>();
		//PlanetScript tmp = GetComponent<PlanestScript>();
		shipsBT = planet.shipsB.Count;
		shipsRT = planet.shipsR.Count;
		shipsNT = planet.shipsN.Count;
		
		refresh = 0.2f;
		tmp = 0;
		vecR = new Vector3(gameObject.transform.position.x, -18, gameObject.transform.position.z);
		
		textR =  Resources.Load("TextSelectRed")as GameObject;
		textR.transform.position = vecR;
		textR = (GameObject) Instantiate(textR);
		textR.transform.RotateAround(textR.transform.position,Vector3.up, 90);
		boolR = true;
		
		textB =  Resources.Load("TextSelectBlue")as GameObject;
		textB.transform.position = vecR;
		textB = (GameObject) Instantiate(textB);
		textB.transform.RotateAround(textB.transform.position,Vector3.up, -90);
		boolB = true;
		
		textN =  Resources.Load("TextSelectNeutre")as GameObject;
		textN.transform.position = vecR;
		textN = (GameObject) Instantiate(textN);
		boolN = false;
		
		textVs =  Resources.Load("TextSelectNeutre")as GameObject;
		textVs.transform.position = vecR;
		textVs = (GameObject) Instantiate(textVs);
		textVs.active = false;
		
		
		
		if(shipsRT >0){
			textB.active = false;
			textN.active = false;
			((TextMesh)textR.GetComponent<TextMesh>()).text = ""+shipsRT;
		}
		
		if(shipsBT >0){
			textR.active = false;
			textN.active = false;
			((TextMesh)textB.GetComponent<TextMesh>()).text = ""+shipsBT;
		}
		
		if(shipsNT >0){
			textR.active = false;
			textB.active = false;
			((TextMesh)textN.GetComponent<TextMesh>()).text = ""+shipsNT;
		}
		
		((TextMesh)textVs.GetComponent<TextMesh>()).text = "/";
		
	}
	
	
	
	void refreshGUI(){
		if(!user.GetComponent<PauseScript>().paused) {
			//int shipsRT = 
			int shipsR = planet.shipsR.Count;
			int shipsB = planet.shipsB.Count;
			int shipsN = planet.shipsN.Count;
			
			if(shipsBT != shipsB){
				shipsBT = planet.shipsB.Count;
				((TextMesh)textB.GetComponent<TextMesh>()).text = ""+shipsB;	
			}
			
			if(shipsRT != shipsR){
				shipsRT = planet.shipsR.Count;
				((TextMesh)textR.GetComponent<TextMesh>()).text = ""+shipsR;	
			}
			
			if(shipsNT != shipsN && boolN == false){
				shipsNT = planet.shipsN.Count;
				((TextMesh)textN.GetComponent<TextMesh>()).text = ""+shipsN;	
			}
			
			if(planet.ship != null){
				
				//rouge contre bleu
				if(shipsR >0 && shipsB >0 ){
					if(boolR == true){
						GUIplanet(1,1);	
					}
					if(boolB == true){
						GUIplanet(0,2);	
					}
				//rouge contre blanc
				}else if(shipsR >0 && shipsN >0 ){
					if(boolR == true){
						GUIplanet(1,1);	
					}
					if(boolN == true){
						GUIplanet(2,2);	
					}
				//bleu contre blanc
				}else if(shipsN >0 && shipsB >0 ){
					if(boolB == true){
						GUIplanet(0,1);	
					}
					if(boolN == true){
						GUIplanet(2,2);	
					}
				}else{
					if(planet.ship.tag == "red"){
						//Camera.mainCamera.ScreenToWorldPoint;
						if(shipsB>0){
							if(boolB == false){
								GUIplanet(0,0);	
							}
						}else if(boolR ==false){
							GUIplanet(1,0);
							Debug.Log("red");
						}
					}else if(planet.ship.tag == "blue"){
						if(shipsR>0 ){
							if(boolR == false){
								GUIplanet(1,0);
							}
						}else if(boolB ==false){
							GUIplanet(0,0);
						}
					}else{
						if(shipsR>0){
							if(boolR ==false){
								GUIplanet(1,0);
							}
						}else if(shipsB>0 ){
							if(boolB == false){
								GUIplanet(0,0);
							}
						}else{
							if(textN.active == false){
								GUIplanet(2,0);
							}
						}
					}
				}
			}
		}

	}
	
	void Update(){
		/*tmp	+= 1*Time.deltaTime;
		
		if(tmp >=refresh){
			tmp = 0;*/
			refreshGUI();
			
			
		//}
		
	}
	
	void GUIplanet(int color, int position){
		//GameObject gui; 
		Vector3 vec = Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position);
		/*if(color ==0){//blue
			gui = textB;
		}else if(color == 1){ // red
			gui = textR;
		}else if(color == 2){//white
			gui = textN;
		}*/

		if(position == 0){//la planete n'est pas en combat
			if(color ==0){//blue
				if(boolB == false){
					textB.active = true;
					textR.active = false;
					textN.active = false;
					textVs.active = false;
					textB.transform.RotateAround(textB.transform.position,Vector3.up, -90);
					textB.transform.position = vecR;
					boolB = true;
				}
			}else if(color == 1){
				if(boolR == false){
					textR.active = true;
					textB.active = false;
					textN.active = false;
					textVs.active = false;
					textR.transform.RotateAround(textR.transform.position,Vector3.up, 90);
					textR.transform.position = vecR;
					boolR = true;
				}
				
			}else if(color == 2){
				if(boolN == true){
					textR.active = false;
					textB.active = false;
					textN.active = true;
					textVs.active = false;
					boolN = false;
					textN.transform.position = vecR;
				}
			}
		}else if(position == 1){//la planete est en combat
			if(color ==0){//blue
				if(boolB == true){
					textB.active = true;
					textR.active = false;
					textN.active = false;
					textB.transform.RotateAround(textB.transform.position,Vector3.up, 90);
					boolB = false;
					Vector3 ve = new Vector3(textB.transform.position.x-1.2f , textB.transform.position.y, textB.transform.position.z);
					textB.transform.position = ve;
				}
			}else if(color == 1){
				if(boolR == true){
					textR.active = true;
					textB.active = false;
					textN.active = false;
					textR.transform.RotateAround(textR.transform.position,Vector3.up, -90);
					boolR = false;
					Vector3 ve = new Vector3(textR.transform.position.x-1.2f , textR.transform.position.y, textR.transform.position.z);
					textR.transform.position = ve;
				}
				
			}
			if(boolN == false){
				textVs.active = true;;
				boolN = true;
			}
				
		}else if(position == 2){
			if(color ==0){//blue
				if(boolB == true){
					textB.active = true;
					textB.transform.RotateAround(textB.transform.position,Vector3.up, 90);
					boolB = false;
					Vector3 ve = new Vector3(textB.transform.position.x+1.4f , textB.transform.position.y, textB.transform.position.z);
					textB.transform.position = ve;
				}
			}else if(color == 1){
				if(boolR == false){
					textR.active = true;
					textR.transform.RotateAround(textR.transform.position,Vector3.up, -90);
					boolR = true;
					Vector3 ve = new Vector3(textR.transform.position.x+1.4f , textR.transform.position.y, textR.transform.position.z);
					textR.transform.position = ve;
				}
				
			}else if(color == 2){
				textN.active = true;
				textN.transform.RotateAround(textN.transform.position,Vector3.up, -90);
				//boolN = true;
				Vector3 ve = new Vector3(textN.transform.position.x+1.4f , textN.transform.position.y, textN.transform.position.z);
				textN.transform.position = ve;		
			}
		}		
	}

}
