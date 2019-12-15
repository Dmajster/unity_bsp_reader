using System.Runtime.InteropServices;

namespace vbsp.structs
{
    public struct OriginalFace
    {
        public ushort PlaneIndex;
        public byte Side;
        public byte OnNode;
        public int FirstEdgeIndex;
        public short EdgeCount;
        public short TextureInfo;
        public short DisplacementInfo;
        public short SurfaceFogVolumeId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Styles;

        public int LightOffsets;
        public float Area;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] LightmapTextureMinsInLuxels;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public int[] LightmapTextureSizeInLuxels;

        public int OriginalFaceIndex;
        public ushort PrimitiveCount;
        public ushort FirstPrimitiveIndex;
        public ushort SmoothingGroups;
    }
}
