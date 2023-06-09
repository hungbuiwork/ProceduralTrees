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
    public Vector3 prevDirection;

    public LSystemState(Vector3 position, Vector3 direction, float scale, Transform transform, Vector3 prevDirection)
    {
        this.position = position;
        this.scale = scale;
        this.transform = transform;
        this.direction = direction;
        this.prevDirection = prevDirection;
    }

}
public class LSystemRenderer : MonoBehaviour
{
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
    [SerializeField] private float currScale = 1f;
    [SerializeField] private Vector3 currDir = Vector3.up;
    [SerializeField] private Transform currTransform; //the transform used for adding parents/children in the hierarchy

    [SerializeField] private Vector3 prevDir = Vector3.up;
    [SerializeField] private Transform recentTransform; //recentmost transform, set as the currtransform whenever a save occurs




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
        currTransform = this.transform;
        Create(system.apply_iterations(iterations));
        Save();
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
                case 'S':
                    Scale();
                    break;

            }
        }
    }



    private void Save()
    {
        //COMPLETED
        currTransform = recentTransform;
        stack.Push(new LSystemState(currPos, currDir, currScale, currTransform, prevDir));
    }

    private void Restore()
    {
        //COMPLETED
        LSystemState restoredState = stack.Pop();
        currPos = restoredState.position;
        currDir = restoredState.direction;
        currScale = restoredState.scale;
        currTransform = restoredState.transform;
        prevDir = restoredState.prevDirection;

    }
    private void Draw_Branch()
    {
        //COMPLETED(for the most part)
        //TODO: set the transforms accordingly
        //Draws a branch, sets transform's parent to the currTransform, updates currPos accordingly(by offsetting currPos by branchLength * currScale)
        prevDir = new Vector3(currDir.x, currDir.y, currDir.z); //UNKNOWN IF THIS IS NEEDED
        Vector3 newPos = currPos + (currDir.normalized * currScale);
        GameObject branch = Instantiate(branchPrefab, newPos, Quaternion.Euler(currDir.x, currDir.y, currDir.z));
        //branch.transform.up = currDir;
        branch.transform.up = currDir;
        branch.transform.localScale = new Vector3(currScale * branch.transform.localScale.x, currScale * branch.transform.localScale.y, currScale * branch.transform.localScale.z);
        branch.transform.parent = this.transform;
        
        /*
        branch.transform.localScale = new Vector3(1, 1, 1);
        branch.transform.SetParent(currTransform, true);
        */
        recentTransform = branch.transform;

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
        Vector3 newDir = Quaternion.AngleAxis(value, Vector3.right) * currDir;//I think Vector3.right has to be replaced with some vector contingent on the previous direction(prevDir)
        currDir = newDir.normalized;
    }

    private void Pitch(int value)
    {
        // &(pitch down) and ^(pitch up)
        //TODO: modify the angle/direction and update currAngle
        Vector3 newDir = Quaternion.AngleAxis(value, Vector3.forward) * currDir;//I think Vector3.forward has to be replaced with some vector contingent on the previous direction(prevDir)
        currDir = newDir.normalized;
    }

    private void Roll(int value)
    {

        // \\(roll left) and /(roll right)
        //TODO: modify the angle/direction and update currAngle
        Vector3 newDir = Quaternion.AngleAxis(value, Vector3.up) * currDir; //I think Vector3.up has to be replaced with some vector contingent on the previous direction(prevDir)
        currDir = newDir.normalized;
    }

    private void Scale()
    {
        currScale = scaleValue * currScale;
        Debug.Log(currScale);
    }



}
