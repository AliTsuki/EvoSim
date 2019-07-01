using System.Collections.Generic;
using System.Linq;

using UnityEngine;

// Superclass for all entities
public abstract class Entity
{
    // Constants
    // Probability Rates
    public const float mutationChanceRate = 0.0016f; // 0.1 percent per second maximum mutation chance
    public const float maxHealingRate = 0.016f; // 1 percent health per second healing maximum
    public const float maxRotRate = 0.16f; // 10 percent health per second rot speed maximum
    // Health and Energy values
    public const float healthScale = 200f; // 200 is highest health value
    public const float energyScale = 100f; // 200 is highest energy value
    // Size scales
    public const float sizeScale = 1.5f; // 150% is largest scale of entities
    public const float smallestSizeScale = 0.05f; // 5% is smallest scale of entities
    // Distance scales
    public const float distanceScale = 15f; // 15 tiles distance maximum
    // Time scales
    public const float maturityScale = 60f; // 1 minutes is longest age to maturity
    public const float ageScale = 300f; // 5 minutes is longest age before death
    public const float gestationTimeScale = 60f; // 1 minutes is longest gestation time
    public const float reproductionCooldownTimeScale = 60f; // 1 minutes is longest time between births


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
    public static string geneReproductionCooldownTime = "Reproduction_Cooldown_Time";
    public static string geneGestationTime = "Gestation_Time";
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
    public Dictionary<string, float> matesGenes = new Dictionary<string, float>();
    public string[] geneTypes;
    public static string[] plantGeneTypes = new string[] { geneMutationChance, geneHealthMax, geneHealingRate, geneEnergyMax, geneSizeMax, geneAgeMature, geneAgeMax, geneRotSpeed, geneEnergyCostReproduction, geneMinimumSimilarityReproduction, geneReproductionCooldownTime, geneGestationTime, genePollinatingDistance, geneSeedingDistance, geneThriveStone, geneThriveCobble, geneThriveGravel, geneThriveDirt, geneThriveSand, geneThriveSilt, geneThriveClay, geneThriveHighland, geneThriveLowland, geneThriveShallows, geneThriveOcean };
    public static string[] animalGeneTypes = new string[] { geneMutationChance, geneHealthMax, geneHealingRate, geneEnergyMax, geneSizeMax, geneAgeMature, geneAgeMax, geneRotSpeed, geneEnergyCostReproduction, geneMinimumSimilarityReproduction, geneReproductionCooldownTime, geneGestationTime };

    // ID
    public int id;
    public enum TypeEnum
    {
        Plant,
        Animal
    }
    public TypeEnum type;
    // Age times
    public float birthTime = 0.0f;
    public float lastSeededTime = 0.0f;
    public float lastBirthedTime = 0.0f;
    // Age percents
    public float percentOfMaxAge = 0.0f;
    public float percentMature = 0.0f;
    // Health
    public float health = 100f;
    public float maxHealth = 100f;
    // Energy
    public float energy = 100f;
    public float maxEnergy = 100f;
    // AI
    public Entity target = null;
    public WorldTile currentTile = null;
    // Bools
    public bool isAlive = false;
    public bool isMature = false;
    public bool isCarryingSpawn = false;
    public bool isReproductionOnCooldown = false;
    // Mutation
    public int numberOfMutations = 0;
    // Reproduction
    public int mateID = 0;
    

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
            this.genes.Add(geneType, Mathematics.GetCauchyDistributedRandomNumber());
        }
    }

    // Gets random genes from two parents
    public void GetGenesFromParents(Dictionary<string, float> _parentOneGenes, Dictionary<string, float> _parentTwoGenes)
    {
        foreach(KeyValuePair<string, float> gene in _parentOneGenes)
        {
            int random = GameController.random.Next(0, 2);
            float geneValue = (random == 0) ? _parentOneGenes[gene.Key] : _parentTwoGenes[gene.Key];
            this.genes.Add(gene.Key, geneValue);
        }
    }

    // Mutate random gene
    public void Mutate()
    {
        string geneType = this.genes.ElementAt(GameController.random.Next(0, this.genes.Count)).Key;
        this.genes[geneType] = Mathematics.GetCauchyDistributedRandomNumber();
        this.RecheckThrive();
        this.numberOfMutations++;
    }

    // Recheck genes
    public virtual void RecheckThrive()
    {
        // Each type has their own method
    }

    // Initializes stat values
    public void InitializeStats()
    {
        this.isAlive = true;
        this.birthTime = Time.time;
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
            similarityPercents[index] = Mathematics.GetHeightAtPointOnNormalDistribution(_currentEntity.genes[gene.Key], _targetedEntity.genes[gene.Key]);
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
        else if(this.isAlive == true && this.health < this.maxHealth && this.energy >= this.maxEnergy * 0.5f)
        {
            this.Heal();
        }
        // If dead, rot
        else if(this.isAlive == false && this.health > 0f)
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
        this.percentOfMaxAge = Mathf.Clamp01(Time.time / ((this.genes[geneAgeMax] * ageScale) + this.birthTime));
        if(this.isMature == false)
        {
            this.percentMature = Mathf.Clamp01(Time.time / ((this.genes[geneAgeMature] * maturityScale) + this.birthTime));
            if(this.percentMature >= 1.0f)
            {
                this.isMature = true;
            }
        }
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
        // Each type has their own method
    }

    // Update scale of entity
    public void UpdateSize()
    {
        if(this.isMature == false)
        {
            if(this.percentMature >= 1.0f)
            {
                this.isMature = true;
                float size = this.genes[geneSizeMax] * sizeScale;
                this.gameObject.transform.localScale = new Vector3(size, size, size);
            }
            else
            {
                float size = Mathf.Clamp(this.genes[geneSizeMax] * sizeScale * this.percentMature, smallestSizeScale, sizeScale);
                this.gameObject.transform.localScale = new Vector3(size, size, size);
            }
        }
    }

    // Update reproduction
    public virtual void UpdateReproduction()
    {
        // Each type has their own method
    }
    #endregion
}
