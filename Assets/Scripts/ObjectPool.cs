using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int count=10;

    Dictionary<GameObject, bool> pool = new Dictionary<GameObject, bool>();
    GameObject CreateNew()
    {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            pool.Add(obj, false);
            return obj;
    }

    public GameObject GetObjectFromPool()
    {
        if (pool.Count < count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateNew();
            }
        }

        foreach (var element in pool)
        {
            if (!element.Value)
            {
                element.Key.SetActive(true);
                pool[element.Key]=true;
                return element.Key;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject g)
    {
        g.SetActive(false);
        pool[g]=false;
    }
}