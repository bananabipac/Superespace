using UnityEngine;
using System.Collections;

public class GUIPlanet : MonoBehaviour {
	
	public GUIStyle style;
	public GUISkin skin;
	//private float x;
	//private float y;
	private GameObject user;
	
	
	private int shipsRT;
	private int shipsBT;
	private int shipsNT;
	
	//private float refresh;
	//private float tmp;
	
	private PlanetScript planet;
	
	private GameObject textR;
	private GameObject textB;
	private GameObject textN;
	private GameObject textVs;
	
	private bool boolR;
	private bool boolB;
	private bool boolN;
	private bool boolVs;
	
	private bool boolRT;
	private bool boolBT;
	private bool boolBT2;
	private bool boolNT;
	
	private Vector3 vecR;
	
	// Use this for initialization
	void Start () {
		
		style.font = skin.font;
		style.alignment = TextAnchor.UpperLeft;
		//x = 14f;
		//y = -8f;
		user = GameObject.FindWithTag("User");
		planet = GetComponent<PlanetScript>();
		//PlanetScript tmp = GetComponent<PlanestScript>();
		shipsBT = planet.shipsB.Count;
		shipsRT = planet.shipsR.Count;
		shipsNT = planet.shipsN.Count;
		
		//refresh = 0.2f;
		//tmp = 0;
		vecR = new Vector3(gameObject.transform.position.x, -18, gameObject.transform.position.z);
		
		textR =  Resources.Load("TextSelectRed")as GameObject;
		textR.transform.position = vecR;
		textR = (GameObject) Instantiate(textR);
		textR.transform.RotateAround(textR.transform.position,Vector3.up, 90);
		boolR = true;
		boolRT = false;
		
		textB =  Resources.Load("TextSelectBlue")as GameObject;
		textB.transform.position = vecR;
		textB = (GameObject) Instantiate(textB);
		textB.transform.RotateAround(textB.transform.position,Vector3.up, -90);
		boolB = true;
		boolBT = false;
		boolBT2 = false;
		
		textN =  Resources.Load("TextSelectNeutre")as GameObject;
		textN.transform.position = vecR;
		textN = (GameObject) Instantiate(textN);
		boolN = true;
		boolNT = false;
		
		textVs =  Resources.Load("TextSelectNeutre")as GameObject;
		textVs.transform.position = vecR;
		textVs = (GameObject) Instantiate(textVs);
		textVs.active = false;
		
		boolVs = false;
		
		
		
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
		
		if(!user.GetComponent<PauseScript>().paused2) {
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
			
			if(shipsNT != shipsN ){
				shipsNT = planet.shipsN.Count;
				((TextMesh)textN.GetComponent<TextMesh>()).text = ""+shipsN;	
			}
			
			if(planet.ship != null){
				
				//rouge contre bleu
				if(shipsR >0 && shipsB >0 ){
					if(boolR || !boolRT ||  textN.active || !textR.active || textN.active){
						//Debug.Log("red/blue modif red");
						GUIplanet(1,1);	
					}
					if(boolB || boolBT || !boolBT2 || !textB.active  || !textVs.active){
						Debug.Log("red/blue modif blue");
						GUIplanet(0,2);	
					}
				//rouge contre blanc
				}else if(shipsR >0 && shipsN >0 ){
					if(boolR || !boolRT || textB.active || !textR.active){
						//Debug.Log("red/neutre modif red");
						GUIplanet(1,1);	
					}
					if(!boolNT || !textN.active || !textVs.active ){
						//Debug.Log("red/neutre modif neutre");
						GUIplanet(2,2);	
					}
				//bleu contre blanc
				}else if(shipsN >0 && shipsB >0 ){
					if(boolB || boolBT2 || !boolBT || !textB.active || textR.active){
						//Debug.Log("blue/neutre modif blue");
						GUIplanet(0,1);	
					}
					if(!boolNT || !textN.active || !textVs.active){
						//Debug.Log("blue/neutre modif neutre");
						GUIplanet(2,2);	
					}
				}else{
					if(planet.ship.tag == "red"){
						//Camera.mainCamera.ScreenToWorldPoint;
						if(shipsB>0){
							if(!boolB || boolBT || boolBT2 || !textB.active || textVs.active){
								GUIplanet(0,0);	
							}
						}else if(!boolR  || boolRT || !textR.active || textVs.active){
							GUIplanet(1,0);
						}
					}else if(planet.ship.tag == "blue"){
						if(shipsR>0 ){
							if(!boolR || boolRT || !textR.active || textVs.active){
								GUIplanet(1,0);
							}
						}else if(!boolB || boolBT || boolBT2 || !textB.active || textVs.active){
							GUIplanet(0,0);
						}
					}else{
						
						if(shipsR>0){
							if(!boolR  || boolRT || !textR.active || textVs.active){
								GUIplanet(1,0);
							}
						}else if(shipsB>0 ){
							if(!boolB || boolBT || boolBT2 || !textB.active || textVs.active){
								GUIplanet(0,0);
							}
						}else{
							
							if(boolNT || !textN.active || textB.active || textR.active || textVs.active){
								GUIplanet(2,0);
							}
						}
					}
				}
			}
		}else{
			textR.active = false;
			textN.active = false;
			textB.active = false;
			textVs.active = false;
			
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
		//Vector3 vec = Camera.mainCamera.WorldToScreenPoint(gameObject.transform.position);

		if(position == 0){//la planete n'est pas en combat
			boolVs = false;
			if(color ==0){//blue
				textB.active = true;
				textR.active = false;
				textN.active = false;
				textVs.active = false;
				if(!boolB){
					textB.transform.RotateAround(textB.transform.position,Vector3.up, -90);
					boolB = true;
				}
				
				if(boolBT || boolBT2){
					textB.transform.position = vecR;
					boolBT = false;
					boolBT2 = false;
				}
				
				
			}else if(color == 1){
				textR.active = true;
				textB.active = false;
				textN.active = false;
				textVs.active = false;
				if(!boolR){
					textR.transform.RotateAround(textR.transform.position,Vector3.up, 90);
					boolR = true;
				}
				if(boolRT){
					textR.transform.position = vecR;
					boolRT = false;
				}
				
			}else if(color == 2){
				
				textR.active = false;
				textB.active = false;
				textN.active = true;
				textVs.active = false;
				if(boolNT){
					boolNT = false;
					textN.transform.position = vecR;
				}
			}
		}else if(position == 1){//la planete est en combat
			Vector3 ve = new Vector3(vecR.x-1.25f , vecR.y, vecR.z);
			if(color ==0){//blue
				textB.active = true;
				textR.active = false;
				textN.active = false;
				if(boolB){
					textB.transform.RotateAround(textB.transform.position,Vector3.up, 90);
					boolB = false;
					
				}
				
				if(boolBT2){
					textB.transform.position = vecR;
					textB.transform.position = ve;
					boolBT2 = false;
					boolBT = true;
				}
				
				if(!boolBT){
					textB.transform.position = ve;
					boolBT = true;
				}
				
				
				
				
			}else if(color == 1){
				textR.active = true;
				textB.active = false;
				textN.active = false;
				//textR.transform.position = vecR;
				
				if(boolR){
					textR.transform.RotateAround(textR.transform.position,Vector3.up, -90);
					boolR = false;
					
				}
				
				if(!boolRT){
					textR.transform.position = ve;
					boolRT = true;
				}	
			}
			
			if(!boolVs){
				textVs.active = true;
				boolVs = true;
			}
				
		}else if(position == 2){
			Vector3 ve = new Vector3(vecR.x+1.25f , vecR.y, vecR.z);
			if(color ==0){//blue
				textB.active = true;
				if(boolB){
					textB.transform.RotateAround(textB.transform.position,Vector3.up, 90);
					boolB = false;
				}
				
				if(boolBT){
					textB.transform.position = vecR;
					textB.transform.position = ve;
					boolBT = false; 
					boolBT2 = true;
				}
				if(!boolBT2){
					textB.transform.position = ve;
					boolBT2 = true;
				}
			}else if(color == 2){
				textN.active = true;
				if(!boolNT){
					boolNT = true;
					textN.transform.position = ve;
				}
			}
		}		
	}

}
