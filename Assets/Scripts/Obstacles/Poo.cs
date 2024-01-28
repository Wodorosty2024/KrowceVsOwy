using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Poo : DynamicallyLoadedLevelElement
{
    public bool isActive = true;

    public override void HandleCollision(PlayerController pc)
    {
        base.HandleCollision(pc);
        GetComponent<BoxCollider2D>().isTrigger = false;
        if (pc.isDead) return;
        if (!pc.isInAir && isActive)
        {
            pc.Die(true, gameObject);
        }
    }
}