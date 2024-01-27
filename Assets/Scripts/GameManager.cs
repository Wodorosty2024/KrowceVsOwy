using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject previewObject;
    public bool isConfirmed;

    void Awake()
    {
        instance=this;
        Random.InitState(seed);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLevelElements();
        var obj = SpawnLane(null);
        var bounds = obj.GetComponentInChildren<Renderer>().bounds;
        obj.transform.position = Vector3.zero + Vector3.right * bounds.extents.x;
        SpawnLane(obj.GetComponentInChildren<Renderer>());
    }

    // Update is called once per frame
    void Update()
    {
        if (previewObject != null && !isConfirmed)
        {
            previewObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var bounds = PlayerController.instance.playableArea.bounds;
            previewObject.transform.position = new Vector3(previewObject.transform.position.x, Mathf.Clamp(previewObject.transform.position.y, bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y), 0);
        }

        foreach (var fragment in spawnedLanes)
        {
            fragment.transform.position -= Vector3.right*PlayerController.instance.currentHorizontalSpeed;
        }
        var horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height)*cameraBoundsMultiplier;
        var last =spawnedLanes.LastOrDefault()?.GetComponentInChildren<SpriteRenderer>();
        if (last != null)
        {
        if (last.bounds.center.x + last.bounds.extents.x < horzExtent)
        {
            SpawnLane(last);
        }
        }
        var first = spawnedLanes.FirstOrDefault()?.GetComponentInChildren<SpriteRenderer>();
        if (first != null)
        {
        if (first.bounds.center.x + first.bounds.extents.x < -horzExtent)
        {
            pool.ReturnToPool(spawnedLanes[0]);
            spawnedLanes.RemoveAt(0);
        }
        }
    }

    public GameObject SpawnLane(Renderer last)
    {
            var obj = pool.GetObjectFromPool();
            var rend = obj.GetComponentInChildren<SpriteRenderer>();
            rend.color = new Color(Random.Range(0,1.0f), Random.Range(0,1.0f), 1,1);
            obj.transform.position = last == null ? Vector3.zero : new Vector3(last.bounds.center.x + last.bounds.extents.x + rend.bounds.extents.x, 0,0);
            obj.GetComponent<Lane>().Prepare(last == null);
            spawnedLanes.Add(obj);
            return obj;
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

    public void CreatePreviewElement(string key)
    {
        var el = dynamicElementsPrefabs.FirstOrDefault(x => x.key == key);
        previewObject = Instantiate(el.gameObject, Vector3.zero, Quaternion.identity);        
        previewObject.GetComponent<DynamicallyLoadedLevelElement>().enabled=false;
        previewObject.GetComponent<Collider2D>().enabled=false;        
    }

    public void ConfirmPreviewElement()
    {
        isConfirmed=true;
        GameUI.instance.ShowMessageScreen();
    }

    public void SendPreviewElement(string sentence)
    {
        var dist = previewObject.transform.position-PlayerController.instance.transform.position;
        MapElementModel model = new MapElementModel()
        {
            key = previewObject.GetComponent<DynamicallyLoadedLevelElement>().key,
            x = PlayerController.instance.accumulatedDistance + dist.x,
            y = previewObject.transform.position.y,
            sentence=sentence
        };
        levelModificatorProvider.SaveNewElement(model);
        previewObject = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
