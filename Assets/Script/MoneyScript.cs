using UnityEngine;
using System.Collections;

public class MoneyScript : MonoBehaviour {
	public int moneyPlayer1;
	public int moneyPlayer2;
	private float timer;
	public int incomePlayer1;
	public int incomePlayer2;
	// Use this for initialization
	void Start () {
		moneyPlayer1 = 100;
		moneyPlayer2 = 100;
		incomePlayer1 = 1;
		incomePlayer2 = 1;
		timer = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - timer >= 1) {
			moneyPlayer1 += incomePlayer1;
			moneyPlayer2 += incomePlayer2;
			timer = Time.timeSinceLevelLoad;
			//Debug.Log(moneyPlayer1.ToString());
			//Debug.Log(moneyPlayer2.ToString());
		}
	}
}
