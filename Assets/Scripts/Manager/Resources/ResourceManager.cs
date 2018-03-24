using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    //注册资源的路径
    public void RegisterResource(string resourceName, string resourcePath, bool pool = false)
    {
        if (this.m_Resources.ContainsKey(resourceName))
        {
            Debug.Log("WARNING: ResourceManager already contains a resource with the name: " + resourceName);
        }
        else
        {
            this.m_Resources[resourceName] = new ResourceData(resourcePath, pool);
            this.resourceNames.Add(resourceName);
            if (pool)
            {
                this.m_ObjectPool[resourceName] = new List<GameObject>();
            }
        }
    }
    //预加载资源
    public void PreWarm()
    {
        foreach(string resourceName in this.m_Resources.Keys)
        {

        }
    }

    public bool ResourceExists(string resourceName)
    {
        return this.m_Resources.ContainsKey(resourceName);
    }

    public string GetResourcesPathByName(string resourceName)
    {
        if(!this.m_Resources.ContainsKey(resourceName))
        {
            Debug.Log("WARNING: ResourceManager.GetResourceByName does not contains a resource with the name" + resourceName);
            return null;
        }
        return this.m_Resources[resourceName].ResourcePath;
    }

    public GameObject GetGameObjectByName(string resourceName)
    {
        return (GameObject)this.GetRawObjectByName(resourceName);
    }

    public UnityEngine.Object GetRawObjectByName(string resourceName)
    {
        if(this.m_ResourceCache.ContainsKey(resourceName))
        {
            return this.m_ResourceCache[resourceName];
        }
        UnityEngine.Object @object = Resources.Load(this.GetResourcesPathByName(resourceName));
        this.m_ResourceCache.Add(resourceName, @object);
        return @object;
    }

    public GameObject InstantiateByName(string resourceName)
    {
        return this.InstantiateByName(resourceName, Vector3.zero, null);
    }

    public GameObject InstantiateByName(string resourceName,Vector3 pos,GameObject parent = null)
    {
        GameObject gameObject = null;
        bool flag = false;
        if(this.m_Resources.ContainsKey(resourceName))
        {
            flag = this.m_Resources[resourceName].pool;
        }
        if(flag && this.m_ObjectPool.ContainsKey(resourceName))
        {
            List<GameObject> list = this.m_ObjectPool[resourceName];
            while(list.Count > 0 && gameObject == null)
            {
                gameObject = list[0];
                list.RemoveAt(0);
            }
            if(gameObject != null)
            {
                gameObject.SetActive(true);
                gameObject.transform.position = pos;
            }
        }
        if(gameObject == null)
        {
            try
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(this.GetGameObjectByName(resourceName), pos, Quaternion.identity);
            }
            catch(ArgumentException ex)
            {
                Debug.LogError(resourceName + "Does not exists (" + ex.Message + ")");
            }
            if(flag)
            {
                this.m_ObjectPoolResourceLookup[gameObject] = resourceName;
            }
        }
        if (parent != null)
        {
            gameObject.transform.SetParent(parent.transform, false);
            gameObject.transform.localPosition = pos;
        }
        return gameObject;
    }

    public GameObject InstantiateByName(string resourceName,Vector3 pos,GameObject parent,Quaternion rotation)
    {
        GameObject gameObject = this.InstantiateByName(resourceName, pos, parent);
        if(rotation != Quaternion.identity)
        {
            gameObject.transform.localRotation = rotation;
        }
        return gameObject;
    }

    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
        if(this.m_ObjectPoolResourceLookup.ContainsKey(go))
        {
            string key = this.m_ObjectPoolResourceLookup[go];
            if(!this.m_ObjectPool[key].Contains(go))
            {
                this.m_ObjectPool[key].Add(go);
            }
        }
        else
        {
            UnityEngine.Object.Destroy(go);
        }
    }

    public void RemoveFromPool(GameObject go)
    {
        if(this.m_ObjectPoolResourceLookup.ContainsKey(go))
        {
            string key = this.m_ObjectPoolResourceLookup[go];
            this.m_ObjectPoolResourceLookup.Remove(go);
            if(this.m_ObjectPool[key].Contains(go))
            {
                this.m_ObjectPool[key].Remove(go);
            }
        }
    }

    private List<string> resourceNames = new List<string>();

    private Dictionary<string, ResourceData> m_Resources = new Dictionary<string, ResourceData>();

    private Dictionary<string, UnityEngine.Object> m_ResourceCache = new Dictionary<string, UnityEngine.Object>();

    private Dictionary<string, List<GameObject>> m_ObjectPool = new Dictionary<string, List<GameObject>>();

    private Dictionary<GameObject, string> m_ObjectPoolResourceLookup = new Dictionary<GameObject, string>();
}
