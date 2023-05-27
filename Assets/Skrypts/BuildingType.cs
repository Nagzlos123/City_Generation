using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingType
{
    [SerializeField] private GameObject[] prefabs;
    public int sizeRequierd;
    public int quantity;
    public int currentQuantity;

    public GameObject GetPrefab()
    {
        currentQuantity++;
        if (prefabs.Length > 1)
        {
            var random = UnityEngine.Random.Range(0, prefabs.Length);
            return prefabs[random];
        }
        return prefabs[0];
    }

    public bool IsBuildingAvailable()
    {
        return currentQuantity < quantity;
    }
    public void Reset()
    {
        currentQuantity = 0;
    }
}
