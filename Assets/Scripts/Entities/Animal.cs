using System.Collections.Generic;

using UnityEngine;

// Defines and controls animals
public class Animal : Entity
{
    // World spawn animal constructor
    public Animal(int _id, WorldTile _tile)
    {
        this.id = _id;
        this.type = TypeEnum.Animal;
        this.currentTile = _tile;
        this.currentTile.animals.Add(this.id, this);
        this.isMature = true;
        this.geneTypes = animalGeneTypes;
        this.percentMature = 1.0f;
        this.RandomizeGenes();
        this.InitializeStats();
        this.gameObject = GameObject.Instantiate(Resources.Load<GameObject>("/Entities/Entity"), new Vector3(_tile.position.x + 0.5f, 0.2f, _tile.position.y + 0.5f), Quaternion.identity, Lifeforms.plantParentObject.transform);
        this.Start();
    }

    // Parent spawn plant constructor
    public Animal(int _id, WorldTile _tile, Dictionary<string, float> _parentOneGenes, Dictionary<string, float> _parentTwoGenes)
    {
        this.id = _id;
        this.type = TypeEnum.Animal;
        this.currentTile = _tile;
        this.currentTile.animals.Add(this.id, this);
        this.isMature = false;
        this.geneTypes = animalGeneTypes;
        this.GetGenesFromParents(_parentOneGenes, _parentTwoGenes);
        this.InitializeStats();
        this.gameObject = GameObject.Instantiate(Resources.Load<GameObject>("/Entities/Entity"), new Vector3(_tile.position.x + 0.5f, 0.2f, _tile.position.y + 0.5f), Quaternion.identity, Lifeforms.plantParentObject.transform);
        this.Start();
    }


    // Set up GameObject
    public void SetupGameObject()
    {
        //this.gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Entities/Entity"), new Vector3(this.currentTile.position.x + 0.5f, 0.2f, this.currentTile.position.y + 0.5f), Quaternion.Euler(90, 0, 0), Lifeforms.plantParentObject.transform);
        //this.gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Shader Graphs/Entity Shader"));
        //this.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", this.GetTexture());
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
