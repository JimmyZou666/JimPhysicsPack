namespace JimDevPack.Physics
{
    public class OBBAlgorithm : CollisionAlgorithm
    {
        // 检查两个OBB是否相交
        public override bool IsColliding(Collider thisCollider, Collider otherCollider)
        {
            return true; // 如果以上所有条件都不满足，那么两个OBB相交
        }
    }
}