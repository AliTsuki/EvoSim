using UnityEngine;
using UnityEngine.Tilemaps;

//
[CreateAssetMenu(menuName = "Settings/Tile Settings")]
public class TileSettings : ScriptableObject
{
    public RandomTile mountainTile;
    public RandomTile stoneTile;
    public AnimatedTile waterTile;
    public AnimatedTile oceanTile;
    public TerrainTile mountainDirtTile;
    public TerrainTile dirtTile;
    public TerrainTile sandTile;
    public Tilemap[] tilemaps = new Tilemap[2];
}
