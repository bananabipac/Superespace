using UnityEngine;
using System.Collections;

public class RandomRotateAroundMenu : MonoBehaviour {
	private GameObject[] asteroids;
	// Use this for initialization
	void Start () {
		asteroids = GameObject.FindGameObjectsWithTag("AstMenu");
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < asteroids.Length; i++) {
			int rand = Mathf.FloorToInt(Random.Range(1f,100f));
			asteroids[i].transform.RotateAround(asteroids[i].transform.position,Vector3.up,rand*Time.deltaTime);
		}
	}
}
