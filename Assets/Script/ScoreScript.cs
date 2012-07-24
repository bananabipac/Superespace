using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {
	private string nameOfObject;
	// Use this for initialization
	void Start () {
		nameOfObject = gameObject.name;
		if(nameOfObject == "timeGame") {
			gameObject.guiText.text += PlayerPrefs.GetInt(nameOfObject+"Min")+":";
			gameObject.guiText.text += PlayerPrefs.GetInt(nameOfObject+"Sec");
		} else {
			gameObject.guiText.text += PlayerPrefs.GetInt(nameOfObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
