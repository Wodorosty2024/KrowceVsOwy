using UnityEngine;
public class DynamicallyLoadedLevelElement : MonoBehaviour
{
    public enum MapElementType {Obstacle, Helper}

    public MapElementType mapElementType;
    public string key;
    public string userName;
    public string userComment;
}