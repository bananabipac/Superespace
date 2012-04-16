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
		
		//DEBUG
		/*foreach(DictionaryEntry i in link){
			Debug.Log("PlaneteStart : "+i.Key);
			foreach(DictionaryEntry s in (Hashtable)i.Value){
				Debug.Log("PlaneteEnd : "+s.Key);
				Debug.Log("PlaneteLink : "+s.Value);
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}