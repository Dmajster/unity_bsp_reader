namespace vbsp.structs
{
    public struct Lump
    {
        public int Offset;
        public int Size;
        public int Version;
        public int Identifier;
    }

    public enum LumpType
    {
        Plane = 1,
        Vertices = 3,
        Face = 7,
        Edges = 12,
        Brush = 18,
        BrushSide = 19,
    }
}