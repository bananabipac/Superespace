using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public Font font;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.font = font;
		style.fontSize = 52;
		if(GUI.Button(new Rect(Screen.width * (1f/6.55f),10*Screen.height * (0.2f/6.3f),Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f)),"Multiplayer", style)) {
			Application.LoadLevel(1);
		}
		if(GUI.Button(new Rect(Screen.width * (1f/6.55f),17*Screen.height * (0.2f/6.3f),Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f)),"Exit", style)) {
			Application.Quit();
		}
		
	}
}
