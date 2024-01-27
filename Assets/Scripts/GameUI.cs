using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject objectSelectionPanel;
    public Transform obstacleContainer;
    public Transform helperContainer;
    public GameObject itemPrefab;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowObjectSelectionPanel()
    {
        obstacleContainer.transform.RemoveChildren();
        helperContainer.transform.RemoveChildren();

        foreach (var obj in GameManager.instance.dynamicElementsPrefabs)
        {
            var instance = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, obj.mapElementType == DynamicallyLoadedLevelElement.MapElementType.Obstacle ? obstacleContainer : helperContainer);
        }

        objectSelectionPanel.SetActive(true);
    }
}
