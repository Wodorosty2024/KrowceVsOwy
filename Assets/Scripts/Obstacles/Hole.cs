using System.Collections.Generic;
using UnityEngine;

public class Hole : DynamicallyLoadedLevelElement
{
    public bool isActive = true;

    void Start()
    {
        List<Collider2D> results = new();
        GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), results);
        foreach (var col in results)
        {
            if (col.GetComponent<Log>())
            {
                isActive = false;
                Debug.Log("Hole deactivated");
                break;
            }
        }
    }

    public override void HandleCollision(PlayerController pc)
    {
        base.HandleCollision(pc);
        if (!pc.isDead && !pc.isInAir && isActive)
        {
            pc.Die(false, gameObject);
            pc.transform.position = transform.position;
            pc.animator.SetBool(AnimatorConstants.inHole, true);
        }
    }
}