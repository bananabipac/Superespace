using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
  

public class GestionLink : MonoBehaviour {
	
	private XmlDocument xmlDocs;
	public Hashtable link;
	//public GameObject[] l;
	public List<GameObject> l;
	
	// Use this for initialization
	void Start () {
		TextAsset test = Resources.Load("generator")as TextAsset;
		xmlDocs = new XmlDocument();
		xmlDocs.LoadXml(test.text);
		link = new Hashtable();
		Hashtable tmp ;
		
		
		
		int niveau =((moveShip)GameObject.FindGameObjectWithTag("User").GetComponent<moveShip>()).lvl;
		
		SpaceBridge hole = GameObject.FindGameObjectWithTag("User").GetComponent<SpaceBridge>();
		
		//Generateur de planete
		XmlNodeList levelsList = xmlDocs.GetElementsByTagName("lvl");
		
		foreach (XmlNode levelInfo in levelsList){
			
			XmlNodeList levelcontent = levelInfo.ChildNodes;
			if(levelcontent[0].InnerText == ""+niveau){
	   			foreach (XmlNode planet in levelcontent){
		    		if(planet.Name == "planet"){
						XmlNodeList planetInfos = planet.ChildNodes;
						
						GameObject pla = Resources.Load("planet"+planetInfos[6].InnerText)as GameObject;
						
						pla.transform.localScale = new Vector3(float.Parse( planetInfos[5].InnerText),float.Parse( planetInfos[5].InnerText), float.Parse( planetInfos[5].InnerText)); 
						
						pla.transform.position = new Vector3(float.Parse( planetInfos[1].InnerText), -23.3f, float.Parse( planetInfos[2].InnerText));
						
						if(planetInfos[3].InnerText == "red"){
							((PlanetScript)pla.GetComponent<PlanetScript>()).ship = Resources.Load("Shipred")as GameObject;
						}else if(planetInfos[3].InnerText == "blue"){
							((PlanetScript)pla.GetComponent<PlanetScript>()).ship = Resources.Load("Shipblue")as GameObject;
						}else if(planetInfos[3].InnerText == "neutre"){
							((PlanetScript)pla.GetComponent<PlanetScript>()).ship = Resources.Load("ShipN")as GameObject;
						}
						
						((PlanetScript)pla.GetComponent<PlanetScript>()).nbShip = int.Parse(planetInfos[4].InnerText);
						((PlanetScript)pla.GetComponent<PlanetScript>()).repop =  int.Parse(planetInfos[7].InnerText);
						
						((PlanetScript)pla.GetComponent<PlanetScript>()).LimitPop = int.Parse(planetInfos[9].InnerText);
						
						GameObject instance = (GameObject)Instantiate(pla);
						
						
						instance.name = planetInfos[0].InnerText;
						
						//trou noir
						if(planetInfos[8].InnerText == "1"){
							hole.planet1 = instance;
						}else if(planetInfos[8].InnerText == "2"){
							hole.planet2 = instance;	
						}
						
		    		}
		  	 	}
			}
		}
		
		XmlNodeList links = xmlDocs.GetElementsByTagName("link");
		
		foreach (XmlNode linksInfo in links){
			XmlNodeList linkInfo = linksInfo.ChildNodes;
			if(linkInfo[0].InnerText == ""+niveau){
	   			foreach (XmlNode planet in linkInfo){
		    		if(planet.Name == "planet"){
						XmlNodeList planetInfos = planet.ChildNodes;
						tmp = new Hashtable();
						string nameS = planetInfos[0].InnerText; 
						GameObject planetStart = GameObject.Find(nameS);
						foreach (XmlNode planetsEnd in planetInfos){
				    		if(planetsEnd.Name == "planet"){	
								XmlNodeList planetEndInfos = planetsEnd.ChildNodes;
								if(PlayerPrefs.GetString("mode") == "classic"){
									tmp.Add(planetEndInfos[0].InnerText,planetEndInfos[1].InnerText);
								}else if(PlayerPrefs.GetString("mode") == "quick"){
									tmp.Add(planetEndInfos[0].InnerText,"1");
								}
								string nameE = planetEndInfos[0].InnerText;
								GameObject planetEnd = GameObject.Find(nameE);
								GameObject instanceLink = (GameObject)Instantiate(Resources.Load("Line")as GameObject);
								instanceLink.name = nameS+"."+nameE;
								LineRenderer line = instanceLink.GetComponent<LineRenderer>();
								
								line.SetPosition(0,planetStart.transform.position);
								
								line.SetPosition(1,planetEnd.transform.position);
								Color c1;
								if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="blue"){
									c1 = new Color(0,0,1,0.5f);
								}else if(((PlanetScript)planetStart.GetComponent<PlanetScript>()).ship.tag =="red"){
									c1 = new Color(1,0,0,0.5f);
								}else{
									c1 = new Color(1,1,1,0.5f);
								}
								Color c2;
								if(((PlanetScript)planetEnd.GetComponent<PlanetScript>()).ship.tag =="blue"){
									c2 = new Color(0,0,1,0.5f);
								}else if(((PlanetScript)planetEnd.GetComponent<PlanetScript>()).ship.tag =="red"){
									c2 = new Color(1,0,0,0.5f);
								}else{
									c2 = new Color(1,1,1,0.5f);
								}
								line.SetColors(c1,c2);
								line.SetWidth(0.15f,0.15f);
								
								if(planetEndInfos[1].InnerText == "0"){
									if(PlayerPrefs.GetString("mode") == "classic"){
										instanceLink.active = false;
									}else if(PlayerPrefs.GetString("mode") == "quick"){
										instanceLink.active = true;
									}	
								}else{
									instanceLink.active = true;	
								}
								
								if(planetEndInfos[2].InnerText == "1"){
									GameObject instanceAstero = (GameObject)Instantiate(Resources.Load("asteroid")as GameObject);
									Vector3 st = planetStart.transform.position;
									Vector3 en = planetEnd.transform.position;
									Vector3 m = st + (0.5f*(en-st));
									//Debug.Log ("Asteroid");
									instanceAstero.transform.position = m;
									instanceAstero.name = "a"+nameS+"."+nameE;
									instanceAstero.active = true;
								}
								
								
								l.Add(instanceLink);
								
				    		}
				  	 	}
						link.Add(planetInfos[0].InnerText, tmp);
						
		    		}
		  	 	}
			}
		}
		
