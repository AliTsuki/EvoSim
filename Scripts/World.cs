using System.Collections.Generic;

using SharpNoise.Modules;

using UnityEngine;

// Controls the background tiles
public static class World
{
    // GameManager reference
    private static GameManager gm = GameManager.instance;

    // Tile dictionary
    public static Dictionary<Vector2Int, WorldTile> Tiles = new Dictionary<Vector2Int, WorldTile>();

    public static float backgroundNoiseAverageValue = 0f;
    public static float backgroundTileAmount = 0f;

    // Tile types
    public enum BackgroundTileTypeEnum
    {
        Mountain,
        Stone,
        Water,
        Ocean,
    }
    public enum ForegroundTileTypeEnum
    {
        None,
        MountainDirt,
        Dirt,
        Sand
    }

    // Noise
    private static Perlin backgroundPerlin = new Perlin()
    {
        Frequency = gm.backgroundFrequency,
        Lacunarity = gm.backgroundLacunarity,
        OctaveCount = gm.backgroundOctaveCount,
        Persistence = gm.backgroundPersistence,
        Seed = gm.backgroundSeed
    };
    private static Perlin foregroundPerlin = new Perlin()
    {
        Frequency = gm.foregroundFrequency,
        Lacunarity = gm.foregroundLacunarity,
        OctaveCount = gm.foregroundOctaveCount,
        Persistence = gm.foregroundPersistence,
        Seed = gm.foregroundSeed
    };

    // Start is called before the first frame update
    public static void Start()
    {
        ClearTilemaps();
        RandomlyAssignTilesToWorld();
        CreateTilemapFromTiles();
    }

    // Update is called once per frame
    public static void Update()
    {
        if(gm.updateInEditor == true)
        {
            ClearTilemaps();
            backgroundPerlin = new Perlin()
            {
                Frequency = gm.backgroundFrequency,
                Lacunarity = gm.backgroundLacunarity,
                OctaveCount = gm.backgroundOctaveCount,
                Persistence = gm.backgroundPersistence,
                Seed = gm.backgroundSeed
            };
            foregroundPerlin = new Perlin()
            {
                Frequency = gm.foregroundFrequency,
                Lacunarity = gm.foregroundLacunarity,
                OctaveCount = gm.foregroundOctaveCount,
                Persistence = gm.foregroundPersistence,
                Seed = gm.foregroundSeed
            };
            RandomlyAssignTilesToWorld();
            CreateTilemapFromTiles();
        }
    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {

    }

    

    // 
    public static void RandomlyAssignTilesToWorld()
    {
        // Create random environment of tiles
        for(int x = -gm.worldSize; x < gm.worldSize; x++)
        {
            for(int y = -gm.worldSize; y < gm.worldSize; y++)
            {
                Vector2Int xy = new Vector2Int(x, y);
                // Get random tile
                double backgroundNoiseValue = backgroundPerlin.GetValue(x, y, 0);
                double foregroundNoiseValue = foregroundPerlin.GetValue(x, y, 0);
                BackgroundTileTypeEnum backgroundTileType = GetBackgroundTileType(backgroundNoiseValue);
                ForegroundTileTypeEnum foregroundTileType = GetForegroundTileType(foregroundNoiseValue, backgroundTileType);
                Tiles.Add(xy, new WorldTile(xy, backgroundTileType, foregroundTileType));
            }
        }
    }

    //
    public static void CreateTilemapFromTiles()
    {
        // Assign tiles to tilemap
        foreach(KeyValuePair<Vector2Int, WorldTile> tile in Tiles)
        {
            // Background tiles
            if(tile.Value.backgroundTileType == BackgroundTileTypeEnum.Mountain)
            {
                gm.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.mountainTile);
            }
            else if(tile.Value.backgroundTileType == BackgroundTileTypeEnum.Stone)
            {
                gm.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.stoneTile);
            }
            else if(tile.Value.backgroundTileType == BackgroundTileTypeEnum.Water)
            {
                gm.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.waterTile);
            }
            else
            {
                gm.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.oceanTile);
            }
            // Foreground tiles
            if(tile.Value.foregroundTileType == ForegroundTileTypeEnum.MountainDirt)
            {
                gm.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.mountainDirtTile);
            }
            else if(tile.Value.foregroundTileType == ForegroundTileTypeEnum.Dirt)
            {
                gm.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.dirtTile);
            }
            else if(tile.Value.foregroundTileType == ForegroundTileTypeEnum.Sand)
            {
                gm.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.sandTile);
            }
            else if(tile.Value.foregroundTileType == ForegroundTileTypeEnum.None)
            {
                gm.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), null);
            }
        }
    }

    //
    public static void ClearTilemaps()
    {
        Tiles.Clear();
        for(int i = 0; i < gm.tilemaps.Length; i++)
        {
            gm.tilemaps[i].ClearAllTiles();
        }
    }

    // Get background tile type
    public static BackgroundTileTypeEnum GetBackgroundTileType(double _value)
    {
        if(_value > gm.mountainCutoff)
        {
            return BackgroundTileTypeEnum.Mountain;
        }
        else if(_value > gm.stoneCutoff)
        {
            return BackgroundTileTypeEnum.Stone;
        }
        else if(_value > gm.waterCutoff)
        {
            return BackgroundTileTypeEnum.Water;
        }
        else
        {
            return BackgroundTileTypeEnum.Ocean;
        }
    }

    // Get foreground tile type
    public static ForegroundTileTypeEnum GetForegroundTileType(double _value, BackgroundTileTypeEnum _backgroundTileType)
    {
        if(_backgroundTileType != BackgroundTileTypeEnum.Water && _backgroundTileType != BackgroundTileTypeEnum.Ocean)
        {
            if(_backgroundTileType == BackgroundTileTypeEnum.Stone)
            {
                if(_value > gm.dirtCutoff)
                {
                    return ForegroundTileTypeEnum.Dirt;
                }
                else if(_value > gm.sandCutoff)
                {
                    return ForegroundTileTypeEnum.Sand;
                }
            }
            else
            {
                if(_value > gm.dirtCutoff)
                {
                    return ForegroundTileTypeEnum.MountainDirt;
                }
            }
        }
        // If background tile is water, no foreground tile allowed
        return ForegroundTileTypeEnum.None;
    }
}
