using UnityEngine;
using System.Collections;

public class Launch_EndScript : MonoBehaviour {
	public bool endGame;
	public GameObject camera;
	public string winner;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(camera.transform.rotation.x < 0.7f && !endGame) {
			camera.transform.Rotate(50*Time.deltaTime, 0, 0);
		}
		if(endGame) {
			camera.transform.Rotate(-50*Time.deltaTime,0,0);
			if(camera.transform.rotation.x < 0f) {
				PlayerPrefs.SetString("winner",winner);
				Application.LoadLevel("GameOver");	
			}
		}
	}
}
