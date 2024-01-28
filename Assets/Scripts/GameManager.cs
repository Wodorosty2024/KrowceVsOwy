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

    public DynamicallyLoadedLevelElement previewObject;
    public bool isConfirmed;

    void Awake()
    {
        instance=this;
        Random.InitState(seed);
    }

    // Start is called before the first frame update
    void Start()
    {
        DynamicallyLoadedLevelElement.lastId=-1;
        LoadLevelElements();
        var obj = SpawnLane(null);
        var bounds = obj.GetComponentInChildren<Renderer>().bounds;
        obj.transform.position = Vector3.zero;
        SpawnLane(obj.GetComponentInChildren<Renderer>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (previewObject != null && !isConfirmed)
        {
            previewObject.MovePreview();
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
            // rend.color = new Color(Random.Range(0,1.0f), Random.Range(0,1.0f), 1,1);
            obj.transform.position = last == null ? Vector3.zero : new Vector3(last.bounds.center.x + last.bounds.extents.x + rend.bounds.extents.x - 0.05f, 0,0);
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
        previewObject = Instantiate(el.gameObject, Vector3.zero, Quaternion.identity).GetComponent<DynamicallyLoadedLevelElement>();
        previewObject.GetComponent<DynamicallyLoadedLevelElement>().enabled=false;
        previewObject.GetComponent<DynamicallyLoadedLevelElement>().isBeingPreviewed=true;
        previewObject.GetComponent<Collider2D>().isTrigger=true;
    }

    public void ConfirmPreviewElement()
    {
        isConfirmed=true;
        GameUI.instance.ShowMessageScreen();
    }

    public void SendPreviewElement(string sentence)
    {
        var dist = previewObject.transform.position-PlayerController.instance.transform.position;
        var key = previewObject.GetComponent<DynamicallyLoadedLevelElement>().key;
        var referencedObject = previewObject.GetComponent<DynamicallyLoadedLevelElement>().referencedObject;
        var lane = spawnedLanes.FirstOrDefault(x => x.GetComponent<Collider2D>().bounds.Contains(previewObject.transform.position)).GetComponent<Lane>();
        Vector2 pos = lane.obstaclesContainer.InverseTransformPoint(previewObject.transform.position);
        // var x = PlayerController.instance.accumulatedDistance + dist.x;
        // var y = PlayerController.instance.accumulatedDistance + dist.x;
        Debug.Log($"Object {key} saved at {pos.x} {pos.y} with lane id {lane.id}");
        MapElementModel model = new MapElementModel()
        {
            key=key,
            x = pos.x,
            y = pos.y,
            sentence = sentence,
            user = PlayerPrefs.GetString("username", "Player"),
            session = PlayerPrefs.GetString("session", "default"),
            disables = referencedObject,
            lane = lane.id,
            score = PlayerController.instance.accumulatedDistance
        };
        levelModificatorProvider.SaveNewElement(model); // meow
        previewObject = null;
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
