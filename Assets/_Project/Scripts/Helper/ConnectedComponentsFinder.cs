using System.Collections.Generic;
using UnityEngine;

namespace Starbend.Voxel
{
    public class ConnectedComponentsFinder
    {
        private Grid _grid;
        private Vector3Int _size;

        public ConnectedComponentsFinder(Grid grid)
        {
            _grid = grid;
            _size = grid.Size;
        }

        public List<Grid> FindConnections()
        {
            var visited = new HashSet<Vector3Int>();
            var connectedComponents = new List<Grid>();

            for (int z = 0; z < _size.z; z++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    for (int x = 0; x < _size.x; x++)
                    {
                        var pos = new Vector3Int(x, y, z);
                        if (_grid.GetCubeStatus(pos) && !visited.Contains(pos))
                        {
                            var component = new List<Vector3Int>();
                            ExploreComponent(pos, visited, component);
                            connectedComponents.Add(CreateGridForComponent(component));
                        }
                    }
                }
            }

            return connectedComponents;
        }

        private void ExploreComponent(Vector3Int start, HashSet<Vector3Int> visited, List<Vector3Int> component)
        {
            var stack = new Stack<Vector3Int>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var pos = stack.Pop();
                if (!visited.Contains(pos) && _grid.GetCubeStatus(pos))
                {
                    visited.Add(pos);
                    component.Add(pos);

                    foreach (var neighbor in GetNeighbors(pos))
                    {
                        if (!visited.Contains(neighbor) && _grid.GetCubeStatus(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }
        }

        private Grid CreateGridForComponent(List<Vector3Int> component)
        {
            var newGrid = new Grid(_size);
            foreach (var pos in component)
            {
                newGrid.SetCubeStatus(pos, true);
            }

            for (int z = 0; z < _size.z; z++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    for (int x = 0; x < _size.x; x++)
                    {
                        var position = new Vector3Int(x, y, z);
                        if (!component.Contains(position))
                        {
                            newGrid.SetCubeStatus(position, false);
                        }
                    }
                }
            }

            return newGrid;
        }

        private IEnumerable<Vector3Int> GetNeighbors(Vector3Int pos)
        {
            var directions = new[]
            {
                Vector3Int.up, Vector3Int.down,
                Vector3Int.left, Vector3Int.right,
                Vector3Int.forward, Vector3Int.back,
                //new Vector3Int(1, 0, 1),   // up-right-forward
                //new Vector3Int(1, 0, -1),  // up-right-back
                //new Vector3Int(-1, 0, 1),  // up-left-forward
                //new Vector3Int(-1, 0, -1), // up-left-back
                //new Vector3Int(1, 1, 0),   // up-right
                //new Vector3Int(1, -1, 0),  // down-right
                //new Vector3Int(-1, 1, 0),  // up-left
                //new Vector3Int(-1, -1, 0), // down-left
                //new Vector3Int(0, 1, 1),    // up-forward
                //new Vector3Int(0, 1, -1),   // up-back
                //new Vector3Int(0, -1, 1),   // down-forward
                //new Vector3Int(0, -1, -1)   // down-back
            };

            foreach (var dir in directions)
            {
                var neighbor = pos + dir;
                if (_grid.IsInBounds(neighbor))
                {
                    yield return neighbor;
                }
            }
        }
    }
}
