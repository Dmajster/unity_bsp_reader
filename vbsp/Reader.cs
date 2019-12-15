using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using vbsp.structs;
using Plane = vbsp.structs.Plane;

namespace vbsp
{
    public class Reader
    {
        public static void Read(string path)
        {
            using var fileStream = File.OpenRead(path);

            var binaryReader = new BinaryReader(fileStream);

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

            ref var planeLump = ref header.GetLump(LumpType.Plane);

            if (planeLump.Size % 20 != 0)
            {
                throw new TargetParameterCountException("Plane lump is of size not divisable by 20!");
            }

            //Ensure this is not corrupted, BSP planes are 20B each
            var planeCount = planeLump.Size / 20;

            var planes = new Plane[planeCount];

            fileStream.Seek(planeLump.Offset, SeekOrigin.Begin);

            ref var vertexLump = ref header.GetLump(LumpType.Vertices);

            for (var i = 0; i < planeCount; i++)
            {
                planes[i] = ByteToType<Plane>(binaryReader);
            }

            Console.WriteLine($"Map plane count is: {planes.Length}");

            //Ensure this is not corrupted, BSP Vectors are 12B each
            if (vertexLump.Size % 12 != 0)
            {
                throw new TargetParameterCountException("Vertex lump is of size not divisable by 12!");
            }

            var vertexCount = vertexLump.Size / 12;

            var vertices = new Vector3[vertexCount];

            fileStream.Seek(vertexLump.Offset, SeekOrigin.Begin);

            for (var i = 0; i < vertexCount; i++)
            {
                vertices[i] = ByteToType<Vector3>(binaryReader);
            }

            Console.WriteLine($"Map vertex count is: {vertices.Length}");

            var brushLump = header.GetLump(LumpType.Brush);

            //Ensure this is not corrupted, BSP brushes are 12B each
            var brushCount = brushLump.Size / 12;

            var brushes = new Brush[brushCount];

            fileStream.Seek(brushLump.Offset, SeekOrigin.Begin);

            for (var i = 0; i < brushCount; i++)
            {
                brushes[i] = ByteToType<Brush>(binaryReader);
            }

            Console.WriteLine($"Map brush count is: {brushes.Length}");

            var faceLump = header.GetLump(LumpType.Face);

            var faceCount = faceLump.Size / 56;

            var faces = new Face[faceCount];

            for (var i = 0; i < faceCount; i++)
            {
                faces[i] = ByteToType<Face>(binaryReader);
            }
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
