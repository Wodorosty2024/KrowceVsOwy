using System.Collections.Generic;
using UnityEngine;

public static class TransformExtenstion
{
    public static void RemoveChildren(this Transform transform)
    {
        List<Transform> toDelete = new List<Transform>();
        foreach (Transform child in transform)
        {
            toDelete.Add(child);
        }
        foreach (var c in toDelete)
        {
            MonoBehaviour.Destroy(c.gameObject);
        }
    }
}