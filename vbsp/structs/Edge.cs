using System.Runtime.InteropServices;

namespace vbsp.structs
{
    public struct Edge
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] VertexIndexPair;
    }
}
