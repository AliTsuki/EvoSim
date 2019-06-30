using System;
using System.Collections.Generic;

using UnityEngine;

// Keeps track of and updates all current entities
public static class Lifeforms
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // GameObject reference
    public static GameObject plantParentObject;
    public static GameObject animalParentObject;

    // Entity lists
    public static Dictionary<int, Plant> plants = new Dictionary<int, Plant>();
    public static Dictionary<int, Animal> animals = new Dictionary<int, Animal>();
    // Removal lists
    public static List<int> plantsToRemove = new List<int>();
    public static List<int> animalsToRemove = new List<int>();

    // IDs
    public static int plantID = 0;
    public static int animalID = 0;


    // Start is called before the first frame update
    public static void Start()
    {
        plantParentObject = new GameObject(name: "Plants");
        animalParentObject = new GameObject(name: "Animals");
    }

    // Update is called once per frame
    public static void Update()
    {
        CleanupLifeformsDictionaries();
        UpdateLifeforms();
    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {
        CleanupLifeformsDictionaries();
        FixedUpdateLifeforms();
    }

    // Clean up lifeforms dictionaries
    public static void CleanupLifeformsDictionaries()
    {
        // Clean up plant dictionary
        foreach(int plantID in plantsToRemove)
        {
            plants.Remove(plantID);
        }
        plantsToRemove.Clear();
        // Clean up animal dictionary
        foreach(int animalID in animalsToRemove)
        {
            animals.Remove(animalID);
        }
        animalsToRemove.Clear();
    }

    // Resets all creatures
    public static void ResetAllLife()
    {
        foreach(KeyValuePair<int, Plant> plant in plants)
        {
            GameObject.Destroy(plant.Value.gameObject);
        }
        plants.Clear();
        foreach(KeyValuePair<int, Animal> animal in animals)
        {
            GameObject.Destroy(animal.Value.gameObject);
        }
        animals.Clear();
    }

    // Spawns lifeforms
    public static void SpawnLifeforms()
    {
        // TODO: Add similar plants next to each other instead of totally random
        while(plants.Count < gm.baseSettings.maxPlantCount)
        {
            WorldTile randomTile = World.Tiles[new Vector2Int(GameController.random.Next(-gm.baseSettings.worldSize, gm.baseSettings.worldSize), GameController.random.Next(-gm.baseSettings.worldSize, gm.baseSettings.worldSize))];
            if(randomTile.plant == null)
            {
                plants.Add(plantID, new Plant(plantID, randomTile));
                plantID++;
            }
        }
        if(animals.Count == 0)
        {
            // TODO: Add animals
        }
    }

    // Spawn parented lifeform
    public static void SpawnParentedLifeform(Entity _entity)
    {
        if(_entity.type == Entity.TypeEnum.Plant)
        {
            plants.Add(plantID, _entity as Plant);
            plantID++;
        }
        else if(_entity.type == Entity.TypeEnum.Animal)
        {
            animals.Add(animalID, _entity as Animal);
            animalID++;
        }
    }

    // Update all lifeforms
    public static void UpdateLifeforms()
    {
        try
        {
            // Update all plants
            foreach(KeyValuePair<int, Plant> plant in plants)
            {
                plant.Value.Update();
            }
            // Update all animals
            foreach(KeyValuePair<int, Animal> animal in animals)
            {
                animal.Value.Update();
            }
        }
        catch(Exception e)
        {
            Debug.Log($@"Error Updating Lifeforms: {e.ToString()}");
        }
    }

    // FixedUpdate all lifeforms
    public static void FixedUpdateLifeforms()
    {
        try
        {
            // FixedUpdate all plants
            foreach(KeyValuePair<int, Plant> plant in plants)
            {
                plant.Value.FixedUpdate();
            }
            // FixedUpdate all animals
            foreach(KeyValuePair<int, Animal> animal in animals)
            {
                animal.Value.FixedUpdate();
            }
        }
        catch(Exception e)
        {
            Debug.Log($@"Error FixedUpdating Lifeforms: {e.ToString()}");
        }
    }
}
