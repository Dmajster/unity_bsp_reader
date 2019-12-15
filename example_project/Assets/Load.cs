using System.IO;
using UnityEngine;

namespace Assets
{
    public class Load : MonoBehaviour
    {
        public vbsp.structs.Map Map;

        // Start is called before the first frame update
        private void Start()
        {
            var fileStream = File.OpenRead(Application.dataPath + "/de_dust2.bsp");

            Map = vbsp.Reader.ReadStream(fileStream);

            foreach (var face in Map.Faces)
            {
                var firstEdgeIndex = face.FirstEdge;
                var edgeCount = face.EdgeCount;

                for (var i = firstEdgeIndex; i < firstEdgeIndex + edgeCount; i++)
                {
                    var surfaceEdge 
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var mapVertex in Map.Vertices)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(mapVertex.X, mapVertex.Y, mapVertex.Z), 5f);
            }
        }
    }
}
