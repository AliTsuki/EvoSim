using System.Collections.Generic;

using UnityEngine;

// Defines and controls animals
public class Animal : Entity
{
    // World spawn animal constructor
    public Animal(int _id, WorldTile _tile)
    {
        this.id = _id;
        this.currentTile = _tile;
        this.currentTile.animals.Add(this);
        this.isMature = true;
        this.geneTypes = this.animalGeneTypes;
        this.RandomizeGenes();
        this.InitializeStats();
        this.gameObject = GameObject.Instantiate(Resources.Load<GameObject>("/Entities/Entity"), new Vector3(_tile.position.x + 0.5f, 0.2f, _tile.position.y + 0.5f), Quaternion.identity, Lifeforms.plantParentObject.transform);
        this.Start();
    }

    // Parent spawn plant constructor
    public Animal(int _id, WorldTile _tile, Dictionary<string, float> _parentOne, Dictionary<string, float> _parentTwo)
    {
        this.id = _id;
        this.currentTile = _tile;
        this.currentTile.animals.Add(this);
        this.isMature = false;
        this.geneTypes = this.animalGeneTypes;
        this.GetGenesFromParents(_parentOne, _parentTwo);
        this.InitializeStats();
        this.gameObject = GameObject.Instantiate(Resources.Load<GameObject>("/Entities/Entity"), new Vector3(_tile.position.x + 0.5f, 0.2f, _tile.position.y + 0.5f), Quaternion.identity, Lifeforms.plantParentObject.transform);
        this.Start();
    }

    // Update energy
    public override void UpdateEnergy()
    {

    }

    // Update reproduction
    public override void UpdateReproduction()
    {

    }
}
