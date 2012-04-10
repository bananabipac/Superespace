using UnityEngine;
using System.Collections;

public class GestionLink : MonoBehaviour {
	
	public PlanetLink01 lv;
	public GameObject[] l;
	// Use this for initialization
	void Start () {
		
		l = GameObject.FindGameObjectsWithTag("link");
		int niveau =((moveShip)GameObject.FindGameObjectWithTag("User").GetComponent<moveShip>()).lvl;
		Hashtable tmp = (Hashtable)lv.level[niveau];
		
		foreach(DictionaryEntry t in tmp){
			int planetS = (int)t.Key;
			
			foreach(DictionaryEntry te in (Hashtable)t.Value){
				int planetE = (int)te.Key;	
				
				if((int)te.Value == 0){
					
					for(int i = 0; i<l.Length; i++){
						
						if(l[i].name == ""+planetS+""+planetE){
							
							l[i].active=false;
						}
					}
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
