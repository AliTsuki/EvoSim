using System.Collections.Generic;

using SharpNoise.Modules;

using UnityEngine;

// Creates the world environment and assigns tiles to tilemaps
public static class World
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // GameObjects
    private static TextureAtlas terrainTextureAtlas;
    private static GameObject terrainObject;
    private static MeshRenderer terrainMeshRenderer;
    private static MeshFilter terrainMeshFilter;
    private static TextureAtlas waterTextureAtlas;
    private static GameObject waterObject;
    private static MeshRenderer waterMeshRenderer;
    private static MeshFilter waterMeshFilter;
    

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
        Stone,
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
        InstantiateWorldGameObjects();
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
    public static void InstantiateWorldGameObjects()
    {
        // Terrain setup
        terrainTextureAtlas = new TextureAtlas();
        terrainTextureAtlas.CreateAtlas("Terrain");
        terrainObject = new GameObject(name: "Terrain", typeof(MeshFilter), typeof(MeshRenderer));
        terrainMeshFilter = terrainObject.GetComponent<MeshFilter>();
        terrainMeshRenderer = terrainObject.GetComponent<MeshRenderer>();
        terrainMeshRenderer.material = new Material(Shader.Find("Shader Graphs/Terrain Shader"));
        Texture2D terrainAtlas = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        terrainAtlas.LoadImage(System.IO.File.ReadAllBytes("Assets/Textures/Atlas/Terrain Atlas.png"));
        terrainAtlas.filterMode = FilterMode.Point;
        terrainAtlas.wrapMode = TextureWrapMode.Clamp;
        terrainMeshRenderer.material.SetTexture("_Texture2D", terrainAtlas);
        // Water setup
        waterTextureAtlas = new TextureAtlas();
        waterTextureAtlas.CreateAtlas("Water");
        waterObject = new GameObject(name: "Water", typeof(MeshFilter), typeof(MeshRenderer));
        waterMeshFilter = waterObject.GetComponent<MeshFilter>();
        waterMeshRenderer = waterObject.GetComponent<MeshRenderer>();
        waterObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Shader Graphs/Water Shader"));
        Texture2D waterAtlas = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        waterAtlas.LoadImage(System.IO.File.ReadAllBytes("Assets/Textures/Atlas/Water Atlas.png"));
        waterAtlas.filterMode = FilterMode.Point;
        waterAtlas.wrapMode = TextureWrapMode.Clamp;
        waterMeshRenderer.material.SetTexture("_Texture2D", waterAtlas);
        waterObject.transform.position = new Vector3(0, 0.1f, 0);
    }

    // Generates a new world
    public static void GenerateNewWorld()
    {
        ClearTiles();
        UpdateNoiseSettings();
        CreateTiles();
        AssignTilesToMesh();
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
    private static void AssignTilesToMesh()
    {
        terrainObject.GetComponent<MeshFilter>().mesh = MeshBuilder.CreateMesh(Tiles, MeshBuilder.MeshTypeEnum.Terrain);
        waterObject.GetComponent<MeshFilter>().mesh = MeshBuilder.CreateMesh(Tiles, MeshBuilder.MeshTypeEnum.Water);
    }

    // Clears the tile dictionary and all tilemaps
    private static void ClearTiles()
    {
        Tiles.Clear();
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
            sedimentTileType = SedimentTileTypeEnum.Stone;
        }
        return sedimentTileType;
    }

    // Get tile position from quad index
    public static Vector2Int GetTilePosFromQuadIndex(int _quadIndex)
    {
        int x = -gm.baseSettings.worldSize + (_quadIndex % (gm.baseSettings.worldSize * 2));
        int y = -gm.baseSettings.worldSize + Mathf.FloorToInt(_quadIndex / (gm.baseSettings.worldSize * 2));
        return new Vector2Int(x, y);
    }
}
