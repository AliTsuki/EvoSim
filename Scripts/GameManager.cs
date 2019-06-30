using UnityEngine;

// Holds editor settings and interfaces with GameController by sending Unity Engine update ticks through
public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;

    // Settings
    public BaseSettings baseSettings;
    public NoiseSettings heightmapNoiseSettings;
    public NoiseSettings temperatureNoiseSettings;
    public NoiseSettings humidityNoiseSettings;
    public NoiseSettings sedimentNoiseSettings;
    public NoiseSettings stoneNoiseSettings;
    public CutoffSettings cutoffSettings;
    public TileSettings tileSettings;

    // Foldout settings
    [HideInInspector]
    public bool baseSettingsFoldout;
    [HideInInspector]
    public bool heightmapNoiseSettingsFoldout;
    [HideInInspector]
    public bool temperatureNoiseSettingsFoldout;
    [HideInInspector]
    public bool humidityNoiseSettingsFoldout;
    [HideInInspector]
    public bool sedimentNoiseSettingsFoldout;
    [HideInInspector]
    public bool stoneNoiseSettingsFoldout;
    [HideInInspector]
    public bool cutoffSettingsFoldout;
    [HideInInspector]
    public bool tileSettingsFoldout;


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

    // Generate a new world
    public void GenerateNewWorld()
    {
        World.GenerateNewWorld();
    }

    // Randomize seeds
    public void RandomizeSeeds()
    {
        World.RandomizeSeeds();
    }

    // Spawn lifeforms
    public void SpawnLifeforms()
    {
        Lifeforms.ResetAllLife();
        Lifeforms.SpawnLifeforms();
    }
}