		int[] income = nbRoad();
		
		MoneyScript money = GameObject.FindGameObjectWithTag("User").GetComponent<MoneyScript>();
		if(PlayerPrefs.GetString("mode") == "classic"){
			money.GetComponent<MoneyScript>().incomePlayer1 =1;
			money.GetComponent<MoneyScript>().incomePlayer2 =1;
		}else if(PlayerPrefs.GetString("mode") == "quick"){
			money.GetComponent<MoneyScript>().incomePlayer1 = 1 + income[0];
			money.GetComponent<MoneyScript>().incomePlayer2 = 1 + income[1];
		}
	}
	
	public bool roadExist(GameObject planetS, GameObject planetE){
		string ps,pe;
		if(planetE.tag != "BlackHole"){
			if(int.Parse(planetS.name) > int.Parse(planetE.name)){
				ps = planetE.name;
				pe = planetS.name;
			}else{
				pe = planetE.name;
				ps = planetS.name;
			}
		
		
			try{
			
				if(((Hashtable)link[ps])[pe] != null){
					return true;
				}else{
					return false;	
				}
			}catch{
				return false;	
			}
		}else{
			return false;	
		}
	}
	
	public bool roadOpen(GameObject planetS, GameObject planetE){
		string ps,pe;
		if(int.Parse(planetS.name) > int.Parse(planetE.name)){
			ps = planetE.name;
			pe = planetS.name;
		}else{
			pe = planetE.name;
			ps = planetS.name;
		}
		
		try{
		
			if(((Hashtable)link[ps])[pe] != null){
				if((string)((Hashtable)link[ps])[pe]  == "1"){
					return true;
				}else{
					return false;	
				}
			}else{
				return false;	
			}
		}catch{
			return false;	
		}
		
	}
	
	public void openRoad(GameObject planetS, GameObject planetE){
		string ps,pe;
		if(int.Parse(planetS.name) > int.Parse(planetE.name)){
			ps = planetE.name;
			pe = planetS.name;
		}else{
			pe = planetE.name;
			ps = planetS.name;
		}
		
		try{
			if(((Hashtable)link[ps])[pe] != null){
				if((string)((Hashtable)link[ps])[pe]  == "0"){
					((Hashtable)link[ps])[pe] = "1";
					for(int i = 0; i<l.Count; i++){
						if(l[i].name == ""+ps+"."+pe){
							l[i].active=true;
						}
					}
				}
			}
		}catch{
				
		}
	}
	
	public void changeColor(GameObject planet){
		
		for(int i=0; i<l.Count; i++){
			string[] split = l[i].name.Split('.');
			if(""+split[0] == ""+planet.name){
				LineRenderer line = l[i].GetComponent<LineRenderer>();
				Color c1;	
				if(((PlanetScript)planet.GetComponent<PlanetScript>()).ship.tag =="blue"){
					c1 = new Color(0,0,1,0.5f);
				}else{
					c1 = new Color(1,0,0,0.5f);
				}
				GameObject planet2 = GameObject.Find(""+split[1]);
				Color c2;
				if(((PlanetScript)planet2.GetComponent<PlanetScript>()).ship.tag =="blue"){
					c2 = new Color(0,0,1,0.5f);
				}else if(((PlanetScript)planet2.GetComponent<PlanetScript>()).ship.tag =="red"){
					c2 = new Color(1,0,0,0.5f);
				}else{
					c2 = new Color(1,1,1,0.5f);
				}
				line.SetColors(c1,c2);
				
			}else if(""+split[1] == ""+planet.name){
				LineRenderer line = l[i].GetComponent<LineRenderer>();
				Color c2;	
				if(((PlanetScript)planet.GetComponent<PlanetScript>()).ship.tag =="blue"){
					c2 = new Color(0,0,1,0.5f);
				}else{
					c2 = new Color(1,0,0,0.5f);
				}
				GameObject planet2 = GameObject.Find(""+split[0]);
				Color c1;
				if(((PlanetScript)planet2.GetComponent<PlanetScript>()).ship.tag =="blue"){
					c1 = new Color(0,0,1,0.5f);
				}else if(((PlanetScript)planet2.GetComponent<PlanetScript>()).ship.tag =="red"){
					c1 = new Color(1,0,0,0.5f);
				}else{
					c1 = new Color(1,1,1,0.5f);
				}
				line.SetColors(c1,c2);
			}
		}
	}
	
	public int[] nbRoad(){
	
		
		int ir =0;
		int ib =0;
		
		for(int i = 0 ; i<l.Count; i++){
			if(l[i].active == true){
				string[] split = l[i].name.Split('.');
				PlanetScript tmp = GameObject.Find(""+split[0]).GetComponent<PlanetScript>();
				
				if(tmp.ship.tag == "blue"){
					PlanetScript tmp2 = GameObject.Find(""+split[1]).GetComponent<PlanetScript>();
					if(tmp2.ship.tag != "red"){
						ib +=1;	
					}
				}else if(tmp.ship.tag =="red"){
					PlanetScript tmp2 = GameObject.Find(""+split[1]).GetComponent<PlanetScript>();
					if(tmp2.ship.tag != "blue"){
						ir +=1;	
					}
					
				}else{
					PlanetScript tmp2 = GameObject.Find(""+split[1]).GetComponent<PlanetScript>();
					if(tmp2.ship.tag == "blue"){
						ib +=1;	
					}
					if(tmp2.ship.tag == "red"){
						ir +=1;	
					}
				}
			}
		}
		
		int[] result = new int[2];
		//result.Length = 2;
		result[0] = ir;
		result[1] = ib;
		
		return result;
				
	}
	
	public void activeAsteroid(GameObject planetS, GameObject planetE, List<GameObject> shipsT){
	
		string ps,pe;
		if(int.Parse(planetS.name) > int.Parse(planetE.name)){
			ps = planetE.name;
			pe = planetS.name;
		}else{
			pe = planetE.name;
			ps = planetS.name;
		}
		
		GameObject asteroid = GameObject.Find("a"+ps+"."+pe);
			if(asteroid != null){
				asteroidScript asteroScript = asteroid.GetComponent<asteroidScript>();
				for(int i=0 ; i<shipsT.Count; i++){
					asteroScript.ships.Add(shipsT[i]);	
				}
				
			}
	}
	
	
}
