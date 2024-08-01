using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JimDevPack.Physics
{
    public class QuadTree
    {
        public Rect boundary;
        public List<Collider2D> objects;
        public QuadTree[] children;

        public bool isDivided;
        public QuadTree(Rect rect)
        {
            this.boundary = rect;
            objects = new List<Collider2D>();
            children = new QuadTree[4];
        }

        public void Insert(Collider2D obj)
        {
            if (!Contains(obj))
            {
                return;
            }

            if (isDivided)
            {

            }
        }

        public bool Contains(Collider2D obj)
        {
            //boundary.Overlaps(obj.boundary);
            return false;
        }
    }
}