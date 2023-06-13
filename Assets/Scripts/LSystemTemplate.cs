using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LSystem/Ruleset")]
public class LSystemTemplate : ScriptableObject
{
    [SerializeField] private string start;
    [SerializeField] private List<Rule> rules = new List<Rule>();
    //Some settings
    [SerializeField] private float branchLength = 1;
    [SerializeField] private int angle = 25;
    [SerializeField] private float scaleValue = 0.9f;
    [SerializeField] private float branchThickness = 1;

    [SerializeField] public GameObject branchPrefab;
    [SerializeField] public bool createLeaves;
    [SerializeField] public GameObject leafPrefab;
    public string getStart() { return start; }
    public List<Rule> getRules() { return rules; }

    public float getBranchLength() { return branchLength; }
    public int getAngle() { return angle; }
    public float getScaleValue() { return scaleValue; }

    public float getBranchThickness() { return branchThickness; }
}
