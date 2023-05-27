using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisualizer : MonoBehaviour
{
    public LSystemGenerator LSystem;
    List<Vector3> pozitions = new List<Vector3>();
    public GameObject prefab;
    public Material lineMaterial;

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

        foreach(var letter in secquence)
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
                    if(savePoinsts.Count > 0)
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
                    DrawLine(tempPosition, currentPosition, Color.red);
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
        foreach (var position in pozitions)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("Line");
        line.transform.position = start;
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public enum EncodeLetters
    {
        unknow = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-'
    }
}
