using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Dmajster.Bsp
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
                header.Lumps[i].FileOffset = binaryReader.ReadInt32();
                header.Lumps[i].FileLength = binaryReader.ReadInt32();
                header.Lumps[i].Version = binaryReader.ReadInt32();
                header.Lumps[i].Identifier = binaryReader.ReadInt32();
            }

            Console.WriteLine("Done reading lumps!");

            var map = new Map();

            ref var vertexLump = ref header.GetLump(LumpType.Vertices);

            //Ensure this is not corrupted, BSP Vectors are 12B each
            if (vertexLump.FileLength % 12 != 0)
            {
                throw new TargetParameterCountException("Vertex lump is of size not divisable by 12!");
            }

            var vertexCount = vertexLump.FileLength / 12;

            map.Vertices = new Vector3[vertexCount];

            fileStream.Seek(vertexLump.FileOffset, SeekOrigin.Begin);

            for (var i = 0; i < vertexCount; i++)
            {
                map.Vertices[i] = new Vector3(
                    binaryReader.ReadSingle(),
                    binaryReader.ReadSingle(),
                    binaryReader.ReadSingle()
                );
            }

            Console.WriteLine($"Map vertex count is: {map.Vertices.Length}");
        }
    }
}
