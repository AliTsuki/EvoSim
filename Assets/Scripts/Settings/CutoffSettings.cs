using UnityEngine;

// Cutoff settings
[CreateAssetMenu(menuName = "Settings/Cutoff Settings")]
public class CutoffSettings : ScriptableObject
{
    [Header("Heightmap Cutoffs")]
    [Range(-2, 2)]
    public float lowlandToHighlandCutoff = 1;
    [Range(-2, 2)]
    public float shallowsToLowlandCutoff = 0;
    [Range(-2, 2)]
    public float oceanToShallowsCutoff = -0.25f;
    [Header("Sediment Cutoffs")]
    [Range(-2, 2)]
    public float cobbleCutoff = 0.625f;
    [Range(-2, 2)]
    public float gravelCutoff = 0.375f;
    [Range(-2, 2)]
    public float dirtCutoff = 0.125f;
    [Range(-2, 2)]
    public float sandCutoff = -0.125f;
    [Range(-2, 2)]
    public float siltCutoff = 0.375f;
    [Range(-2, 2)]
    public float clayCutoff = -0.625f;
    [Range(-2, 2)]
    public float stoneCutoff = 1f;
}
