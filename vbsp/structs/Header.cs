﻿namespace vbsp.structs
{
    public struct Header
    {
        public int Identifier;
        public int Version;
        public Lump[] Lumps;
        public int MapRevision;

        public const int LumpCount = 64;
    }
}