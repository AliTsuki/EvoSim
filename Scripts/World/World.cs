using System.Collections.Generic;

using SharpNoise.Modules;

using UnityEngine;
using UnityEngine.Tilemaps;

// Creates the world environment and assigns tiles to tilemaps
public static class World
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // GameObjects
    private static GameObject grid;
    private static GameObject heightmapTilemap;
    private static GameObject sedimentTilemap;

    // Tile dictionary
    public static Dictionary<Vector2Int, WorldTile> Tiles = new Dictionary<Vector2Int, WorldTile>();

    // TODO: was going to use these for debugging random value stuff
    public static float backgroundNoiseAverageValue = 0f;
    public static float backgroundTileAmount = 0f;

    // Tile types
    public enum HeightmapTileTypeEnum
    {
        Mountain,
        Stone,
        Water,
        Ocean,
    }
    public enum SedimentTileTypeEnum
    {
        None,
        MountainDirt,
        Dirt,
        Sand
    }

    // Noise maps
    private static Perlin heightmapPerlin = new Perlin()
    {
        Frequency = gm.heightmapNoiseSettings.frequency,
        Lacunarity = gm.heightmapNoiseSettings.lacunarity,
        OctaveCount = gm.heightmapNoiseSettings.octaveCount,
        Persistence = gm.heightmapNoiseSettings.persistence,
        Seed = gm.heightmapNoiseSettings.seed
    };
    private static Perlin temperaturePerlin = new Perlin()
    {
        Frequency = gm.temperatureNoiseSettings.frequency,
        Lacunarity = gm.temperatureNoiseSettings.lacunarity,
        OctaveCount = gm.temperatureNoiseSettings.octaveCount,
        Persistence = gm.temperatureNoiseSettings.persistence,
        Seed = gm.temperatureNoiseSettings.seed
    };
    private static Perlin humidityPerlin = new Perlin()
    {
        Frequency = gm.humidityNoiseSettings.frequency,
        Lacunarity = gm.humidityNoiseSettings.lacunarity,
        OctaveCount = gm.humidityNoiseSettings.octaveCount,
        Persistence = gm.humidityNoiseSettings.persistence,
        Seed = gm.humidityNoiseSettings.seed
    };
    private static Perlin sedimentPerlin = new Perlin()
    {
        Frequency = gm.sedimentNoiseSettings.frequency,
        Lacunarity = gm.sedimentNoiseSettings.lacunarity,
        OctaveCount = gm.sedimentNoiseSettings.octaveCount,
        Persistence = gm.sedimentNoiseSettings.persistence,
        Seed = gm.sedimentNoiseSettings.seed
    };

    // Start is called before the first frame update
    public static void Start()
    {
        InstantiateTilemapGameObject();
        GenerateNewWorld();
    }

    // Update is called once per frame
    public static void Update()
    {

    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {

    }

    // Instantiates the grid and tilemaps
    public static void InstantiateTilemapGameObject()
    {
        grid = GameObject.Instantiate(Resources.Load<GameObject>("Grid"));
        heightmapTilemap = grid.transform.GetChild(0).gameObject;
        sedimentTilemap = grid.transform.GetChild(1).gameObject;
        gm.tileSettings.tilemaps[0] = heightmapTilemap.GetComponent<Tilemap>();
        gm.tileSettings.tilemaps[1] = sedimentTilemap.GetComponent<Tilemap>();
    }

    // Generates a new world
    public static void GenerateNewWorld()
    {
        ClearTilesAndTilemap();
        UpdateNoiseSettings();
        CreateTiles();
        AssignTilesToTilemap();
        Lifeforms.ResetAllLife();
        Lifeforms.SpawnLifeforms();
        Debug.Log($@"New World Generated!");
    }

    // Randomize seeds
    public static void RandomizeSeeds()
    {
        gm.heightmapNoiseSettings.seed = GameController.random.Next();
        gm.temperatureNoiseSettings.seed = GameController.random.Next();
        gm.humidityNoiseSettings.seed = GameController.random.Next();
        gm.sedimentNoiseSettings.seed = GameController.random.Next();
        GenerateNewWorld();
    }

    // Updates noise maps with current noise settings
    public static void UpdateNoiseSettings()
    {
        heightmapPerlin = new Perlin()
        {
            Frequency = gm.heightmapNoiseSettings.frequency,
            Lacunarity = gm.heightmapNoiseSettings.lacunarity,
            OctaveCount = gm.heightmapNoiseSettings.octaveCount,
            Persistence = gm.heightmapNoiseSettings.persistence,
            Seed = gm.heightmapNoiseSettings.seed
        };
        temperaturePerlin = new Perlin()
        {
            Frequency = gm.temperatureNoiseSettings.frequency,
            Lacunarity = gm.temperatureNoiseSettings.lacunarity,
            OctaveCount = gm.temperatureNoiseSettings.octaveCount,
            Persistence = gm.temperatureNoiseSettings.persistence,
            Seed = gm.temperatureNoiseSettings.seed
        };
        humidityPerlin = new Perlin()
        {
            Frequency = gm.humidityNoiseSettings.frequency,
            Lacunarity = gm.humidityNoiseSettings.lacunarity,
            OctaveCount = gm.humidityNoiseSettings.octaveCount,
            Persistence = gm.humidityNoiseSettings.persistence,
            Seed = gm.humidityNoiseSettings.seed
        };
        sedimentPerlin = new Perlin()
        {
            Frequency = gm.sedimentNoiseSettings.frequency,
            Lacunarity = gm.sedimentNoiseSettings.lacunarity,
            OctaveCount = gm.sedimentNoiseSettings.octaveCount,
            Persistence = gm.sedimentNoiseSettings.persistence,
            Seed = gm.sedimentNoiseSettings.seed
        };
    }

    // Creates a dictionary of world tiles
    private static void CreateTiles()
    {
        for(int x = -gm.baseSettings.worldSize; x < gm.baseSettings.worldSize; x++)
        {
            for(int y = -gm.baseSettings.worldSize; y < gm.baseSettings.worldSize; y++)
            {
                Vector2Int xy = new Vector2Int(x, y);
                double backgroundNoiseValue = heightmapPerlin.GetValue(x, y, 0);
                double foregroundNoiseValue = sedimentPerlin.GetValue(x, y, 0);
                HeightmapTileTypeEnum backgroundTileType = GetHeightmapTileType(backgroundNoiseValue);
                SedimentTileTypeEnum foregroundTileType = GetSedimentTileType(foregroundNoiseValue, backgroundTileType);
                Tiles.Add(xy, new WorldTile(xy, backgroundTileType, foregroundTileType));
            }
        }
    }

    // Assigns tiles to tilemaps using tile dictionary
    private static void AssignTilesToTilemap()
    {
        foreach(KeyValuePair<Vector2Int, WorldTile> tile in Tiles)
        {
            // Heightmap tiles
            if(tile.Value.backgroundTileType == HeightmapTileTypeEnum.Mountain)
            {
                gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.mountainTile);
            }
            else if(tile.Value.backgroundTileType == HeightmapTileTypeEnum.Stone)
            {
                gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.stoneTile);
            }
            else if(tile.Value.backgroundTileType == HeightmapTileTypeEnum.Water)
            {
                gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.waterTile);
            }
            else
            {
                gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.oceanTile);
            }
            // Sediment tiles
            if(tile.Value.foregroundTileType == SedimentTileTypeEnum.MountainDirt)
            {
                gm.tileSettings.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.mountainDirtTile);
            }
            else if(tile.Value.foregroundTileType == SedimentTileTypeEnum.Dirt)
            {
                gm.tileSettings.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.dirtTile);
            }
            else if(tile.Value.foregroundTileType == SedimentTileTypeEnum.Sand)
            {
                gm.tileSettings.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.sandTile);
            }
            else if(tile.Value.foregroundTileType == SedimentTileTypeEnum.None)
            {
                gm.tileSettings.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), null);
            }
        }
    }

    // Clears the tile dictionary and all tilemaps
    private static void ClearTilesAndTilemap()
    {
        Tiles.Clear();
        for(int i = 0; i < gm.tileSettings.tilemaps.Length; i++)
        {
            gm.tileSettings.tilemaps[i].ClearAllTiles();
        }
    }

    // Get heightmap tile type using noise map output value at position and cutoff settings
    private static HeightmapTileTypeEnum GetHeightmapTileType(double _value)
    {
        if(_value > gm.cutoffSettings.valleyToMountainCutoff)
        {
            return HeightmapTileTypeEnum.Mountain;
        }
        else if(_value > gm.cutoffSettings.waterToLandCutoff)
        {
            return HeightmapTileTypeEnum.Stone;
        }
        else if(_value > gm.cutoffSettings.oceanToShallowsCutoff)
        {
            return HeightmapTileTypeEnum.Water;
        }
        else
        {
            return HeightmapTileTypeEnum.Ocean;
        }
    }

    // Get sediment tile type using noise map output value at position and cutoff settings
    private static SedimentTileTypeEnum GetSedimentTileType(double _value, HeightmapTileTypeEnum _backgroundTileType)
    {
        if(_backgroundTileType != HeightmapTileTypeEnum.Water && _backgroundTileType != HeightmapTileTypeEnum.Ocean)
        {
            if(_backgroundTileType == HeightmapTileTypeEnum.Stone)
            {
                if(_value > gm.cutoffSettings.dirtCutoff)
                {
                    return SedimentTileTypeEnum.Dirt;
                }
                else if(_value > gm.cutoffSettings.sandCutoff)
                {
                    return SedimentTileTypeEnum.Sand;
                }
            }
            else
            {
                if(_value > gm.cutoffSettings.dirtCutoff)
                {
                    return SedimentTileTypeEnum.MountainDirt;
                }
            }
        }
        // If heightmap tile is water or ocean, no sediment tile allowed
        return SedimentTileTypeEnum.None;
    }
}
