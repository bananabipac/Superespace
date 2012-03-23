using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
	
		if(GUI.Button(new Rect(Screen.width * (1f/6.55f),Screen.height * (0.2f/6.3f),Screen.width * (4.8f/6.55f),Screen.height * (0.85f/6.3f)),"Multiplayer")) {
			Application.LoadLevel(1);
		}
	}
}
