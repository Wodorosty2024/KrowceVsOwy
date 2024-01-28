using System.Collections.Generic;
using UnityEngine;

public class Hole : DynamicallyLoadedLevelElement
{
    public bool isActive = true;

    void Start()
    {
        List<Collider2D> results = new();
        GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), results);
        foreach (var col in results)
        {
            if (col.GetComponent<Log>())
            {
                isActive = false;
                break;
            }
        }
    }

    public override void HandleCollision(PlayerController pc)
    {
        base.HandleCollision(pc);
        if (!pc.isDead && !pc.isInAir)
        {
            pc.Die(false, gameObject);
            pc.transform.position = transform.position;
            pc.animator.SetBool(AnimatorConstants.inHole, true);
        }
    }
}