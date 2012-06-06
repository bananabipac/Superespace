using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public GUIStyle style;
	
	private Rect initialPosMulti;
	private Rect posMulti;
	private Rect finalPosMulti;
	
	private Rect initialPosExit;
	private Rect posExit;
	private Rect finalPosExit;
	
	private float xPosMulti;
	private float yPosMulti;
	
	private float xPosExit;
	private float yPosExit;
	
	private Rect initialPlayerOne;
	private Rect posPlayerOne;
	private Rect finalPlayerOne;
	
	private Rect initialPlayerTwo;
	private Rect posPlayerTwo;
	private Rect finalPlayerTwo;
	
	private Rect initialQuality;
	private Rect posQuality;
	private Rect finalQuality;
	
	private Rect initialLevel1;
	private Rect posLevel1;
	private Rect finalLevel1;
	
	private Rect initialLevel2;
	private Rect posLevel2;
	private Rect finalLevel2;
	
	private Rect initialBack;
	private Rect posBack;
	private Rect finalBack;
	
	public GUIStyle style2;
	public string handPlayer1 = "Right-Handed";
	public string handPlayer2 = "Right-Handed";
	private string paramHand1;
	private string paramHand2;
	private string paramQualitySettings = "Low";
	
	private bool rotateToEnd = false;
	private bool rotateToBegin = false;
	
	public GameObject planet;
	public float speed;
	
	private bool moving = false;
	// Use this for initialization
	void Start () {
		style.alignment = TextAnchor.MiddleCenter;
		
		xPosMulti = Screen.width * (1f/6.55f);
		yPosMulti = 10*Screen.height * (0.2f/6.3f);
		
		xPosExit = Screen.width * (1f/6.55f);
		yPosExit = 24*Screen.height * (0.2f/6.3f);
		
		initialPosMulti = new Rect(xPosMulti,yPosMulti,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		posMulti = initialPosMulti;
		finalPosMulti = new Rect(xPosMulti-1500,yPosMulti,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		
		initialPosExit = new Rect(xPosExit,yPosExit,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		posExit = initialPosExit;
		finalPosExit = new Rect(xPosExit-1500,yPosExit,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		
		initialPlayerOne = new Rect(Screen.width * (1f/6.55f),17*Screen.height * (0.2f/7f),Screen.width * (4.8f/14f),Screen.height * (0.85f/8.5f));
		posPlayerOne = initialPlayerOne;
		finalPlayerOne = new Rect(Screen.width * (1f/6.55f)-1500,17*Screen.height * (0.2f/7f),Screen.width * (4.8f/14f),Screen.height * (0.85f/8.5f));
		
		initialPlayerTwo = new Rect(Screen.width * (1f/2f),17*Screen.height * (0.2f/7f),Screen.width * (4.8f/14f),Screen.height * (0.85f/8.5f));
		posPlayerTwo = initialPlayerTwo;
		finalPlayerTwo = new Rect(Screen.width * (1f/2f)-1500,17*Screen.height * (0.2f/7f),Screen.width * (4.8f/14f),Screen.height * (0.85f/8.5f));
		
		initialQuality = new Rect(Screen.width * (1f/6.55f),17*Screen.height * (0.2f/5.5f),Screen.width * (4.8f/6.94f),Screen.height * (0.85f/8.5f));
		posQuality = initialQuality;
		finalQuality = new Rect(Screen.width * (1f/6.55f)-1500,17*Screen.height * (0.2f/5.5f),Screen.width * (4.8f/6.94f),Screen.height * (0.85f/8.5f));
		
		initialLevel1 = new Rect(xPosMulti+1500,yPosMulti,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		posLevel1 = initialLevel1;
		finalLevel1 = new Rect(xPosMulti,yPosMulti,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		
		initialLevel2 = new Rect(xPosMulti+1500,1.5f*yPosMulti,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		posLevel2 = initialLevel2;
		finalLevel2 = new Rect(xPosMulti,1.5f*yPosMulti,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		
		initialBack = new Rect(xPosExit+1500,yPosExit,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		posBack = initialBack;
		finalBack = new Rect(xPosExit,yPosExit,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));
		
		style.alignment = TextAnchor.MiddleCenter;
		if(PlayerPrefs.GetString("paramHand1") == "left" ){
			paramHand1 = PlayerPrefs.GetString("paramHand1");
			handPlayer1 = "Left-Handed";
		} else if ( PlayerPrefs.GetString("paramHand1") == "right") {
			paramHand1 = "right";
			handPlayer1 = "Right-Handed";
		} else {
			paramHand1 = "right";
			handPlayer1 = "Right-Handed";	
		}
		if(PlayerPrefs.GetString("paramHand2") == "left" ) {
			paramHand2 = PlayerPrefs.GetString("paramHand2");
			handPlayer2 = "Left-Handed";
		}else if ( PlayerPrefs.GetString("paramHand2") == "right" ) {
			paramHand2 = "right";
			handPlayer2 = "Right-Handed";
		} else {
			paramHand2 = "right";
			handPlayer2 = "Right-Handed";
		}
		Debug.Log(paramHand1);
		Debug.Log(paramHand2);
		
		QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("graphParam"));
		if(QualitySettings.GetQualityLevel() == 0) {
			paramQualitySettings = "Low";
		} else if(QualitySettings.GetQualityLevel() == 1) {
			paramQualitySettings = "Medium";	
		} else if(QualitySettings.GetQualityLevel() == 2) {
			paramQualitySettings = "High";	
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(rotateToEnd) {
			this.transform.RotateAround	(planet.transform.position,Vector3.up,speed*Time.deltaTime);
		}
		if(rotateToBegin) {
			this.transform.RotateAround	(planet.transform.position,Vector3.up,-speed*Time.deltaTime);	
		}
		
	}
	
	void OnGUI() {
		
		
			if(GUI.Button(posMulti,"Multiplayer", style)) {
				if(!moving){
					iTween.ValueTo(gameObject,iTween.Hash("from",initialPosMulti,"to",finalPosMulti,"onupdate","MoveButtonMulti","easetype",iTween.EaseType.easeInOutSine,"oncomplete","StopRotationEnd"));
					iTween.ValueTo(gameObject,iTween.Hash("from",initialPosExit,"to",finalPosExit,"onupdate","MoveButtonExit","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",initialPlayerOne,"to",finalPlayerOne,"onupdate","MoveButtonPlayerOne","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",initialPlayerTwo,"to",finalPlayerTwo,"onupdate","MoveButtonPlayerTwo","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",initialQuality,"to",finalQuality,"onupdate","MoveButtonQuality","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",initialLevel1,"to",finalLevel1,"onupdate","MoveButtonLevel1","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",initialLevel2,"to",finalLevel2,"onupdate","MoveButtonLevel2","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",initialBack,"to",finalBack,"onupdate","MoveButtonBack","easetype",iTween.EaseType.easeInOutSine));
					rotateToEnd = true;
					moving = true;
				}
			}
			if(GUI.Button(posExit,"Exit", style)) {
				PlayerPrefs.DeleteKey("paramHand1");
				PlayerPrefs.DeleteKey("paramHand2");
				Application.Quit();
			}
			if(GUI.Button(posLevel1,"Level 1",style)) {
				PlayerPrefs.SetInt("paramLevel", 1);
				Application.LoadLevel(1);
			}
			if(GUI.Button(posLevel2,"Level 2",style)) {
				PlayerPrefs.SetInt("paramLevel", 2);
				Application.LoadLevel(1);
			}
			if(GUI.Button (posBack,"Back",style)) {
				if(!moving){
					iTween.ValueTo(gameObject,iTween.Hash("from",finalPosMulti,"to",initialPosMulti,"onupdate","MoveButtonMulti","easetype",iTween.EaseType.easeInOutSine,"oncomplete","StopRotationBegin"));
					iTween.ValueTo(gameObject,iTween.Hash("from",finalPosExit,"to",initialPosExit,"onupdate","MoveButtonExit","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",finalPlayerOne,"to",initialPlayerOne,"onupdate","MoveButtonPlayerOne","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",finalPlayerTwo,"to",initialPlayerTwo,"onupdate","MoveButtonPlayerTwo","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",finalQuality,"to",initialQuality,"onupdate","MoveButtonQuality","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",finalLevel1,"to",initialLevel1,"onupdate","MoveButtonLevel1","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",finalLevel2,"to",initialLevel2,"onupdate","MoveButtonLevel2","easetype",iTween.EaseType.easeInOutSine));
					iTween.ValueTo(gameObject,iTween.Hash("from",finalBack,"to",initialBack,"onupdate","MoveButtonBack","easetype",iTween.EaseType.easeInOutSine));
					rotateToBegin = true;
					moving = true;
				}
			}
			
			
			
			
			if(GUI.Button(posPlayerOne, "Player 1:\n"+handPlayer1,style2)) {
				if(paramHand1 == "right") {
					paramHand1 = "left";
					handPlayer1 = "Left-Handed";
					PlayerPrefs.SetString("paramHand1",paramHand1);
				} else {
					paramHand1 = "right";	
					handPlayer1 = "Right-Handed";
					PlayerPrefs.SetString("paramHand1",paramHand1);
				}
			}
			if(GUI.Button(posPlayerTwo, "Player 2:\n"+handPlayer2,style2)) {
				if(paramHand2 == "right") {
					paramHand2 = "left";
					handPlayer2 = "Left-Handed";
					PlayerPrefs.SetString("paramHand2",paramHand2);
				} else {
					paramHand2 = "right";	
					handPlayer2 = "Right-Handed";
					PlayerPrefs.SetString("paramHand2",paramHand2);
				}
			}
			if(GUI.Button(posQuality, "Graphics :\n"+paramQualitySettings,style2)) {
				if(QualitySettings.GetQualityLevel() == 0) {
					paramQualitySettings = "Medium";
					QualitySettings.SetQualityLevel(1);
					PlayerPrefs.SetInt ("graphParam",QualitySettings.GetQualityLevel());
				}
				else if(QualitySettings.GetQualityLevel() == 1) {
					paramQualitySettings = "High";
					QualitySettings.SetQualityLevel(2);
					PlayerPrefs.SetInt ("graphParam",QualitySettings.GetQualityLevel());
				}
				else if(QualitySettings.GetQualityLevel() == 2) {
					paramQualitySettings = "Low";
					QualitySettings.SetQualityLevel(0);
					PlayerPrefs.SetInt ("graphParam",QualitySettings.GetQualityLevel());
				}
				
			}
		
		
	}
	void MoveButtonMulti(Rect newCoordinates) {
		posMulti = newCoordinates;
		
	}
	void MoveButtonExit(Rect newCoordinates) {
		posExit = newCoordinates;
	}
	void MoveButtonPlayerOne(Rect newCoordinates) {
		posPlayerOne = newCoordinates;
	}
	void MoveButtonPlayerTwo(Rect newCoordinates) {
		posPlayerTwo = newCoordinates;
	}
	void MoveButtonQuality(Rect newCoordinates) {
		posQuality = newCoordinates;
	}
	void MoveButtonLevel1(Rect newCoordinates) {
		posLevel1 = newCoordinates;
	}
	void MoveButtonLevel2(Rect newCoordinates) {
		posLevel2 = newCoordinates;
	}
	void MoveButtonBack(Rect newCoordinates) {
		posBack = newCoordinates;
	}
	
	void StopRotationEnd() {
		rotateToEnd = false;
		moving = false;
	}
	void StopRotationBegin() {
		rotateToBegin = false;
		moving = false;
	}
}
