using System.Linq;
using UnityEngine;

public class Log : DynamicallyLoadedLevelElement
{

    bool snaps = false;
    public override bool OnSpawned()
    {
        if (!string.IsNullOrEmpty(referencedObject))
        {
            var holes = FindObjectsOfType<Hole>();
            var obj = holes.FirstOrDefault(x => x.id == referencedObject);
            if (obj != null)
            {
                obj.isActive = false;
                transform.position = obj.transform.position;
                Debug.Log($"{name} {key} found {referencedObject}");
                return true;
            }
            else
            {
                Debug.LogError($"{name} {key} references {referencedObject} but it can't be found");
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
        var col = hit.FirstOrDefault(x => x.collider != null && x.collider.GetComponent<Hole>() != null).collider?.GetComponent<Hole>();
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