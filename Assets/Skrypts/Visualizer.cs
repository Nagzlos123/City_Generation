using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LineVisualizer;

public class Visualizer : MonoBehaviour
{
    public LSystemGenerator LSystem;
    List<Vector3> pozitions = new List<Vector3>();
    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    private int lenght = 8;
    private float angle = 90;

    public int Lenght
    {
        get
        {
            if (lenght > 0)
            {
                return lenght;
            }
            else
            {
                return 1;
            }
        }

        set => lenght = value;

    }

    private void Start()
    {
        var secquence = LSystem.GenerateSentence();
        VisualizeSecquence(secquence);

    }

    private void VisualizeSecquence(string secquence)
    {
        Stack<AgentPrameters> savePoinsts = new Stack<AgentPrameters>();
        var currentPosition = Vector3.zero;
        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        pozitions.Add(currentPosition);

        foreach (var letter in secquence)
        {
            EncodeLetters encoding = (EncodeLetters)letter;



            switch (encoding)
            {

                case EncodeLetters.save:
                    savePoinsts.Push(new AgentPrameters
                    {
                        position = currentPosition,
                        direction = direction,
                        lenght = Lenght
                    }
                        );
                    break;
                case EncodeLetters.load:
                    if (savePoinsts.Count > 0)
                    {
                        var agentParameter = savePoinsts.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Lenght = agentParameter.lenght;
                    }
                    else
                    {
                        throw new System.Exception("Don't have save point in ouer stack!");
                    }
                    break;
                case EncodeLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * lenght;
                    roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), Lenght);
                    Lenght -= 2;
                    pozitions.Add(currentPosition);
                    break;
                case EncodeLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case EncodeLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;

                default:
                    break;
            }
        }
        roadHelper.FixRoad();
        structureHelper.PlaceStructures(roadHelper.GetRoadPositions());
    }
}
