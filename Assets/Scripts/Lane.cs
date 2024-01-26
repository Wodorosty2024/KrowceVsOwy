using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Transform obstaclesContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Prepare()
    {
        var toDelete = new List<Transform>();
        foreach (var child in obstaclesContainer)
        {
            toDelete.Add(child as Transform);
        }
        for (int i=0; i < toDelete.Count; i++) Destroy(toDelete[i].gameObject);

        var rend = GetComponentInChildren<SpriteRenderer>();
        Vector2 extents = new Vector2(rend.bounds.center.x-rend.bounds.extents.x, rend.bounds.center.x+rend.bounds.extents.x);
        var elements = GameManager.instance.levelConfig.Where(x => x.x >= extents.x && x.x <= extents.y);
        foreach (var element in elements)
        {
            var refObj = GameManager.instance.dynamicElementsPrefabs.FirstOrDefault(x => x.key == element.key);
            if (refObj == null)
            {
                Debug.LogError($"Key {element.key} doesn't exists in prefabs list");
                continue;
            }
            var obj = Instantiate(refObj, new Vector3(element.x-extents.x, element.y, -1), Quaternion.identity, transform);
        }
        SpawnRandomObstacles();
    }

    void SpawnRandomObstacles()
    {
        var count = 5;
        var bounds = GetComponentInChildren<SpriteRenderer>().bounds;
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(bounds.center.x-bounds.extents.x, bounds.center.x+bounds.extents.x);
            float y = Random.Range(bounds.center.y-bounds.extents.y, bounds.center.y+bounds.extents.y);
            Instantiate(GameManager.instance.dynamicElementsPrefabs[0], new Vector3(x,y,0), Quaternion.identity, obstaclesContainer);
        }
    }
}
