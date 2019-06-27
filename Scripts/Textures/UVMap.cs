using System;
using System.Collections.Generic;

using UnityEngine;

// Class for creating UVMap
public class UVMap
{
    // UVMap fields
    public static Dictionary<string, UVMap> Maps = new Dictionary<string, UVMap>();
    public string Name;
    public Vector2[] UVMaps;

    // UVMap constructor
    public UVMap(string _name, Vector2[] _uvMap)
    {
        this.Name = _name;
        this.UVMaps = _uvMap;
        this.RegisterUVMap();
    }

    // Register UVMaps
    public void RegisterUVMap()
    {
        Maps.Add(this.Name, this);
    }

    // Get UVMap
    public static UVMap GetUVMap(string _name)
    {
        try
        {
            return Maps[_name];
        }
        catch(Exception e)
        {
            Debug.Log($@"Can't find associated image: {_name}");
            Debug.Log($@"{e.ToString()}");
            return null;
        }
    }
}
