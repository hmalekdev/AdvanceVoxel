using System;
using System.Collections.Generic;
using UnityEngine;

namespace Starbend.Voxel
{
    public struct MeshData 
    {
        public Mesh mesh;
        public List<Vector3> vertices;
        public List<int> triangles;
        public List<Vector3> UVs;
        public bool isInitialized;

        public void ClearData()
        {
            if(!isInitialized)
            {
                vertices = new();
                triangles = new ();
                UVs = new ();
                mesh = new();

                isInitialized = true;
                return;
            }

            vertices.Clear();
            triangles.Clear();
            UVs.Clear();
            mesh.Clear();
        }

        public readonly void UploadMesh()
        {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetUVs(0, UVs);

            mesh.Optimize();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.UploadMeshData(false);
        }
    }
}
