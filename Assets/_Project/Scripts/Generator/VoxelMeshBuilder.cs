using System.Collections.Generic;
using UnityEngine;

namespace Starbend.Voxel
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Rigidbody))]

    public class VoxelMeshBuilder : MonoBehaviour
    {
        [SerializeField] private Vector3Int size;
        [SerializeField] private Material mat;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Rigidbody _rigidbody;

        private MeshData _meshData;
        private Grid _grid;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _rigidbody = GetComponent<Rigidbody>();

            _grid = new Grid(size);
        }

        private void Start()
        {
            GenerateMesh();
        }

        public void GenerateMesh(bool isUpdateMesh = false)
        {
            _meshData = new MeshData();
            _meshData.ClearData();

            for (int z = 0; z < size.z; z++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int x = 0; x < size.x; x++)
                    {
                        var position = new Vector3Int(x, y, z);
                        if (_grid.GetCubeStatus(position))
                        {
                            GenerateCube(position);
                            
                            if(!isUpdateMesh)
                                GenerateBoxCollider(position);
                        }
                    }
                }
            }

            if (_meshData.vertices.Count <= 0)
                Destroy(gameObject);

            ApplyMesh();
        }

        private void GenerateCube(Vector3Int position)
        {
            foreach (Block.EnumQuad dir in System.Enum.GetValues(typeof(Block.EnumQuad)))
            {
                var start = dir switch
                {
                    Block.EnumQuad.ABCD_back => 0,
                    Block.EnumQuad.EFHG_front => 6,
                    Block.EnumQuad.EFBA_top => 12,
                    Block.EnumQuad.GHCD_bottom => 18,
                    Block.EnumQuad.EAGC_left => 24,
                    Block.EnumQuad.FBDH_right => 30,
                    _ => 0
                };

                var end = start + 6;
                var faceId = start / 6;

                for (int i = start; i < end; i++)
                {
                    _meshData.vertices.Add(position + Block.Faces[faceId][i - start]);
                    _meshData.triangles.Add(_meshData.vertices.Count - 1);
                    _meshData.UVs.Add(Block.UVs[i % 6]);
                }
            }
        }

        private void GenerateBoxCollider(Vector3Int position)
        {
            Destroy(_grid.Colliders[position.x, position.y, position.z]);

            var cubeObject = new GameObject($"{position.x},{position.y},{position.z}");
            cubeObject.transform.SetParent(transform);
            cubeObject.transform.localPosition = position + Vector3.one / 2;
            cubeObject.AddComponent<BoxCollider>();
            _grid.Colliders[position.x, position.y, position.z] = cubeObject;
        }

        private void ApplyMesh()
        {
            _meshData.UploadMesh();
            _meshFilter.sharedMesh = _meshData.mesh;
            _meshRenderer.material = mat;
        }

        public void RemoveCube(Vector3Int position)
        {
            if (_grid.IsInBounds(position))
            {
                _grid.SetCubeStatus(position, false);
                _grid.Colliders[position.x, position.y, position.z].SetActive(false);
                GenerateMesh(true);
                CheckConnections();
            }
        }

        public void CheckConnections()
        {
            var Component = new ConnectedComponentsFinder(_grid);
            var connectedComponents = Component.FindConnections();

            if (connectedComponents.Count > 1)
                CreateSeparatedObjects(connectedComponents);
        }

        private void CreateSeparatedObjects(List<Grid> connectedComponents)
        {
            foreach (var grid in connectedComponents)
            {
                var newObject = new GameObject("SeparatedMesh");
                var newVoxelMeshBuilder = newObject.AddComponent<VoxelMeshBuilder>();

                newVoxelMeshBuilder.size = grid.Size;
                newVoxelMeshBuilder._grid = grid;
                newVoxelMeshBuilder.mat = mat;
                _rigidbody.mass = 2;
            }
            
            Destroy(gameObject);
        }
    }
}
