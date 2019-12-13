namespace Dmajster.Bsp
{
    public struct Lump
    {
        public int FileOffset;
        public int FileLength;
        public int Version;
        public int Identifier;
    }

    public enum LumpType
    {
        Vertices = 3,
        Edges = 12
    }
}