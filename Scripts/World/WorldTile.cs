using UnityEngine;

public class WorldTile
{
    public Vector2Int position;
    public World.HeightmapTileTypeEnum backgroundTileType;
    public World.SedimentTileTypeEnum foregroundTileType;

    public WorldTile(Vector2Int _position, World.HeightmapTileTypeEnum _backgroundTileType, World.SedimentTileTypeEnum _foregroundTileType)
    {
        this.position = _position;
        this.backgroundTileType = _backgroundTileType;
        this.foregroundTileType = _foregroundTileType;
    }
}
