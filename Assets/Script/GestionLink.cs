using UnityEngine;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
  

public class GestionLink : MonoBehaviour {
	
	public GameObject[] l;
	private XmlDocument xmlDoc;
	public Hashtable link;
	// Use this for initialization
	void Start () {
		TextAsset test = Resources.Load("dataLevel")as TextAsset;
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(test.text);
		link = new Hashtable();
		Hashtable tmp ;

		l = GameObject.FindGameObjectsWithTag("link");
		int niveau =((moveShip)GameObject.FindGameObjectWithTag("User").GetComponent<moveShip>()).lvl;
		XmlNodeList levelsList = xmlDoc.GetElementsByTagName("level");//recuperation des niveaux
		
		foreach (XmlNode levelInfo in levelsList){//parcours de tout les niveaux
			
			XmlNodeList levelcontent = levelInfo.ChildNodes;//recuperation des infos du niveau
			if(levelcontent[0].InnerText == ""+niveau){//si le niveau est le niveau choisit
	   			foreach (XmlNode planetS in levelcontent){//parcours des planetes du niveau
		    		if(planetS.Name == "planet"){
						XmlNodeList planetsEn = planetS.ChildNodes;
						tmp = new Hashtable();
						foreach (XmlNode planetE in planetsEn){
							if(planetE.Name =="planet"){
								XmlNodeList planetEI = planetE.ChildNodes;
								tmp.Add(planetEI[0].InnerText,planetEI[1].InnerText);
								if(planetEI[1].InnerText == "0"){
									for(int i = 0; i<l.Length; i++){
										if(l[i].name == ""+planetsEn[0].InnerText+""+planetEI[0].InnerText){
											l[i].active=false;
											
											LineRenderer line = l[i].GetComponent<LineRenderer>();
											line.SetPosition(0, GameObject.Find(planetsEn[0].InnerText).transform.position);
											line.SetPosition(1, GameObject.Find(planetEI[0].InnerText).transform.position);
											line.SetColors(new Color(1,1,1,0.5f),new Color(1,1,1,0.5f));
											line.SetWidth(0.1f,0.1f);
											
											
										}
									}
								}
							}
						}
						link.Add(planetsEn[0].InnerText, tmp);
		    		}
		  	 	}
			}
		}
			
	}
	
	// Update is called once per frame
	void Update () {
	
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
					for(int i = 0; i<l.Length; i++){
						if(l[i].name == ""+ps+""+pe){
							l[i].active=true;
						}
					}
				}
			}
		}catch{
				
		}
	}
}
