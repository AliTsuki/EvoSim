using System;
using TMPro;

using UnityEngine;

// Controls the UI
public static class UIController
{
    private static readonly GameObject tooltipContainer = GameObject.Find("Tooltip Container");
    private static readonly TextMeshProUGUI tileText = GameObject.Find("Tile Text").GetComponent<TextMeshProUGUI>();

    // Start is called before the first frame update
    public static void Start()
    {
        
    }

    // Update is called once per frame
    public static void Update()
    {
        UpdateTooltip();
    }

    // FixedUpdate is called a fixed number of times a second
    public static void FixedUpdate()
    {

    }

    // Update tooltip
    public static void UpdateTooltip()
    {
        WorldTile tile = World.GetTileFromWorldPos(CameraController.mousePositionInWorld);
        if(tile != null)
        {
            Vector2Int position = tile.position;
            string sediment = tile.sedimentTileType.ToString();
            string heightmap = tile.heightmapTileType.ToString();
            string plantID = (tile.plant != null) ? tile.plant.id.ToString() : "N/A";
            string plantHealth = (tile.plant != null) ? tile.plant.health.ToString() : "N/A";
            string plantEnergy = (tile.plant != null) ? tile.plant.energy.ToString() : "N/A";
            tileText.text = $@"Tile: ({position.x}, {position.y}){Environment.NewLine}Sediment: {sediment}{Environment.NewLine}Height: {heightmap}{Environment.NewLine}Plant: ID({plantID}){Environment.NewLine}Health: {plantHealth}{Environment.NewLine}Energy: {plantEnergy}";
        }
    }
}
