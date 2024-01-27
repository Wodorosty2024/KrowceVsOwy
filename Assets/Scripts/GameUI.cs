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

    public GameObject runInfoContainer;
    public GameObject runInfoMilkContainer;
    public TextMeshProUGUI runInfoDistance;
    public TextMeshProUGUI runInfoMilkCounter;
    public GameObject gameOverPanel;
    public GameObject summaryScrollView;
    public Transform summaryContainer;
    public GameObject summaryEntryPrefab;
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
        runInfoDistance.text = PlayerController.instance.accumulatedDistance.ToString("F2");
        runInfoMilkContainer.SetActive(PlayerController.instance.health > 1);
        runInfoMilkCounter.text = PlayerController.instance.health.ToString();
    }  

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        runInfoContainer.SetActive(false);
        summaryScrollView.SetActive(PlayerController.instance.encounteredElements.Count > 0);
        foreach (var tuple in PlayerController.instance.encounteredElements)
        {
            var obj = Instantiate(summaryEntryPrefab, Vector3.zero, Quaternion.identity, summaryContainer).GetComponent<EncounteredMapElementEntry>();
            obj.summaryText.text = $"{tuple.element.userName} {(tuple.element.mapElementType == DynamicallyLoadedLevelElement.MapElementType.Obstacle ? "tricked you with" : "helped you with ")} {tuple.element.ui_name} at {tuple.distance}.\nTheir message:";
            obj.username.text = tuple.element.userName;
            obj.message.text = tuple.element.userComment;
        }
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
            item.title.text = obj.ui_name;
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
