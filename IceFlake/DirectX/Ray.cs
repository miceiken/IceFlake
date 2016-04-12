using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceFlake.DirectX {
    // Represents a ray in 2D space originating from pt0 and passing through pt1
    public struct Ray2 {
        private Vector2 pt0;
        private Vector2 pt1;

        public Ray2(Vector2 pt0, Vector2 pt1) {
            this.pt0 = pt0;
            this.pt1 = pt1;
        }

        public bool IsOnRightSide(Vector2 pt2) {
            float val = (pt1.X - pt0.X) * (pt2.Y - pt0.Y) - (pt2.X - pt0.X) * (pt1.Y - pt0.Y);
            return val < 0;
        }

        public bool IsOnLeftSide(Vector2 pt) {
            float val = (pt1.X - pt0.X) * (pt.Y - pt0.Y) - (pt.X - pt0.X) * (pt1.Y - pt0.Y);
            return val > 0;
        }
    }
}
