using UnityEngine;
using System.Collections;

public class TextsColor : MonoBehaviour {
	
	
	public float price;
	private float timer;
	private TextMesh text;
	private GameObject user;
	// Use this for initialization
	void Start () {
		timer = Time.timeSinceLevelLoad;
		text = GetComponent<TextMesh>();
		user = GameObject.FindWithTag("User");
		if(gameObject.tag == "GUIPlayer2" || gameObject.tag == "GUIPlayer1") {
			text.text = price+" Cr.";
		}
		if (gameObject.tag == "UpgradePlayer2") {
			if(gameObject.name == "attaque2") {
				text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack2;
			}
		}
		if (gameObject.tag == "UpgradePlayer2") {
			if(gameObject.name == "vie2") {
				text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife2;
			}
		}
		if (gameObject.tag == "UpgradePlayer2") {
			if(gameObject.name == "vitesse2") {
				text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed2;
			}
		}
		if (gameObject.tag == "UpgradePlayer1") {
			if(gameObject.name == "attaque1") {
				text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack1;
			}
		}
		if (gameObject.tag == "UpgradePlayer1") {
			if(gameObject.name == "vie1") {
				text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife1;
			}
		}
		if (gameObject.tag == "UpgradePlayer1") {
			if(gameObject.name == "vitesse1") {
				text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed1;
			}
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - timer >= 0.5f) {
			if(gameObject.tag == "GUIPlayer2") {
				if(user.GetComponent<MoneyScript>().moneyPlayer2 >= price) {
					renderer.material.color = Color.green;
				} else {
					renderer.material.color = Color.red;	
				}
			} else if(gameObject.tag == "GUIPlayer1") {
				if(user.GetComponent<MoneyScript>().moneyPlayer1 >= price) {
					renderer.material.color = Color.green;
				} else {
					renderer.material.color = Color.red;	
				}
			} else if (gameObject.tag == "UpgradePlayer2") {
				if(gameObject.name == "attaque2") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlAttack2 + 1) > user.GetComponent<MoneyScript>().moneyPlayer2) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					text.text = "Up Attack\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack2;
				}
				if(gameObject.name == "vie2") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlLife2 + 1) > user.GetComponent<MoneyScript>().moneyPlayer2) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					text.text = "Up Life\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife2;
				}
				if(gameObject.name == "vitesse2") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlSpeed2 + 1) > user.GetComponent<MoneyScript>().moneyPlayer2) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					text.text = "Up Speed\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed2;
				}
			} else {
				if(gameObject.name == "attaque1") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlAttack1 + 1) > user.GetComponent<MoneyScript>().moneyPlayer1) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					text.text = "Up Attack\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack1;
				}
				if(gameObject.name == "vie1") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlLife1 + 1) > user.GetComponent<MoneyScript>().moneyPlayer1) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					text.text = "Up Life\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife1;
				}
				if(gameObject.name == "vitesse1") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlSpeed1 + 1) > user.GetComponent<MoneyScript>().moneyPlayer1) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					text.text = "Up Speed\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed1;
				}
			}
			timer = Time.timeSinceLevelLoad;
		}
	}
}
