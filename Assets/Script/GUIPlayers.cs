using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIPlayers : MonoBehaviour {
	
	public GameObject sabotage1;
	public GameObject nuke1;
	public GameObject crash1;
	
	public GameObject sabotage2;
	public GameObject nuke2;
	public GameObject crash2;
	
	public Vector3 posSabotage1;
	public Vector3 posNuke1;
	public Vector3 posCrash1;
	
	public Vector3 posSabotage2;
	public Vector3 posNuke2;
	public Vector3 posCrash2;
	
	public int lvlSpeed1;
	public int lvlAttack1;
	public int lvlLife1;
	
	public int lvlSpeed2;
	public int lvlAttack2;
	public int lvlLife2;
	
	public int crash;
	public int sabotage;
	public int apocalypse;
	
	private bool warnedAboutMaxTouches = false;
	private Vector2[] touchPos;
	private TouchPhase[] touchPhase;
	private int maxTouches = 5;
	
	public int crashPrice;
	public int sabotagePrice;
	public int nukePrice;
	private Dictionary<int,GameObject> listBonusesTouched = new Dictionary<int,GameObject>();
	
	private GameObject user;
	
	//public Transform prefabNuke;
	// Use this for initialization
	void Start () {
		
		posSabotage1 = sabotage1.transform.position;
		posNuke1 = nuke1.transform.position;
		posCrash1 = crash1.transform.position;
		
		posSabotage2 = sabotage2.transform.position;
		posNuke2 = nuke2.transform.position;
		posCrash2 = crash2.transform.position;
		
		lvlSpeed1 = 0;
		lvlAttack1 = 0;
		lvlLife1 = 0;
		
		lvlSpeed2 = 0;
		lvlAttack2 = 0;
		lvlLife2 = 0;
		
		user = GameObject.FindWithTag("User");
		
	
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
					//Debug.Log( "Oops! We got a finderId greater than maxTouches: " + touch.fingerId );
					warnedAboutMaxTouches = true;
				}
			}
			Ray cursorRay = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			if(touch.phase == TouchPhase.Ended) {
				//if(!user.GetComponent<PauseScript>().paused) {
					if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
						if (hit.collider.tag == "UpgradePlayer1") {
							if(hit.collider.name == "vie1") {
								if(lvlLife1 < 4){
									if( 100 * (lvlLife1+1) <= user.GetComponent<MoneyScript>().moneyPlayer1) {
										(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).lifeShip += 2;
										user.GetComponent<MoneyScript>().moneyPlayer1 -= (100 * (lvlLife1+1));
										lvlLife1++;
										GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeRed")as GameObject);
										Vector3 vec = GameObject.Find("vie1").transform.position;
										vec.x -= 1.5f;
										vec.z -= 1.4f;
										upgradeFeedBack.transform.position = vec;
										
									}
								}
							}
							if(hit.collider.name == "attaque1") {
								if(lvlAttack1 < 4) {
									if( 100 * (lvlAttack1+1) <= user.GetComponent<MoneyScript>().moneyPlayer1) {
										(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMin += 2;
										(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).powerMax += 2;
										user.GetComponent<MoneyScript>().moneyPlayer1 -= (100 * (lvlAttack1+1));
										lvlAttack1++;
										GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeRed")as GameObject);
										Vector3 vec = GameObject.Find("attaque1").transform.position;
										vec.x -= 1.5f;
										vec.z -= 1.4f;
										upgradeFeedBack.transform.position = vec;
										
									}
								}
							}
							if(hit.collider.name == "vitesse1") {
								if(lvlSpeed1 < 4) {
									if( 100 * (lvlSpeed1+1) <= user.GetComponent<MoneyScript>().moneyPlayer1) {
										(GameObject.FindGameObjectWithTag("infoUserRed").GetComponent<infoUser>()).speedShip -= 0.2f;
										user.GetComponent<MoneyScript>().moneyPlayer1 -= (100 * (lvlSpeed1+1));
										lvlSpeed1++;
										GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeRed")as GameObject);
										Vector3 vec = GameObject.Find("vitesse1").transform.position;
										vec.x -= 1.5f;
										vec.z -= 1.4f;
										upgradeFeedBack.transform.position = vec;
										
									}
								}
							}
						}
						if(PlayerPrefs.GetString("GameType").Equals("versus")) {
							if (hit.collider.tag == "UpgradePlayer2") {
								if(hit.collider.name == "vie2") {
									if(lvlLife2 < 4){
										if( 100 * (lvlLife2+1) <= user.GetComponent<MoneyScript>().moneyPlayer2) {
											(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).lifeShip += 2;
											user.GetComponent<MoneyScript>().moneyPlayer2 -= (100 * (lvlLife2+1));
											lvlLife2++;
											GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeBlue")as GameObject);
											Vector3 vec = GameObject.Find("vie2").transform.position;
											vec.x += 1.5f;
											vec.z += 1.4f;
											upgradeFeedBack.transform.position = vec;
											
										}
									}
								}
								if(hit.collider.name == "attaque2") {
									if(lvlAttack2 < 4) {
										if( 100 * (lvlAttack2+1) <= user.GetComponent<MoneyScript>().moneyPlayer2) {
											(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).powerMin += 2;
											(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).powerMax += 2;
											user.GetComponent<MoneyScript>().moneyPlayer2 -= (100 * (lvlAttack2+1));
											lvlAttack2++;
											GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeBlue")as GameObject);
											Vector3 vec = GameObject.Find("attaque2").transform.position;
											vec.x += 1.5f;
											vec.z += 1.4f;
											upgradeFeedBack.transform.position = vec;
											
										}
									}
								}
								if(hit.collider.name == "vitesse2") {
									if(lvlSpeed2 < 4) {
										//Debug.Log ("speed");
										if( 100 * (lvlSpeed2+1) <= user.GetComponent<MoneyScript>().moneyPlayer2) {
											(GameObject.FindGameObjectWithTag("infoUserBlue").GetComponent<infoUser>()).speedShip -= 0.2f;
											user.GetComponent<MoneyScript>().moneyPlayer2 -= (100 * (lvlSpeed2+1));
											lvlSpeed2++;
											GameObject upgradeFeedBack = (GameObject)Instantiate(Resources.Load("upgradeBlue")as GameObject);
											Vector3 vec = GameObject.Find("vitesse2").transform.position;
											vec.x += 1.5f;
											vec.z += 1.4f;
											upgradeFeedBack.transform.position = vec;
											//Debug.Log ("upgraded");
										}
									}
								}
							}
						}
					}
				//}
				GameObject bonus = new GameObject();
				if(listBonusesTouched.ContainsKey(fingerId)) {
					bonus = listBonusesTouched[fingerId];
				}
				RaycastHit[] hits;
				hits = Physics.RaycastAll(cursorRay,1000f);
				if(!user.GetComponent<PauseScript>().paused2) {
					for(int i =0; i< hits.Length; i++) {
						RaycastHit hitted = hits[i];
						GameObject objet = hitted.collider.gameObject;
						if(objet.tag == "planet") {
							if (bonus != null) {
								if(bonus.tag == "GUIPlayer1"){
									if(bonus.name == "crash1") {
										if(user.GetComponent<MoneyScript>().moneyPlayer1 >= crashPrice) {
											if(objet.GetComponent<PlanetScript>().ship.tag != "red" && objet.GetComponent<PlanetScript>().ship.tag != "neutre") {
												objet.GetComponent<PlanetScript>().repop += crash;
												user.GetComponent<MoneyScript>().moneyPlayer1-= crashPrice;
												GameObject expl = (GameObject)Instantiate(Resources.Load("crash")as GameObject);
												expl.transform.position = objet.transform.position;
											
												GameObject.FindGameObjectWithTag("User").GetComponent<stats>().nbCrashRed ++;
											
											}
										}
										
									}
									if(bonus.name == "sabotage1") {
										if(user.GetComponent<MoneyScript>().moneyPlayer1 >= sabotagePrice) {
											if(objet.GetComponent<PlanetScript>().ship.tag != "red") {
												if(objet.GetComponent<PlanetScript>().ship.tag == "neutre"){
													int deleteShip = (int)Mathf.Floor(objet.GetComponent<PlanetScript>().shipsN.Count*sabotage/100);
													for(int ships = 0; ships < deleteShip; ships++) {
														GameObject temp = objet.GetComponent<PlanetScript>().shipsN[0];
														objet.GetComponent<PlanetScript>().shipsN.RemoveAt(0);
														GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
														expl.transform.position = temp.transform.position;
														Destroy (temp);
														GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipNPower ++;
														
													}
													user.GetComponent<MoneyScript>().moneyPlayer1-= sabotagePrice;
												}
												if(objet.GetComponent<PlanetScript>().ship.tag == "blue"){
													int deleteShip = (int)Mathf.Floor(objet.GetComponent<PlanetScript>().shipsB.Count*sabotage/100);
													for(int ships = 0; ships < deleteShip; ships++) {
														GameObject temp = objet.GetComponent<PlanetScript>().shipsB[0];
														objet.GetComponent<PlanetScript>().shipsB.RemoveAt(0);
														GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
														expl.transform.position = temp.transform.position;
														Destroy (temp);
														GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipBPower ++;
														
													}
													user.GetComponent<MoneyScript>().moneyPlayer1-= sabotagePrice;
												}
												GameObject.FindGameObjectWithTag("User").GetComponent<stats>().nbSabotageRed ++;
												
											}
											
										}
										
									}
									if(bonus.name == "nuke1") {
										if(user.GetComponent<MoneyScript>().moneyPlayer1 >= nukePrice) {
											if(objet.GetComponent<PlanetScript>().ship.tag != "red") {
												if(objet.GetComponent<PlanetScript>().ship.tag == "neutre"){
													int deleteShip = objet.GetComponent<PlanetScript>().shipsN.Count;
													GameObject apo = (GameObject)Instantiate(Resources.Load("apocalypse")as GameObject);
													apo.transform.position = objet.transform.position;
													for(int ships = 0; ships < deleteShip; ships++) {
														GameObject temp = objet.GetComponent<PlanetScript>().shipsN[0];
														objet.GetComponent<PlanetScript>().shipsN.RemoveAt(0);
														GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
														expl.transform.position = temp.transform.position;
														Destroy (temp);
														GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipNPower ++;
													}
													user.GetComponent<MoneyScript>().moneyPlayer1-= nukePrice;
												}
												if(objet.GetComponent<PlanetScript>().ship.tag == "blue"){
													int deleteShip = objet.GetComponent<PlanetScript>().shipsB.Count;
													GameObject apo = (GameObject)Instantiate(Resources.Load("apocalypse")as GameObject);
													apo.transform.position = objet.transform.position;
													for(int ships = 0; ships < deleteShip; ships++) {
														GameObject temp = objet.GetComponent<PlanetScript>().shipsB[0];
														objet.GetComponent<PlanetScript>().shipsB.RemoveAt(0);
														GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
														expl.transform.position = temp.transform.position;
														Destroy (temp);
														GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipBPower ++;
													}
													objet.GetComponent<PlanetScript>().repop += apocalypse;
													user.GetComponent<MoneyScript>().moneyPlayer1-= nukePrice;
													//Debug.Log ("Explosion");
													//Instantiate(prefabNuke,hit.collider.gameObject.transform.position,Quaternion.identity);
												}
												GameObject.FindGameObjectWithTag("User").GetComponent<stats>().nbNukeRed ++;
												
											}
											
										}
										
									}
								}
								if(PlayerPrefs.GetString("GameType").Equals("versus")) {
									if(bonus.tag == "GUIPlayer2"){
										if(bonus.name == "crash2") {
											if(user.GetComponent<MoneyScript>().moneyPlayer2 >= crashPrice) {
												if(objet.GetComponent<PlanetScript>().ship.tag != "blue" && objet.GetComponent<PlanetScript>().ship.tag != "neutre") {
													objet.GetComponent<PlanetScript>().repop += crash;
													user.GetComponent<MoneyScript>().moneyPlayer2-= crashPrice;
													GameObject expl = (GameObject)Instantiate(Resources.Load("crash")as GameObject);
													expl.transform.position = objet.transform.position;
												}
												GameObject.FindGameObjectWithTag("User").GetComponent<stats>().nbCrashBlue ++;
												
											}
											
										}
										if(bonus.name == "sabotage2") {
											if(user.GetComponent<MoneyScript>().moneyPlayer2 >= sabotagePrice) {
												if(objet.GetComponent<PlanetScript>().ship.tag != "blue") {
													if(objet.GetComponent<PlanetScript>().ship.tag == "neutre"){
														int deleteShip = (int)Mathf.Floor(objet.GetComponent<PlanetScript>().shipsN.Count*sabotage/100);
														for(int ships = 0; ships < deleteShip; ships++) {
															GameObject temp = objet.GetComponent<PlanetScript>().shipsN[0];
															objet.GetComponent<PlanetScript>().shipsN.RemoveAt(0);
															GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
															expl.transform.position = temp.transform.position;
															Destroy (temp);
															GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipNPower ++;
														}
														user.GetComponent<MoneyScript>().moneyPlayer2-= sabotagePrice;
													}
													if(objet.GetComponent<PlanetScript>().ship.tag == "red"){
														int deleteShip = (int)Mathf.Floor(objet.GetComponent<PlanetScript>().shipsR.Count*sabotage/100);
														for(int ships = 0; ships < deleteShip; ships++) {
															GameObject temp = objet.GetComponent<PlanetScript>().shipsR[0];
															objet.GetComponent<PlanetScript>().shipsR.RemoveAt(0);
															GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
															expl.transform.position = temp.transform.position;
															Destroy (temp);
															GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipRPower ++;
														}
														user.GetComponent<MoneyScript>().moneyPlayer2-= sabotagePrice;
													}
												}
												GameObject.FindGameObjectWithTag("User").GetComponent<stats>().nbSabotageBlue ++;
												
											}
											
										}
										if(bonus.name == "nuke2") {
											if(user.GetComponent<MoneyScript>().moneyPlayer2 >= nukePrice) {
												if(objet.GetComponent<PlanetScript>().ship.tag != "blue") {
													if(objet.GetComponent<PlanetScript>().ship.tag == "neutre"){
														int deleteShip = objet.GetComponent<PlanetScript>().shipsN.Count;
														GameObject apo = (GameObject)Instantiate(Resources.Load("apocalypse")as GameObject);
														apo.transform.position = objet.transform.position;
														for(int ships = 0; ships < deleteShip; ships++) {
															GameObject temp = objet.GetComponent<PlanetScript>().shipsN[0];
															objet.GetComponent<PlanetScript>().shipsN.RemoveAt(0);
															GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
															expl.transform.position = temp.transform.position;
															Destroy (temp);
															GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipNPower ++;
														}
														user.GetComponent<MoneyScript>().moneyPlayer2-= nukePrice;
													}
													if(objet.GetComponent<PlanetScript>().ship.tag == "red"){
														int deleteShip = objet.GetComponent<PlanetScript>().shipsR.Count;
														GameObject apo = (GameObject)Instantiate(Resources.Load("apocalypse")as GameObject);
														apo.transform.position = objet.transform.position;
														for(int ships = 0; ships < deleteShip; ships++) {
															GameObject temp = objet.GetComponent<PlanetScript>().shipsR[0];
															objet.GetComponent<PlanetScript>().shipsR.RemoveAt(0);
															GameObject expl = (GameObject)Instantiate(Resources.Load("explosion")as GameObject);
															expl.transform.position = temp.transform.position;
															Destroy (temp);
															GameObject.FindGameObjectWithTag("User").GetComponent<stats>().destroyShipRPower ++;
														}
														objet.GetComponent<PlanetScript>().repop += apocalypse;
														user.GetComponent<MoneyScript>().moneyPlayer2-= nukePrice;
														//Debug.Log ("Explosion");
														//Instantiate(prefabNuke,hit.collider.gameObject.transform.position,Quaternion.identity);	
													}
													GameObject.FindGameObjectWithTag("User").GetComponent<stats>().nbNukeBlue ++;
													
												}
												
											}
											
										}
									}
								}
							}
						}
						crash1.transform.position = posCrash1;
						sabotage1.transform.position = posSabotage1;
						nuke1.transform.position = posNuke1;
						crash2.transform.position = posCrash2;
						sabotage2.transform.position = posSabotage2;
						nuke2.transform.position = posNuke2;
					}
					if(listBonusesTouched.ContainsKey(fingerId)) {
						listBonusesTouched.Remove(fingerId);
					}
				}
			}
			if(touch.phase == TouchPhase.Began) {
				if(!user.GetComponent<PauseScript>().paused2) {
					if(Physics.Raycast(cursorRay, out hit, 1000.0f)) {
						if (hit.collider.tag == "GUIPlayer1" || hit.collider.tag == "GUIPlayer2") {
							if(!PlayerPrefs.GetString("GameType").Equals("versus")) {
								if(hit.collider.tag == "GUIPlayer1"){
									GameObject bonus = hit.collider.gameObject;
									listBonusesTouched.Add(fingerId,bonus);
								}
							}else{
								GameObject bonus = hit.collider.gameObject;
								listBonusesTouched.Add(fingerId,bonus);
							}
						}
					}
				}
			}
			if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
				if(!user.GetComponent<PauseScript>().paused2) {
					if(listBonusesTouched.ContainsKey(fingerId)) {
						GameObject bonus = listBonusesTouched[fingerId];
						Vector3 positionTemp = bonus.transform.position;
						Vector3 newPosition = Camera.main.ScreenToWorldPoint(touch.position);
						newPosition.y = positionTemp.y;
						bonus.transform.position = newPosition;
					}
				}
			}
		}
	}
}