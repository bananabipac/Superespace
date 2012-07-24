using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverScript : MonoBehaviour {
	public GameObject blueShip;
	public GameObject blueModel;
	public GameObject redShip;
	public GameObject redModel;
	private GameObject ship;
	private string winner;
	public GameObject winnerText;
	private TextMesh winnerTextMesh;
	private Rect posExit;
	public GUIStyle style;
	public bool traveled = false;
	// Use this for initialization
	void Start () {
		winner = PlayerPrefs.GetString("winner");
		winnerTextMesh = winnerText.GetComponent<TextMesh>();
		float xPosExit = Screen.width * (1f/6.55f);
		float yPosMulti = 10*Screen.height * (0.2f/6.3f);
		posExit = new Rect(xPosExit,2.5f*yPosMulti,Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f));;
		
		if(winner == "blue") {
			blueShip.active = true;
			blueModel.active = true;
			winnerTextMesh.text = "Blue wins";
			winnerText.renderer.material.color = Color.blue;
			ship = blueShip;	
		} else {
			redShip.active = true;
			redModel.active = true;
			winnerTextMesh.text = "Red wins";
			winnerText.renderer.material.color = Color.red;
			ship = redShip;	
		}
		
		iTween.MoveTo(ship,iTween.Hash("z",-15f,"time",3f,"oncomplete","Travel","oncompletetarget",gameObject));
	}
	
	// Update is called once per frame
	void Update () {
		if(traveled) {
			iTween.MoveTo(ship,iTween.Hash("y",-1.01f,"looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.easeOutSine,"time",2f));
		}
	}
	
	void OnGUI() {
		if(GUI.Button(posExit,"Back to menu",style)) {
			Application.LoadLevel("Menu");	
		}
	}
	void Travel() {
		traveled = true;
	}
}
