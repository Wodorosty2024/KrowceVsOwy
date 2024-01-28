using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : DynamicallyLoadedLevelElement
{
    public GameObject fenceRendererGO;
    public GameObject brokenFenceGO;

    public override void HandleCollision(PlayerController pc)
    {
        GetComponent<BoxCollider2D>().isTrigger=false;
        if (pc.isDead) return;
        if (!pc.isInAir)
        {
            pc.health--;
            if (pc.health <= 0)
            {
                pc.Die(true, gameObject);
                brokenFenceGO.SetActive(true);
                fenceRendererGO.SetActive(false);
            }
            else
            {
                GetComponentInChildren<Collider2D>().enabled = false;
            }

            Debug.Log("hit");
        }
    }

}
