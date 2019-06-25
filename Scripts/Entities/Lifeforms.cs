using System;
using System.Collections.Generic;

using UnityEngine;

public static class Lifeforms
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // Entity lists
    public static Dictionary<int, Plant> Plants = new Dictionary<int, Plant>();
    public static Dictionary<int, Animal> Animals = new Dictionary<int, Animal>();


    // Start is called before the first frame update
    public static void Start()
    {
        SpawnLifeforms();
    }

    // Update is called once per frame
    public static void Update()
    {
        UpdateLifeforms();
    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {
        FixedUpdateLifeforms();
    }

    // Resets all creatures
    public static void ResetAllLife()
    {
        Plants.Clear();
        Animals.Clear();
    }

    // Spawns lifeforms
    public static void SpawnLifeforms()
    {
        if(Plants.Count == 0)
        {
            // TODO: Add plants
        }
        if(Animals.Count == 0)
        {
            // TODO: Add animals
        }
    }

    // Update all lifeforms
    public static void UpdateLifeforms()
    {
        try
        {
            // Update all plants
            foreach(KeyValuePair<int, Plant> plant in Plants)
            {
                plant.Value.Update();
            }
            // Update all animals
            foreach(KeyValuePair<int, Animal> animal in Animals)
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
            foreach(KeyValuePair<int, Plant> plant in Plants)
            {
                plant.Value.FixedUpdate();
            }
            // FixedUpdate all animals
            foreach(KeyValuePair<int, Animal> animal in Animals)
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
