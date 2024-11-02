using UnityEngine;

namespace Starbend.Voxel
{
    public class CustomRaycast : MonoBehaviour
    {
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            if (!hit.transform.TryGetComponent(out VoxelMeshBuilder builder)) return;

            var cubePosition = GetPositionFromName(hit.collider.name);
            builder.RemoveCube(cubePosition); 
        }

        private Vector3Int GetPositionFromName(string name)
        {
            return ParseVector3(name);
        }

        private Vector3Int ParseVector3(string input)
        {
            string[] parts = input.Split(',');
            return new Vector3Int(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
    }
}

