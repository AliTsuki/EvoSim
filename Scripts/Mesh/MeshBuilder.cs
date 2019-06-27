using System.Collections.Generic;

using UnityEngine;

public static class MeshBuilder
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // Mesh data
    public static Mesh mesh;
    public static Vector3[] verts;
    public static Vector2[] uvs;
    public static int[] tris;

    public enum MeshTypeEnum
    {
        Terrain,
        Water
    }

    // Creates a mesh
    public static Mesh CreateMesh(Dictionary<Vector2Int, WorldTile> _tiles, MeshTypeEnum _type)
    {
        // Set limits
        int worldSize = gm.baseSettings.worldSize;
        int numRows = worldSize * 2;
        int numTriIndicesPerRow = worldSize * 2 * 2 * 3;
        int numVertsPerRow = numTriIndicesPerRow + 2;
        int numTriIndicesTotal = numRows * numTriIndicesPerRow * 3;
        int numVertsTotal = numRows * numVertsPerRow;
        // Assign limits to arrays
        mesh = new Mesh();
        verts = new Vector3[numVertsTotal];
        uvs = new Vector2[numVertsTotal];
        tris = new int[numTriIndicesTotal];
        // Assign vertices and triangles
        // Loop through rows
        for(int rowIndex = 0; rowIndex < numRows; rowIndex++)
        {
            // Assing starting x and z values
            int x = -worldSize;
            int y = 0;
            int z = -worldSize + rowIndex;
            // Assign vertices
            for(int vertIndex = rowIndex * numVertsPerRow; vertIndex < (rowIndex + 1) * numVertsPerRow; vertIndex++)
            {
                verts[vertIndex] = new Vector3(x, y, z);
                if(vertIndex % 2 == 0)
                {
                    x += 0;
                    z += 1;
                }
                else
                {
                    x += 1;
                    z -= 1;
                }
            }
            // Assign starting x, y, and z values
            int vertIndexStartForTris = rowIndex * numVertsPerRow;
            x = vertIndexStartForTris;
            y = vertIndexStartForTris + 1;
            z = vertIndexStartForTris + 2;
            // Assign triangles
            for(int triIndex = rowIndex * numTriIndicesPerRow; triIndex < (rowIndex + 1) * numTriIndicesPerRow; triIndex += 3)
            {
                tris[triIndex]     = x;
                tris[triIndex + 1] = y;
                tris[triIndex + 2] = z;
                x++;
                if(triIndex % 2 == 0)
                {
                    y += 2;
                }
                else
                {
                    z += 2;
                }
            }
        }
        // Assign mesh data to mesh
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        // Return mesh
        return mesh;
    }
}
