namespace JimDevPack.Physics
{
    public abstract class CollisionAlgorithm2D
    {
        public abstract bool IsColliding(Collider2D thisCollider, Collider2D otherCollider);
    }
}