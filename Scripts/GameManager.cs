using UnityEngine;
using UnityEngine.Tilemaps;

// Calls GameController
public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;

    // Editor references
    public bool updateInEditor;
    [Range(1, 256)]
    public int worldSize;
    [Range(0, 256)]
    public int maxAnimalCount;
    [Range(0, 256)]
    public int maxPlantCount;
    [Header("Heightmap Noise")]
    [Range(0, 1)]
    public double backgroundFrequency;
    [Range(0, 10)]
    public double backgroundLacunarity;
    [Range(1, 8)]
    public int backgroundOctaveCount;
    [Range(0, 10)]
    public double backgroundPersistence;
    public int backgroundSeed;
    [Header("Foreground Noise")]
    [Range(0, 1)]
    public double foregroundFrequency;
    [Range(0, 10)]
    public double foregroundLacunarity;
    [Range(1, 8)]
    public int foregroundOctaveCount;
    [Range(0, 10)]
    public double foregroundPersistence;
    public int foregroundSeed;
    [Header("Cutoffs")]
    [Range(-2, 2)]
    public float mountainCutoff;
    [Range(-2, 2)]
    public float stoneCutoff;
    [Range(-2, 2)]
    public float waterCutoff;
    [Range(-2, 2)]
    public float dirtCutoff;
    [Range(-2, 2)]
    public float sandCutoff;
    [Header("Tiles")]
    public RandomTile mountainTile;
    public RandomTile stoneTile;
    public AnimatedTile waterTile;
    public AnimatedTile oceanTile;
    public TerrainTile mountainDirtTile;
    public TerrainTile dirtTile;
    public TerrainTile sandTile;
    public Tilemap[] tilemaps;


    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        GameController.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        GameController.Update();
    }

    // FixedUpdate called a fixed number of times a second
    private void FixedUpdate()
    {
        GameController.FixedUpdate();
    }
}
