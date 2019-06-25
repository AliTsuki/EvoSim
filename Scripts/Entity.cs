﻿using UnityEngine;

public abstract class Entity
{
    public GameObject gameObject;
    public Transform transform;
    public bool isAlive;
    

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    // FixedUpdate is called a fixed number of times a second
    public virtual void FixedUpdate()
    {

    }
}
