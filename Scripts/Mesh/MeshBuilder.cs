using System.Collections.Generic;

using UnityEngine;

public static class MeshBuilder
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

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
        int numQuadsPerRow = numRows;
        const int numVertsPerQuad = 4;
        const int numTriIndicesPerQuad = 6;
        int numVertsTotal = numRows * numVertsPerQuad * numQuadsPerRow;
        int numTriIndicesTotal = numVertsTotal * 3 / 2;
        // Assign limits to arrays
        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[numVertsTotal];
        Vector2[] uvs = new Vector2[numVertsTotal];
        int[] tris = new int[numTriIndicesTotal];
        // Assign all verts, uvs, and tris to arrays
        // Loop through rows
        for(int rowIndex = 0; rowIndex < numRows; rowIndex++)
        {
            // Verts
            // Assign starting x and z values for vertices
            int vx = -worldSize;
            int vy = 0;
            int vz = -worldSize + rowIndex;
            // Loop through quads in row
            for(int quadIndex = rowIndex * numQuadsPerRow; quadIndex < (rowIndex + 1) * numQuadsPerRow; quadIndex++)
            {
                // Assign vertices
                int vertIndex = quadIndex * numVertsPerQuad;
                verts[vertIndex] = new Vector3(vx, vy, vz);
                vertIndex++;
                vx += 0;
                vz += 1;
                verts[vertIndex] = new Vector3(vx, vy, vz);
                vertIndex++;
                vx += 1;
                vz -= 1;
                verts[vertIndex] = new Vector3(vx, vy, vz);
                vertIndex++;
                vx += 0;
                vz += 1;
                verts[vertIndex] = new Vector3(vx, vy, vz);
                vz -= 1;
            }
            // UVs
            // Loop through quads in row
            for(int quadIndex = rowIndex * numQuadsPerRow; quadIndex < (rowIndex + 1) * numQuadsPerRow; quadIndex++)
            {
                // Assign UVs
                int uvIndex = quadIndex * numVertsPerQuad;
                {
                    Vector2Int tilePos = World.GetTilePosFromQuadIndex(quadIndex);
                    string name;
                    if(_type == MeshTypeEnum.Terrain)
                    {
                        name = _tiles[tilePos].sedimentTileType.ToString();
                    }
                    else
                    {
                        name = _tiles[tilePos].heightmapTileType.ToString();
                        if(name != World.HeightmapTileTypeEnum.Shallows.ToString() && name != World.HeightmapTileTypeEnum.Ocean.ToString())
                        {
                            name = "Blank";
                        }
                    }
                    Vector2[] map = UVMap.GetUVMap(name).UVMaps;
                    uvs[uvIndex] = map[0];
                    uvIndex++;
                    uvs[uvIndex] = map[1];
                    uvIndex++;
                    uvs[uvIndex] = map[2];
                    uvIndex++;
                    uvs[uvIndex] = map[3];
                }
            }
            // Tris
            // Assign starting x, y, and z values for triangles
            int vertIndexStartForTris = rowIndex * numVertsPerQuad * numQuadsPerRow;
            int tx = vertIndexStartForTris;
            int ty = vertIndexStartForTris + 1;
            int tz = vertIndexStartForTris + 2;
            // Loop through quads in row
            for(int quadIndex = rowIndex * numQuadsPerRow; quadIndex < (rowIndex + 1) * numQuadsPerRow; quadIndex++)
            {
                // Assign triangles
                int triIndex = quadIndex * numTriIndicesPerQuad;
                tris[triIndex] = tx;
                triIndex++;
                tris[triIndex] = ty;
                triIndex++;
                tris[triIndex] = tz;
                triIndex++;
                tx++;
                ty += 2;
                tris[triIndex] = tx;
                triIndex++;
                tris[triIndex] = ty;
                triIndex++;
                tris[triIndex] = tz;
                tx += 3;
                ty += 2;
                tz += 4;
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
