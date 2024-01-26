using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int count=10;

    List<GameObject> pool = new List<GameObject>();

    public void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        foreach (var element in pool)
        {
            if (!element.activeInHierarchy)
            {
                element.SetActive(true);
                return element;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject g)
    {
        g.SetActive(false);
    }
}