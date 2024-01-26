using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int seed = 2137;
    public float cameraBoundsMultiplier=2;
    public BaseLevelModificator levelModificatorProvider;
    public List<DynamicallyLoadedLevelElement> dynamicElementsPrefabs;

    public List<GameObject> lanesPrefabs;
    public ObjectPool pool;

    public List<MapElementModel> levelConfig = new List<MapElementModel>();
    List<GameObject> spawnedLanes = new List<GameObject>();

    void Awake()
    {
        instance=this;
        Random.InitState(seed);
    }

    // Start is called before the first frame update
    void Start()
    {
        var obj = pool.GetObjectFromPool();
        spawnedLanes.Add(obj);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var fragment in spawnedLanes)
        {
            fragment.transform.position -= Vector3.right*PlayerController.instance.currentHorizontalSpeed;
        }
        var last =spawnedLanes.Last().GetComponentInChildren<SpriteRenderer>();
        var horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height)*cameraBoundsMultiplier;
        if (last.bounds.center.x + last.bounds.extents.x < horzExtent)
        {
            var obj = pool.GetObjectFromPool();
            var rend = obj.GetComponentInChildren<SpriteRenderer>();
            rend.color = new Color(Random.Range(0,1.0f), Random.Range(0,1.0f), 1,1);
            obj.transform.position = new Vector3(last.bounds.center.x + last.bounds.extents.x + rend.bounds.extents.x, 0,0);
            obj.GetComponent<Lane>().Prepare();
            spawnedLanes.Add(obj);
        }
        var first = spawnedLanes[0].GetComponentInChildren<SpriteRenderer>();
        if (first.bounds.center.x + first.bounds.extents.x < -horzExtent)
        {
            pool.ReturnToPool(spawnedLanes[0]);
            spawnedLanes.RemoveAt(0);
        }
    }

    public void SpawnElement()
    {
        var randObj = dynamicElementsPrefabs[Random.Range(0, dynamicElementsPrefabs.Count)];
        var pos = transform.position + new Vector3(Random.Range(-5,5), Random.Range(-5,5));
        Instantiate(randObj, pos, Quaternion.identity);
        SaveLevelElements();
    }

    public void DeleteAll()
    {
        levelModificatorProvider.SaveLevelElements(new());
    }

    public void LoadLevelElements()
    {
        levelConfig = levelModificatorProvider.LoadLevelElements();
        
    }

    public void SaveLevelElements()
    {
        var elements = FindObjectsOfType<DynamicallyLoadedLevelElement>();
        levelModificatorProvider.SaveLevelElements(elements.Select(x => new MapElementModel() {
            key = x.key,
            x = x.transform.position.x,
            y = x.transform.position.y
        }).ToList());
    }
}
