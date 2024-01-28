using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : DynamicallyLoadedLevelElement
{
    public GameObject fenceRendererGO;
    public GameObject brokenFenceGO;

    public override void HandleCollision(PlayerController pc)
    {
        base.HandleCollision(pc);
        GetComponent<Collider2D>().isTrigger = false;
        if (pc.isDead) return;
        if (!pc.isInAir)
        {
            pc.health--;
            if (pc.health <= 0)
            {
                 pc.Die(true, gameObject);
            }
            else
            {
                GetComponentInChildren<Collider2D>().enabled = false;
            }
            brokenFenceGO.SetActive(true);
            fenceRendererGO.SetActive(false);
        }
    }

}
