using System.Collections.Generic;

using UnityEngine;

// Defines and controls plants
public class Plant : Entity
{
    // World spawn plant constructor
    public Plant(int _id, WorldTile _tile)
    {
        this.id = _id;
        this.currentTile = _tile;
        this.currentTile.plant = this;
        this.isMature = true;
        this.geneTypes = this.plantGeneTypes;
        this.RandomizeGenes();
        this.SetupGameObject();
        this.InitializeStats();
        this.Start();
    }

    // Parent spawn plant constructor
    public Plant(int _id, WorldTile _tile, Dictionary<string, float> _parentOne, Dictionary<string, float> _parentTwo)
    {
        this.id = _id;
        this.currentTile = _tile;
        this.currentTile.plant = this;
        this.isMature = false;
        this.geneTypes = this.plantGeneTypes;
        this.GetGenesFromParents(_parentOne, _parentTwo);
        this.SetupGameObject();
        this.InitializeStats();
        this.Start();
    }


    // Set up GameObject
    public void SetupGameObject()
    {
        this.gameObject = GameObject.Instantiate(GameManager.instance.entity, new Vector3(this.currentTile.position.x + 0.5f, 0.2f, this.currentTile.position.y + 0.5f), Quaternion.Euler(90, 0, 0), Lifeforms.plantParentObject.transform);
        this.gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Shader Graphs/Entity Shader"));
        this.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", this.GetTexture());
    }

    // Update energy
    public override void UpdateEnergy()
    {
        // TODO: remove energy for existing in environments not suited for
    }

    // Update reproduction
    public override void UpdateReproduction()
    {
        // TODO: when energy is above energy cost to reproduce, check nearby tiles in radius of pollinating distance, if another plant of similar genetics is there, produce a spawn within seeding distance
    }

    // Get texture for plant
    public Texture2D GetTexture()
    {
        // TODO: make unique plant textures and assign them here based on plant stats
        return new Texture2D(32, 32, TextureFormat.ARGB32, false);
    }
}
