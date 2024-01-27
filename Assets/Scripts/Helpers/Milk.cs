public class Milk : DynamicallyLoadedLevelElement
{
    public override void HandleCollision(PlayerController pc)
    {
        base.HandleCollision(pc);
        pc.health++;
        Destroy(gameObject);
    }
}