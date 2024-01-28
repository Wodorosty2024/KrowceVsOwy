using System.Linq;
using UnityEngine;

public class PoopRemover : DynamicallyLoadedLevelElement
{
    bool snaps = false;

    public override void MovePreview()
    {
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
        RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        var col = hit.FirstOrDefault(x => x.collider != null && x.collider.GetComponent<Poop>() != null).collider;
        if (col != null)
        {
            transform.position = col.transform.position;
            snaps = true;
        }
        else
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var bounds = PlayerController.instance.playableArea.bounds;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y), 0);
            snaps = false;
        }
    }

    public override bool CanBePlaced()
    {
        return snaps;
    }
}