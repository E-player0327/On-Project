using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public int spawnAmount;
    public List<GameObject> prefebs;
    Dictionary<string, Queue<GameObject>> poolingObject = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        

        for (int i = 0; i < prefebs.Count; i++)
        {
            if (!transform.Find(prefebs[i].name + "Pool"))
            {
                var poolingObj = new GameObject(prefebs[i].name + "Pool");
                poolingObj.transform.SetParent(this.gameObject.transform);
            }
            else
            {
                prefebs.RemoveAt(i);
                i--;
            }
        }
    }
    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        for (int i = 0; i < prefebs.Count; i++)
        {
            for (int j = 0; j < spawnAmount; j++)
            {
                if (!poolingObject.ContainsKey(prefebs[i].name + "Pool"))
                {
                    Queue<GameObject> Queue = new Queue<GameObject>();
                    poolingObject.Add(prefebs[i].name + "Pool", Queue);
                }
                var newObj = CreateObject(prefebs[i]);
                poolingObject[prefebs[i].name + "Pool"].Enqueue(newObj);
            }
            Debug.Log(poolingObject[prefebs[i].name + "Pool"].Count);
        }
    }

    public GameObject CreateObject(GameObject obj)
    {
        GameObject newObj = Instantiate(obj, transform.Find(obj.name + "Pool").transform);
        newObj.name = newObj.name.Substring(0, newObj.name.IndexOf("(Clone)"));
        newObj.SetActive(false);
        return newObj;
    }

    /// <summary>
    /// Get objects in the ObjectPool using list index
    /// </summary>
    ///<param name="index">Index of list</param>
    ///<param name="parent">Set object's parent</param>
    public static GameObject GetObject(int index, Transform parent)
    {
        GameObject getObject = instance.prefebs[index];
        if (instance.poolingObject.ContainsKey(getObject.name + "Pool"))
        {
            if (instance.poolingObject[getObject.name + "Pool"].Count < 1)
            {
                GameObject newObj = instance.CreateObject(getObject);
                newObj.SetActive(true);
                newObj.transform.SetParent(parent);
                return newObj;
            }
            else
            {
                var obj = instance.poolingObject[getObject.name + "Pool"].Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                return obj;
            }
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Get objects in the ObjectPool using prefeb name
    /// </summary>
    ///<param name="prefebName">Name of prefebs</param>
    ///<param name="parent">Set object's parent</param>
    public static GameObject GetObjectWithName(string prefebName, Transform parent)
    {
        int prefebIndex = 0;
        for (int i = 0; i < instance.prefebs.Count; i++)
        {
            if (prefebName == instance.prefebs[i].name)
                prefebIndex = i;
        }

        if (instance.poolingObject.ContainsKey(prefebName + "Pool"))
        {
            if (instance.poolingObject[prefebName + "Pool"].Count < 1)
            {
                GameObject newObj = instance.CreateObject(instance.prefebs[prefebIndex]);
                newObj.SetActive(true);
                newObj.transform.SetParent(parent);
                return newObj;
            }
            else
            {
                var obj = instance.poolingObject[prefebName + "Pool"].Dequeue();
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                return obj;
            }
        }
        return null;
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform.Find(obj.name + "Pool").transform);
        instance.poolingObject[obj.name + "Pool"].Enqueue(obj);
    }
}
