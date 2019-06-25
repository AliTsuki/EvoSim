using UnityEngine;

//
[CreateAssetMenu(menuName = "Settings/Noise Settings")]
public class NoiseSettings : ScriptableObject
{
    [Range(0.0000001f, 1)]
    public double frequency = 0.5f;
    [Range(0.0000001f, 10)]
    public double lacunarity = 0.5f;
    [Range(1, 8)]
    public int octaveCount = 4;
    [Range(0.0000001f, 10)]
    public double persistence = 0.5f;
    public int seed = 0;
}
