using UnityEngine;
using System.Collections;

public class stats : MonoBehaviour {
	
	public int destroyShipBAsteroid;
	public int destroyShipRAsteroid;
	
	public int destroyShipBBattle;
	public int destroyShipNBattle;
	public int destroyShipRBattle;
	
	public int destroyShipBPower;
	public int destroyShipNPower;
	public int destroyShipRPower;
	
	public int nbCaptureBlue;
	public int nbCaptureRed;

	public int nbSabotageBlue;
	public int nbSabotageRed;
	
	public int nbCrashBlue;
	public int nbCrashRed;
	
	public int nbNukeBlue;
	public int nbNukeRed;
	
	public float time;
	
	
	// Use this for initialization
	void Start () {
		
		destroyShipBAsteroid = 0;
		destroyShipRAsteroid = 0;
	
		destroyShipBBattle = 0;
		destroyShipNBattle = 0;
		destroyShipRBattle = 0;
	
		destroyShipBPower = 0;
		destroyShipNPower = 0;
		destroyShipRPower = 0;
	
		nbCaptureBlue = 0;
		nbCaptureRed = 0;

		nbSabotageBlue = 0;
		nbSabotageRed = 0;
	
		nbCrashBlue = 0;
		nbCrashRed = 0;
	
		nbNukeBlue = 0;
		nbNukeRed = 0;
		
		time = 0;
	
	
	}
	
	public void SendData(){
		
		PlayerPrefs.SetInt("destroyShipBAsteroid", destroyShipBAsteroid);
		PlayerPrefs.SetInt("destroyShipRAsteroid", destroyShipRAsteroid);
		
		PlayerPrefs.SetInt("destroyShipBBattle", destroyShipBBattle);
		PlayerPrefs.SetInt("destroyShipNBattle", destroyShipNBattle);
		PlayerPrefs.SetInt("destroyShipRBattle", destroyShipRBattle);
		
		PlayerPrefs.SetInt("destroyShipBPower", destroyShipBPower);
		PlayerPrefs.SetInt("destroyShipNPower", destroyShipNPower);
		PlayerPrefs.SetInt("destroyShipRPower", destroyShipRPower);
		
		PlayerPrefs.SetInt("destroyShipBAll", (destroyShipBAsteroid + destroyShipBBattle + destroyShipBPower));
		PlayerPrefs.SetInt("destroyShipNAll", (destroyShipNBattle + destroyShipNPower));
		PlayerPrefs.SetInt("destroyShipRAll", (destroyShipRAsteroid + destroyShipRBattle + destroyShipRPower));
		
		PlayerPrefs.SetInt("nbCaptureBlue", nbCaptureBlue);
		PlayerPrefs.SetInt("nbCaptureRed", nbCaptureRed);
		
		PlayerPrefs.SetInt("nbSabotageBlue", nbSabotageBlue);
		PlayerPrefs.SetInt("nbSabotageRed", nbSabotageRed);
		
		PlayerPrefs.SetInt("nbCrashBlue", nbCrashBlue);
		PlayerPrefs.SetInt("nbCrashRed", nbCrashRed);
		
		PlayerPrefs.SetInt("nbNukeBlue", nbNukeBlue);
		PlayerPrefs.SetInt("nbNukeRed", nbNukeRed);
		
		PlayerPrefs.SetInt("nbPowerBlue", nbSabotageBlue + nbCrashBlue + nbNukeBlue);
		PlayerPrefs.SetInt("nbPowerRed", nbSabotageRed + nbCrashRed + nbNukeRed);
		
		float minutes = time / 60;
		float secondes = time % 60;
		
		//Debug.Log(Mathf.FloorToInt(minutes));
		//Debug.Log(Mathf.FloorToInt(secondes));
   		PlayerPrefs.SetInt("timeGameMin", Mathf.FloorToInt(minutes));
		PlayerPrefs.SetInt("timeGameSec", Mathf.FloorToInt(secondes));
		
	}
	
	// Update is called once per frame
	void Update () {
		time +=  Time.deltaTime;
		
		//Debug.Log(time);
	}
}
