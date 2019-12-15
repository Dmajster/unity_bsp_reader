using System;

namespace vbsp.structs
{
    public struct SurfaceEdge
    {
        public int EdgeIndex;

        public int GetIndex => Math.Abs(EdgeIndex);

        public bool Clockwise => EdgeIndex >= 0;
    }
}
