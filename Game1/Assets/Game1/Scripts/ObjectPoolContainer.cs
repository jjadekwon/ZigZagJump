using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolContainer : MonoBehaviour
{
    private static ObjectPoolContainer instance;
    public static ObjectPoolContainer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ObjectPoolContainer");
                instance = obj.AddComponent<ObjectPoolContainer>();
            }

            return instance;
        }

    }


    // 누가, 무엇을, 몇개를 저장했는지 알 수 있는 변수
    Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();

    public void Create (string poolName, GameObject poolObj, int createCount)
    {
        List<GameObject> poolList = new List<GameObject>();

        for (int i = 0; i < createCount; i++)
        {
            GameObject cloneObj = Instantiate(poolObj, transform);
            cloneObj.name = poolName;
            poolList.Add(cloneObj);
        }
        poolDic.Add(poolName, poolList);
    }

    public GameObject Pop (string poolName)
    {
        GameObject returnObj;
        List<GameObject> poolList = poolDic[poolName];

        if (poolList.Count == 1)
        {
            GameObject cloneObj = Instantiate(poolList[0], transform);
            cloneObj.name = poolName;
            poolList.Add(cloneObj);
            Debug.LogError("Need More ObjectPool :" + poolName);
        }

        returnObj = poolList[0];
        poolList.RemoveAt(0);
        return returnObj;
    }

    public void Return (GameObject obj)
    {
        List<GameObject> poolList = poolDic[obj.name];
        poolList.Add(obj);
    }
    

    void OnDestroy()
    {
        instance = null;
    }
}
