using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class LSystemState
{
    public Vector3 position;
    public Vector3 direction;
    public float scale;
    public Transform transform;
    //[TODO: create variable to store direction]

    public LSystemState(Vector3 position, Vector3 direction, float scale, Transform transform)
    {
        this.position = position;
        this.scale = scale;
        this.transform = transform;
        this.direction = direction;
    }

}
public class LSystemRenderer : MonoBehaviour
{
    [SerializeField] private int iterationsToRun;
    [SerializeField] private Stack<LSystemState> stack = new Stack<LSystemState>(); //the stack to append/pop off of

    //variables to store static info
    [SerializeField] private LSystem system;
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private float branchLength = 1;
    [SerializeField] private uint iterations = 5;
    [SerializeField] private int turnAngle = 30;
    [SerializeField] private float scaleValue = 1.0f;

    //variables to store current state
    [SerializeField] private Vector3 currPos = Vector3.zero;
    [SerializeField] private float currScale = 1;
    [SerializeField] private Vector3 currDir = Vector3.up;
    [SerializeField] private Transform currTransform; //the transform used for adding parents/children in the hierarchy


    //For Gizmos drawing
    public struct Line
    {
        public Vector3 start, end;
        public Line(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
        }
    }

    private List<Line> gizmoLines = new List<Line>();
    private List<Vector3> gizmoPoints= new List<Vector3>();


    /*
     + = turn right
     - = turn left
     & = pitch down
     ^ = pitch up
     \\ = roll left
     / = roll right
     | = turn 180 degree [Possibly Not Needed]
     F = draw branch (and go forward)
     g = go forward [Possibly Not Needed]
     [ = save state
     ] = restore state
     " = scale state (YET TO IMPLEMENT)
     */

    private void Awake()
    {
        currPos = this.transform.position;
        Create(system.apply_iterations(iterations));
    }

    private void Create(string template)
    {
        //TODO: Use helper functions to create the entire thing
        foreach(char c in template)
        {
            switch (c)
            {
                case '[':
                    Save();
                    break;
                case ']':
                    Restore();
                    break;
                case 'F':
                    Draw_Branch();
                    break;
                case '+':
                    Turn(turnAngle);
                    break;
                case '-':
                    Turn(-turnAngle);
                    break;
                case '&':
                    Pitch(turnAngle);
                    break;
                case '^':
                    Pitch(-turnAngle);
                    break;
                case '\\':
                    Roll(turnAngle);
                    break;
                case '/':
                    Roll(turnAngle);
                    break;
                case '?':
                    Roll(-turnAngle);
                    break;
                case '"':
                    Scale();
                    break;

            }
        }
    }



    private void Save()
    {
        //COMPLETED
        stack.Push(new LSystemState(currPos, currDir, currScale, currTransform));
    }

    private void Restore()
    {
        //COMPLETED
        LSystemState restoredState = stack.Pop();
        currPos = restoredState.position;
        currDir = restoredState.direction;
        currScale = restoredState.scale;
        currTransform = restoredState.transform;

    }
    private void Draw_Branch()
    {
        //COMPLETED(for the most part)
        //TODO: set the transforms accordingly
        //Draws a branch, sets transform's parent to the currTransform, updates currPos accordingly(by offsetting currPos by branchLength * currScale)
        Vector3 newPos = currPos + (currDir.normalized * currScale);
        gizmoLines.Add(new Line(currPos, newPos));
        gizmoPoints.Add(newPos);
        currPos = newPos;

    }

    private void OnDrawGizmos()
    {
        foreach(Line line in gizmoLines)
        {
            Gizmos.DrawLine(line.start, line.end);
        }
        foreach(Vector3 point in gizmoPoints)
        {
            Gizmos.DrawSphere(point, 0.02f);
        }
    }
    private void Turn(int value)
    {

        // + and - symbols
        //TODO: modify the angle/direction and update currAngle
        Vector3 newDir = Quaternion.AngleAxis(value, Vector3.right) * currDir;
        currDir = newDir;
    }

    private void Pitch(int value)
    {
        // &(pitch down) and ^(pitch up)
        //TODO: modify the angle/direction and update currAngle
        Vector3 newDir = Quaternion.AngleAxis(value, Vector3.forward) * currDir;
        currDir = newDir;
    }

    private void Roll(int value)
    {

        // \(roll left) and /(roll right)
        //TODO: modify the angle/direction and update currAngle
        Vector3 newDir = Quaternion.AngleAxis(value, Vector3.up) * currDir;
        currDir = newDir;
    }

    private void Scale()
    {
        currScale = scaleValue * currScale;
    }



}
