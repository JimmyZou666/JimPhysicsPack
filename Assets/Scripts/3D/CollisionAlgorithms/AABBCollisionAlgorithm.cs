namespace JimDevPack.Physics
{
    public class AABBCollisionAlgorithm : CollisionAlgorithm
    {
        public override bool IsColliding(Collider thisCollider, Collider otherCollider)
        {
            AABBCollider box1 = thisCollider as AABBCollider;
            AABBCollider box2 = otherCollider as AABBCollider;

            // 检查每个轴上的投影是否重叠
            return box1.min.x <= box2.max.x && box1.max.x >= box2.min.x &&
                   box1.min.y <= box2.max.y && box1.max.y >= box2.min.y &&
                   box1.min.z <= box2.max.z && box1.max.z >= box2.min.z;
        }
    }
}