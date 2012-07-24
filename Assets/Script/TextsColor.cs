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
			if (gameObject.name == "TextCrash2" || gameObject.name == "TextCrash1") {
				text.text = "Crash:\n"+ price+" Cr.";
			}
			if (gameObject.name == "TextNuke2" || gameObject.name == "TextNuke1") {
				text.text = "Nuke:\n"+ price+" Cr.";
			}
			if (gameObject.name == "TextSabotage2" || gameObject.name == "TextSabotage1") {
				text.text = "Sabotage:\n"+ price+" Cr.";
			}
			
		}
		if (gameObject.tag == "UpgradePlayer2") {
			if(gameObject.name == "attaque2") {
				if(user.GetComponent<GUIPlayers>().lvlAttack2 == 3){
					text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack2;
				}else{
					text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack2;
				}
				
			}
		}
		if (gameObject.tag == "UpgradePlayer2") {
			if(gameObject.name == "vie2") {
				if(user.GetComponent<GUIPlayers>().lvlLife2 == 3){
					text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife2;
				}else{
					text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife2;	
				}
			}
		}
		if (gameObject.tag == "UpgradePlayer2") {
			if(gameObject.name == "vitesse2") {
				if(user.GetComponent<GUIPlayers>().lvlSpeed2 == 3){
					text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed2;
				}else{
					text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed2;
				}
				
			}
		}
		if (gameObject.tag == "UpgradePlayer1") {
			if(gameObject.name == "attaque1") {
				if(user.GetComponent<GUIPlayers>().lvlAttack1 == 3){
					text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack1;
				}else{
					text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack1;
				}
				
			}
		}
		if (gameObject.tag == "UpgradePlayer1") {
			if(gameObject.name == "vie1") {
				if(user.GetComponent<GUIPlayers>().lvlLife1 == 3){
					text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife1;
				}else{
					text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife1;
				}
				
			}
		}
		if (gameObject.tag == "UpgradePlayer1") {
			if(gameObject.name == "vitesse1") {
				if(user.GetComponent<GUIPlayers>().lvlSpeed1 == 3){
					text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed1;
				}else{
					text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed1;
				}
				
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
					if(user.GetComponent<GUIPlayers>().lvlAttack2 == 3){
						text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack2;
					}else{
						text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack2;
					}
				}
				if(gameObject.name == "vie2") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlLife2 + 1) > user.GetComponent<MoneyScript>().moneyPlayer2) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					if(user.GetComponent<GUIPlayers>().lvlLife2 == 3){
						text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife2;
					}else{
						text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife2;	
					}
				}
				if(gameObject.name == "vitesse2") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlSpeed2 + 1) > user.GetComponent<MoneyScript>().moneyPlayer2) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					if(user.GetComponent<GUIPlayers>().lvlSpeed2 == 3){
						text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed2;
					}else{
						text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed2+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed2;
					}
				}
			} else {
				if(gameObject.name == "attaque1") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlAttack1 + 1) > user.GetComponent<MoneyScript>().moneyPlayer1) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					if(user.GetComponent<GUIPlayers>().lvlAttack1 == 3){
						text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack1;
					}else{
						text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlAttack1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlAttack1;
					}
				}
				if(gameObject.name == "vie1") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlLife1 + 1) > user.GetComponent<MoneyScript>().moneyPlayer1) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					if(user.GetComponent<GUIPlayers>().lvlLife1 == 3){
						text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife1;
					}else{
						text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlLife1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlLife1;
					}
				}
				if(gameObject.name == "vitesse1") {
					if(100 * (user.GetComponent<GUIPlayers>().lvlSpeed1 + 1) > user.GetComponent<MoneyScript>().moneyPlayer1) {
						renderer.material.color = Color.red;	
					} else {
						renderer.material.color = Color.green;	
					}
					if(user.GetComponent<GUIPlayers>().lvlSpeed1 == 3){
						text.text += "\nMax\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed1;
					}else{
						text.text += "\n" + 100 * (user.GetComponent<GUIPlayers>().lvlSpeed1+1) + " Cr.\nLevel : " + user.GetComponent<GUIPlayers>().lvlSpeed1;
					}
				}
			}
			timer = Time.timeSinceLevelLoad;
		}
	}
}
