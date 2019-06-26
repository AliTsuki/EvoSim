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
    private static GameObject sedimentTilemap;
    private static GameObject waterTilemap;

    // Tile dictionary
    public static Dictionary<Vector2Int, WorldTile> Tiles = new Dictionary<Vector2Int, WorldTile>();

    // TODO: was going to use these for debugging random value stuff
    public static float backgroundNoiseAverageValue = 0f;
    public static float backgroundTileAmount = 0f;

    // Tile types
    public enum HeightmapTileTypeEnum
    {
        Highland,
        Lowland,
        Shallows,
        Ocean,
    }
    public enum SedimentTileTypeEnum
    {
        None,
        Cobble,
        Gravel,
        Dirt,
        Sand,
        Silt,
        Clay
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
    private static Perlin stonePerlin = new Perlin()
    {
        Frequency = gm.stoneNoiseSettings.frequency,
        Lacunarity = gm.stoneNoiseSettings.lacunarity,
        OctaveCount = gm.stoneNoiseSettings.octaveCount,
        Persistence = gm.stoneNoiseSettings.persistence,
        Seed = gm.stoneNoiseSettings.seed
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
        sedimentTilemap = grid.transform.GetChild(0).gameObject;
        waterTilemap = grid.transform.GetChild(1).gameObject;
        gm.tileSettings.tilemaps[0] = sedimentTilemap.GetComponent<Tilemap>();
        gm.tileSettings.tilemaps[1] = waterTilemap.GetComponent<Tilemap>();
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
        gm.stoneNoiseSettings.seed = GameController.random.Next();
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
        stonePerlin = new Perlin()
        {
            Frequency = gm.stoneNoiseSettings.frequency,
            Lacunarity = gm.stoneNoiseSettings.lacunarity,
            OctaveCount = gm.stoneNoiseSettings.octaveCount,
            Persistence = gm.stoneNoiseSettings.persistence,
            Seed = gm.stoneNoiseSettings.seed
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
                float heightmapNoiseValue = (float)heightmapPerlin.GetValue(x, y, 0);
                float temperatureNoiseValue = (float)temperaturePerlin.GetValue(x, y, 0);
                float humidityNoiseValue = (float)humidityPerlin.GetValue(x, y, 0);
                float sedimentNoiseValue = (float)sedimentPerlin.GetValue(x, y, 0);
                float stoneNoiseValue = (float)stonePerlin.GetValue(x, y, 0);
                WorldTile newWorldTile = GetNewWorldTile(xy, heightmapNoiseValue, temperatureNoiseValue, humidityNoiseValue, sedimentNoiseValue, stoneNoiseValue);
                Tiles.Add(xy, newWorldTile);
            }
        }
    }

    // Assigns tiles to tilemaps using tile dictionary
    private static void AssignTilesToTilemap()
    {
        foreach(KeyValuePair<Vector2Int, WorldTile> tile in Tiles)
        {
            // Sediment tiles
            switch(tile.Value.sedimentTileType)
            {
                case SedimentTileTypeEnum.Cobble:
                    {
                        gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.cobbleTile);
                        break;
                    }
                case SedimentTileTypeEnum.Gravel:
                    {
                        gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.gravelTile);
                        break;
                    }
                case SedimentTileTypeEnum.Dirt:
                    {
                        gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.dirtTile);
                        break;
                    }
                case SedimentTileTypeEnum.Sand:
                    {
                        gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.sandTile);
                        break;
                    }
                case SedimentTileTypeEnum.Silt:
                    {
                        gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.siltTile);
                        break;
                    }
                case SedimentTileTypeEnum.Clay:
                    {
                        gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.clayTile);
                        break;
                    }
                case SedimentTileTypeEnum.None:
                    {
                        gm.tileSettings.tilemaps[0].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.stoneTile);
                        break;
                    }
            }
            // Water tiles
            switch(tile.Value.heightmapTileType)
            {
                case HeightmapTileTypeEnum.Shallows:
                    {
                        gm.tileSettings.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.shallowsTile);
                        break;
                    }
                case HeightmapTileTypeEnum.Ocean:
                    {
                        gm.tileSettings.tilemaps[1].SetTile(new Vector3Int(tile.Key.x, tile.Key.y, 0), gm.tileSettings.oceanTile);
                        break;
                    }
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

    // Get new world tile from noise values
    private static WorldTile GetNewWorldTile(Vector2Int _xy, float _heightmapNoiseValue, float _temperatureNoiseValue, float _humidityNoiseValue, float _sedimentNoiseValue, float _stoneNoiseValue)
    {
        HeightmapTileTypeEnum heightmapTileType = GetHeightmapTileType(_heightmapNoiseValue);
        SedimentTileTypeEnum sedimentTileType = GetSedimentTileType(_sedimentNoiseValue, _stoneNoiseValue);
        return new WorldTile(_xy, _heightmapNoiseValue, _temperatureNoiseValue, _humidityNoiseValue, heightmapTileType, sedimentTileType);
    }

    // Get heightmap tile type using noise map output value and cutoff settings
    private static HeightmapTileTypeEnum GetHeightmapTileType(float _heightmapNoiseValue)
    {
        HeightmapTileTypeEnum heightmapTileType;
        // Highland
        if(_heightmapNoiseValue >= gm.cutoffSettings.lowlandToHighlandCutoff)
        {
            heightmapTileType = HeightmapTileTypeEnum.Highland;
        }
        // Lowland
        else if(_heightmapNoiseValue >= gm.cutoffSettings.shallowsToLowlandCutoff && _heightmapNoiseValue < gm.cutoffSettings.lowlandToHighlandCutoff)
        {
            heightmapTileType = HeightmapTileTypeEnum.Lowland;
        }
        // Shallows
        else if(_heightmapNoiseValue >= gm.cutoffSettings.oceanToShallowsCutoff && _heightmapNoiseValue < gm.cutoffSettings.shallowsToLowlandCutoff)
        {
            heightmapTileType = HeightmapTileTypeEnum.Shallows;
        }
        // Oceans
        else
        {
            heightmapTileType = HeightmapTileTypeEnum.Ocean;
        }
        return heightmapTileType;
    }

    // Get heightmap tile type using noise map output value and cutoff settings
    private static SedimentTileTypeEnum GetSedimentTileType(float _sedimentNoiseValue, float _stoneNoiseValue)
    {
        SedimentTileTypeEnum sedimentTileType;
        if(_stoneNoiseValue < gm.cutoffSettings.stoneCutoff)
        {
            if(_sedimentNoiseValue >= gm.cutoffSettings.cobbleCutoff)
            {
                sedimentTileType = SedimentTileTypeEnum.Cobble;
            }
            else if(_sedimentNoiseValue >= gm.cutoffSettings.gravelCutoff && _sedimentNoiseValue < gm.cutoffSettings.cobbleCutoff)
            {
                sedimentTileType = SedimentTileTypeEnum.Gravel;
            }
            else if(_sedimentNoiseValue >= gm.cutoffSettings.dirtCutoff && _sedimentNoiseValue < gm.cutoffSettings.gravelCutoff)
            {
                sedimentTileType = SedimentTileTypeEnum.Dirt;
            }
            else if(_sedimentNoiseValue >= gm.cutoffSettings.sandCutoff && _sedimentNoiseValue < gm.cutoffSettings.dirtCutoff)
            {
                sedimentTileType = SedimentTileTypeEnum.Sand;
            }
            else if(_sedimentNoiseValue >= gm.cutoffSettings.siltCutoff && _sedimentNoiseValue < gm.cutoffSettings.sandCutoff)
            {
                sedimentTileType = SedimentTileTypeEnum.Silt;
            }
            else
            {
                sedimentTileType = SedimentTileTypeEnum.Clay;
            }
        }
        else
        {
            sedimentTileType = SedimentTileTypeEnum.None;
        }
        return sedimentTileType;
    }
}
