using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	public bool paused = false;
	
	private float xPosResume;
	private float yPosResume;
	
	private Rect initialPosResume;
	private Rect posResume;
	private Rect finalPosResume;
	
	private Rect initialPosExit;
	private Rect posExit;
	private Rect finalPosExit;
	
	private Vector3 initialPosPlane;
	private Vector3 posPlane;
	private Vector3 finalPosPlane;
	
	
	public GameObject plane;
	public GUIStyle style;
	
	// Use this for initialization
	void Start () {
		xPosResume = 1.6f*Screen.width * (1f/6.55f);
		yPosResume = 8*Screen.height * (0.2f/6.3f);
		
		initialPosResume = new Rect(xPosResume,yPosResume-500,Screen.width * (4.8f/10f),Screen.height * (0.85f/6.3f));
		posResume = initialPosResume;
		finalPosResume = new Rect(xPosResume,yPosResume,Screen.width * (4.8f/10f),Screen.height * (0.85f/6.3f));
		
		initialPosExit = new Rect(xPosResume,yPosResume+150-500,Screen.width * (4.8f/10f),Screen.height * (0.85f/6.3f));
		posExit = initialPosExit;
		finalPosExit = new Rect(xPosResume,yPosResume+150,Screen.width * (4.8f/10f),Screen.height * (0.85f/6.3f));
		
		initialPosPlane = new Vector3 (0,-15f,30);
		posPlane = initialPosPlane;
		finalPosPlane = new Vector3(0,-15f,0);
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI() {
		if(!paused){
			Matrix4x4 matrixBackup = GUI.matrix;
			
			GUIUtility.RotateAroundPivot(90f, new Vector2(55,50));
			if(GUI.Button(new Rect(5,30,100,40),"Pause")) {
				paused = true;
				iTween.ValueTo(gameObject,iTween.Hash("from",initialPosResume,"to",finalPosResume,"onupdate","MoveResume","easetype",iTween.EaseType.easeInOutSine,"oncomplete","StopGame"));					
				iTween.ValueTo(gameObject,iTween.Hash("from",initialPosExit,"to",finalPosExit,"onupdate","MoveExit","easetype",iTween.EaseType.easeInOutSine));
				iTween.ValueTo(gameObject,iTween.Hash("from",initialPosPlane,"to",finalPosPlane,"onupdate","MovePlane","easetype",iTween.EaseType.easeInOutSine));
				
				plane.active = true;
			}
			GUI.matrix = matrixBackup;
			
			matrixBackup = GUI.matrix;
			
			GUIUtility.RotateAroundPivot(-90f, new Vector2(920,545));
			if(GUI.Button(new Rect(870,525,100,40),"Pause")) {
				paused = true;
				iTween.ValueTo(gameObject,iTween.Hash("from",initialPosResume,"to",finalPosResume,"onupdate","MoveResume","easetype",iTween.EaseType.easeInOutSine,"oncomplete","StopGame"));					
				iTween.ValueTo(gameObject,iTween.Hash("from",initialPosExit,"to",finalPosExit,"onupdate","MoveExit","easetype",iTween.EaseType.easeInOutSine));
				iTween.ValueTo(gameObject,iTween.Hash("from",initialPosPlane,"to",finalPosPlane,"onupdate","MovePlane","easetype",iTween.EaseType.easeInOutSine));
				
				plane.active = true;
			}
			GUI.matrix = matrixBackup;
		}
		if(paused) {
			if(GUI.Button (posResume,"Resume",style)){
				
				iTween.ValueTo(gameObject,iTween.Hash("from",finalPosResume,"to",initialPosResume,"onupdate","MoveResume","easetype",iTween.EaseType.easeInOutSine,"oncomplete","DesactivePause"));					
				iTween.ValueTo(gameObject,iTween.Hash("from",finalPosExit,"to",initialPosExit,"onupdate","MoveExit","easetype",iTween.EaseType.easeInOutSine));
				iTween.ValueTo(gameObject,iTween.Hash("from",finalPosPlane,"to",initialPosPlane,"onupdate","MovePlane","easetype",iTween.EaseType.easeInOutSine));
				Time.timeScale = 1;
			}
			if(GUI.Button (posExit,"Exit",style)){
				Time.timeScale = 1;
				Application.LoadLevel(0);
			}
			
		}
		
	}
	void MoveResume(Rect newCoordinates) {
		posResume = newCoordinates;
	}
	void MoveExit(Rect newCoordinates) {
		posExit = newCoordinates;
	}
	void MovePlane(Vector3 newCoordinates) {
		plane.transform.position = newCoordinates;	
	}
	void StopGame() {
		Time.timeScale = 0;	
	}
	void DesactivePause() {
		plane.active = false;
		paused = false;
	}
}
