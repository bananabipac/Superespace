using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public GUIStyle style;
	// Use this for initialization
	void Start () {
		style.alignment = TextAnchor.MiddleCenter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		
		if(GUI.Button(new Rect(Screen.width * (1f/6.55f),10*Screen.height * (0.2f/6.3f),Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f)),"Multiplayer", style)) {
			Application.LoadLevel(1);
		}
		if(GUI.Button(new Rect(Screen.width * (1f/6.55f),24*Screen.height * (0.2f/6.3f),Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f)),"Exit", style)) {
			PlayerPrefs.DeleteKey("paramHand1");
			PlayerPrefs.DeleteKey("paramHand2");
			Application.Quit();
		}
		
	}
}
