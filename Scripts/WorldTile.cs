using UnityEngine;

public class WorldTile
{
    public Vector2Int position;
    public World.BackgroundTileTypeEnum backgroundTileType;
    public World.ForegroundTileTypeEnum foregroundTileType;

    public WorldTile(Vector2Int _position, World.BackgroundTileTypeEnum _backgroundTileType, World.ForegroundTileTypeEnum _foregroundTileType)
    {
        this.position = _position;
        this.backgroundTileType = _backgroundTileType;
        this.foregroundTileType = _foregroundTileType;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
