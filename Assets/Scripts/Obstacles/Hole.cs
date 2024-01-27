public class Hole : DynamicallyLoadedLevelElement
{
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