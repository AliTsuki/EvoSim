using UnityEngine;

//
[CreateAssetMenu(menuName = "Settings/Cutoff Settings")]
public class CutoffSettings : ScriptableObject
{
    [Range(-2, 2)]
    public float valleyToMountainCutoff = 1;
    [Range(-2, 2)]
    public float waterToLandCutoff = 0;
    [Range(-2, 2)]
    public float oceanToShallowsCutoff = -0.25f;
    [Range(-2, 2)]
    public float dirtCutoff = 0.5f;
    [Range(-2, 2)]
    public float sandCutoff = 0.25f;
}
