using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public List<Transform> layers;
    public List<float> speeds;

    List<Vector2> originalPosition;
    void Start()
    {
        originalPosition = new List<Vector2>();
        foreach (var layer in layers) originalPosition.Add(layer.localPosition);
    }

    public void Prepare()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].localPosition = originalPosition[i];
        }
    }

    void Update()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].transform.localPosition -= Vector3.right*speeds[i]*PlayerController.instance.currentHorizontalSpeed;
        }
    }
}
