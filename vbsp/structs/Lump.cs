using System;

namespace vbsp.structs
{
    public struct Lump
    {
        public int Offset;
        public int Size;
        public int Version;
        public int Identifier;

        public static int GetLumpIndex<T>()
        {
            if (typeof(T) == typeof(Plane))
            {
                return 1;
            }
            if (typeof(T) == typeof(Vector))
            {
                return 3;
            }
            if (typeof(T) == typeof(Face))
            {
                return 7;
            }
            if (typeof(T) == typeof(Edge))
            {
                return 12;
            }
            if (typeof(T) == typeof(SurfaceEdge))
            {
                return 13;
            }

            throw new Exception($"Type not stored in any lump! {typeof(T)}");
        }
    }
}