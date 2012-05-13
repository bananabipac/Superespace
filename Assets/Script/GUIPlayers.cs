using UnityEngine;
using System.Collections;

public class GUIPlayers : MonoBehaviour {
	
	public GameObject sabotage1;
	public GameObject nuke1;
	public GameObject crash1;
	
	public GameObject sabotage2;
	public GameObject nuke2;
	public GameObject crash2;
	
	private Vector3 posSabotage1;
	private Vector3 posNuke1;
	private Vector3 posCrash1;
	
	private Vector3 posSabotage2;
	private Vector3 posNuke2;
	private Vector3 posCrash2;
	
	
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	// Use this for initialization
	void Start () {
		
		posSabotage1 = sabotage1.transform.position;
		posNuke1 = nuke1.transform.position;
		posCrash1 = crash1.transform.position;
		
		posSabotage2 = sabotage2.transform.position;
		posNuke2 = nuke2.transform.position;
		posCrash2 = crash2.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Touch touch in Input.touches) {
			
			int fingerId  = touch.fingerId;
			if ( fingerId >= maxTouches )
			{
				// I'm not sure if this is a bug or how the  SDK reports finger IDs,
				// however, IMO there should only be five finger IDs max.
				if ( !warnedAboutMaxTouches )
				{
					Debug.Log( "Oops! We got a finderId greater than maxTouches: " + touch.fingerId );
					warnedAboutMaxTouches = true;
				}
			}
			Ray cursorRay = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			if(touch.phase == TouchPhase.Ended) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "UpgradePlayer1") {
						if(hit.collider.name == "vie1") {
							(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).lifeShip += 2;
							Debug.Log((GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).lifeShip);
						}
						if(hit.collider.name == "attaque1") {
							(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMin += 2;
							(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMax += 2;
							Debug.Log((GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMin);
							Debug.Log((GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMax);
						}
						if(hit.collider.name == "speed1") {
							(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).speedShip += 1;
							Debug.Log((GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).speedShip);
						}
					}
					if (hit.collider.tag == "UpgradePlayer2") {
						if(hit.collider.name == "vie2") {
							(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).lifeShip += 2;
						}
						if(hit.collider.name == "attaque2") {
							(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).powerMin += 2;
							(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).powerMax += 2;
						}
						if(hit.collider.name == "speed2") {
							(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).speedShip += 1;
						}
					}
				}
			}
			if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
				if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
					if (hit.collider.tag == "GUIPlayer1") {
						GameObject bonus = hit.collider.gameObject;
						Vector3 positionTemp = bonus.transform.position;
						Vector3 newPosition = Camera.main.ScreenToWorldPoint(touch.position);
						newPosition.y = positionTemp.y;
						bonus.transform.position = newPosition;
						Debug.Log (bonus.transform.position.ToString());
					}
				}
			}
		}
	}
}
