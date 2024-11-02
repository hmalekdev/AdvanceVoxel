using UnityEngine;

namespace Starbend.Voxel
{
    public struct Grid
    {
        public Vector3Int Size { get; private set; }
        public bool[,,] IsEnabled { get; private set; }
        public GameObject[,,] Colliders { get; private set; }

        public Grid(Vector3Int size)
        {
            Size = size;
            IsEnabled = new bool[size.x, size.y, size.z];
            Colliders = new GameObject[size.x, size.y, size.z];

            InitializeGrid();
        }

        private readonly void InitializeGrid()
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    for (int z = 0; z < Size.z; z++)
                    {
                        IsEnabled[x, y, z] = true;
                        Colliders[x, y, z] = null;
                    }
                }
            }
        }

        public readonly bool GetCubeStatus(Vector3Int position)
        {
            if (IsInBounds(position))
                return IsEnabled[position.x, position.y, position.z];
            return false;
        }

        public readonly void SetCubeStatus(Vector3Int position, bool status)
        {
            if (IsInBounds(position))
                IsEnabled[position.x, position.y, position.z] = status;
        }

        public readonly bool IsInBounds(Vector3Int position)
        {
            return position.x >= 0 && position.x < Size.x &&
                   position.y >= 0 && position.y < Size.y &&
                   position.z >= 0 && position.z < Size.z;
        }
    }
}
