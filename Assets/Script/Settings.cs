using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	
	
	public string handPlayer1 = "Right-Handed";
	public string handPlayer2 = "Right-Handed";
	private string paramHand1;
	private string paramHand2;
	private string paramQualitySettings = "Low";
	// Use this for initialization
	void Start () {
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
	
	}
	
	void OnGUI() {
		if(GUI.Button(new Rect(Screen.width * (1f/6.55f),17*Screen.height * (0.2f/7f),Screen.width * (4.8f/14f),Screen.height * (0.85f/8.5f)), "Player 1:\n"+handPlayer1)) {
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
		if(GUI.Button(new Rect(Screen.width * (1f/2f),17*Screen.height * (0.2f/7f),Screen.width * (4.8f/14f),Screen.height * (0.85f/8.5f)), "Player 2:\n"+handPlayer2)) {
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
		if(GUI.Button(new Rect(Screen.width * (1f/6.55f),17*Screen.height * (0.2f/5.5f),Screen.width * (4.8f/6.94f),Screen.height * (0.85f/8.5f)), "Graphics :\n"+paramQualitySettings)) {
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
}
