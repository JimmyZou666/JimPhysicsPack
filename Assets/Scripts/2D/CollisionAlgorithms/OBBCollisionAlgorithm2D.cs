
namespace JimDevPack.Physics
{
    public class OBBAlgorithm2D : CollisionAlgorithm2D
    {
        // 检查两个OBB是否相交
        public override bool IsColliding(Collider2D thisCollider, Collider2D otherCollider)
        {
            return false; // 如果以上所有条件都不满足，那么两个OBB相交
        }
    }
}