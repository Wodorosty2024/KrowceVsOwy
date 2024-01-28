using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Transform obstaclesContainer;
    public Parallax parallax;
    public int id;

    public static Queue<DynamicallyLoadedLevelElement> queued = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void PostPrepare()
    {
        for (int i = 0; i < queued.Count; i++)
        {
            var e = queued.Dequeue();
            if (!e.OnSpawned())
                queued.Enqueue(e);
        }
    }

    public void Prepare(bool initialLane=false)
    {
        id = DynamicallyLoadedLevelElement.lastId++;
        obstaclesContainer.RemoveChildren();
        if (parallax != null) parallax.Prepare();
        if (initialLane) return;        

        var rend = GetComponentInChildren<SpriteRenderer>();
        Vector2 extentsPlusDistance = new Vector2(PlayerController.instance.accumulatedDistance + rend.bounds.center.x-rend.bounds.extents.x, PlayerController.instance.accumulatedDistance + rend.bounds.center.x+rend.bounds.extents.x);
        // var elements = GameManager.instance.levelConfig.Where(x => x.x >= extentsPlusDistance.x && x.x <= extentsPlusDistance.y).ToList();
        var elements = GameManager.instance.levelConfig.Where(x => x.lane == id).ToList();
        foreach (var element in elements)
        {
            var refObj = GameManager.instance.dynamicElementsPrefabs.FirstOrDefault(x => x.key == element.key);
            if (refObj == null)
            {
                Debug.LogError($"Key {element.key} doesn't exists in prefabs list");
                continue;
            }
            var obj = Instantiate(refObj, new Vector3(element.x, element.y, -1), Quaternion.identity, obstaclesContainer);
            obj.transform.localPosition = new Vector3(element.x, element.y, 0);
            obj.id = element.id;
            obj.userName = element.user;
            obj.userComment=element.sentence;
            obj.name+="_UG";
            obj.referencedObject=element.disables;
            obj.OnSpawned();
        }
            SpawnRandomObstacles();
            PostPrepare();
    }

    void SpawnRandomObstacles()
    {
        var count = 5;
        var bounds = GetComponentInChildren<SpriteRenderer>().bounds;
        var playableAreaBounds = PlayerController.instance.playableArea.bounds;

        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(bounds.center.x-bounds.extents.x, bounds.center.x+bounds.extents.x);
            float y = Random.Range(playableAreaBounds.center.y-playableAreaBounds.extents.y, playableAreaBounds.center.y+playableAreaBounds.extents.y);
            var obj = Instantiate(GameManager.instance.dynamicElementsPrefabs[Random.Range(0, GameManager.instance.dynamicElementsPrefabs.Count)], new Vector3(x,y,0), Quaternion.identity, obstaclesContainer).GetComponent<DynamicallyLoadedLevelElement>();
            // obj.GenerateRandomID();
            obj.id = $"{id}:{i}";
            obj.OnSpawned();
        }
    }
}
