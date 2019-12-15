using System.Collections.Generic;
using System.IO;
using UnityEngine;
using vbsp.structs;

namespace Assets
{
    public class Load : MonoBehaviour
    {
        public vbsp.structs.Map Map;

        public Mesh Mesh;

        // Start is called before the first frame update
        private void Start()
        {
            Map = vbsp.Reader.Read(Application.dataPath + "/de_dust2.bsp");

            var vertices = new List<Vector3>();
            var indices = new List<int>();

            foreach (var face in Map.Faces)
            {
                var firstEdgeIndex = face.FirstEdgeIndex;
                var edgeCount = face.EdgeCount;

                var firstVector = Vector3.zero;

                for (var i = firstEdgeIndex; i < firstEdgeIndex + edgeCount; i++)
                {
                    var surfaceEdge = Map.SurfaceEdges[i];

                    var edge = Map.Edges[surfaceEdge.GetIndex];

                    var position = Map.Vertices[edge.VertexIndexPair[0]];
                    var vector1 = new Vector3(position.X, position.Z, position.Y);

                    if (i == firstEdgeIndex)
                    {
                        firstVector = vector1;
                    }

                    position = Map.Vertices[edge.VertexIndexPair[1]];
                    var vector2 = new Vector3(position.X, position.Z, position.Y);

                    var clockwise = surfaceEdge.Clockwise;

                    vertices.Add(firstVector);
                    vertices.Add(vector1);
                    vertices.Add(vector2);

                    if (!clockwise)
                    {
                        indices.Add(vertices.Count - 1);
                        indices.Add(vertices.Count - 2);
                        indices.Add(vertices.Count - 3);
                    }
                    else
                    {
                        indices.Add(vertices.Count - 2);
                        indices.Add(vertices.Count - 1);
                        indices.Add(vertices.Count - 3);
                    }
                }
            }


            Mesh = new Mesh { vertices = vertices.ToArray(), triangles = indices.ToArray() };

            GetComponent<MeshFilter>().mesh = Mesh;
        }

        private void OnDrawGizmos()
        {
            if (Mesh == null)
            {
                return;
            }

            Gizmos.DrawMesh(Mesh);
        }
    }
}
