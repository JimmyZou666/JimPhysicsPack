using System;
using UnityEngine;

namespace JimPhysicsPack
{
    public class OBBAlgorithm : ICollisionAlgorithm
    {
        // 检查两个OBB是否相交
        public bool IsColliding(ICollider thisCollider, ICollider otherCollider)
        {
            return true; // 如果以上所有条件都不满足，那么两个OBB相交
        }
    }
}