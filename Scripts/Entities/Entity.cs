using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Superclass for all entities
public abstract class Entity
{
    // Statics
    public const float mutationChanceScale = 0.016f; // 1 percent per second maximum mutation chance
    public const float healthScale = 100f; // 100 is highest health value
    public const float energyScale = 100f; // 100 is highest energy value
    public const float sizeScale = 2f; // 200% is largest scale of entities
    public const float smallestSizeScale = 0.2f; // 20% is smallest scale of entities
    public const float maturityScale = 120f; // 2 minutes is longest age to maturity
    public const float ageScale = 600f; // 10 minutes is longest age before death
    public const float maxRotSpeed = 0.016f; // 1 health per second rot speed maximum

    // Unity elements
    public GameObject gameObject;
    public Transform transform;
    public Sprite sprite;
    // ID
    public int id;
    // Birthday
    public float birthMoment;

    // Genes
    public Dictionary<string, float> genes = new Dictionary<string, float>();
    public string[] geneTypes;
    public string[] plantGeneTypes = new string[] { "Mutation_Chance", "Health_Max", "Energy_Max", "Size_Max", "Age_Mature", "Age_Max", "Rot_Speed", "Energy_Cost_Reproduction", "Minimum_Similarity_Reproduction", "Pollinating_Distance", "Seeding_Distance", "Thrive_Stone", "Thrive_Cobble", "Thrive_Gravel", "Thrive_Dirt", "Thrive_Sand", "Thrive_Silt", "Thrive_Clay", "Thrive_Highland", "Thrive_Lowland", "Thrive_Shallows", "Thrive_Ocean" };
    public string[] animalGeneTypes = new string[] { };

    // Stats
    public float health;
    public float maxHealth;
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

    // Take damage
    public void TakeDamage(float _damageAmount)
    {
        this.health -= _damageAmount;
    }

    // What to do when entity dies
    public virtual void Death()
    {

    }

    // Creates a random set of genes for this entity
    public void RandomizeGenes()
    {
        foreach(string geneType in this.geneTypes)
        {
            this.genes.Add(geneType, (float)GameController.random.NextDouble() + 0.0001f);
        }
    }

    // Gets random genes from two parents
    public void GetGenesFromParents(Dictionary<string, float> _parentOneGenes, Dictionary<string, float> _parentTwoGenes)
    {
        foreach(KeyValuePair<string, float> gene in _parentOneGenes)
        {
            int r = GameController.random.Next(0, 2);
            float geneValue = (r == 0) ? _parentOneGenes[gene.Key] : _parentTwoGenes[gene.Key];
            this.genes.Add(gene.Key, geneValue);
        }
    }

    // Mutate random gene
    public void Mutate()
    {
        string geneType = this.genes.ElementAt(GameController.random.Next(0, this.genes.Count)).Key;
        this.genes[geneType] = (float)GameController.random.NextDouble() + 0.0001f;
    }

    // Initializes stat values
    public void InitializeStats()
    {
        this.isAlive = true;
        this.birthMoment = Time.time;
        this.maxHealth = this.genes["Health_Max"] * healthScale;
        this.health = this.maxHealth;
        this.maxEnergy = this.genes["Energy_Max"] * energyScale;
        this.energy = this.maxEnergy;
        if(this.isMature == false)
        {
            float size = Mathf.Clamp(this.genes["Size_Max"], smallestSizeScale, sizeScale);
            this.gameObject.transform.localScale = new Vector3(size, size, size);
        }
        else
        {
            float size = this.genes["Size_Max"] * sizeScale;
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

    // Update health
    public void UpdateHealth()
    {
        if(this.isAlive == true && this.health <= 0f)
        {
            this.isAlive = false;
            this.health = this.maxHealth;
        }
        else
        {
            this.health -= this.genes["Rot_Speed"] * 0.5f;
        }
    }

    // Update mutation
    public void UpdateMutation()
    {
        float mutationChance = this.genes["Mutation_Chance"] * mutationChanceScale;
        float r = (float)GameController.random.NextDouble();
        if(r < mutationChance)
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
            float percentMature = Time.time / ((this.genes["Age_Mature"] * maturityScale) + this.birthMoment);
            if(percentMature >= 1.0f)
            {
                this.isMature = true;
                float size = this.genes["Size_Max"] * sizeScale;
                this.gameObject.transform.localScale = new Vector3(size, size, size);
            }
            else
            {
                float size = Mathf.Clamp(this.genes["Size_Max"] * sizeScale * percentMature, smallestSizeScale, sizeScale);
                this.gameObject.transform.localScale = new Vector3(size, size, size);
            }
        }
    }

    // Update reproduction
    public virtual void UpdateReproduction()
    {

    }
}
