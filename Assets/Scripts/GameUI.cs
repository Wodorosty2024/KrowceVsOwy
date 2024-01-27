using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public GameObject objectSelectionPanel;
    public Transform obstacleContainer;
    public Transform helperContainer;
    public GameObject itemPrefab;

    public GameObject gameOverPanel;
    public GameObject objectPlacementPanel;
    public GameObject messagePanel;
    public GameObject restartPanel;

    public TMP_InputField messageField;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowMessageScreen()
    {
        messagePanel.SetActive(true);
    }

    public void ShowObjectSelectionPanel()
    {
        obstacleContainer.transform.RemoveChildren();
        helperContainer.transform.RemoveChildren();

        foreach (var obj in GameManager.instance.dynamicElementsPrefabs)
        {
            var instance = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, obj.mapElementType == DynamicallyLoadedLevelElement.MapElementType.Obstacle ? obstacleContainer : helperContainer);
            var item = instance.GetComponent<UIItem>();
            item.image.sprite = obj.GetComponentInChildren<SpriteRenderer>().sprite;
            item.title.text = obj.name;
            item.description.text = obj.description;
            instance.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.instance.CreatePreviewElement(obj.key);
                objectSelectionPanel.SetActive(false);
                objectPlacementPanel.SetActive(true);
            });
        }

        objectSelectionPanel.SetActive(true);
    }

    public void CancelObjectPlacement(InputAction.CallbackContext context)
    {
        if (GameManager.instance.previewObject != null)
        {
            Destroy(GameManager.instance.previewObject);
        }
        objectPlacementPanel.SetActive(false);
        objectSelectionPanel.SetActive(true);
    }

    public void ConfirmNewObject(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.instance.previewObject != null)
        {
            GameManager.instance.ConfirmPreviewElement();
            objectPlacementPanel.gameObject.SetActive(false);
        }
    }

    public void SendElement()
    {
        GameManager.instance.SendPreviewElement(messageField.text);
        restartPanel.gameObject.SetActive(true);
    }
}
