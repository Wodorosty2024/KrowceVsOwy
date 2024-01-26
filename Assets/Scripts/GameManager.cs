using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BaseLevelModificator levelModificatorProvider;
    public List<DynamicallyLoadedLevelElement> dynamicElementsPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        var elements = levelModificatorProvider.LoadLevelElements();
        foreach (var element in elements)
        {
            var refObj = dynamicElementsPrefabs.FirstOrDefault(x => x.key == element.key);
            if (refObj == null)
            {
                Debug.LogError($"Key {element.key} doesn't exists in prefabs list");
                continue;
            }
            var obj = Instantiate(refObj, new Vector3(element.x, element.y, -1), Quaternion.identity);
        }
    }

    public void SaveLevelElements()
    {
        var elements = FindObjectsOfType<DynamicallyLoadedLevelElement>();
        levelModificatorProvider.SaveLevelElements(new List<DynamicallyLoadedLevelElement>(elements));
    }
}
