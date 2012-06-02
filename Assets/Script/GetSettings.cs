using UnityEngine;
using System.Collections;

public class GetSettings : MonoBehaviour {
	private string paramHand1;
	private string paramHand2;
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetString("paramHand1") != "" ){
			paramHand1 = PlayerPrefs.GetString("paramHand1");
		} else {
			paramHand1 = "right";
		}
		if(PlayerPrefs.GetString("paramHand2") != "" ) {
			paramHand2 = PlayerPrefs.GetString("paramHand2");
		}else {
			paramHand2 = "right";
		}
		Debug.Log(paramHand1);
		Debug.Log(paramHand2);
		if(paramHand1 == "right") {
			GameObject.FindWithTag("Droitier1").active = true;
			GameObject.FindWithTag("Gaucher1").active = false;
		} else {
			//Debug.Log (GameObject.FindGameObjectWithTag("Droitier1").active);
			GameObject.FindWithTag("Droitier1").active = false;
			GameObject.FindWithTag("Gaucher1").active = true;
		}
		if(paramHand2 == "right") {
			GameObject.FindWithTag("Droitier2").active = true;
			GameObject.FindWithTag("Gaucher2").active = false;
		} else {
			GameObject.FindWithTag("Droitier2").active = false;
			GameObject.FindWithTag("Gaucher2").active = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
