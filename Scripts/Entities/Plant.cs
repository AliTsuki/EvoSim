using UnityEngine;

// Defines and controls plants
public class Plant : Entity
{
    // GameManager reference
    private static readonly GameManager gm = GameManager.instance;

    // Plant-only fields
    public World.SedimentTileTypeEnum bestSedimentThrive;
    public float currentSedimentThrive;
    public World.HeightmapTileTypeEnum bestHeightmapThrive;
    public float currentHeightmapThrive;

    // World spawn plant constructor
    public Plant(int _id, WorldTile _tile)
    {
        this.id = _id;
        this.type = TypeEnum.Plant;
        this.currentTile = _tile;
        this.currentTile.plant = this;
        this.isMature = true;
        this.geneTypes = plantGeneTypes;
        this.RandomizeGenes();
        this.SetupGameObject();
        this.InitializeStats();
        this.CheckBestThriveType();
        this.GetTexture();
        this.Start();
    }

    // Parent spawn plant constructor
    public Plant(int _id, WorldTile _tile, Plant _parentOne, Plant _parentTwo)
    {
        this.id = _id;
        this.type = TypeEnum.Plant;
        this.currentTile = _tile;
        this.currentTile.plant = this;
        this.isMature = false;
        this.geneTypes = plantGeneTypes;
        this.GetGenesFromParents(_parentOne, _parentTwo);
        this.SetupGameObject();
        this.InitializeStats();
        this.CheckBestThriveType();
        this.GetTexture();
        this.Start();
    }


    // Set up GameObject
    public void SetupGameObject()
    {
        this.gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Entities/Entity"), new Vector3(this.currentTile.position.x + 0.5f, 0.2f, this.currentTile.position.y + 0.5f), Quaternion.Euler(90, 0, 0), Lifeforms.plantParentObject.transform);
        this.gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Shader Graphs/Entity Shader"));
    }

    // Update energy
    public override void UpdateEnergy()
    {
        
        if(this.energy < 0f)
        {
            this.ModifyHealth(-this.energy);
            this.energy = 0f;
        }
        else if(this.energy > this.maxEnergy)
        {
            this.energy = this.maxEnergy;
        }
        this.ThriveCheck();
    }

    // Thrive check
    private void ThriveCheck()
    {
        string gene = "";
        // Sediment types
        if(this.currentTile.sedimentTileType == World.SedimentTileTypeEnum.Stone)
        {
            gene = geneThriveStone;
        }
        else if(this.currentTile.sedimentTileType == World.SedimentTileTypeEnum.Cobble)
        {
            gene = geneThriveCobble;
        }
        else if(this.currentTile.sedimentTileType == World.SedimentTileTypeEnum.Gravel)
        {
            gene = geneThriveGravel;
        }
        else if(this.currentTile.sedimentTileType == World.SedimentTileTypeEnum.Dirt)
        {
            gene = geneThriveDirt;
        }
        else if(this.currentTile.sedimentTileType == World.SedimentTileTypeEnum.Sand)
        {
            gene = geneThriveSand;
        }
        else if(this.currentTile.sedimentTileType == World.SedimentTileTypeEnum.Silt)
        {
            gene = geneThriveSilt;
        }
        else if(this.currentTile.sedimentTileType == World.SedimentTileTypeEnum.Clay)
        {
            gene = geneThriveClay;
        }
        this.currentSedimentThrive = this.genes[gene];
        this.ModifyEnergy(this.genes[gene] - 0.49f);
        // Heightmap types
        if(this.currentTile.heightmapTileType == World.HeightmapTileTypeEnum.Highland)
        {
            gene = geneThriveHighland;
        }
        else if(this.currentTile.heightmapTileType == World.HeightmapTileTypeEnum.Lowland)
        {
            gene = geneThriveLowland;
        }
        else if(this.currentTile.heightmapTileType == World.HeightmapTileTypeEnum.Shallows)
        {
            gene = geneThriveShallows;
        }
        else if(this.currentTile.heightmapTileType == World.HeightmapTileTypeEnum.Ocean)
        {
            gene = geneThriveOcean;
        }
        this.currentHeightmapThrive = this.genes[gene];
        this.ModifyEnergy(this.genes[gene] - 0.49f);
    }

    // Recheck thrive
    public override void RecheckThrive()
    {
        this.CheckBestThriveType();
    }

