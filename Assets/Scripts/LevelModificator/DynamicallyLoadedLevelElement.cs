using System.Collections.Generic;
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

    public bool requireFeetContact=false;

    public bool isBeingPreviewed=false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isBeingPreviewed) return;
        PlayerController pc = col.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            if (!requireFeetContact || (requireFeetContact && col.tag == "Feet"))
                HandleCollision(pc);
        }
    }

    public virtual bool CanBePlaced()
    {
        var col = GetComponent<Collider2D>();
        List<Collider2D> cols = new List<Collider2D>();
        var f = new ContactFilter2D();
        f.SetLayerMask(LayerMask.NameToLayer("Obstacles"));
        col.OverlapCollider(f, cols);
        bool canBePlaced = cols.Count == 0;
        GetComponentInChildren<SpriteRenderer>().color = canBePlaced ? Color.white : new Color(1, .8f, .8f, 0.4f);

        return canBePlaced;
    }

    public virtual void MovePreview()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var bounds = PlayerController.instance.playableArea.bounds;
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y), 0);
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 15;
        CanBePlaced();
    }

    public virtual void HandleCollision(PlayerController pc)
    {
        if (!string.IsNullOrEmpty(userName))
            PlayerController.instance.encounteredElements.Add((PlayerController.instance.accumulatedDistance, this));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isBeingPreviewed) return;

        PlayerController pc = col.collider.GetComponentInParent<PlayerController>();
        if (pc != null)
        {
            if (!requireFeetContact || (requireFeetContact && col.collider.tag == "Feet"))
                HandleCollision(pc);
        }
    }
}