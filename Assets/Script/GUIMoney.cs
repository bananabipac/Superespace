using UnityEngine;
using System.Collections;

public class GUIMoney : MonoBehaviour {
	public int player;
	private GameObject user;
	private float timer;
	
	// Use this for initialization
	void Start () {
		//GUIText texte = gameObject.guiText;
		user = GameObject.FindWithTag("User");
		//iTween.RotateTo(texte.gameObject,new Vector3(90f,90f,90f),1f);
		timer = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - timer >= 1) {
			TextMesh credits = (TextMesh)gameObject.GetComponent<TextMesh>();
			if(player == 1) {
				credits.text = ""+((MoneyScript)user.GetComponent<MoneyScript>()).moneyPlayer1+"Cr.";
				//gameObject.guiText.transform.position = Camera.mainCamera.WorldToViewportPoint(gameObject.transform.position);
			} else {
				credits.text = ""+((MoneyScript)user.GetComponent<MoneyScript>()).moneyPlayer2+"Cr.";
				//gameObject.guiText.transform.position = Camera.mainCamera.WorldToViewportPoint(gameObject.transform.position);
			}
			timer = Time.timeSinceLevelLoad;
		}
	}
}
