namespace JimPhysicsPack
{
    public abstract class CollisionAlgorithm
    {
        public abstract bool IsColliding(Collider thisCollider, Collider otherCollider);
    }
}