using System;
using TMPro;

using UnityEngine;

// Controls the UI
public static class UIController
{
    // Unity GameObjects
    private static readonly GameObject tooltipContainer = GameObject.Find("Tooltip Container");
    private static readonly TextMeshProUGUI tileText = GameObject.Find("Tile Text").GetComponent<TextMeshProUGUI>();
    private static readonly GameObject counterContainer = GameObject.Find("Counter Container");
    private static readonly TextMeshProUGUI counterText = GameObject.Find("Counter Text").GetComponent<TextMeshProUGUI>();

    // Tooltip fields
    private static Vector2Int position = new Vector2Int();
    private static string sediment = "N/A";
    private static string heightmap = "N/A";
    private static Plant plant = null;
    private static string plantID = "N/A";
    private static string plantIsAlive = "N/A";
    private static string plantIsMature = "N/A";
    private static string plantPercentMature = "N/A";
    private static string plantPercentOfMaxAge = "N/A";
    private static string plantIsCarryingSpawn = "N/A";
    private static string plantMateType = "N/A";
    private static string plantMateID = "N/A";
    private static string plantIsReproductionOnCooldown = "N/A";
    private static string plantHealth = "N/A";
    private static string plantMaxHealth = "N/A";
    private static string plantEnergy = "N/A";
    private static string plantMaxEnergy = "N/A";
    private static string plantThriveAmount = "N/A";
    private static string plantCurrentSedimentThrive = "N/A";
    private static string plantBestSedimentThrive = "N/A";
    private static string plantCurrentHeightmapThrive = "N/A";
    private static string plantBestHeightmapThrive = "N/A";
    private static string plantNumberOfMutations = "N/A";

    // Start is called before the first frame update
    public static void Start()
    {
        
    }

    // Update is called once per frame
    public static void Update()
    {
        UpdateTooltip();
        UpdateCounter();
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
            tooltipContainer.SetActive(true);
            position = tile.position;
            sediment = tile.sedimentTileType.ToString();
            heightmap = tile.heightmapTileType.ToString();
            plant = tile.plant;
            if(plant != null)
            {
                plantID = plant.id.ToString();
                plantIsAlive = plant.isAlive.ToString();
                plantIsMature = plant.isMature.ToString();
                plantPercentMature = plant.percentMature.ToString("N2");
                plantPercentOfMaxAge = plant.percentOfMaxAge.ToString("N2");
                plantIsCarryingSpawn = plant.isCarryingSpawn.ToString();
                if(plant.isCarryingSpawn == true)
                {
                    plantMateType = plant.mateType.ToString();
                    plantMateID = plant.mateID.ToString();
                }
                else
                {
                    plantMateType = "N/A";
                    plantMateID = "N/A";
                }
                plantIsReproductionOnCooldown = plant.isReproductionOnCooldown.ToString();
                plantHealth = plant.health.ToString("N2");
                plantMaxHealth = plant.maxHealth.ToString("N2");
                plantEnergy = plant.energy.ToString("N2");
                plantMaxEnergy = plant.maxEnergy.ToString("N2");
                plantThriveAmount = plant.thrivingAmount.ToString("N2");
                plantCurrentSedimentThrive = plant.currentSedimentThrive.ToString("N2");
                plantBestSedimentThrive = plant.bestSedimentThrive.ToString();
                plantCurrentHeightmapThrive = plant.currentHeightmapThrive.ToString("N2");
                plantBestHeightmapThrive = plant.bestHeightmapThrive.ToString();
                plantNumberOfMutations = plant.numberOfMutations.ToString();
                tileText.text = $@"Tile: ({position.x}, {position.y}){Environment.NewLine}Sediment: {sediment}{Environment.NewLine}Height: {heightmap}{Environment.NewLine}Plant: ID({plantID}){Environment.NewLine}Alive: {plantIsAlive}{Environment.NewLine}Mature: {plantIsMature}{Environment.NewLine}% of Mature Age: {plantPercentMature}{Environment.NewLine}% of Max Age: {plantPercentOfMaxAge}{Environment.NewLine}Carrying Spawn: {plantIsCarryingSpawn}  Mate: {plantMateType}  Mate ID: {plantMateID}{Environment.NewLine}Repro on CD: {plantIsReproductionOnCooldown}{Environment.NewLine}Health: {plantHealth} / {plantMaxHealth}{Environment.NewLine}Energy: {plantEnergy} / {plantMaxEnergy}{Environment.NewLine}Thriving Amount: {plantThriveAmount}{Environment.NewLine}Current Sediment Thrive: {plantCurrentSedimentThrive} -- Best: {plantBestSedimentThrive}{Environment.NewLine}Current Height Thrive: {plantCurrentHeightmapThrive} -- Best: {plantBestHeightmapThrive}{Environment.NewLine}Number of Mutations: {plantNumberOfMutations}";
            }
            else
            {
                tileText.text = $@"Tile: ({position.x}, {position.y}){Environment.NewLine}Sediment: {sediment}{Environment.NewLine}Height: {heightmap}";
            }
            
        }
        else
        {
            tooltipContainer.SetActive(false);
        }
    }

    // Update counter
    public static void UpdateCounter()
    {
        counterText.text = $@"Plants: {Logger.plantsTotal}{Environment.NewLine}Alive: {Logger.plantsAlive}  Dead: {Logger.plantsTotal - Logger.plantsAlive}  Percent: {Logger.plantsPercentAlive.ToString("N2")}";
    }
}
