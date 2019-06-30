using System.Collections.Generic;
using System.Linq;

using UnityEngine;

// Superclass for all entities
public abstract class Entity
{
    // Constants
    // Rates
    public const float mutationChanceRate = 0.016f; // 1 percent per second maximum mutation chance
    public const float maxHealingRate = 0.016f; // 1 percent per second health healing maximum
    public const float maxRotRate = 0.016f; // 1 health per second rot speed maximum
    // Scales
    public const float healthScale = 100f; // 100 is highest health value
    public const float energyScale = 100f; // 100 is highest energy value
    public const float sizeScale = 2f; // 200% is largest scale of entities
    public const float smallestSizeScale = 0.2f; // 20% is smallest scale of entities
    public const float maturityScale = 120f; // 2 minutes is longest age to maturity
    public const float ageScale = 600f; // 10 minutes is longest age before death
    public const float distanceScale = 10f; // 10 tiles distance maximum
    

    // Unity objects
    public GameObject gameObject;
    public Transform transform;
    public Sprite sprite;

    // Gene values
    #region shared gene values
    public static string geneMutationChance = "Mutation_Chance";
    public static string geneHealthMax = "Health_Max";
    public static string geneHealingRate = "Healing_Rate";
    public static string geneEnergyMax = "Energy_Max";
    public static string geneSizeMax = "Size_Max";
    public static string geneAgeMature = "Age_Mature";
    public static string geneAgeMax = "Age_Max";
    public static string geneRotSpeed = "Rot_Speed";
    public static string geneEnergyCostReproduction = "Energy_Cost_Reproduction";
    public static string geneMinimumSimilarityReproduction = "Minimum_Similarity_Reproduction";
    #endregion
    #region plant gene values
    public static string genePollinatingDistance = "Pollinating_Distance";
    public static string geneSeedingDistance = "Seeding_Distance";
    public static string geneThriveStone = "Thrive_Stone";
    public static string geneThriveCobble = "Thrive_Cobble";
    public static string geneThriveGravel = "Thrive_Gravel";
    public static string geneThriveDirt = "Thrive_Dirt";
    public static string geneThriveSand = "Thrive_Sand";
    public static string geneThriveSilt = "Thrive_Silt";
    public static string geneThriveClay = "Thrive_Clay";
    public static string geneThriveHighland = "Thrive_Highland";
    public static string geneThriveLowland = "Thrive_Lowland";
    public static string geneThriveShallows = "Thrive_Shallows";
    public static string geneThriveOcean = "Thrive_Ocean";
    public static string[] plantThriveGenes = { geneThriveStone, geneThriveCobble, geneThriveGravel, geneThriveDirt, geneThriveSand, geneThriveSilt, geneThriveClay, geneThriveHighland, geneThriveLowland, geneThriveShallows, geneThriveOcean };
    #endregion
    #region animal gene values
    public static string geneSOMETHINGANIMALGENE = "";
    #endregion
    // Genes
    public Dictionary<string, float> genes = new Dictionary<string, float>();
    public string[] geneTypes;
    public static string[] plantGeneTypes = new string[] { geneMutationChance, geneHealthMax, geneHealingRate, geneEnergyMax, geneSizeMax, geneAgeMature, geneAgeMax, geneRotSpeed, geneEnergyCostReproduction, geneMinimumSimilarityReproduction, genePollinatingDistance, geneSeedingDistance, geneThriveStone, geneThriveCobble, geneThriveGravel, geneThriveDirt, geneThriveSand, geneThriveSilt, geneThriveClay, geneThriveHighland, geneThriveLowland, geneThriveShallows, geneThriveOcean };
    public static string[] animalGeneTypes = new string[] { geneMutationChance, geneHealthMax, geneHealingRate, geneEnergyMax, geneSizeMax, geneAgeMature, geneAgeMax, geneRotSpeed, geneEnergyCostReproduction, geneMinimumSimilarityReproduction };

