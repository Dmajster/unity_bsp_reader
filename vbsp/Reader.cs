using System;
using System.IO;
using System.Runtime.InteropServices;
using vbsp.structs;

namespace vbsp
{
    public class Reader
    {
        public static Map ReadStream(Stream stream)
        {
            var binaryReader = new BinaryReader(stream);

            var header = new Header
            {
                Identifier = binaryReader.ReadInt32()
            };

            //check if the file contains the VBSP file identifier 
            if (header.Identifier != 0x50534256)
            {
                throw new FormatException("File doesn't contain the VBSP file header!'");
            }

            Console.WriteLine("File contains VBSP file identifier!");

            header.Version = binaryReader.ReadInt32();

            Console.WriteLine($"Format version: {header.Version}");

            header.Lumps = new Lump[Header.LumpCount];

            for (var i = 0; i < header.Lumps.Length; i++)
            {
                header.Lumps[i].Offset = binaryReader.ReadInt32();
                header.Lumps[i].Size = binaryReader.ReadInt32();
                header.Lumps[i].Version = binaryReader.ReadInt32();
                header.Lumps[i].Identifier = binaryReader.ReadInt32();
            }

            Console.WriteLine("Done reading lumps!");

            return new Map
            {
                Planes = DeserializeLumpArray<Plane>(header, stream, binaryReader),
                Vertices = DeserializeLumpArray<Vector>(header, stream, binaryReader),
                Edges = DeserializeLumpArray<Edge>(header, stream, binaryReader),
                SurfaceEdges = DeserializeLumpArray<SurfaceEdge>(header, stream, binaryReader),
                Faces = DeserializeLumpArray<Face>(header, stream, binaryReader)
            };
        }

        public static T[] DeserializeLumpArray<T>(Header header, Stream fileStream, BinaryReader binaryReader)
        {
            var lumpIndex = Lump.GetLumpIndex<T>();
            var lumpElementSize = Marshal.SizeOf(typeof(T));

            ref var lump = ref header.Lumps[lumpIndex];

            if (lump.Size % lumpElementSize != 0)
            {
                throw new Exception($"Error reading lump type: {typeof(T)}");
            }

            var elementCount = lump.Size / lumpElementSize;

            var elementArray = new T[elementCount];

            fileStream.Seek(lump.Offset, SeekOrigin.Begin);

            for (var i = 0; i < elementCount; i++)
            {
                elementArray[i] = ByteToType<T>(binaryReader);
            }

            return elementArray;
        }

        public static T ByteToType<T>(BinaryReader reader)
        {
            var bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }
    }
}
