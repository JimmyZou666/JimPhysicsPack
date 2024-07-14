namespace JimPhysicsPack
{
    public class AABBCollisionAlgorithm2D : CollisionAlgorithm2D
    {
        public override bool IsColliding(Collider2D thisCollider, Collider2D otherCollider)
        {
            AABBCollider2D box1 = thisCollider as AABBCollider2D;
            AABBCollider2D box2 = otherCollider as AABBCollider2D;

            // 检查每个轴上的投影是否重叠
            return box1.min.x <= box2.max.x && box1.max.x >= box2.min.x &&
                   box1.min.y <= box2.max.y && box1.max.y >= box2.min.y;
        }
    }
}