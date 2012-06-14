using UnityEngine;
using System.Collections;

public class GUIFPS : MonoBehaviour {
	private float fps;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		FPSUpdate ();
	}
	
	void OnGUI() {
		GUI.Label(new Rect(0f,Screen.height-20,100f,50f),"FPS: "+fps);
	}
	
	
	void FPSUpdate() {
        float delta = Time.smoothDeltaTime;
        fps = 1 / delta;
    }
}
