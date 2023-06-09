using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadDirection;
using System;

public class StructureHelper : MonoBehaviour
{
    public BuildingType[] buildingTypes;
    Dictionary<Vector3Int, GameObject> structureDictionary = new Dictionary<Vector3Int, GameObject>();
    

    public void PlaceStructures(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesArundRoad(roadPositions);
        List<Vector3Int> blockedPositions = new List<Vector3Int>();
        foreach (var freeSpot in freeEstateSpots)
        {
            if (blockedPositions.Contains(freeSpot.Key))
            {
                continue;
            }

            var rotation = Quaternion.identity;
            switch (freeSpot.Value)
            {
                case Direction.Up:
                    rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.Down:
                    rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Direction.Right:
                    rotation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    break;
            }
            for (int i = 0; i < buildingTypes.Length; i++)
            {
                if(buildingTypes[i].quantity == -1)
                {
                    var building = SpamPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                    structureDictionary.Add(freeSpot.Key, building);
                    break;
                }

                if (buildingTypes[i].IsBuildingAvailable())
                {
                    if(buildingTypes[i].sizeRequierd > 1)
                    {
                        var halfSize = Mathf.FloorToInt(buildingTypes[i].sizeRequierd / 2);
                        List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                        if (VerifyIfBuildingsFits(halfSize, freeEstateSpots, freeSpot, ref tempPositionsBlocked))
                        {
                            blockedPositions.AddRange(tempPositionsBlocked);
                            var building = SpamPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                            structureDictionary.Add(freeSpot.Key, building);
                            foreach (var pos in tempPositionsBlocked)
                            {
                                structureDictionary.Add(pos, building);
                            }
                            break;
                        }
                    }
                    else
                    {
                        var building = SpamPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                        structureDictionary.Add(freeSpot.Key, building);
                    }
                    break;
                }

            }
            
        }
    }

    private bool VerifyIfBuildingsFits(int halfSize, Dictionary<Vector3Int, Direction> freeEstateSpots, KeyValuePair<Vector3Int, Direction> freeSpot, ref List<Vector3Int> tempPositionsBlocked)
    {
        Vector3Int direction = Vector3Int.zero;
        if (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            direction = Vector3Int.right;
        }
        else
        {
            direction = new Vector3Int(0, 0, 1);
        }

        for (int i = 0; i <= halfSize; i++)
        {
            var pos1 = freeSpot.Key + direction * i;
            var pos2 = freeSpot.Key - direction * i;
            if (!freeEstateSpots.ContainsKey(pos1) || !freeEstateSpots.ContainsKey(pos2))
            {
                return false;
            }
            tempPositionsBlocked.Add(pos1);
            tempPositionsBlocked.Add(pos2);
        }
        return true;
    }

    private GameObject SpamPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        var newStructure = Instantiate(prefab, position, rotation, transform);
        return newStructure;
    }

    private Dictionary<Vector3Int, Direction> FindFreeSpacesArundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
            var neighboursDirections = PlacementHelper.FindNeighbour(position, roadPositions);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (neighboursDirections.Contains(direction) == false)
                {
                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                    if (freeSpaces.ContainsKey(newPosition))
                    {
                        continue;
                    }
                    freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                }
            }
        }
        return freeSpaces;
    }
}
