using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Poo : DynamicallyLoadedLevelElement
{
    bool isActive = true;
    void Start()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.zero);
        var col = hit.FirstOrDefault(x => x.collider != null && x.collider.GetComponent<PoopRemover>() != null).collider;
        if (col != null)
        {
            isActive = false;
            Debug.Log("Poo deactivated");
        } 
    }

    public override void HandleCollision(PlayerController pc)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        if (pc.isDead) return;
        if (!pc.isInAir && isActive)
        {
            pc.Die(true, gameObject);
        }
    }
}