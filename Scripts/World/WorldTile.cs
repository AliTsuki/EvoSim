using System.Collections.Generic;

using UnityEngine;

// World tiles
public class WorldTile
{
    // Constructor fields
    public Vector2Int position;
    public float height;
    public float temperature;
    public float humidity;
    public World.HeightmapTileTypeEnum heightmapTileType;
    public World.SedimentTileTypeEnum sedimentTileType;

    // Tile occupants
    public Plant plant;
    public Dictionary<int, Animal> animals;

    // World tile constructor
    public WorldTile(Vector2Int _position, float _height, float _temperature, float _humidity, World.HeightmapTileTypeEnum _backgroundTileType, World.SedimentTileTypeEnum _foregroundTileType)
    {
        this.position = _position;
        this.height = _height;
        this.temperature = _temperature;
        this.humidity = _humidity;
        this.heightmapTileType = _backgroundTileType;
        this.sedimentTileType = _foregroundTileType;
    }
}
