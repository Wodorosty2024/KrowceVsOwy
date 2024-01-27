using UnityEngine;
public class DynamicallyLoadedLevelElement : MonoBehaviour
{
    public enum MapElementType {Obstacle, Helper}

    public bool allowRandomGeneration=true;

    public MapElementType mapElementType;
    public string key;
    public string userName;
    public string userComment;
}