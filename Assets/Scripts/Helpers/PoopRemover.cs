using System.Linq;
using UnityEngine;

public class PoopRemover : DynamicallyLoadedLevelElement
{
    bool snaps = false;
    bool initialized = false;

    public override bool OnSpawned()
    {
        if (!string.IsNullOrEmpty(referencedObject))
        {
            var obj = FindObjectsOfType<Poo>().FirstOrDefault(x => x.id == referencedObject);
            if (obj != null)
            {
                obj.isActive = false;
                Debug.Log($"{key} {id} found {referencedObject}");
                return true;
            }
            else
            {
                Debug.LogError($"{key} {id} references {referencedObject} but it can't be found");
                Lane.queued.Enqueue(this);
                return false;
            }
        }
        return true;
    }

    public override void MovePreview()
    {
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
        RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        var col = hit.FirstOrDefault(x => x.collider != null && x.collider.GetComponent<Poo>() != null).collider?.GetComponent<Poo>();
        if (col != null && col.isActive)
        {
            transform.position = col.transform.position;
            snaps = true;
            referencedObject = col.GetComponent<DynamicallyLoadedLevelElement>().id;
        }
        else
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var bounds = PlayerController.instance.playableArea.bounds;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y), 0);
            snaps = false;
            referencedObject = null;
        }
        CanBePlaced();
    }

    public override bool CanBePlaced()
    {
        GetComponentInChildren<SpriteRenderer>().color = snaps ? Color.white : semitransparent;
        return snaps;
    }
}