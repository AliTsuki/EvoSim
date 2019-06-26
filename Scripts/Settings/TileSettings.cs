using UnityEngine;
using UnityEngine.Tilemaps;

// Tile settings
[CreateAssetMenu(menuName = "Settings/Tile Settings")]
public class TileSettings : ScriptableObject
{
    public Tile stoneTile;
    public Tile cobbleTile;
    public Tile gravelTile;
    public Tile dirtTile;
    public Tile sandTile;
    public Tile siltTile;
    public Tile clayTile;
    public Tile shallowsTile;
    public Tile oceanTile;
    public Tilemap[] tilemaps = new Tilemap[2];
}
