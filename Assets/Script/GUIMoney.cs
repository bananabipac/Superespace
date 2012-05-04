using UnityEngine;
using System.Collections;

public class GUIMoney : MonoBehaviour {
	public int player;
	public GameObject user;
	
	// Use this for initialization
	void Start () {
		//GUIText texte = gameObject.guiText;
		user = GameObject.FindWithTag("User");
		//iTween.RotateTo(texte.gameObject,new Vector3(90f,90f,90f),1f);
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh credits = (TextMesh)gameObject.GetComponent<TextMesh>();
		if(player == 1) {
			credits.text = ""+((MoneyScript)user.GetComponent<MoneyScript>()).moneyPlayer1+"Cr.";
			//gameObject.guiText.transform.position = Camera.mainCamera.WorldToViewportPoint(gameObject.transform.position);
		} else {
			credits.text = ""+((MoneyScript)user.GetComponent<MoneyScript>()).moneyPlayer2+"Cr.";
			//gameObject.guiText.transform.position = Camera.mainCamera.WorldToViewportPoint(gameObject.transform.position);
		}
	}
}
