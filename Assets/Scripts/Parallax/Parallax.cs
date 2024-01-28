using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public List<Transform> layers;
    public List<float> speeds;

    List<Vector2> originalPositions;
    void Start()
    {
        SavePositions();
    }

    void SavePositions()
    {
        if (originalPositions != null) return;
        originalPositions = new List<Vector2>();
        foreach (var layer in layers) originalPositions.Add(layer.localPosition);
    }

    public void Prepare()
    {
        if (originalPositions == null) SavePositions();

        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].localPosition = originalPositions[i];
        }
    }

    void Update()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].transform.localPosition -= Vector3.right * PlayerController.instance.currentHorizontalSpeed/speeds[i];
        }
    }
}
