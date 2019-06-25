using UnityEngine;
using UnityEngine.Tilemaps;

// Calls GameController
public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;

    // Editor references
    [SerializeField]
    public bool updateInEditor;
    [SerializeField, Range(1, 256)]
    public int worldSize;
    [SerializeField, Range(0, 256)]
    public int maxAnimalCount;
    [SerializeField, Range(0, 256)]
    public int maxPlantCount;
    [SerializeField, Range(0, 1)]
    public double backgroundFrequency;
    [SerializeField, Range(0, 10)]
    public double backgroundLacunarity;
    [SerializeField, Range(1, 8)]
    public int backgroundOctaveCount;
    [SerializeField, Range(0, 10)]
    public double backgroundPersistence;
    [SerializeField]
    public int backgroundSeed;
    [SerializeField, Range(0, 1)]
    public double foregroundFrequency;
    [SerializeField, Range(0, 10)]
    public double foregroundLacunarity;
    [SerializeField, Range(1, 8)]
    public int foregroundOctaveCount;
    [SerializeField, Range(0, 10)]
    public double foregroundPersistence;
    [SerializeField]
    public int foregroundSeed;
    [SerializeField, Range(-2, 2)]
    public float stoneCutoff;
    [SerializeField, Range(-2, 2)]
    public float dirtCutoff;
    [SerializeField, Range(-2, 2)]
    public float sandCutoff;
    [SerializeField]
    public RandomTile stoneTile;
    [SerializeField]
    public AnimatedTile waterTile;
    [SerializeField]
    public TerrainTile dirtTile;
    [SerializeField]
    public TerrainTile sandTile;
    [SerializeField]
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
