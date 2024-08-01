namespace JimDevPack.Physics
{
    public abstract class CollisionAlgorithm
    {
        public abstract bool IsColliding(Collider thisCollider, Collider otherCollider);
    }
}