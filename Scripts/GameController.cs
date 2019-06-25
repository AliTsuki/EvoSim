using System.Collections.Generic;

// Controls the state of the game
public static class GameController
{
    // Entity lists
    public static Dictionary<int, Plant> Plants = new Dictionary<int, Plant>();
    public static Dictionary<int, Animal> Animals = new Dictionary<int, Animal>();


    // Start is called before the first frame update
    public static void Start()
    {
        World.Start();
        // Add starting plants and animals

    }

    // Update is called once per frame
    public static void Update()
    {
        World.Update();
        foreach(KeyValuePair<int, Plant> plant in Plants)
        {
            plant.Value.Update();
        }
        foreach(KeyValuePair<int, Animal> animal in Animals)
        {
            animal.Value.Update();
        }
    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {
        World.FixedUpdate();
        foreach(KeyValuePair<int, Plant> plant in Plants)
        {
            plant.Value.FixedUpdate();
        }
        foreach(KeyValuePair<int, Animal> animal in Animals)
        {
            animal.Value.FixedUpdate();
        }
    }
}
