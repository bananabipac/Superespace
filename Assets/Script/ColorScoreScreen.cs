using UnityEngine;
using System.Collections;

public class ColorScoreScreen : MonoBehaviour {
	public Font fontWhite;
	public Font fontBlue;
	public Font fontRed;
	private int destroyShipBAsteroid;
	private int destroyShipRAsteroid;
	private int destroyShipBBattle;
	private int destroyShipRBattle;
	private int destroyShipBAll;
	private int destroyShipRAll;
	private int nbCaptureBlue;
	private int nbCaptureRed;
	private int nbSabotageBlue;
	private int nbSabotageRed;
	private int nbCrashBlue;
	private int nbCrashRed;
	private int nbNukeBlue;
	private int nbNukeRed;
	
	// Use this for initialization
	void Start () {
		destroyShipBAsteroid = PlayerPrefs.GetInt("destroyShipBAsteroid");
		destroyShipRAsteroid = PlayerPrefs.GetInt("destroyShipRAsteroid");
		destroyShipBBattle = PlayerPrefs.GetInt("destroyShipBBattle");
		destroyShipRBattle = PlayerPrefs.GetInt("destroyShipRBattle");
		destroyShipBAll = PlayerPrefs.GetInt("destroyShipBAll");
		destroyShipRAll = PlayerPrefs.GetInt("destroyShipRAll");
		nbCaptureBlue = PlayerPrefs.GetInt("nbCaptureBlue");
		nbCaptureRed = PlayerPrefs.GetInt("nbCaptureRed");
		nbSabotageBlue = PlayerPrefs.GetInt("nbSabotageBlue");
		nbSabotageRed = PlayerPrefs.GetInt("nbSabotageRed");
		nbCrashBlue = PlayerPrefs.GetInt("nbCrashBlue");
		nbCrashRed = PlayerPrefs.GetInt("nbCrashRed");
		nbNukeBlue = PlayerPrefs.GetInt("nbNukeBlue");
		nbNukeRed = PlayerPrefs.GetInt("nbNukeRed");
		
		if(destroyShipBAsteroid < destroyShipRAsteroid) {
			GameObject.FindWithTag("destroyShipBAsteroid").GetComponent<GUIText>().font = fontBlue;
		}else if(destroyShipBAsteroid > destroyShipRAsteroid){
			GameObject.FindWithTag("destroyShipRAsteroid").GetComponent<GUIText>().font = fontRed;
		}
		if(destroyShipBBattle < destroyShipRBattle) {
			GameObject.FindWithTag("destroyShipBBattle").GetComponent<GUIText>().font = fontBlue;
		}else if(destroyShipBBattle > destroyShipRBattle){
			GameObject.FindWithTag("destroyShipRBattle").GetComponent<GUIText>().font = fontRed;
		}
		if(destroyShipBAll < destroyShipRAll) {
			GameObject.FindWithTag("destroyShipBAll").GetComponent<GUIText>().font = fontBlue;
		}else if(destroyShipBAll > destroyShipRAll){
			GameObject.FindWithTag("destroyShipRAll").GetComponent<GUIText>().font = fontRed;
		}
		if(nbCaptureBlue > nbCaptureRed) {
			GameObject.FindWithTag("nbCaptureBlue").GetComponent<GUIText>().font = fontBlue;
		}else if(nbCaptureBlue < nbCaptureRed){
			GameObject.FindWithTag("nbCaptureRed").GetComponent<GUIText>().font = fontRed;
		}
		if(nbSabotageBlue > nbSabotageRed) {
			GameObject.FindWithTag("nbSabotageBlue").GetComponent<GUIText>().font = fontBlue;
		}else if(nbSabotageBlue < nbSabotageRed){
			GameObject.FindWithTag("nbSabotageRed").GetComponent<GUIText>().font = fontRed;
		}
		if(nbCrashBlue > nbCrashRed) {
			GameObject.FindWithTag("nbCrashBlue").GetComponent<GUIText>().font = fontBlue;
		}else if(nbCrashBlue < nbCrashRed){
			GameObject.FindWithTag("nbCrashRed").GetComponent<GUIText>().font = fontRed;
		}
		if(nbNukeBlue > nbNukeRed) {
			GameObject.FindWithTag("nbNukeBlue").GetComponent<GUIText>().font = fontBlue;
		}else if(nbNukeBlue < nbNukeRed){
			GameObject.FindWithTag("nbNukeRed").GetComponent<GUIText>().font = fontRed;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
