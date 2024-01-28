using System.Collections.Generic;
using UnityEngine;

public class Hole : DynamicallyLoadedLevelElement
{
    public bool isActive = true;

    public override void HandleCollision(PlayerController pc)
    {        
        if (!pc.isDead && !pc.isInAir && isActive)
        {
            base.HandleCollision(pc);
            pc.Die(false, gameObject);
            pc.transform.position = transform.position;
            pc.animator.SetBool(AnimatorConstants.inHole, true);
        }
    }
}