    // ID
    public int id;
    public enum TypeEnum
    {
        Plant,
        Animal
    }
    public TypeEnum type;
    // Age
    public float birthMoment;
    public float percentOfMaxAge;
    // Health
    public float health;
    public float maxHealth;
    // Energy
    public float energy;
    public float maxEnergy;
    // AI
    public Entity target;
    public WorldTile currentTile;
    // Bools
    public bool isAlive;
    public bool isMature;
    

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        this.UpdateHealth();
        if(this.isAlive == true)
        {
            this.UpdateAge();
            this.UpdateMutation();
            this.UpdateEnergy();
            this.UpdateSize();
            this.UpdateReproduction();
        }
    }

    // FixedUpdate is called a fixed number of times a second
    public void FixedUpdate()
    {

    }

    // Get texture
    public void GetTexture()
    {
        // TODO: make unique plant textures and assign them here based on plant stats
        if(this.type == TypeEnum.Plant)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", new Texture2D(32, 32, TextureFormat.ARGB32, false));
        }
        else if(this.type == TypeEnum.Animal)
        {
            this.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", new Texture2D(32, 32, TextureFormat.ARGB32, false));
        }
    }

    #region actions
    // Heal
    public void Heal()
    {
        float rate = this.genes[geneHealingRate] * maxHealingRate;
        this.ModifyHealth(rate * this.maxHealth);
        this.ModifyEnergy(-rate * this.maxEnergy);
    }

    // Modify health
    public void ModifyHealth(float _amount)
    {
        this.health += _amount;
    }

    // Modify energy
    public void ModifyEnergy(float _amount)
    {
        this.energy += _amount;
    }

    // Rot
    public void Rot()
    {
        this.ModifyHealth(-this.genes[geneRotSpeed] * maxRotRate);
    }

    // Die
    public virtual void Die()
    {
        this.isAlive = false;
        this.health = this.maxHealth;
    }

    // Despawn
    public void Despawn()
    {
        GameObject.Destroy(this.gameObject);
        if(this.type == TypeEnum.Plant)
        {
            this.currentTile.plant = null;
            Lifeforms.plantsToRemove.Add(this.id);
        }
        else if(this.type == TypeEnum.Animal)
        {
            this.currentTile.animals.Remove(this.id);
            Lifeforms.animalsToRemove.Add(this.id);
        }
    }
    #endregion

    #region genetics
    // Creates a random set of genes for this entity
    public void RandomizeGenes()
    {
        foreach(string geneType in this.geneTypes)
        {
            this.genes.Add(geneType, (float)GameController.random.NextDouble() + 0.0001f);
        }
    }

    // Gets random genes from two parents
    public void GetGenesFromParents(Entity _parentOne, Entity _parentTwo)
    {
        foreach(KeyValuePair<string, float> gene in _parentOne.genes)
        {
            int random = GameController.random.Next(0, 2);
            float geneValue = (random == 0) ? _parentOne.genes[gene.Key] : _parentTwo.genes[gene.Key];
            this.genes.Add(gene.Key, geneValue);
        }
    }

    // Mutate random gene
    public void Mutate()
    {
        string geneType = this.genes.ElementAt(GameController.random.Next(0, this.genes.Count)).Key;
        this.genes[geneType] = (float)GameController.random.NextDouble() + 0.0001f;
        this.RecheckThrive();
    }

    // Recheck genes
    public virtual void RecheckThrive()
    {

    }

    // Initializes stat values
    public void InitializeStats()
    {
        this.isAlive = true;
        this.birthMoment = Time.time;
        this.maxHealth = this.genes[geneHealthMax] * healthScale;
        this.health = this.maxHealth;
        this.maxEnergy = this.genes[geneEnergyMax] * energyScale;
        this.energy = this.maxEnergy;
        if(this.isMature == false)
        {
            float size = Mathf.Clamp(this.genes[geneSizeMax], smallestSizeScale, sizeScale);
            this.gameObject.transform.localScale = new Vector3(size, size, size);
        }
        else
        {
            float size = this.genes[geneSizeMax] * sizeScale;
            this.gameObject.transform.localScale = new Vector3(size, size, size);
        }
    }

    // Calculate genetic similarity to target
    public float CalculateGeneticSimilarityToTarget(Entity _currentEntity, Entity _targetedEntity)
    {
        float[] similarityPercents = new float[_currentEntity.genes.Count];
        float averageSimilarity = 0f;
        int index = 0;
        foreach(KeyValuePair<string, float> gene in _currentEntity.genes)
        {
            similarityPercents[index] = Mathematics.GetPointOnNormalDistribution(_currentEntity.genes[gene.Key], _targetedEntity.genes[gene.Key]);
            averageSimilarity += similarityPercents[index];
            index++;
        }
        averageSimilarity /= similarityPercents.Length;
        return averageSimilarity;
    }
    #endregion

    #region updates
    // Update health
    public void UpdateHealth()
    {
        // If alive and health is 0 or less or reached maximum age, die
        if(this.isAlive == true && (this.health <= 0f || this.percentOfMaxAge >= 1f))
        {
            this.Die(); // :(
        }
        // If alive and health is less than max and energy is greater than half max, heal
        else if(this.isAlive == true && this.health < this.maxHealth && this.energy > this.maxEnergy * 0.5f)
        {
            this.Heal();
        }
        // If dead, rot
        else if(this.isAlive == false)
        {
            this.Rot();
        }
        // If dead and fully rotted, despawn
        else if(this.isAlive == false && this.health <= 0f)
        {
            this.Despawn();
        }
        // If health went over limit, bring back down
        if(this.health > this.maxHealth)
        {
            this.health = this.maxHealth;
        }
    }

    // Update age
    public void UpdateAge()
    {
        this.percentOfMaxAge = Time.time / this.genes[geneAgeMax] * ageScale;
    }

    // Update mutation
    public void UpdateMutation()
    {
        float mutationChance = this.genes[geneMutationChance] * mutationChanceRate;
        float random = (float)GameController.random.NextDouble();
        if(random < mutationChance)
        {
            this.Mutate();
        }
    }

    // Update energy
    public virtual void UpdateEnergy()
    {

    }

    // Update scale of entity
    public void UpdateSize()
    {
        if(this.isMature == false)
        {
            float percentMature = Time.time / ((this.genes[geneAgeMature] * maturityScale) + this.birthMoment);
            if(percentMature >= 1.0f)
            {
                this.isMature = true;
                float size = this.genes[geneSizeMax] * sizeScale;
                this.gameObject.transform.localScale = new Vector3(size, size, size);
            }
            else
            {
                float size = Mathf.Clamp(this.genes[geneSizeMax] * sizeScale * percentMature, smallestSizeScale, sizeScale);
                this.gameObject.transform.localScale = new Vector3(size, size, size);
            }
        }
    }

    // Update reproduction
    public virtual void UpdateReproduction()
    {

    }
    #endregion
}