    // Check best thrive types
    public void CheckBestThriveType()
    {
        string bestThriveString = "";
        float bestThriveAmount = 0f;
        float newThriveAmount = 0f;
        // Check sediment types
        foreach(string gene in plantThriveGenes)
        {
            if(gene != geneThriveHighland && gene != geneThriveLowland && gene != geneThriveShallows && gene != geneThriveOcean)
            {
                newThriveAmount = this.genes[gene];
                if(newThriveAmount > bestThriveAmount)
                {
                    bestThriveAmount = newThriveAmount;
                    bestThriveString = gene;
                }
            }
        }
        if(bestThriveString == geneThriveStone)
        {
            this.bestSedimentThrive = World.SedimentTileTypeEnum.Stone;
        }
        else if(bestThriveString == geneThriveCobble)
        {
            this.bestSedimentThrive = World.SedimentTileTypeEnum.Cobble;
        }
        else if(bestThriveString == geneThriveGravel)
        {
            this.bestSedimentThrive = World.SedimentTileTypeEnum.Gravel;
        }
        else if(bestThriveString == geneThriveDirt)
        {
            this.bestSedimentThrive = World.SedimentTileTypeEnum.Dirt;
        }
        else if(bestThriveString == geneThriveSand)
        {
            this.bestSedimentThrive = World.SedimentTileTypeEnum.Sand;
        }
        else if(bestThriveString == geneThriveSilt)
        {
            this.bestSedimentThrive = World.SedimentTileTypeEnum.Silt;
        }
        else if(bestThriveString == geneThriveClay)
        {
            this.bestSedimentThrive = World.SedimentTileTypeEnum.Clay;
        }
        // Check heightmap types
        foreach(string gene in plantThriveGenes)
        {
            if(gene == geneThriveHighland || gene == geneThriveLowland || gene == geneThriveShallows || gene == geneThriveOcean)
            {
                newThriveAmount = this.genes[gene];
                if(newThriveAmount > bestThriveAmount)
                {
                    bestThriveAmount = newThriveAmount;
                    bestThriveString = gene;
                }
            }
        }
        if(bestThriveString == geneThriveHighland)
        {
            this.bestHeightmapThrive = World.HeightmapTileTypeEnum.Highland;
        }
        else if(bestThriveString == geneThriveLowland)
        {
            this.bestHeightmapThrive = World.HeightmapTileTypeEnum.Lowland;
        }
        else if(bestThriveString == geneThriveShallows)
        {
            this.bestHeightmapThrive = World.HeightmapTileTypeEnum.Shallows;
        }
        else if(bestThriveString == geneThriveOcean)
        {
            this.bestHeightmapThrive = World.HeightmapTileTypeEnum.Ocean;
        }
    }

    // Update reproduction
    public override void UpdateReproduction()
    {
        if(this.energy >= this.genes[geneEnergyCostReproduction] * energyScale)
        {
            this.CheckForMate();
        }
    }

    // Check for mate
    public void CheckForMate()
    {
        for(int x = this.currentTile.position.x - Mathf.RoundToInt(this.genes[genePollinatingDistance] * distanceScale); x < this.currentTile.position.x + Mathf.RoundToInt(this.genes[genePollinatingDistance] * distanceScale); x++)
        {
            for(int y = this.currentTile.position.y - Mathf.RoundToInt(this.genes[genePollinatingDistance] * distanceScale); y < this.currentTile.position.y + Mathf.RoundToInt(this.genes[genePollinatingDistance] * distanceScale); y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if(World.IsPositionValid(position) == true)
                {
                    Plant checkingPlant = World.Tiles[new Vector2Int(x, y)].plant;
                    if(this.CalculateGeneticSimilarityToTarget(this, checkingPlant) > this.genes[geneMinimumSimilarityReproduction])
                    {
                        this.Mate(checkingPlant);
                    }
                }
            }
        }
    }

    // Mate
    public void Mate(Plant _mate)
    {
        int x = GameController.random.Next(this.currentTile.position.x - Mathf.RoundToInt(this.genes[geneSeedingDistance] * distanceScale), this.currentTile.position.x + Mathf.RoundToInt(this.genes[geneSeedingDistance] * distanceScale));
        int y = GameController.random.Next(this.currentTile.position.y - Mathf.RoundToInt(this.genes[geneSeedingDistance] * distanceScale), this.currentTile.position.y + Mathf.RoundToInt(this.genes[geneSeedingDistance] * distanceScale));
        Vector2Int seedingLocation = new Vector2Int(x, y);
        if(World.IsPositionValid(seedingLocation))
        {
            WorldTile seedingTile = World.Tiles[seedingLocation];
            if(seedingTile.plant == null)
            {
                Plant newPlant = new Plant(Lifeforms.plantID, seedingTile, this, _mate);
                Lifeforms.SpawnParentedLifeform(newPlant);
            }
        }
        this.ModifyEnergy(-this.genes[geneEnergyCostReproduction] * energyScale);
    }
}
