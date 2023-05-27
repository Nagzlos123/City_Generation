using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadDirection;
using System.Linq;

public class RoadHelper : MonoBehaviour
{
    public GameObject roadStraight, roadCorner, road3way, road4way, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

    public List<Vector3Int> GetRoadPositions()
    {
        return roadDictionary.Keys.ToList();
    }

    public void PlaceStreetPositions(Vector3 starPosition, Vector3Int direction, int lenght)
    {
        var rotation = Quaternion.identity;
        if(direction.x == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }

        for(int i = 0; i < lenght; i++)
        {
            var position = Vector3Int.RoundToInt(starPosition + direction * i);

            if (roadDictionary.ContainsKey(position))
            {
                continue;
            }
            var road = Instantiate(roadStraight, position, rotation, transform);
            roadDictionary.Add(position, road);

            if (i == 0 || i == lenght - 1)
            {
                fixRoadCandidates.Add(position);
            }
        }
    }

    public void FixRoad()
    {
        foreach(var position in fixRoadCandidates)
        {
            List<Direction> neighboursDirections = PlacementHelper.FindNeighbour(position, roadDictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            if (neighboursDirections.Count == 1)
            {
              
                Destroy(roadDictionary[position]);
                if (neighboursDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighboursDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighboursDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);
            }
            else if(neighboursDirections.Count == 2)
            {
                if ((neighboursDirections.Contains(Direction.Up) && neighboursDirections.Contains(Direction.Down)) 
                    || (neighboursDirections.Contains(Direction.Right) && neighboursDirections.Contains(Direction.Left)))
                {
                    continue;
                }

                Destroy(roadDictionary[position]);
                if (neighboursDirections.Contains(Direction.Up) && neighboursDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighboursDirections.Contains(Direction.Right) && neighboursDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighboursDirections.Contains(Direction.Down) && neighboursDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);
            }
            else if(neighboursDirections.Count == 3)
            {
                Destroy(roadDictionary[position]);
                if (neighboursDirections.Contains(Direction.Right) 
                    && neighboursDirections.Contains(Direction.Down)
                    && neighboursDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighboursDirections.Contains(Direction.Down) 
                    && neighboursDirections.Contains(Direction.Left)
                    && neighboursDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighboursDirections.Contains(Direction.Left) 
                    && neighboursDirections.Contains(Direction.Up)
                    && neighboursDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[position] = Instantiate(road3way, position, rotation, transform);
            }
            else
            {
                Destroy(roadDictionary[position]);
                roadDictionary[position] = Instantiate(road4way, position, rotation, transform);
            }
        }
    }
}
