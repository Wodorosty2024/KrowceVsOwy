using UnityEngine;

public class Grave : DynamicallyLoadedLevelElement
{
    public override void HandleCollision(PlayerController pc)
    {
        GetComponent<BoxCollider2D>().isTrigger=false;
        if (pc.isDead) return;
        if (!pc.isInAir)
        {
                pc.Die(true, gameObject);
        }
    }
}