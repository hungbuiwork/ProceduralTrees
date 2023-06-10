using Palmmedia.ReportGenerator.Core.Parser.Analysis;
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
    [SerializeField] private GameObject leafPrefab;
    private float branchLength = 1;
    private float branchThickness = 1;
    [SerializeField] private uint iterations = 5;
    private int turnAngle = 30;
    private float scaleValue = 1.0f;

    //variables to store current state
    private Vector3 currPos = Vector3.zero;
    private float currScale = 1f;
    private Vector3 currDir = Vector3.up;
    private Transform currTransform; //the transform used for adding parents/children in the hierarchy

    private Vector3 prevDir = Vector3.up;
    private Transform recentTransform; //recentmost transform, set as the currtransform whenever a save occurs
    private int currentRecursionDepth;


    //To store trees, and animate them
    [SerializeField] private List<GameObject> generatedTrees = new List<GameObject>();
    [SerializeField] private GameObject currentTree;
    [SerializeField] private int currentActiveTree = 0;
    [SerializeField] private float timeBetweenSwap = 0.5f;
    [SerializeField] private float timeSinceLastSwap = 0f;
    [SerializeField] private bool animating = false;




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
        branchLength = system.template.getBranchLength();
        scaleValue = system.template.getScaleValue();
        turnAngle = system.template.getAngle();
        branchThickness = system.template.getBranchThickness();
        system.apply_iterations(iterations);
        List<string> templateIterations = system.saves;
        foreach(string s in templateIterations)
        {
            Create(s);
        }
        Save();
        foreach(GameObject g in generatedTrees)
        {
            g.SetActive(false);
        }
        generatedTrees[0].SetActive(true);
        animating = true;
    }

    private void Update()
    {
        timeSinceLastSwap += Time.deltaTime;
        if (timeSinceLastSwap > timeBetweenSwap)
        {
            timeSinceLastSwap = 0f;
            NextTree();
        }
    }
    private void NextTree()
    {
        generatedTrees[currentActiveTree].SetActive(false);
        currentActiveTree = (currentActiveTree + 1) % generatedTrees.Count;
        generatedTrees[currentActiveTree].SetActive(true);

    }
    private void Create(string template)
    {
        //TODO: Use helper functions to create the entire thing
        currentTree = new GameObject() { name = "Iteration " + generatedTrees.Count.ToString()};
        generatedTrees.Add(currentTree);
        currentTree.transform.parent = this.transform;
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
        Reset();
    }

    private void Reset()
    {
        ///Reset between iteration creation
        currScale = 1f;
        currDir = Vector3.up;
        currTransform = null;
        recentTransform = null;
        prevDir= Vector3.up;
        currPos = this.transform.position;
        currentRecursionDepth = 0;

    }

    private void Save()
    {
        //COMPLETED
        currentRecursionDepth += 1;
        currTransform = recentTransform;
        stack.Push(new LSystemState(currPos, currDir, currScale, currTransform, prevDir));
    }

    private void Restore()
    {
        if (currentRecursionDepth > 1)
        {
            GameObject leaf = Instantiate(leafPrefab, currPos + 0.5f * currDir.normalized * branchLength * currScale, Quaternion.Euler(currDir.x, currDir.y, currDir.z));
            leaf.transform.parent = currentTree.transform;
            leaf.transform.localScale = new Vector3(currScale * branchThickness * leaf.transform.localScale.x, currScale * branchLength * leaf.transform.localScale.y, currScale * branchThickness * leaf.transform.localScale.z) * 2.0f;
        }
        //COMPLETED
        currentRecursionDepth -= 1;
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
        Vector3 newPos = currPos + (currDir.normalized * currScale * branchLength);
        GameObject branch = Instantiate(branchPrefab, newPos, Quaternion.Euler(currDir.x, currDir.y, currDir.z));
        //branch.transform.up = currDir;
        branch.transform.up = currDir;
        branch.transform.localScale = new Vector3(currScale * branchThickness * branch.transform.localScale.x, currScale * branchLength * branch.transform.localScale.y, currScale * branchThickness * branch.transform.localScale.z);
        branch.transform.parent = currentTree.transform;
        
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
    }



}
