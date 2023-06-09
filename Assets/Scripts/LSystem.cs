using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Rule
{
    public char Old;
    public string New;
    
}


public class LSystem : MonoBehaviour
{
    ///Basic L System where you can define rules
    ///
    [SerializeField] private LSystemTemplate template;
    private string start;
    private List<Rule> rules = new List<Rule>();

    [SerializeField] private string current;
    [SerializeField] private int currentIteration;
    [SerializeField] private List<string> saves;


    private void Awake()
    {
        start = template.getStart();
        rules = template.getRules();
        current = start;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            apply_iterations(1);
        }
    }

    private string apply_rules(string old)
    {
        string newString = "";
        foreach(char c in old)
        {
            bool isRule = false;
            foreach(Rule rule in rules)
            {
                if (c == rule.Old)
                {
                    newString += rule.New;
                    isRule = true;
                    break;
                }
            }
            if (!isRule)
            {
                newString += c;
            }
        }
        return newString;
    }

    private string step()
    {
        saves.Add(current);
        current = apply_rules(current);
        return current;
    }

    public string apply_iterations(uint quantity)
    {
        for(int i = 0; i < quantity; i++)
        {
            step();
        }
        currentIteration += (int) quantity;
        return current;
    }
}
