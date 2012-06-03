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
		
		//l= new ArrayList<GameObject>();
		
		
		
		
		int niveau =((moveShip)GameObject.FindGameObjectWithTag("User").GetComponent<moveShip>()).lvl;
		
		
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
						
						
						GameObject instance = (GameObject)Instantiate(pla);
						
						
						
						instance.name = planetInfos[0].InnerText;
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
								tmp.Add(planetEndInfos[0].InnerText,planetEndInfos[1].InnerText);
								string nameE = planetEndInfos[0].InnerText;
								GameObject planetEnd = GameObject.Find(nameE);
								GameObject instanceLink = (GameObject)Instantiate(Resources.Load("Line")as GameObject);
								instanceLink.name = nameS+""+nameE;
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
									instanceLink.active = false;	
								}else{
									instanceLink.active = true;	
								}
								
								
								//l.Length = l.Length+1;
								
								//l[l.Length-1] = instanceLink;
								
								l.Add(instanceLink);
								
				    		}
				  	 	}
						link.Add(planetInfos[0].InnerText, tmp);
						
		    		}
		  	 	}
			}
		}
		
		//l = GameObject.FindGameObjectsWithTag("link");
	}
	
	public bool roadExist(GameObject planetS, GameObject planetE){
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
				return true;
			}else{
				return false;	
			}
		}catch{
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
						if(l[i].name == ""+ps+""+pe){
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
			
			if(l[i].name[0] == planet.name[0]){
				LineRenderer line = l[i].GetComponent<LineRenderer>();
				Color c1;	
				if(((PlanetScript)planet.GetComponent<PlanetScript>()).ship.tag =="blue"){
					c1 = new Color(0,0,1,0.5f);
				}else{
					c1 = new Color(1,0,0,0.5f);
				}
				GameObject planet2 = GameObject.Find(""+l[i].name[1]);
				Color c2;
				if(((PlanetScript)planet2.GetComponent<PlanetScript>()).ship.tag =="blue"){
					c2 = new Color(0,0,1,0.5f);
				}else if(((PlanetScript)planet2.GetComponent<PlanetScript>()).ship.tag =="red"){
					c2 = new Color(1,0,0,0.5f);
				}else{
					c2 = new Color(1,1,1,0.5f);
				}
				line.SetColors(c1,c2);
				
			}else if(l[i].name[1] == planet.name[0]){
				LineRenderer line = l[i].GetComponent<LineRenderer>();
				Color c2;	
				if(((PlanetScript)planet.GetComponent<PlanetScript>()).ship.tag =="blue"){
					c2 = new Color(0,0,1,0.5f);
				}else{
					c2 = new Color(1,0,0,0.5f);
				}
				GameObject planet2 = GameObject.Find(""+l[i].name[0]);
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
}
