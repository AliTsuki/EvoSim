using UnityEngine;

// Base settings
[CreateAssetMenu(menuName = "Settings/Base Settings")]
public class BaseSettings : ScriptableObject
{
    public bool updateInEditor = true;
    [Range(1, 60)]
    public int worldSize = 40;
    [Range(0, 256)]
    public int maxAnimalCount = 50;
    [Range(0, 256)]
    public int maxPlantCount = 100;
}
