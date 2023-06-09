using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LSystem/Ruleset")]
public class LSystemTemplate : ScriptableObject
{
    [SerializeField] private string start;
    [SerializeField] private List<Rule> rules = new List<Rule>();
    public string getStart() { return start; }
    public List<Rule> getRules() { return rules; }
}
