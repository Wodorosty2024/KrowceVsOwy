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
        obstaclesContainer.RemoveChildren();

        var rend = GetComponentInChildren<SpriteRenderer>();
        Vector2 extentsPlusDistance = new Vector2(PlayerController.instance.accumulatedDistance + rend.bounds.center.x-rend.bounds.extents.x, PlayerController.instance.accumulatedDistance + rend.bounds.center.x+rend.bounds.extents.x);
        var elements = GameManager.instance.levelConfig.Where(x => x.x >= extentsPlusDistance.x && x.x <= extentsPlusDistance.y).ToList();
        foreach (var element in elements)
        {
            var refObj = GameManager.instance.dynamicElementsPrefabs.FirstOrDefault(x => x.key == element.key);
            if (refObj == null)
            {
                Debug.LogError($"Key {element.key} doesn't exists in prefabs list");
                continue;
            }
            var obj = Instantiate(refObj, new Vector3(element.x, element.y, -1), Quaternion.identity, obstaclesContainer);
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
            Instantiate(GameManager.instance.dynamicElementsPrefabs[Random.Range(0, GameManager.instance.dynamicElementsPrefabs.Count)], new Vector3(x,y,0), Quaternion.identity, obstaclesContainer);
        }
    }
}
