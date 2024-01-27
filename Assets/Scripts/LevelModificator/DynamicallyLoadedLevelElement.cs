using UnityEngine;
public class DynamicallyLoadedLevelElement : MonoBehaviour
{
    public enum MapElementType { Obstacle, Helper }

    public bool allowRandomGeneration = true;

    public MapElementType mapElementType;
    public string key;

    public string ui_name;
    public string description;

    public string userName;
    public string userComment;

    void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController pc;
        if (col.TryGetComponent<PlayerController>(out pc))
        {
            HandleCollision(pc);
        }
    }

    public virtual void HandleCollision(PlayerController pc)
    {
        PlayerController.instance.encounteredElements.Add((PlayerController.instance.accumulatedDistance, this));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        PlayerController pc;
        if (col.collider.TryGetComponent<PlayerController>(out pc))
        {
            HandleCollision(pc);
        }
    }
}