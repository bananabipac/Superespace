using UnityEngine;
using System.Collections;

public class MoneyScript : MonoBehaviour {
	public int moneyPlayer1;
	public int moneyPlayer2;
	private float timer;
	// Use this for initialization
	void Start () {
		moneyPlayer1 = 100;
		moneyPlayer2 = 100;
		timer = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - timer >= 1) {
			moneyPlayer1 ++;
			moneyPlayer2 ++;
			timer = Time.timeSinceLevelLoad;
			//Debug.Log(moneyPlayer1.ToString());
			//Debug.Log(moneyPlayer2.ToString());
		}
	}
	void OnGUI() {
		
	}
}
