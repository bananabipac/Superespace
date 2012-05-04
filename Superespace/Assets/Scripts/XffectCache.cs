using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//NOTICE:Objects���ɺ�������Destroy()����active=false�ͷš�
public class XffectCache : MonoBehaviour {

    Dictionary<string, ArrayList> ObjectDic = new Dictionary<string, ArrayList>();

	void Awake () {
        foreach (Transform child in transform)
        {
            ObjectDic[child.name] = new ArrayList();
            ObjectDic[child.name].Add(child);
            Xffect xft = child.GetComponent<Xffect>();
            //make sure all child is init.
            if (xft != null)
                xft.Initialize();
            child.gameObject.active = false;
        }
	}

    void Start()
    {
    }

    //Ĭ��activeΪ false��Ӧ���ɵ��÷�active.
    protected Transform AddObject(string name)
    {
        Transform baseobj = transform.Find(name);
        if (baseobj == null)
        {
            Debug.Log("object:" + name + "doesn't exist!");
            return null;
        }
        Transform newobj = Instantiate(baseobj, Vector3.zero, Quaternion.identity) as Transform;
        ObjectDic[name].Add(newobj);
        newobj.gameObject.active = false;
        Xffect xft = newobj.GetComponent<Xffect>();
        if (xft != null)
            xft.Initialize();
        return newobj;
    }

    public ArrayList GetObjectCache(string name)
    {
        ArrayList cache = ObjectDic[name];
        if (cache == null)
        {
            Debug.LogError(name + ": cache doesnt exist!");
            return null;
        }
        return cache;
    }

    public Transform GetObject(string name)
    {
        ArrayList cache = ObjectDic[name];
        if (cache == null)
        {
            Debug.LogError(name + ": cache doesnt exist!");
            return null;
        }
        foreach (Transform obj in cache)
        {
            if (!obj.gameObject.active)
            {
                obj.gameObject.active = true;
                return obj;
            }
        }
        return AddObject(name);
    }
}
