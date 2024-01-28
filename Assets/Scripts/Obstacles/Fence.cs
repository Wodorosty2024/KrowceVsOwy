using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : DynamicallyLoadedLevelElement
{
    public SpriteRenderer fenceRenderer;
    public Sprite brokenFenceSprite;

    public override void HandleCollision(PlayerController pc)
    {
        GetComponent<BoxCollider2D>().isTrigger=false;
        if (pc.isDead) return;
        if (!pc.isInAir)
        {
            pc.health--;
            if (pc.health <= 0)
                pc.Die(true, gameObject);
            else
            {
                GetComponentInChildren<Collider2D>().enabled = false;
                fenceRenderer.sprite = brokenFenceSprite;
            }

            Debug.Log("hit");
        }
    }

}
