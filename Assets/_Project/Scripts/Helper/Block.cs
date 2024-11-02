using UnityEngine;

namespace Starbend.Voxel
{
    public static class Block
    {
        private const float OFFSET = 0.95f;

        public enum EnumQuad
        {
            ABCD_back = 0,
            EFHG_front = 6,
            EFBA_top = 12,
            GHCD_bottom = 18,
            EAGC_left = 24,
            FBDH_right = 30
        }

        public static Vector3 A = new(0, OFFSET, 0);
        public static Vector3 B = new(OFFSET, OFFSET, 0);
        public static Vector3 C = new(0, 0, 0);
        public static Vector3 D = new(OFFSET, 0, 0);
        public static Vector3 E = new(0, OFFSET, OFFSET);
        public static Vector3 F = new(OFFSET, OFFSET, OFFSET);
        public static Vector3 G = new(0, 0, OFFSET);
        public static Vector3 H = new(OFFSET, 0, OFFSET);


        public static readonly Vector3[][] Faces = new Vector3[][]
        {
            new Vector3[] { A,D,C,A,B,D },
            new Vector3[] { F,G,H,F,E,G },
            new Vector3[] { E,B,A,E,F,B },
            new Vector3[] { C,H,G,C,D,H },
            new Vector3[] { E,C,G,E,A,C },
            new Vector3[] { B,H,D,B,F,H },
        };

        public static readonly Vector2[] UVs = new Vector2[6]
        {
            new(0,1),
            new(1,0),
            new(0,0),
            new(0,1),
            new(1,1),
            new(1,0)
        };
    }
}
