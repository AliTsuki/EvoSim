using UnityEngine;

// Base settings
[CreateAssetMenu(menuName = "Settings/Base Settings")]
public class BaseSettings : ScriptableObject
{
    public bool updateInEditor = true;
    [Range(1, 60)]
    public int worldSize = 60;
    [Range(0, 512)]
    public int maxAnimalCount = 100;
    [Range(0, 1024)]
    public int maxPlantCount = 1024;
}
